using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New KingChoiceDataSO", menuName = "KingChoiceDataSO", order = 52)]
public class KingChoiceDataSO : ScriptableObject
{
    [BoxGroup("General Info", true, true)]
    [LabelWidth(100)]
    public List<CustomString> choiceDescription = new List<CustomString>();

    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public KingChoiceEffectType effect;

    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public KingChoiceEffectCategory category;

    [BoxGroup("General Info")]
    [LabelWidth(100)]
    public KingChoiceImpactLevel impactLevel;

    [ShowIf("effect", KingChoiceEffectType.ModifyMaxHealth)]
    public int maxHealthGainedOrLost;

    [ShowIf("effect", KingChoiceEffectType.ModifyHealth)]
    public int healthGainedOrLost;

    [ShowIf("effect", KingChoiceEffectType.GainRandomCard)]
    public Rarity randomCardRarity;

    [ShowIf("effect", KingChoiceEffectType.DiscoverCard)]
    public Rarity discoveryCardRarity;

    [ShowIf("effect", KingChoiceEffectType.UpgradeRandomCards)]
    public int randomCardsUpgraded;

    [ShowIf("effect", KingChoiceEffectType.TransformRandomCard)]
    public int randomCardsTransformed;

    [ShowIf("effect", KingChoiceEffectType.ModifyAttribute)]
    public CoreAttribute attributeModified;

    [ShowIf("effect", KingChoiceEffectType.ModifyAttribute)]
    public int attributeAmountModified;

    [ShowIf("effect", KingChoiceEffectType.GainPassive)]
    public List<PassivePairingData> possiblePassives;

    [ShowIf("effect", KingChoiceEffectType.GainRandomAffliction)]
    public int afflicationsGained;

    [ShowIf("effect", KingChoiceEffectType.ModifyGold)]
    public int goldGainedOrLost;

    [ShowIf("effect", KingChoiceEffectType.GainRandomItem)]
    public Rarity itemRarity;

    [ShowIf("effect", KingChoiceEffectType.GainState)]
    public StateName stateGained;

}
public enum KingChoiceImpactLevel
{
    None = 0,
    Low = 1,
    Medium = 2,
    High = 3,
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
    GainRandomAffliction = 4,
    GainRandomItem = 13,
    GainState = 14,
    ModifyAttribute = 5,
    ModifyHealth = 6,
    ModifyGold = 12,
    ModifyMaxHealth = 7,  
    RandomizeDeck = 15,
    TransformCard = 8,
    TransformRandomCard = 9,
    UpgradeCard = 10,
    UpgradeRandomCards = 11,

}
