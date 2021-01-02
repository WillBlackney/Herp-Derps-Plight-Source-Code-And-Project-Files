#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class SummonedCharacterEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Summoned Character")]
    private static void OpenWindow()
    {
        GetWindow<SummonedCharacterEditor>().Show();
    }

    private CreateNewSummonedCharacter createNewSummonedCharacter;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewSummonedCharacter != null)
        {
            DestroyImmediate(createNewSummonedCharacter.character);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createNewSummonedCharacter = new CreateNewSummonedCharacter();
        tree.Add("Create New", new CreateNewSummonedCharacter());
        tree.AddAllAssetsAtPath("Summoned Character", "Assets/SO Assets/Summoned Characters", typeof(SummonedCharacterDataSO));
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
                SummonedCharacterDataSO asset = selected.SelectedValue as SummonedCharacterDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewSummonedCharacter
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SummonedCharacterDataSO character;

        public CreateNewSummonedCharacter()
        {
            character = CreateInstance<SummonedCharacterDataSO>();
            character.myName = "New Character Name";
        }

        [Button("Add New Summoned Character")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(character, "Assets/SO Assets/Summoned Characters/" + character.myName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            character = CreateInstance<SummonedCharacterDataSO>();
            character.myName = "New Character Template";
        }

    }

}
#endif