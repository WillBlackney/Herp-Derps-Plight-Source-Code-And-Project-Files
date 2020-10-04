#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class CardDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Card Data")]
    private static void OpenWindow()
    {
        GetWindow<CardDataEditor>().Show();
    }

    private CreateNewCardData createNewCardData;
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

        createNewCardData = new CreateNewCardData();
        tree.Add("Create New", new CreateNewCardData());
        tree.AddAllAssetsAtPath("Card Data", "Assets/SO Assets/Cards", typeof(CardDataSO));
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
                CardDataSO asset = selected.SelectedValue as CardDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }

    public class CreateNewCardData
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public CardDataSO cardData;

        public CreateNewCardData()
        {
            cardData = CreateInstance<CardDataSO>();
            cardData.cardName = "New Card Name";
        }

        [Button("Add New CardDataSO")]
        public void CreateNewData()
        {
            AssetDatabase.CreateAsset(cardData, "Assets/SO Assets/Cards/" + cardData.cardName + ".asset");
            AssetDatabase.SaveAssets();

            // Create the SO 
            cardData = CreateInstance<CardDataSO>();
            cardData.cardName = "New Card Data";
        }

    }

}
#endif