using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData 
{
    public Sprite itemSprite;
    public string itemName;
    public ItemType itemType;
    public Rarity itemRarity;
    public List<PassivePairingData> passivePairings;
    public List<CustomString> customDescription;
    public List<KeyWordModel> keyWordModels;
    public bool lootable;
}
