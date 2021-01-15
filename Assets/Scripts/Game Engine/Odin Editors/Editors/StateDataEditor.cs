#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class StateDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/State Data")]
    private static void OpenWindow()
    {
        GetWindow<StateDataEditor>().Show();
    }

    private CreateNewStateData createnewStateData;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createnewStateData != null)
        {
            DestroyImmediate(createnewStateData.stateData);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createnewStateData = new CreateNewStateData();
        tree.Add("Create New", new CreateNewStateData());
        tree.AddAllAssetsAtPath("State Data", "Assets/SO Assets/States", typeof(StateDataSO));
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
                StateDataSO asset = selected.SelectedValue as StateDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewStateData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public StateDataSO stateData;

        public CreateNewStateData()
        {
            stateData = CreateInstance<StateDataSO>();
            stateData.stateName = "New State Name";
        }

        [Button("Add New StateDataSO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(stateData, "Assets/SO Assets/States/" + stateData.stateName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            stateData = CreateInstance<StateDataSO>();
            stateData.stateName = "New State Data";
        }

    }

}
#endif
