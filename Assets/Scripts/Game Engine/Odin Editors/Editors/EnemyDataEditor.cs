#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;


public class EnemyDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Enemy Data")]
    private static void OpenWindow()
    {
        GetWindow<EnemyDataEditor>().Show();
    }

    private CreateNewEnemyData createNewEnemyData;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(createNewEnemyData != null)
        {
            DestroyImmediate(createNewEnemyData.enemyData);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createNewEnemyData = new CreateNewEnemyData();
        tree.Add("Create New", new CreateNewEnemyData());
        tree.AddAllAssetsAtPath("Enemy Data", "Assets/SO Assets/Enemies", typeof (EnemyDataSO));
        tree.SortMenuItemsByName();
        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if(SirenixEditorGUI.ToolbarButton("Delete Current"))
            {
                EnemyDataSO asset = selected.SelectedValue as EnemyDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewEnemyData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public EnemyDataSO enemyData;

        public CreateNewEnemyData()
        {
            enemyData = CreateInstance<EnemyDataSO>();
            enemyData.enemyName = "New Enemy Data";
        }

        [Button("Add New EnemyDataSO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(enemyData, "Assets/SO Assets/Enemies/" + enemyData.enemyName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            enemyData = CreateInstance<EnemyDataSO>();
            enemyData.enemyName = "New Enemy Data";
        }
     
    }
}

#endif