using Assets.HeroEditor.Common.Data;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts.Firearms
{
    /// <summary>
    /// Firearm manager, contains all links to child components.
    /// </summary>
    public class Firearm : MonoBehaviour
    {
        public AnimationEvents AnimationEvents;

        [Header("Params")]
        public FirearmParams Params;

        [Header("Pivots")]
        public Transform SlideTransform;
        public Transform MagazineTransform;
        public Transform FireTransform;

        [Header("Components")]
        public FirearmFire Fire;
        public FirearmReload Reload;

        [HideInInspector] public int AmmoShooted;

        public void Start()
        {
            if (AnimationEvents != null) AnimationEvents.OnCustomEvent += OnAnimationEvent;
        }

        public void OnDestroy()
        {
            if (AnimationEvents != null) AnimationEvents.OnCustomEvent -= OnAnimationEvent;
        }

        private void OnAnimationEvent(string eventName)
        {
            AudioClip clip;

            switch (eventName)
            {
                case "ClipOut": clip = Params.SoundClipOut; break;
                case "ClipIn": clip = Params.SoundClipIn; break;
                case "Load": clip = Params.SoundLoad; break;
                case "Pump": clip = Params.SoundPump; break;
                default: return;
            }

            GetComponent<AudioSource>().PlayOneShot(clip, 0.5f);
        }
    }
}