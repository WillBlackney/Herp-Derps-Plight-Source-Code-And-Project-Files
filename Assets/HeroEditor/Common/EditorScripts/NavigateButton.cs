using UnityEngine;

namespace Assets.HeroEditor.Common.EditorScripts
{
	/// <summary>
	/// Used to open links.
	/// </summary>
	public class NavigateButton : MonoBehaviour
	{
		public void Navigate(string url)
		{
            #if UNITY_WEBGL && !UNITY_EDITOR

            Application.ExternalEval($"window.open('{url}')");

            #else

			Application.OpenURL(url);

            #endif
		}
	}
}