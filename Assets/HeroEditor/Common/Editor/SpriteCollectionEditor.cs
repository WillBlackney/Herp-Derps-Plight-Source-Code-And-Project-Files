using HeroEditor.Common;
using HeroEditor.Common.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.HeroEditor.Common.Editor
{
    /// <summary>
    /// Add "Refresh" button to SpriteCollection script
    /// </summary>
    [CustomEditor(typeof(SpriteCollection))]
    public class SpriteCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var spriteCollection = (SpriteCollection) target;

            if (GUILayout.Button("Refresh"))
            {
	            Debug.ClearDeveloperConsole();
				SpriteCollectionRefresh.Refresh(spriteCollection);
            }
        }
    }
}