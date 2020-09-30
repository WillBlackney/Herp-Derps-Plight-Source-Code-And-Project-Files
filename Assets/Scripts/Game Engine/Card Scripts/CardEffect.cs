using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CardEffect
{
    [LabelWidth(200)]
    public CardWeaponRequirement weaponRequirement;
    [LabelWidth(200)]
    public CardEffectType cardEffectType;   

    [ShowIf("ShowDiscoveryLocation")]
    [LabelWidth(200)]
    public CardCollection discoveryLocation;

    [VerticalGroup("Search Filters")]
    [ShowIf("ShowCardLibraryFilter")]
    [LabelWidth(200)]
    public TalentSchool talentSchoolFilter;

    [ShowIf("ShowCardLibraryFilter")]
    [VerticalGroup("Search Filters")]
    [LabelWidth(200)]
    public Rarity rarityFilter;

    [ShowIf("ShowCardLibraryFilter")]
    [VerticalGroup("Search Filters")]
    [LabelWidth(200)]
    public bool blessing;

    [ShowIf("ShowOnDiscoveryChoiceMadeEffects")]
    [LabelWidth(200)]
    public List<OnDiscoveryChoiceMadeEffect> onDiscoveryChoiceMadeEffects;

    [ShowIf("ShowChooseCardInHandChoiceMadeEffects")]
    [LabelWidth(200)]
    public List<OnCardInHandChoiceMadeEffect> onChooseCardInHandChoiceMadeEffects;

    [ShowIf("ShowDrawStacksFromOverload")]
    [LabelWidth(200)]
    public bool drawStacksFromOverload;

    [ShowIf("ShowBlockGainValue")]
    [LabelWidth(200)]
    public int blockGainValue;

    [ShowIf("ShowBaseDamageValue")]
    [LabelWidth(200)]
    public int baseDamageValue;

    [ShowIf("ShowDrawDamageFromBlock")]
    [LabelWidth(250)]
    public bool drawBaseDamageFromCurrentBlock;

    [ShowIf("ShowDrawDamageFromTargetPoisoned")]
    [LabelWidth(250)]
    public bool drawBaseDamageFromTargetPoisoned;

    [ShowIf("ShowDamageType")]
    [LabelWidth(200)]
    public DamageType damageType;

    [ShowIf("cardEffectType", CardEffectType.LoseHP)]
    [LabelWidth(200)]
    public int healthLost;

    [ShowIf("cardEffectType", CardEffectType.GainEnergy)]
    [LabelWidth(200)]
    public int energyGained;

    [ShowIf("cardEffectType", CardEffectType.DrawCards)]
    [LabelWidth(200)]
    public int cardsDrawn;

    [ShowIf("cardEffectType", CardEffectType.DrawCards)]
    [LabelWidth(200)]
    public ExtraDrawEffect extraDrawEffect;

    [ShowIf("extraDrawEffect", ExtraDrawEffect.ReduceEnergyCostThisCombat)]
    [LabelWidth(200)]
    public int cardEnergyReduction;

    [ShowIf("ShowPassivePairing")]
    [LabelWidth(200)]
    public PassivePairingData passivePairing;

    [ShowIf("cardEffectType", CardEffectType.AddCardsToHand)]
    [LabelWidth(200)]
    public CardDataSO cardAdded;

    [ShowIf("cardEffectType", CardEffectType.AddCardsToHand)]
    [LabelWidth(200)]
    public int copiesAdded;

    [ShowIf("cardEffectType", CardEffectType.AddRandomBlessingsToHand)]
    [LabelWidth(200)]
    public int blessingsGained;

    public List<AnimationEventData> visualEventsOnStart;
    public List<AnimationEventData> visualEventsOnFinish;

    public bool ShowPassivePairing()
    {
        if(cardEffectType == CardEffectType.ApplyPassiveToAllAllies ||
            cardEffectType == CardEffectType.ApplyPassiveToAllEnemies ||
            cardEffectType == CardEffectType.ApplyPassiveToSelf ||
            cardEffectType == CardEffectType.ApplyPassiveToTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowBlockGainValue()
    {
        if (cardEffectType == CardEffectType.GainBlockSelf ||
            cardEffectType == CardEffectType.GainBlockTarget ||
            cardEffectType == CardEffectType.GainBlockAllAllies)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowBaseDamageValue()
    {
        if ((cardEffectType != CardEffectType.DamageTarget && cardEffectType != CardEffectType.DamageSelf && cardEffectType != CardEffectType.DamageAllEnemies) ||
            ((drawBaseDamageFromCurrentBlock )||
             (drawBaseDamageFromTargetPoisoned))
            )
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool ShowDrawDamageFromBlock()
    {
        if ((cardEffectType == CardEffectType.DamageTarget || cardEffectType == CardEffectType.DamageSelf || cardEffectType == CardEffectType.DamageAllEnemies)  &&
            drawBaseDamageFromTargetPoisoned == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDrawDamageFromTargetPoisoned()
    {
        if ((cardEffectType == CardEffectType.DamageTarget || cardEffectType == CardEffectType.DamageSelf || cardEffectType == CardEffectType.DamageAllEnemies) &&
            drawBaseDamageFromCurrentBlock == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDamageType()
    {
        if (cardEffectType == CardEffectType.DamageTarget || cardEffectType == CardEffectType.DamageSelf || cardEffectType == CardEffectType.DamageAllEnemies)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDrawStacksFromOverload()
    {
        if (cardEffectType == CardEffectType.ApplyPassiveToAllAllies ||
            cardEffectType == CardEffectType.ApplyPassiveToAllEnemies || 
            cardEffectType == CardEffectType.ApplyPassiveToSelf ||
            cardEffectType == CardEffectType.ApplyPassiveToTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDiscoveryLocation()
    {
        return cardEffectType == CardEffectType.DiscoverCards;
    }
    public bool ShowOnDiscoveryChoiceMadeEffects()
    {
        return cardEffectType == CardEffectType.DiscoverCards;
    }
    public bool ShowChooseCardInHandChoiceMadeEffects()
    {
        return cardEffectType == CardEffectType.ChooseCardInHand;
    }
    public bool ShowCardLibraryFilter()
    {
        if(discoveryLocation == CardCollection.CardLibrary &&
            cardEffectType == CardEffectType.DiscoverCards)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[Serializable]
public class OnDiscoveryChoiceMadeEffect
{
    [LabelWidth(200)]
    public OnDiscoveryChoiceMadeEffectType discoveryEffect;

    [ShowIf("ShowCopiesAdded")]
    [LabelWidth(200)]
    public int copiesAdded;
    [ShowIf("ShowEnergyReduction")]
    [LabelWidth(200)]
    public int energyReduction;
    [ShowIf("ShowNewEnergyCost")]
    [LabelWidth(200)]
    public int newEnergyCost;

    public bool ShowCopiesAdded()
    {
        if(discoveryEffect == OnDiscoveryChoiceMadeEffectType.AddToHand ||
           discoveryEffect == OnDiscoveryChoiceMadeEffectType.AddCopyToHand)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowEnergyReduction()
    {
        return discoveryEffect == OnDiscoveryChoiceMadeEffectType.ReduceEnergyCost;
    }
    public bool ShowNewEnergyCost()
    {
        return discoveryEffect == OnDiscoveryChoiceMadeEffectType.SetEnergyCost;
    }

}

public enum OnDiscoveryChoiceMadeEffectType
{
    None = 0,
    AddToHand = 1,
    AddCopyToHand = 2,
    ReduceEnergyCost = 3,
    SetEnergyCost = 4,
}


[Serializable]
public class OnCardInHandChoiceMadeEffect
{
    [LabelWidth(200)]
    public OnCardInHandChoiceMadeEffectType choiceEffect;

    [ShowIf("ShowCopiesAdded")]
    [LabelWidth(200)]
    public int copiesAdded;
    [ShowIf("ShowEnergyReduction")]
    [LabelWidth(200)]
    public int energyReduction;
    [ShowIf("ShowNewEnergyCost")]
    [LabelWidth(200)]
    public int newEnergyCost;
    [ShowIf("ShowPassivePairing")]
    [LabelWidth(200)]
    public PassivePairingData passivePairing;

    public bool ShowCopiesAdded()
    {
        if (choiceEffect == OnCardInHandChoiceMadeEffectType.AddCopyToHand)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowEnergyReduction()
    {
        return choiceEffect == OnCardInHandChoiceMadeEffectType.ReduceEnergyCost;
    }
    public bool ShowNewEnergyCost()
    {
        return choiceEffect == OnCardInHandChoiceMadeEffectType.SetEnergyCost;
    }
    public bool ShowPassivePairing()
    {
        return choiceEffect == OnCardInHandChoiceMadeEffectType.GainPassive;
    }

}

public enum OnCardInHandChoiceMadeEffectType
{
    None = 0,
    AddCopyToHand = 1,
    ReduceEnergyCost = 2,
    SetEnergyCost = 3,
    ExpendIt = 4,
    GainPassive = 5,
}




