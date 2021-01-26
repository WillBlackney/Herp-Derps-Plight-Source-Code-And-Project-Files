using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using UnityEditor;
using UnityEngine;

namespace Assets.HeroEditor.Common.Editor
{
    /// <summary>
    /// Add "Refresh" button to SpriteCollection script
    /// </summary>
    [CustomEditor(typeof(FirearmCollection))]
    public class FirearmCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var firearmCollection = (FirearmCollection) target;

            if (GUILayout.Button("Remove excess"))
            {
	            firearmCollection.RemoveExcess();
            }

            if (GUILayout.Button("Sort by name"))
            {
				firearmCollection.Firearms = firearmCollection.Firearms.OrderBy(i => i.Name).ToList();
			}

	        if (GUILayout.Button("Update names"))
	        {
		        firearmCollection.UpdateNames();
	        }
		}
    }
}