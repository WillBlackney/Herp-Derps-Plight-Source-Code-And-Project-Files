using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.Common.EditorScripts
{
    /// <summary>
    /// Plays animation from character editor. Just for example.
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        public Character Character;
        public Text UpperClipName;
        public Text LowerClipName;

        /// <summary>
        /// Called automatically on app start.
        /// </summary>
        public void Start()
        {
            Character.UpdateAnimation();
            Refresh();
        }

        public void Refresh()
        {
            UpperClipName.text = "Relax / Ready";
            LowerClipName.text = Character.GetState().ToString();
        }

        /// <summary>
        /// Change upper body animation and play it.
        /// </summary>
        public void PlayUpperBodyAnimation(int direction)
        {
            if (Character.IsReady())
            {
                Character.Relax();
            }
            else
            {
                Character.GetReady();
            }

            Refresh();
        }

        /// <summary>
        /// Change lower body animation and play it.
        /// </summary>
        public void PlayLowerBodyAnimation(int direction)
        {
            var state = Character.GetState();

            state += direction;

            if (state < 0)
            {
                state = CharacterState.DeathF;
            }
            else if (state > CharacterState.DeathF)
            {
                state = CharacterState.Idle;
            }

            Character.SetState(state);

            Refresh(); 
        }
    }
}