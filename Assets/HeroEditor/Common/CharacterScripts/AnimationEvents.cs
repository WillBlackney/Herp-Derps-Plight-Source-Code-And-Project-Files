using System;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
	/// <summary>
	/// Animation events. If you want to get animation callback, use it.
	/// For example, if you want to know exact hit moment for attack animation, use custom event 'Hit' that is fired in most attack animations.
	/// </summary>
	public class AnimationEvents : MonoBehaviour
    {
		/// <summary>
		/// Subscribe it to get animation callback.
		/// </summary>
		public event Action<string> OnCustomEvent = s => { };

		/// <summary>
		/// Set bool param, usage example: Idle=false
		/// </summary>
		public void SetBool(string value)
        {
            var parts = value.Split('=');

            GetComponent<Animator>().SetBool(parts[0], bool.Parse(parts[1]));
        }

		/// <summary>
		/// Set integer param, usage example: WeaponType=2
		/// </summary>
		public void SetInteger(string value)
        {
            var parts = value.Split('=');

            GetComponent<Animator>().SetInteger(parts[0], int.Parse(parts[1]));
        }

	    /// <summary>
	    /// Called from animation.
	    /// </summary>
	    public void CustomEvent(string eventName)
	    {
		    OnCustomEvent(eventName);
	    }

	    /// <summary>
	    /// Set characters' expression. Called from animation.
	    /// </summary>
		public void SetExpression(string expression)
	    {
		    transform.parent.GetComponent<Character>().SetExpression(expression);
		}

	    /// <summary>
	    /// Reset animation.
	    /// </summary>
	    public void ResetAnimation()
	    {
			transform.parent.GetComponent<Character>().UpdateAnimation();
	    }
	}
}