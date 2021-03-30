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

    [VerticalGroup("Description Info")]
    [LabelWidth(200)]
    public List<CustomString> customDescription;
    [VerticalGroup("Description Info")]
    [LabelWidth(200)]
    public List<KeyWordModel> keyWordModels;
}
