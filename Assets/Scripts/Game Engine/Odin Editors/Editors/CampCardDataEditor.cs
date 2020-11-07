#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class CampCardDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Camp Card Data")]
    private static void OpenWindow()
    {
        GetWindow<CampCardDataEditor>().Show();
    }

    private CreateNewCampCardData createNewCardData;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewCardData != null)
        {
            DestroyImmediate(createNewCardData.cardData);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createNewCardData = new CreateNewCampCardData();
        tree.Add("Create New", new CreateNewCampCardData());
        tree.AddAllAssetsAtPath("Camp Card Data", "Assets/SO Assets/Camp Cards", typeof(CampCardDataSO));
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
                CampCardDataSO asset = selected.SelectedValue as CampCardDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewCampCardData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public CampCardDataSO cardData;

        public CreateNewCampCardData()
        {
            cardData = CreateInstance<CampCardDataSO>();
            cardData.cardName = "New Camp Card Name";
        }

        [Button("Add New CampCardDataSO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(cardData, "Assets/SO Assets/Camp Cards/" + cardData.cardName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            cardData = CreateInstance<CampCardDataSO>();
            cardData.cardName = "New Camp Card Data";
        }

    }

}
#endif