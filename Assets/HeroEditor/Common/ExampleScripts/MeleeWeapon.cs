using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

namespace Assets.HeroEditor.Common.ExampleScripts
{
    /// <summary>
    /// General melee weapon behaviour.
    /// First thing you need to check is hit event. Use animation events or check user input.
    /// Second thing is to resolve impacts to other objects (enemies, environment). Use collisions or raycasts.
    /// </summary>
    public class MeleeWeapon : MonoBehaviour
    {
        public AnimationEvents AnimationEvents;
        public Transform Edge;

        /// <summary>
        /// Listen animation events to determine hit moments.
        /// </summary>
        public void Start()
        {
            AnimationEvents.OnCustomEvent += OnAnimationEvent;
        }

        public void OnDestroy()
        {
            AnimationEvents.OnCustomEvent -= OnAnimationEvent;
        }

        private void OnAnimationEvent(string eventName)
        {
            switch (eventName)
            {
                case "Hit":
                    // Place hit behaviour here. For example, you could check/raycast collisons here.
                    break;
                default: return;
            }
        }
    }
}