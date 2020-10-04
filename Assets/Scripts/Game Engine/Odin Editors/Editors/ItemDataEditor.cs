#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;


public class ItemDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Item Data")]
    private static void OpenWindow()
    {
        GetWindow<ItemDataEditor>().Show();
    }

    private CreateItemData createNewItemData;
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewItemData != null)
        {
            DestroyImmediate(createNewItemData.itemData);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        createNewItemData = new CreateItemData();
        tree.Add("Create New", new CreateItemData());
        tree.AddAllAssetsAtPath("Item Data", "Assets/SO Assets/Items", typeof(ItemDataSO));
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
                ItemDataSO asset = selected.SelectedValue as ItemDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateItemData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public ItemDataSO itemData;

        public CreateItemData()
        {
            itemData = CreateInstance<ItemDataSO>();
            itemData.itemName = "New Item Name";

        }

        [Button("Add New Item Data SO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(itemData, "Assets/SO Assets/Items/" + itemData.itemName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            itemData = CreateInstance<ItemDataSO>();
            itemData.itemName = "New Item Data";
        }

    }

}
#endif