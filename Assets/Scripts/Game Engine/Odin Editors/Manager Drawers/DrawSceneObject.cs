#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace CustomOdinGUI
{
    public class DrawSceneObject<T> where T : MonoBehaviour
    {
        // InlineEditor force window to draw the full class
        // Holds the reference the specific manager game object

        [Title("Title", "Sub Title", TitleAlignments.Centered)]
        [ShowIf("@myObject != null")]
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public T myObject;
        public void FindMyObject()
        {
            // Finds the manager game object in the scene
            if (myObject == null)
            {
                myObject = GameObject.FindObjectOfType<T>();
            }
        }

        // Button group forces manager specific buttons to be grouped together
        // at the top of the editor window
        [ShowIf("@myObject != null")]
        [GUIColor(0.7f, 1f, 0.7f)]
        [ButtonGroup("Top Button", -1000)]
        private void SelectSceneObject()
        {
            if (myObject != null)
            {
                Selection.activeGameObject = myObject.gameObject;
            }
        }

        [ShowIf("@myObject == null")]
        [Button]
        private void CreateManagerObject()
        {
            GameObject newManager = new GameObject();
            newManager.name = "New " + typeof(T).ToString();
            myObject = newManager.AddComponent<T>();
        }
        
    }
}

#endif
