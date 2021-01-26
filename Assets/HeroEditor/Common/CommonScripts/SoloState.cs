using System;
using UnityEngine;

namespace Assets.HeroEditor.Common.CommonScripts
{
    /// <summary>
    /// This script can be added to an animation state that should block transitions to other states via 'Action' parameter.
    /// </summary>
    public class SoloState : StateMachineBehaviour
    {
        public string Name;
        public bool Continuous;
        public bool Active;
        public bool KeepAction;
        public Func<bool> Continue;

        private float _enterTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enterTime = Time.time;
            animator.SetBool("Action", true);
            Active = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime >= 1 && !Continuous)
            {
                Exit(animator, stateInfo);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Exit(animator, stateInfo);
        }

        private void Exit(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!Active || Time.time - _enterTime < stateInfo.length) return;

            Active = false;

            if (Continue == null)
            {
                animator.SetBool("Action", KeepAction);
            }
            else if (Continue != null)
            {
                if (!Continue())
                {
                    animator.SetBool("Action", KeepAction);
                }

                Continue = null;
            }
        }
    }
}