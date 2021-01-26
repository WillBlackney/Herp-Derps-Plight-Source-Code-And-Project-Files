using System.Collections;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts.Firearms
{
    /// <summary>
    /// Firearm reload process.
    /// </summary>
    public class FirearmReload : MonoBehaviour
    {
        public Character Character;
        public Firearm Firearm;
        public AudioSource AudioSource;

        /// <summary>
        /// Should be set outside (by input manager or AI).
        /// </summary>
        [HideInInspector] public bool ReloadButtonDown;
        [HideInInspector] public bool Reloading;

        public void Update()
        {
            if (ReloadButtonDown && !Reloading && Firearm.AmmoShooted > 0)
            {
                StartCoroutine(Reload());
            }
        }

        public IEnumerator Reload()
        {
            var clip = Firearm.Params.ReloadAnimation;
            var duration = Firearm.Params.MagazineType == MagazineType.Removable ? clip.length : clip.length * Firearm.AmmoShooted;

            Reloading = true;
            Character.Animator.SetBool("Reloading", true);

            switch (Firearm.Params.LoadType)
            {
	            case FirearmLoadType.Drum:
		            for (var i = 0; i < Firearm.AmmoShooted; i++)
		            {
			            Firearm.Fire.CreateShell();
		            }

		            break;
	            case FirearmLoadType.Lamp:
		            Firearm.Fire.SetLamp(Firearm.Params.GetColorFromMeta("LampReload"));
					break;
            }

            yield return new WaitForSeconds(duration);

            if (Firearm.Params.LoadType == FirearmLoadType.Lamp)
	        {
		        Firearm.Fire.SetLamp(Firearm.Params.GetColorFromMeta("LampReady"));
			}

            Firearm.AmmoShooted = 0;
            Character.Animator.SetBool("Reloading", false);
            Character.Animator.SetInteger("HoldType", (int) Firearm.Params.HoldType);
            Reloading = false;
        }

        public void PlayAudioEffect()
        {
            AudioSource.Play();
        }
    }
}