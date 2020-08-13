using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace Engine
{
    public class DrawSceneObject<T> where T : MonoBehaviour
    {
        // InlineEditor force window to draw the full class
        // Holds the reference the specific manager game object

        [Title("$MyTitle", "$MySubTitle", TitleAlignments.Centered)]
        [ShowIf("@myObject != null")]
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public T myObject;
      

        // Button group forces manager specific buttons to be grouped together
        // at the top of the editor window
        [ShowIf("@myObject != null")]
        [GUIColor(0.7f, 1f, 0.7f)]
        [ButtonGroup("Top Button", -1000)]
        private void SelectSceneObject()
        {
            if (myObject != null)
            {
                Selection.activeObject = myObject.gameObject;
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

        protected virtual string MyTitle()
        {
            return "Unnamed Title";
        }
        protected virtual string MySubTitle()
        {
            return "Unnamed Subtitle";
        }
        public void FindMyObject()
        {
            // Finds the manager game object in the scene
            if (myObject == null)
            {
                myObject = Object.FindObjectOfType<T>();
            }
        }
    }
}
