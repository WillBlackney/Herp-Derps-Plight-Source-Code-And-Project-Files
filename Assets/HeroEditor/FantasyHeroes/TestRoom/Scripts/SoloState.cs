using System;
using UnityEngine;

namespace Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts
{
    /// <summary>
    /// This animation script prevents all possible transitions to another states.
    /// </summary>
    public class SoloState : StateMachineBehaviour
    {
        public bool Active;
        public Action Continue;

        private bool _enter;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("Action", true);
            Active = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime >= 1)
            {
                OnStateExit(animator, stateInfo, layerIndex);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!Active) return;

            Active = false;

            if (Continue == null)
            {
                animator.SetBool("Action", false);
            }
            else
            {
                Continue();
                Continue = null;
            }
        }
    }
}