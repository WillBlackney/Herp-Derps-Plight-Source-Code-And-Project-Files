using UnityEngine;
using UnityEditor;

static class CharacterUnityIntegration {

	[MenuItem("Assets/Create/CharacterAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<CharacterAsset>();
	}

}
