using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingChoiceData 
{
    public string choiceDescription;

    public KingChoiceEffectType effect;

    public KingChoiceEffectCategory category;

    public KingChoiceImpactLevel impactLevel;

    public int maxHealthGainedOrLost;

    public int healthGainedOrLost;

    public Rarity randomCardRarity;

    public Rarity discoveryCardRarity;

    public int randomCardsUpgraded;

    public int randomCardsTransformed;

    public CoreAttribute attributeModified;

    public int attributeAmountModified;

    public List<PassivePairingData> possiblePassives;

    public int afflicationsGained;
}
