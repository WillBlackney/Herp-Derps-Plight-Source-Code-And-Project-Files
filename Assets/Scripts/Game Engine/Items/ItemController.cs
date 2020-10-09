using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : Singleton<ItemController>
{
    // Variables + Properties
    #region
    [Header("Item Library Properties")]
    [SerializeField] private ItemDataSO[] allItemScriptableObjects;
    private ItemData[] allItems;
    #endregion

    // Getters
    #region
    public ItemData[] AllItems
    {
        get { return allItems; }
        private set { allItems = value; }
    }
    #endregion

    // Library Logic
    #region
    private void Start()
    {
        BuildItemLibrary();
    }
    private void BuildItemLibrary()
    {
        List<ItemData> tempList = new List<ItemData>();

        foreach (ItemDataSO dataSO in allItemScriptableObjects)
        {
            tempList.Add(BuildItemDataFromScriptableObjectData(dataSO));
        }

        AllItems = tempList.ToArray();
    }
    public ItemData BuildItemDataFromScriptableObjectData(ItemDataSO data)
    {
        ItemData i = new ItemData();
        i.itemSprite = data.itemSprite;
        i.itemName = data.itemName;
        i.itemType = data.itemType;
        i.itemRarity = data.itemRarity;
        i.passivePairings = data.passivePairings;

        return i;
    }
    public ItemData GetItemDataByName(string name)
    {
        ItemData itemReturned = null;

        foreach (ItemData icon in AllItems)
        {
            if (icon.itemName == name)
            {
                itemReturned = icon;
                break;
            }
        }

        if (itemReturned == null)
        {
            Debug.Log("ItemController.GetItemDataByName() could not find a item SO with the name " +
                name + ", returning null...");
        }

        return itemReturned;
    }
    #endregion

    // Conditional Checks
    #region
    public bool IsDualWielding(ItemManagerModel iManager)
    {
        bool boolReturned = false;

        if(iManager.mainHandItem != null &&
            iManager.offHandItem != null &&
            iManager.offHandItem.itemType == ItemType.OneHandMelee)
        {
            boolReturned = true;
        }

        return boolReturned;
    }
    public bool IsTwoHanding(ItemManagerModel iManager)
    {
        bool boolReturned = false;

        if (iManager.mainHandItem != null &&
            iManager.mainHandItem.itemType == ItemType.TwoHandMelee)
        {
            boolReturned = true;
        }

        return boolReturned;
    }
    public bool IsRanged(ItemManagerModel iManager)
    {
        bool boolReturned = false;

        if (iManager.mainHandItem != null &&
            iManager.mainHandItem.itemType == ItemType.TwoHandRanged)
        {
            boolReturned = true;
        }

        return boolReturned;
    }
    public bool IsShielded(ItemManagerModel iManager)
    {
        bool boolReturned = false;

        if (iManager.offHandItem != null &&
            iManager.offHandItem.itemType == ItemType.Shield)
        {
            boolReturned = true;
        }

        return boolReturned;
    }
    #endregion

    // Character Entity Logic
    #region
    public void RunItemSetupOnCharacterEntityFromItemManagerData(CharacterEntityModel character, ItemManagerModel iManagerData)
    {
        Debug.Log("ItemController.RunItemSetupOnCharacterEntityFromItemManagerData() called on character: " + character.myName);

        character.iManager = new ItemManagerModel();
        CopyItemManagerDataIntoOtherItemManager(iManagerData, character.iManager);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(character.iManager, character.characterEntityView.ucm);

        if (character.iManager.mainHandItem != null)
        {
            ApplyItemEffectsToCharacterEntity(character, character.iManager.mainHandItem);
        }
        if (character.iManager.offHandItem != null)
        {
            ApplyItemEffectsToCharacterEntity(character, character.iManager.offHandItem);
        }

    }
    public void CopyItemManagerDataIntoOtherItemManager(ItemManagerModel originalData, ItemManagerModel clone)
    {
        if(originalData == null)
        {
            Debug.Log("CopyItemManagerDataIntoOtherItemManager ORIGINAL IS NULL!");
        }
        if (clone == null)
        {
            Debug.Log("CopyItemManagerDataIntoOtherItemManager CLONE DATA IS NULL!");
        }

        clone.mainHandItem = originalData.mainHandItem;
        clone.offHandItem = originalData.offHandItem;
    }   
    private void ApplyItemEffectsToCharacterEntity(CharacterEntityModel character, ItemData item)
    {
        foreach(PassivePairingData passive in item.passivePairings)
        {
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(character.pManager, passive.passiveData.passiveName, passive.passiveStacks, false);
        }
    }
    public void CopySerializedItemManagerIntoStandardItemManager(SerializedItemManagerModel data, ItemManagerModel iManager)
    {
        if(data.mainHandItem != null)
        {
            iManager.mainHandItem = GetItemDataByName(data.mainHandItem.itemName);
        }
      
        if (data.offHandItem != null)
        {
            iManager.offHandItem = GetItemDataByName(data.offHandItem.itemName);
        }
      
    }
    #endregion
}
