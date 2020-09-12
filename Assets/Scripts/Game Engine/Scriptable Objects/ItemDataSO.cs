using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New ItemDataSO", menuName = "ItemDataSO", order = 52)]
[Serializable]
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

    [BoxGroup("Item Effects Info", true, true)]
    [LabelWidth(100)]
    public List<PassivePairingData> passivePairings;
}
