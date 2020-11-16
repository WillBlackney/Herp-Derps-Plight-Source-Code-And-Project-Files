using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CampCardEffect
{
    [LabelWidth(200)]
    public CampCardEffectType cardEffectType;

    // Healing properties
    [ShowIf("ShowHealingType")]
    [LabelWidth(200)]
    public HealingType healingType;

    [ShowIf("ShowFlatHealAmount")]
    [LabelWidth(200)]
    public int flatHealAmount;

    [ShowIf("ShowHealAmountPercentage")]
    [LabelWidth(200)]
    [Range(0f, 1f)]
    public float healAmountPercentage;

    // Core Attribute properties
    [ShowIf("ShowAttributeProperties")]
    [LabelWidth(200)]
    public CoreAttribute attributeChanged;

    [ShowIf("ShowAttributeProperties")]
    [LabelWidth(200)]
    public int attributeAmountGained;

    // Passive properties
    [ShowIf("ShowPassivePairing")]
    [LabelWidth(200)]
    public PassivePairingData passivePairing;

    // Card draw properites
    [ShowIf("ShowCardsDrawn")]
    [LabelWidth(200)]
    public int cardsDrawn;

    // Max health gains
    [ShowIf("ShowMaxHealthGained")]
    [LabelWidth(200)]
    public int maxHealthGained;


    // Visual event logic
    public List<AnimationEventData> visualEventsOnStart;


    // Healing Show Ifs
    public bool ShowHealingType()
    {
        return cardEffectType == CampCardEffectType.Heal ||
            cardEffectType == CampCardEffectType.HealAllCharacters; 
    }
    public bool ShowFlatHealAmount()
    {
        return healingType == HealingType.FlatAmount && (cardEffectType == CampCardEffectType.Heal ||
            cardEffectType == CampCardEffectType.HealAllCharacters);
    }
    public bool ShowHealAmountPercentage()
    {
        return healingType == HealingType.PercentageOfMaxHealth && (cardEffectType == CampCardEffectType.Heal ||
            cardEffectType == CampCardEffectType.HealAllCharacters);
    }

    // Card Show Ifs
    public bool ShowCardsDrawn()
    {
        return cardEffectType == CampCardEffectType.DrawCards;
    }
    public bool ShowMaxHealthGained()
    {
        return cardEffectType == CampCardEffectType.IncreaseMaxHealth || 
            cardEffectType == CampCardEffectType.IncreaseMaxHealthAll;
    }

    // Attrbiute Show Ifs
    public bool ShowAttributeProperties()
    {
        return cardEffectType == CampCardEffectType.ModifyCoreAttribute;
    }


    // Passive Show Ifs
    public bool ShowPassivePairing()
    {
        return cardEffectType == CampCardEffectType.ApplyPassive;
    }
}
public enum CampCardEffectType
{
    None = 0,
    Heal = 1,
    HealAllCharacters = 5,
    ApplyPassive = 2,
    ShuffleHandIntoDrawPile =3,
    DrawCards = 4,
    IncreaseMaxHealth = 8,
    IncreaseMaxHealthAll = 7,
    ModifyCoreAttribute = 9,
    UpgradeCard = 10,

}
public enum HealingType
{
    None = 0,
    FlatAmount = 1,
    PercentageOfMaxHealth = 2,
}

