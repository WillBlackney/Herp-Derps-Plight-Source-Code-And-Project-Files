﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData 
{
    public string itemName;
    public ItemCategory itemType;
    public Rarity itemRarity;
    public List<PassivePairingData> passivePairings;
    public List<CustomString> customDescription;
    public List<KeyWordModel> keyWordModels;
    public bool lootable;
    private Sprite itemSprite;

    public Sprite ItemSprite
    {
        get
        {
            if(itemSprite == null)
            {
                itemSprite = GetMySprite();
                return itemSprite;
            }
            else
            {
                return itemSprite;
            }
        }
    }

    public Sprite GetMySprite()
    {
        Sprite s = null;

        foreach(ItemDataSO i in ItemController.Instance.AllItemScriptableObjects)
        {
            if(i.itemName == itemName)
            {
                s = i.itemSprite;
                break;
            }
        }

        if(s == null)        
            Debug.LogWarning("ItemData.GetMySprite() could not sprite for item " + itemName + ", returning null...");
        

        return s;
    }
}
