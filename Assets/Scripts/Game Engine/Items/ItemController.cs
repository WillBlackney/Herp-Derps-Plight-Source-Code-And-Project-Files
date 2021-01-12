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
    protected override void Awake()
    {
        base.Awake();
        BuildItemLibrary();
    }
    private void BuildItemLibrary()
    {
        Debug.LogWarning("ItemController.BuildItemLibrary() called...");

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
        i.lootable = data.lootable;

        // Custom string Data
        i.customDescription = new List<CustomString>();
        foreach (CustomString cs in data.customDescription)
        {
            i.customDescription.Add(ObjectCloner.CloneJSON(cs));
        }

        // Keyword Model Data
        i.keyWordModels = new List<KeyWordModel>();
        foreach (KeyWordModel kwdm in data.keyWordModels)
        {
            i.keyWordModels.Add(ObjectCloner.CloneJSON(kwdm));
        }

        return i;
    }
    public ItemData CloneItem(ItemData data)
    {
        ItemData i = new ItemData();
        i.itemSprite = data.itemSprite;
        i.itemName = data.itemName;
        i.itemType = data.itemType;
        i.itemRarity = data.itemRarity;
        i.passivePairings = data.passivePairings;
        i.lootable = data.lootable;

        // Custom string Data
        i.customDescription = new List<CustomString>();
        foreach (CustomString cs in data.customDescription)
        {
            i.customDescription.Add(ObjectCloner.CloneJSON(cs));
        }

        // Keyword Model Data
        i.keyWordModels = new List<KeyWordModel>();
        foreach (KeyWordModel kwdm in data.keyWordModels)
        {
            i.keyWordModels.Add(ObjectCloner.CloneJSON(kwdm));
        }

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
    public List<ItemData> GetAllLootableItems()
    {
        List<ItemData> lootableItems = new List<ItemData>();

        for(int i = 0; i < AllItems.Length; i++)
        {
            if (AllItems[i].lootable)
                lootableItems.Add(AllItems[i]);
        }
        return lootableItems;
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
        CharacterModelController.Instance.ApplyItemManagerDataToCharacterModelView(character.iManager, character.characterEntityView.ucm);

        if (character.iManager.mainHandItem != null)
        {
            ApplyItemEffectsToCharacterEntity(character, character.iManager.mainHandItem);
        }
        if (character.iManager.offHandItem != null)
        {
            ApplyItemEffectsToCharacterEntity(character, character.iManager.offHandItem);
        }

        if (character.iManager.trinketOne != null)
        {
            ApplyItemEffectsToCharacterEntity(character, character.iManager.trinketOne);
        }
        if (character.iManager.trinketTwo != null)
        {
            ApplyItemEffectsToCharacterEntity(character, character.iManager.trinketTwo);
        }
    }    
    private void ApplyItemEffectsToCharacterEntity(CharacterEntityModel character, ItemData item)
    {
        foreach(PassivePairingData passive in item.passivePairings)
        {
            string passiveName = TextLogic.SplitByCapitals(passive.passiveData.ToString());

            PassiveController.Instance.ModifyPassiveOnCharacterEntity(character.pManager, passiveName, passive.passiveStacks, false);
        }

        // TO DO: apply non passive related effects

    }
    public void CopyItemManagerDataIntoOtherItemManager(ItemManagerModel originalData, ItemManagerModel clone)
    {
        if (originalData == null)
        {
            Debug.Log("CopyItemManagerDataIntoOtherItemManager ORIGINAL IS NULL!");
        }
        if (clone == null)
        {
            Debug.Log("CopyItemManagerDataIntoOtherItemManager CLONE DATA IS NULL!");
        }

        clone.mainHandItem = originalData.mainHandItem;
        clone.offHandItem = originalData.offHandItem;
        clone.trinketOne = originalData.trinketOne;
        clone.trinketTwo = originalData.trinketTwo;
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

        if (data.trinketOne != null)
        {
            iManager.trinketOne = GetItemDataByName(data.trinketOne.itemName);
        }
        if (data.trinketTwo != null)
        {
            iManager.trinketTwo = GetItemDataByName(data.trinketTwo.itemName);
        }

    }
    public void HandleGiveItemToCharacterFromInventory(CharacterData character, ItemData newItem, RosterItemSlot slot)
    {
        Debug.LogWarning("ItemController.HandleGiveItemToCharacterFromInventory() called, character = " +
            character.myName + ", item = " + newItem.itemName + ", slot = " + slot.slotType.ToString());

        ItemData previousItem = slot.itemDataRef;

        // remove new item from inventory
        InventoryController.Instance.RemoveItemFromInventory(newItem);

        if (previousItem != null)
        {
            Debug.LogWarning("Item "+ previousItem.itemName + " already in slot: " + slot.slotType.ToString() + ", returning it to inventory...");
            InventoryController.Instance.AddItemToInventory(previousItem);

            // check 2h logic
            if(newItem.itemType == ItemType.TwoHandMelee || newItem.itemType == ItemType.TwoHandRanged)
            {
                ItemData offhandItem = character.itemManager.offHandItem;
                if(offhandItem != null)
                {
                    InventoryController.Instance.AddItemToInventory(offhandItem);
                    character.itemManager.offHandItem = null;

                }
            }
        }

        // 2H items
        if(newItem.itemType == ItemType.TwoHandMelee || newItem.itemType == ItemType.TwoHandMelee)
        {
            character.itemManager.mainHandItem = newItem;
        }

        // 1h melee items
        if (newItem.itemType == ItemType.OneHandMelee)
        {
            if(slot.slotType == RosterSlotType.MainHand)
                character.itemManager.mainHandItem = newItem;

            if (slot.slotType == RosterSlotType.OffHand)
                character.itemManager.offHandItem = newItem;
        }

        // shields
        if (newItem.itemType == ItemType.Shield)
        {
            character.itemManager.offHandItem = newItem;
        }

        // trinkets
        if (newItem.itemType == ItemType.Trinket)
        {
            if(slot.slotType == RosterSlotType.TrinketOne)
                character.itemManager.trinketOne = newItem;

            if (slot.slotType == RosterSlotType.TrinketTwo)
                character.itemManager.trinketTwo = newItem;
        }


        // check for previous item in slot /  if an item is also being removed
        // if there is one, return it to inventory, remove bonuses of item?

        // apply item to character
    }
    #endregion
}
