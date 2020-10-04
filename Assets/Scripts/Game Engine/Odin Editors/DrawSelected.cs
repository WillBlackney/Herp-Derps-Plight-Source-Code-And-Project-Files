#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace CustomOdinGUI
{
    public class DrawSelected<T> where T : ScriptableObject
    {
        // This class is responsible for drawing editor windows
        // that allow for the creation and modification of
        // scriptable objects from the editor/inspector
        // Generic T value represents the type of SO
        // that is drawn

        // The SO selected in the menu tree
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public T selected;

        [LabelWidth(100)]
        [PropertyOrder(-1)]
        [ColorFoldoutGroup("CreateNew", 1f, 1f, 1f)]
        [HorizontalGroup("CreateNew/Horizontal")]
        public string nameForNew;

        // Directory path for saving the new SO
        private string path;

        [HorizontalGroup("CreateNew/Horizontal")]
        [GUIColor(0.5f, 1f, 0.5f)]
        [Button]
        public void CreateNew()
        {
            // Prevent creation of unnamed scriptable objects
            if (nameForNew == "")
            {
                return;
            }

            // Create SO and assign its name
            T newItem = ScriptableObject.CreateInstance<T>();
            newItem.name = "New " + typeof(T).ToString();

            // if the path was empty, just place the new SO
            // in the root asset folder
            if (path == "")
            {
                path = "Assets/";
            }

            // Create the new asset and save
            AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
            AssetDatabase.SaveAssets();

            // Reset the name field: sets up the window for the next asset creation
            nameForNew = "";
        }

        [HorizontalGroup("CreateNew/Horizontal")]
        [GUIColor(1f, 0f, 0f)]
        [Button]
        public void DeleteSelected()
        {
            // make sure something is actually selected before 
            // trying to delete
            if (selected != null)
            {
                string _path = AssetDatabase.GetAssetPath(selected);
                AssetDatabase.DeleteAsset(_path);
                AssetDatabase.SaveAssets();
            }
        }

        public void SetSelected(object item)
        {
            // Check if object type is correct first because
            // Menu tree may not always have the correct type of object in it
            // (this can occur when using the enum toggle buttons to
            // change what objects are displayed)

            var attempt = item as T;
            if (attempt != null)
            {
                this.selected = attempt;
            }
        }

        // Used for setting up the directory path
        // that an SO is saved to.
        public void SetPath(string _path)
        {
            this.path = _path;
        }
    }
}

#endif