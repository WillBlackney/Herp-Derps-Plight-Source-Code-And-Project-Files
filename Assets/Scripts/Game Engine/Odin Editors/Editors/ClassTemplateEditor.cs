#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class ClassTemplateEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Class Templates")]
    private static void OpenWindow()
    {
        GetWindow<ClassTemplateEditor>().Show();
    }

    private CreateNewClassTemplate createNewClassTemplate;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewClassTemplate != null)
        {
            DestroyImmediate(createNewClassTemplate.classTemplate);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createNewClassTemplate = new CreateNewClassTemplate();
        tree.Add("Create New", new CreateNewClassTemplate());
        tree.AddAllAssetsAtPath("Character Template", "Assets/SO Assets/Character Generation/Class Templates", typeof(ClassTemplateSO));
        tree.SortMenuItemsByName();
        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if (SirenixEditorGUI.ToolbarButton("Delete Current"))
            {
                ClassTemplateSO asset = selected.SelectedValue as ClassTemplateSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewClassTemplate
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public ClassTemplateSO classTemplate;

        public CreateNewClassTemplate()
        {
            classTemplate = CreateInstance<ClassTemplateSO>();
            classTemplate.templateName = "New Class Name";
        }

        [Button("Add New CharacterTemplateSO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(classTemplate, "Assets/SO Assets/Character Generation/Class Templates/" + classTemplate.templateName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            classTemplate = CreateInstance<ClassTemplateSO>();
            classTemplate.templateName = "New Class Template";
        }

    }

}
#endif
