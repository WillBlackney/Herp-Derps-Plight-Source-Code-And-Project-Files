using System;
using UnityEngine;

namespace Assets.HeroEditor.Common.EditorScripts
{
	/// <summary>
	/// Take a screenshot in play mode [S].
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class ScreenshotTransparent : MonoBehaviour
	{
		public int Width = 1920;
		public int Height = 1280;
		public string Directory = "Screenshots";

		public string GetPath()
		{
			return $"{Directory}/ScreenshotTransparent_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				Capture(GetPath());
			}
		}

		public void Capture(string path)
		{
			var texture = Capture();
			var bytes = texture.EncodeToPNG();
			var directoryInfo = new System.IO.FileInfo(path).Directory;

			if (directoryInfo == null) return;

			directoryInfo.Create();
			System.IO.File.WriteAllBytes(path, bytes);
			Debug.Log($"Screenshot saved: {path}");
		}

		public Texture2D Capture()
		{
			var cam = GetComponent<Camera>();
			var renderTexture = new RenderTexture(Width, Height, 24);
			var texture2D = new Texture2D(Width, Height, TextureFormat.ARGB32, false);

			cam.targetTexture = renderTexture;
			cam.Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
			cam.targetTexture = null;
			RenderTexture.active = null;
			Destroy(renderTexture);

			return texture2D;
		}
	}
}