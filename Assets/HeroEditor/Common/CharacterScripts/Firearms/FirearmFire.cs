using System.Collections;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts.Firearms.Enums;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts.Firearms
{
    /// <summary>
    /// Firearm fire process.
    /// </summary>
    public class FirearmFire : MonoBehaviour
    {
        public Character Character;
        public Firearm Firearm;
        public FingerTrigger Finger;
        public Transform Slide;
        public Transform Forestock;
        public Transform ArmL;
        public Transform ArmR;
        public GameObject Shell;
        public ParticleSystem FireMuzzle;
        public bool CreateBullets = true;

        private bool _fire;
        private float _fireTime;
        
        /// <summary>
        /// Should be set outside (by input manager or AI).
        /// </summary>
        [HideInInspector] public bool FireButtonDown;
        [HideInInspector] public bool FireButtonPressed;
        [HideInInspector] public bool FireButtonUp;

        public void Update()
        {
			if (Character.WeaponType != WeaponType.Firearms1H && Character.WeaponType != WeaponType.Firearms2H) return;
            if (Firearm.Params == null) return;

            if (Firearm.Params.AutomaticFire ? FireButtonPressed : FireButtonDown)
            {
                StartCoroutine(Fire());
            }
            else if (FireButtonUp)
            {
                Finger.Pressed = AngryFace = false;
            }
        }

        public void CreateShell()
        {
            Instantiate(Shell, Shell.transform.position, Shell.transform.rotation, Shell.transform.parent).SetActive(true);
        }

	    public void SetLamp(Color color)
	    {
		    Slide.GetComponent<SpriteRenderer>().color = color;
		}

        private IEnumerator Fire()
        {
            if (_fire || Time.time - _fireTime < 60f / Firearm.Params.FireRateInMinute || Firearm.Reload.Reloading) yield break;

            if (Firearm.AmmoShooted == Firearm.Params.MagazineCapacity)
            {
                Finger.Pressed = AngryFace = false;
                Slide.localPosition = Vector3.zero;
                yield return StartCoroutine(Firearm.Reload.Reload());
                Finger.Pressed = AngryFace = FireButtonPressed;
                yield break; 
            }

            if (FireButtonDown)
            {
                Finger.Pressed = AngryFace = true;
            }

            _fire = true;
            _fireTime = Time.time;

            Firearm.AmmoShooted++;
            CreateBullet();
            FireMuzzlePlay();
            GetComponent<AudioSource>().PlayOneShot(Firearm.Params.SoundFire, 0.5f);
            
            var offset = -Firearm.Params.Recoil * ArmR.parent.InverseTransformDirection(Firearm.FireTransform.right);

            StartCoroutine(AnimateOffset(ArmR, offset, ArmR.localPosition, spring: true));
            
            if (!Firearm.Params.AutomaticLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }

	        switch (Firearm.Params.LoadType)
	        {
				case FirearmLoadType.Bolt:
					StartCoroutine(ShellOut(Time.time));
					break;
				case FirearmLoadType.Drum:
					StartCoroutine(RotateRevolverDrum());
					break;
				case FirearmLoadType.Lamp:
					StartCoroutine(BlinkLamp());
					break;
	        }

            if (!Firearm.Params.AutomaticLoad)
            {
                Character.Animator.CrossFade(Firearm.Params.LoadAnimation.name, 0.1f, 0);
            }
        }

        private void FireMuzzlePlay()
        {
            if (FireMuzzle != null && FireMuzzle.name != Firearm.Params.FireMuzzlePrefab.name)
            {
                Destroy(FireMuzzle.gameObject);
                FireMuzzle = null;
            }

            if (FireMuzzle == null)
            {
                FireMuzzle = Instantiate(Firearm.Params.FireMuzzlePrefab, Firearm.FireTransform);
                FireMuzzle.name = Firearm.Params.FireMuzzlePrefab.name;
                FireMuzzle.transform.localPosition = Vector3.zero;
                FireMuzzle.transform.localRotation = Quaternion.Euler(0, -90, 0);

                var rifleSortingOrder = Character.FirearmsRenderers.Single(i => i.name == "Rifle").sortingOrder;

                foreach (var fx in Firearm.FireTransform.GetComponentsInChildren<Renderer>())
                {
                    fx.sortingOrder = rifleSortingOrder - 1;
                }
            }

            FireMuzzle.Play(true);
        }

        private IEnumerator ShellOut(float startTime)
        {
            CreateShell();

            var state = 0f;
            var duration = Firearm.Params.AutomaticLoad ? 60f / Mathf.Max(600, Firearm.Params.FireRateInMinute) : Firearm.Params.LoadAnimation.length;

            while (state < 1)
            {
                state = (Time.time - startTime) / duration;

                if (state <= 1)
                {
                    Slide.localPosition = Vector3.zero - 0.2f * new Vector3(Mathf.Sin(state * Mathf.PI), 0);
                    yield return null;
                }
                else
                {
                    _fire = false;

                    if (Firearm.Params.AutomaticFire && FireButtonPressed) break;

                    Slide.localPosition = Vector3.zero;

                    break;
                }
            }
        }

        private void CreateBullet() // TODO: Preload and caching prefabs is recommended to improve game performance
        {
            if (!CreateBullets) return;

	        var iterations = 1;

	        if (Firearm.Params.Type == FirearmType.Shotgun)
	        {
		        var meta = Firearm.Params.MetaAsDictionary;

		        if (meta.ContainsKey("Spread"))
		        {
			        iterations = int.Parse(Firearm.Params.MetaAsDictionary["Spread"]);
				}
		        else
		        {
					Debug.LogWarningFormat("Please add meta to SpriteCollection for {0}: 'Spread=N', where N is bullet fraction.", Firearm.Params.Name);
			        iterations = 1;
		        }
	        }

            for (var i = 0; i < iterations; i++)
            {
                var bullet = Instantiate(Firearm.Params.ProjectilePrefab, Firearm.FireTransform);
                var spread = Firearm.FireTransform.up * Random.Range(-1f, 1f) * (1 - Firearm.Params.Accuracy);

                bullet.transform.localPosition = Vector3.zero;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.transform.SetParent(null);
                bullet.GetComponent<SpriteRenderer>().sprite = Character.Firearms.Single(j => j.name == "Bullet");
                bullet.GetComponent<Rigidbody>().velocity = Firearm.Params.MuzzleVelocity * (Firearm.FireTransform.right + spread)
                    * Mathf.Sign(Character.transform.lossyScale.x) * Random.Range(0.85f, 1.15f);

                var sortingOrder = Character.FirearmsRenderers.Single(j => j.name == "Rifle").sortingOrder;

                /*
                foreach (var r in bullet.Renderers)
                {
                    r.sortingOrder = sortingOrder;
                }
                */

                var ignoreCollider = Character.GetComponent<Collider>();

                if (ignoreCollider != null)
                {
                    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), ignoreCollider);
                }

                bullet.gameObject.layer = 31; // TODO: Create layer in your project and disable collision for it (in psysics settings)
                Physics.IgnoreLayerCollision(31, 31, true);
            }
        }

        private IEnumerator RotateRevolverDrum()
        {
            var duration = 60f / Mathf.Max(600, Firearm.Params.FireRateInMinute);

            Slide.GetComponent<SpriteRenderer>().sprite = Character.Firearms[7];

            yield return new WaitForSeconds(duration / 2);

            Slide.GetComponent<SpriteRenderer>().sprite = Character.Firearms[6];

            _fire = false;
        }

	    private IEnumerator BlinkLamp()
	    {
		    var duration = 60f / Mathf.Max(600, Firearm.Params.FireRateInMinute);

			SetLamp(Firearm.Params.GetColorFromMeta("LampFire"));

		    yield return new WaitForSeconds(duration);

		    SetLamp(Firearm.Params.GetColorFromMeta("LampReady"));

			_fire = false;
		}

		private bool AngryFace
        {
            set { Character.SetExpression(value ? "Angry" : "Default"); }
        }

        private static IEnumerator AnimateOffset(Transform target, Vector3 offset, Vector3 origin, bool spring = false, float duration = 0.05f)
        {
            var state = 0f;
            var startTime = Time.time;

            while (state < 1)
            {
                state = (Time.time - startTime) / duration;

                if (state <= 1)
                {
                    target.localPosition = origin + offset * (spring ? Mathf.Sin(state * Mathf.PI) : state);
                    yield return null;
                }
                else
                {
                    target.localPosition = spring ? origin : origin + offset;
                    break;
                }
            }
        }
    }
}