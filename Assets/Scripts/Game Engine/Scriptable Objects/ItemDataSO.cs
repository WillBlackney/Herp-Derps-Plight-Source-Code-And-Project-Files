using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New ItemDataSO", menuName = "ItemDataSO", order = 52)]
public class ItemDataSO : ScriptableObject
{
    [HorizontalGroup("General Info", 75)]
    [HideLabel]
    [PreviewField(75)]
    public Sprite itemSprite;

    [VerticalGroup("General Info/Stats")]
    [LabelWidth(100)]
    public string itemName;
    [VerticalGroup("General Info/Stats")]
    [LabelWidth(100)]
    public ItemType itemType;
    [VerticalGroup("General Info/Stats")]
    [LabelWidth(100)]
    public Rarity itemRarity;
    [VerticalGroup("General Info/Stats")]
    [LabelWidth(100)]
    public bool lootable;
    [VerticalGroup("General Info/Stats")]
    [LabelWidth(100)]
    public bool includeInLibrary = true;

    [BoxGroup("Item Effects Info", true, true)]
    [LabelWidth(100)]
    public List<PassivePairingData> passivePairings;

    [BoxGroup("Item Effects Info", true, true)]
    [LabelWidth(100)]
    public List<ItemEffect> itemEffects;

    [VerticalGroup("Description Info")]
    [LabelWidth(200)]
    public List<CustomString> customDescription;
    [VerticalGroup("Description Info")]
    [LabelWidth(200)]
    public List<KeyWordModel> keyWordModels;
}

[Serializable]
public class ItemEffect
{
    public ItemEffectType effect;
    [ShowIf("ShowAttributeBonus")]
    public CoreAttribute attribute;
    [ShowIf("ShowAttributeBonus")]
    public int attributeBonus;
    [ShowIf("ShowStartingBlockBonus")]
    public int startingBlockBonus;

    public bool ShowAttributeBonus()
    {
        return effect == ItemEffectType.ModifyCoreAttribute;
    }
    public bool ShowStartingBlockBonus()
    {
        return effect == ItemEffectType.ModifyStartingBlock;
    }

}
public enum ItemEffectType
{
    None = 0,
    ModifyCoreAttribute = 1,
    ModifyStartingBlock = 2,
}