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


    // Healing Show Ifs
    public bool ShowHealingType()
    {
        return cardEffectType == CampCardEffectType.Heal;
    }
    public bool ShowFlatHealAmount()
    {
        return healingType == HealingType.FlatAmount && cardEffectType == CampCardEffectType.Heal;
    }
    public bool ShowHealAmountPercentage()
    {
        return healingType == HealingType.PercentageOfMaxHealth && cardEffectType == CampCardEffectType.Heal;
    }

}
public enum CampCardEffectType
{
    None = 0,
    Heal = 1,
    Resurrect =2,
    ApplyPassive = 3,

}
public enum HealingType
{
    None = 0,
    FlatAmount = 1,
    PercentageOfMaxHealth = 2,

}
