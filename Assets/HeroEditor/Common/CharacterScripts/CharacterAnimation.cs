using System.Collections;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
    public partial class Character
    {
        public void ResetAnimation()
        {
            SetState(CharacterState.Idle);
            UpdateAnimation();
        }

        public void GetReady()
        {
            Animator.SetBool("Ready", true);
        }

        public void Relax()
        {
            Animator.SetBool("Ready", false);
        }

        public bool IsReady()
        {
            return Animator.GetBool("Ready");
        }

        public void SetState(CharacterState state)
        {
            switch (state)
            {
                case CharacterState.Ready:
                    GetReady();
                    state = CharacterState.Idle;
                    break;
                case CharacterState.Relax:
                    Relax();
                    state = CharacterState.Idle;
                    break;
            }

            Animator.SetInteger("State", (int) state);
        }

        public CharacterState GetState()
        {
            return (CharacterState) Animator.GetInteger("State");
        }

        public void Slash()
        {
            Animator.SetTrigger("Slash");
        }

        public void Jab()
        {
            Animator.SetTrigger("Jab");
        }

        public IEnumerator Shoot()
        {
            Animator.SetInteger("Charge", 1); // 0 = ready, 1 = charging, 2 = release, 3 = cancel.

            yield return new WaitForSeconds(1);

            Animator.SetInteger("Charge", 2);

            yield return new WaitForSeconds(1);

            Animator.SetInteger("Charge", 0);
        }
    }
}