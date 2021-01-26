using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

namespace Assets.HeroEditor.Common.ExampleScripts
{
	/// <summary>
	/// Playing different animations example. For full list of animation params and states please open Animator window and select Human.controller.
	/// </summary>
	public class PlayingAnimations : MonoBehaviour
	{
		public Character Character;

		public void Reset()
		{
			Character.ResetAnimation();
		}

        public void GetReady()
        {
            Character.GetReady();
        }

        public void Relax()
        {
            Character.Relax();
        }

		public void Idle()
		{
			Character.SetState(CharacterState.Idle);
		}

		public void Walk()
		{
            Character.SetState(CharacterState.Walk);
		}

		public void Run()
		{
            Character.SetState(CharacterState.Run);
		}

		public void Jump()
		{
            Character.SetState(CharacterState.Jump);
		}

		public void Slash()
		{
			Character.Slash();
		}

		public void Jab()
		{
			Character.Jab();
		}

        public void Die()
        {
			Character.SetState(CharacterState.DeathB);
		}
	}
}