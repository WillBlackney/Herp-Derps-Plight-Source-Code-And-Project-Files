using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.Common.EditorScripts
{
    /// <summary>
    /// Used for creating sprite sheets for frame-by-frame animation
    /// </summary>
    public class SpriteSheetMaker : MonoBehaviour
    {
        public GameObject Canvas;
	    public GameObject Camera;
        public string UpperBodyAnimationFolder;
	    public string LowerBodyAnimationFolder;
        public List<string> UpperBodyClipNames;
	    public List<string> LowerBodyClipNames;
		public Dropdown UpperAnimationDropdown;
	    public Dropdown LowerAnimationDropdown;
	    public Slider CameraSizeSlider;
		public Dropdown FrameSizeDropdown;
        public Dropdown FrameRatioDropdown;
        public Dropdown ScreenshotIntervalDropdown;
        public Dropdown ShadowDropdown;
        public InputField FolderName;
	    public GameObject Shadow;

	    private Character _character;
	    private ScreenshotTransparent _screenshotTransparent;

		#if UNITY_EDITOR

		/// <summary>
		/// Called only in Editor
		/// </summary>
		public void OnValidate()
        {
	        UpperBodyClipNames = System.IO.Directory.GetFiles(UpperBodyAnimationFolder, "*.anim", System.IO.SearchOption.AllDirectories).Select(System.IO.Path.GetFileNameWithoutExtension).ToList();
			LowerBodyClipNames = System.IO.Directory.GetFiles(LowerBodyAnimationFolder, "*.anim", System.IO.SearchOption.AllDirectories).Select(System.IO.Path.GetFileNameWithoutExtension).ToList();

			UpperAnimationDropdown.options = new List<Dropdown.OptionData> { new Dropdown.OptionData("All") };
			LowerAnimationDropdown.options = new List<Dropdown.OptionData> { new Dropdown.OptionData("All") };

            foreach (var clipName in UpperBodyClipNames)
            {
	            UpperAnimationDropdown.options.Add(new Dropdown.OptionData(clipName));
            }

	        foreach (var clipName in LowerBodyClipNames)
	        {
		        LowerAnimationDropdown.options.Add(new Dropdown.OptionData(clipName));
	        }
		}

        /// <summary>
        /// Called on start
        /// </summary>
        public void Start()
        {
	        _character = FindObjectOfType<Character>();
	        _screenshotTransparent = Camera.AddComponent<ScreenshotTransparent>();

            foreach (var dropdown in new[] { UpperAnimationDropdown, LowerAnimationDropdown, FrameSizeDropdown, FrameRatioDropdown, ScreenshotIntervalDropdown, ShadowDropdown })
            {
                dropdown.RefreshShownValue();
            }

			if (UpperBodyClipNames.Count == 0) OnValidate();
        }

        /// <summary>
        /// Load character from prefab
        /// </summary>
        public void Load()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Open character prefab", "", "prefab");

            if (path.Length > 0)
            {
                path = "Assets" + path.Replace(Application.dataPath, null);
                Load(path);
            }
        }

        /// <summary>
        /// Load character from prefab by given path
        /// </summary>
        public void Load(string path)
        {
            var character = UnityEditor.AssetDatabase.LoadAssetAtPath<Character>(path);

            if (character == null) throw new Exception("Error loading character, please make sure you are loading correct prefab!");

			if (_character != null) Destroy(_character.gameObject);

	        _character = Instantiate(character);
	        _character.transform.SetParent(transform);
			_character.transform.localPosition = Vector3.zero;
	        _character.transform.localScale = Vector3.one;

			Debug.LogWarning("All materials were replaced by [Sprites/Default] to avoid outline artefacts.");

	        var mat = new Material(Shader.Find("Sprites/Default"));

			foreach (var spriteRenderer in _character.GetComponentsInChildren<SpriteRenderer>())
	        {
		        if (spriteRenderer.name != "Eyes")
		        {
			        spriteRenderer.material = mat;
		        }
	        }
        }

        /// <summary>
        /// Create sprite sheet
        /// </summary>
        public void Capture()
        {
			if (_character == null) throw new Exception("Please load character first!");
			if (UpperAnimationDropdown.value == 0 && LowerAnimationDropdown.value == 0) throw new Exception("[Any] + [Any] animation is not supported!");

			var frameSize = new[] { 256, 512, 1024 }[FrameSizeDropdown.value];
            var frameRatio = FrameRatioDropdown.value + 4;
            var screenshotInterval = new[] { 0.1f, 0.25f, 0.5f, 1f }[ScreenshotIntervalDropdown.value];

	        Camera.GetComponent<Camera>().orthographicSize = CameraSizeSlider.value;
	        Shadow.SetActive(ShadowDropdown.value == 1);

			var upperClips = UpperAnimationDropdown.value == 0 ? UpperBodyClipNames : new List<string> { UpperBodyClipNames[UpperAnimationDropdown.value - 1] };
	        var lowerClips = LowerAnimationDropdown.value == 0 ? LowerBodyClipNames : new List<string> { LowerBodyClipNames[LowerAnimationDropdown.value - 1] };

	        StartCoroutine(CaptureFrames(upperClips, lowerClips, frameSize, frameRatio, screenshotInterval));
        }

        private void ShowFrame(string upperClip, string lowerClip, float normalizedTime)
        {
            if (upperClip == null)
            {
                _character.Animator.SetBool(lowerClip, true);
            }
            else
            {
                _character.Animator.Play(upperClip, 0, normalizedTime);
            }

	        _character.Animator.Play(lowerClip, 1, normalizedTime);
			_character.Animator.speed = 0;

	        if (_character.Animator.IsInTransition(1))
	        {
				Debug.Log("IsInTransition");
	        }
		}

        private IEnumerator CaptureFrames(List<string> upperClips, List<string> lowerClips, int frameSize, int frameRatio, float screenshotInterval)
        {
            Canvas.SetActive(false);

            var folderName = FolderName.text;
            var death = lowerClips.Any(i => i.Contains("Die"));

            foreach (var upperClip in upperClips)
            {
	            foreach (var lowerClip in lowerClips)
	            {
                    for (var i = 0; i < frameRatio; i++)
                    {
                        ShowFrame(death ? null : upperClip, lowerClip, (float) i / (frameRatio - 1));

			            yield return new WaitForSeconds(screenshotInterval);

			            _screenshotTransparent.Width = _screenshotTransparent.Height = frameSize;

                        if (death)
                        {
                            _screenshotTransparent.Capture($"{Application.dataPath.Replace("/Assets", null)}/SpriteSheets/{folderName}/{lowerClip}/{i}.png");
                        }
                        else
                        {
                            _screenshotTransparent.Capture($"{Application.dataPath.Replace("/Assets", null)}/SpriteSheets/{folderName}/{upperClip}-{lowerClip}/{i}.png");
                        }
		            }
	            }

                if (death) break;
            }

            Canvas.SetActive(true);
        }

        #endif
    }
}