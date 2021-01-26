using Assets.HeroEditor.Common.CharacterScripts;
using UnityEditor;
using UnityEngine;

namespace Assets.HeroEditor.Common.Editor
{
    /// <summary>
    /// Add action buttons to LayerManager script
    /// </summary>
    [CustomEditor(typeof(LayerManager))]
    public class LayerManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (LayerManager) target;

            EditorGUILayout.LabelField("By Sorting Order", EditorStyles.boldLabel);

            if (GUILayout.Button("Read Current Order"))
            {
                script.ReadCurrentOrderBySortingOrder();
            }

            if (GUILayout.Button("Set Order"))
            {
                script.SetOrderBySortingOrder();
            }

            EditorGUILayout.LabelField("By Z Coortidate", EditorStyles.boldLabel);

            if (GUILayout.Button("Read Current Order"))
            {
                script.ReadCurrentOrderByZCoortidate();
            }

            if (GUILayout.Button("Set Order"))
            {
                script.SetOrderByZCoordinate();
            }
        }
    }
}