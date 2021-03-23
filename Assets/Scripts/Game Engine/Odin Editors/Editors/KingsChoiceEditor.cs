#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class KingsChoiceEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Kings Choice Data")]
    private static void OpenWindow()
    {
        GetWindow<KingsChoiceEditor>().Show();
    }

    private CreateNewKingsChoiceData createNewChoiceData;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewChoiceData != null)
        {
            DestroyImmediate(createNewChoiceData.choiceData);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createNewChoiceData = new CreateNewKingsChoiceData();
        tree.Add("Create New", new CreateNewKingsChoiceData());
        tree.AddAllAssetsAtPath("Kings Choice Data", "Assets/SO Assets/King Choices", typeof(KingChoiceDataSO));
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
                KingChoiceDataSO asset = selected.SelectedValue as KingChoiceDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewKingsChoiceData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public KingChoiceDataSO choiceData;

        public CreateNewKingsChoiceData()
        {
            choiceData = CreateInstance<KingChoiceDataSO>();
        }

        [Button("Add New KingChoiceDataSO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(choiceData, "Assets/SO Assets/King Choices/" + choiceData.choiceDescription + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            choiceData = CreateInstance<KingChoiceDataSO>();
        }

    }

}
#endif
