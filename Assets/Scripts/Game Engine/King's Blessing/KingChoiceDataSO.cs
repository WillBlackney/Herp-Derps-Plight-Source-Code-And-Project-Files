using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New KingChoiceDataSO", menuName = "KingChoiceDataSO", order = 52)]
public class KingChoiceDataSO : ScriptableObject
{
    [BoxGroup("General Info", true, true)]
    [LabelWidth(100)]
    public string choiceDescription;

    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public KingChoiceEffectType effect;

    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public KingChoiceEffectCategory category;

    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public KingChoiceImpactLevel impactLevel;

}
public enum KingChoiceImpactLevel
{
    None = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Extreme = 4,
}
public enum KingChoiceEffectCategory
{
    None = 0,
    Benefit = 1,
    Consequence = 2,
}
public enum KingChoiceEffectType
{
    None = 0,
    DiscoverCard = 1,
    GainPassive = 2,
    GainRandomCard = 3,
    GainRandomCurse = 4,
    ModifyAttribute = 5,
    ModifyHealth = 6,
    ModifyMaxHealth = 7,     
    TransformCard = 8,
    TransformRandomCard = 9,
    UpgradeCard = 10,
    UpgradeRandomCards = 11,

}
