#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class CharacterTemplateEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Character Templates")]
    private static void OpenWindow()
    {
        GetWindow<CharacterTemplateEditor>().Show();
    }

    private CreateNewCharacterTemplate createNewCharacterTemplate;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewCharacterTemplate != null)
        {
            DestroyImmediate(createNewCharacterTemplate.characterTemplate);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createNewCharacterTemplate = new CreateNewCharacterTemplate();
        tree.Add("Create New", new CreateNewCharacterTemplate());
        tree.AddAllAssetsAtPath("Character Template", "Assets/SO Assets/Character Templates", typeof(CharacterTemplateSO));
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
                CharacterTemplateSO asset = selected.SelectedValue as CharacterTemplateSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewCharacterTemplate
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public CharacterTemplateSO characterTemplate;

        public CreateNewCharacterTemplate()
        {
            characterTemplate = CreateInstance<CharacterTemplateSO>();
            characterTemplate.myName = "New Character Name";
        }

        [Button("Add New CharacterTemplateSO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(characterTemplate, "Assets/SO Assets/Character Templates/" + characterTemplate.myName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            characterTemplate = CreateInstance<CharacterTemplateSO>();
            characterTemplate.myName = "New Character Template";
        }

    }

}
#endif