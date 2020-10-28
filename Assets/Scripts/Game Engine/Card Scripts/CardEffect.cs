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

    [LabelWidth(200)]
    public bool splitTargetEffect = false;

    [ShowIf("ShowSplitTargetType")]
    [LabelWidth(200)]
    public TargettingType splitTargetType;

    [ShowIf("ShowRemoveBlockFrom")]
    [LabelWidth(200)]
    public RemoveBlockFrom removeBlockFrom;    

    [ShowIf("ShowModifyDeckCardEffect")]
    [LabelWidth(200)]
    public ModifyDeckCardEffectType modifyDeckCardEffect;

    [ShowIf("ShowPermanentBlockGain")]
    [LabelWidth(200)]
    public int permanentBlockGain;

    [ShowIf("ShowPermanentDamageGain")]
    [LabelWidth(200)]
    public int permanentDamageGain;
   
    [ShowIf("ShowChooseTargetRandomly")]
    [LabelWidth(200)]
    public bool chooseTargetRandomly;

    [ShowIf("ShowPassiveApplicationLoops")]
    [LabelWidth(200)]
    public int passiveApplicationLoops;

    [ShowIf("ShowPassiveApplicationLoops")]
    [LabelWidth(200)]
    public TargettingType validRandomTargetsType;

    [ShowIf("ShowDamageLoops")]
    [LabelWidth(200)]
    public int damageLoops;   

    [ShowIf("ShowDrawSpecifcType")]
    [LabelWidth(200)]
    public bool drawSpecificType;

    [LabelWidth(200)]
    [ShowIf("ShowSpecificCardType")]
    public SpecificTypeDrawn specificTypeDrawn;

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

    [ShowIf("ShowModifyAllCardsInHandEffects")]
    [LabelWidth(200)]
    public List<ModifyAllCardsInHandEffect> modifyCardsInHandEffects;

    [ShowIf("ShowDrawStacksFromOverload")]
    [LabelWidth(200)]
    public bool drawStacksFromOverload;

    [ShowIf("ShowDrawStacksFromWeakenedOnEnemies")]
    [LabelWidth(200)]
    public bool drawStacksFromWeakenedOnEnemies;

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

    [ShowIf("ShowDrawDamageFromMeleeAttacksPlayed")]
    [LabelWidth(250)]
    public bool drawBaseDamageFromMeleeAttacksPlayed;

    [ShowIf("ShowDrawDamageFromOverloadOnSelf")]
    [LabelWidth(250)]
    public bool drawBaseDamageFromOverloadOnSelf;

    [ShowIf("ShowDrawDamageFromBurningOnSelf")]
    [LabelWidth(250)]
    public bool drawBaseDamageFromBurningOnSelf;

    [ShowIf("ShowDamageMultiplier")]
    [LabelWidth(250)]
    public int baseDamageMultiplier = 1;

    [ShowIf("ShowDamageType")]
    [LabelWidth(200)]
    public DamageType damageType;

    [ShowIf("ShowHealthRestored")]
    [LabelWidth(200)]
    public int healthRestored;       

    [ShowIf("cardEffectType", CardEffectType.LoseHP)]
    [LabelWidth(200)]
    public int healthLost;

    [ShowIf("ShowEnergyGained")]
    [LabelWidth(200)]
    public int energyGained;  

    [ShowIf("ShowCardsDrawn")]
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


    public bool ShowCardsDrawn()
    {
        if (cardEffectType == CardEffectType.DrawCards &&
            drawStacksFromOverload == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowEnergyGained()
    {
        if ((cardEffectType == CardEffectType.GainEnergySelf || cardEffectType == CardEffectType.GainEnergyTarget) &&
            drawStacksFromOverload == false)
        {
            return true;
        }
        else 
        { 
            return false;
        }
    }
    public bool ShowRemoveBlockFrom()
    {
        return cardEffectType == CardEffectType.RemoveAllBlock;
    }
    public bool ShowPermanentDamageGain()
    {
        if (cardEffectType == CardEffectType.ModifyMyPermanentDeckCard &&
            modifyDeckCardEffect == ModifyDeckCardEffectType.IncreaseBaseDamage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowPermanentBlockGain()
    {
        if (cardEffectType == CardEffectType.ModifyMyPermanentDeckCard &&
            modifyDeckCardEffect == ModifyDeckCardEffectType.IncreaseBaseBlockGain)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowModifyDeckCardEffect()
    {
        return cardEffectType == CardEffectType.ModifyMyPermanentDeckCard;
    }

    public bool ShowHealthRestored()
    {
        if (cardEffectType == CardEffectType.HealSelf ||
            cardEffectType == CardEffectType.HealSelfAndAllies ||
            cardEffectType == CardEffectType.HealTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDamageLoops()
    {
        if (cardEffectType == CardEffectType.DamageTarget &&
            chooseTargetRandomly)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowPassiveApplicationLoops()
    {
        if (cardEffectType == CardEffectType.ApplyPassiveToTarget &&
            chooseTargetRandomly)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowChooseTargetRandomly()
    {
        if(cardEffectType == CardEffectType.DamageTarget ||
            cardEffectType == CardEffectType.ApplyPassiveToTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDrawSpecifcType()
    {
        if(cardEffectType == CardEffectType.DrawCards)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowSpecificCardType()
    {
        return drawSpecificType;
    }
    public bool ShowSplitTargetType()
    {
        return splitTargetEffect;
    }
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
             (drawBaseDamageFromTargetPoisoned) ||
             (drawBaseDamageFromBurningOnSelf) ||
             (drawBaseDamageFromMeleeAttacksPlayed) ||
             (drawBaseDamageFromOverloadOnSelf))
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
            (drawBaseDamageFromTargetPoisoned == false && drawBaseDamageFromMeleeAttacksPlayed == false && drawBaseDamageFromOverloadOnSelf == false && drawBaseDamageFromBurningOnSelf == false))
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
            (drawBaseDamageFromCurrentBlock == false && drawBaseDamageFromMeleeAttacksPlayed == false && drawBaseDamageFromOverloadOnSelf == false && drawBaseDamageFromBurningOnSelf == false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDrawDamageFromBurningOnSelf()
    {
        if ((cardEffectType == CardEffectType.DamageTarget || cardEffectType == CardEffectType.DamageSelf || cardEffectType == CardEffectType.DamageAllEnemies) &&
            (drawBaseDamageFromCurrentBlock == false && drawBaseDamageFromMeleeAttacksPlayed == false && drawBaseDamageFromTargetPoisoned == false && drawBaseDamageFromOverloadOnSelf == false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDrawDamageFromOverloadOnSelf()
    {
        if ((cardEffectType == CardEffectType.DamageTarget || cardEffectType == CardEffectType.DamageSelf || cardEffectType == CardEffectType.DamageAllEnemies) &&
            (drawBaseDamageFromCurrentBlock == false && drawBaseDamageFromMeleeAttacksPlayed == false && drawBaseDamageFromTargetPoisoned == false && drawBaseDamageFromBurningOnSelf == false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDrawDamageFromMeleeAttacksPlayed()
    {
        if ((cardEffectType == CardEffectType.DamageTarget || cardEffectType == CardEffectType.DamageSelf || cardEffectType == CardEffectType.DamageAllEnemies) &&
           (drawBaseDamageFromCurrentBlock == false && drawBaseDamageFromTargetPoisoned == false && drawBaseDamageFromOverloadOnSelf == false && drawBaseDamageFromBurningOnSelf == false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDamageMultiplier()
    {
        if ((cardEffectType == CardEffectType.DamageTarget || cardEffectType == CardEffectType.DamageSelf || cardEffectType == CardEffectType.DamageAllEnemies) &&
            (drawBaseDamageFromBurningOnSelf == true || drawBaseDamageFromCurrentBlock == true || drawBaseDamageFromTargetPoisoned == true || drawBaseDamageFromMeleeAttacksPlayed == true || drawBaseDamageFromOverloadOnSelf == true))
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
        if ((cardEffectType == CardEffectType.ApplyPassiveToAllAllies ||
            cardEffectType == CardEffectType.ApplyPassiveToAllEnemies || 
            cardEffectType == CardEffectType.ApplyPassiveToSelf ||
            cardEffectType == CardEffectType.ApplyPassiveToTarget ||
            cardEffectType == CardEffectType.GainEnergySelf ||
            cardEffectType == CardEffectType.GainEnergyTarget ||
            cardEffectType == CardEffectType.DrawCards) &&
            drawStacksFromWeakenedOnEnemies == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool ShowDrawStacksFromWeakenedOnEnemies()
    {
        if ((cardEffectType == CardEffectType.ApplyPassiveToAllAllies ||
            cardEffectType == CardEffectType.ApplyPassiveToAllEnemies ||
            cardEffectType == CardEffectType.ApplyPassiveToSelf ||
            cardEffectType == CardEffectType.ApplyPassiveToTarget) &&
            drawStacksFromOverload == false)
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
    public bool ShowModifyAllCardsInHandEffects()
    {
        return cardEffectType == CardEffectType.ModifyAllCardsInHand;
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


[Serializable]
public class ModifyAllCardsInHandEffect
{
    [LabelWidth(200)]
    public ModifyAllCardsInHandEffectType modifyEffect;

    [LabelWidth(200)]
    public bool onlySpecificCards;

    [ShowIf("ShowSpecificCardType")]
    [LabelWidth(200)]
    public SpecificCardType specificCardType;

    [ShowIf("ShowEnergyReduction")]
    [LabelWidth(200)]
    public int energyReduction;
    [ShowIf("ShowNewEnergyCost")]
    [LabelWidth(200)]
    public int newEnergyCost;

    [ShowIf("ShowCardAdded")]
    [LabelWidth(200)]
    public CardDataSO cardAdded;

    [ShowIf("ShowTalentFilter")]
    [LabelWidth(200)]
    public TalentSchool talentSchoolFilter;

    [ShowIf("ShowRarityFilter")]
    [LabelWidth(200)]
    public Rarity rarityFilter;

    [ShowIf("ShowBaseDamage")]
    [LabelWidth(200)]
    public int baseDamage;

    [ShowIf("ShowDamageType")]
    [LabelWidth(200)]
    public DamageType damageType;

    public bool ShowBaseDamage()
    {
        return modifyEffect == ModifyAllCardsInHandEffectType.DamageAllEnemies;
    }
    public bool ShowDamageType()
    {
        return modifyEffect == ModifyAllCardsInHandEffectType.DamageAllEnemies;
    }
    public bool ShowSpecificCardType()
    {
        return onlySpecificCards;
    }
    public bool ShowCardAdded()
    {
        return modifyEffect == ModifyAllCardsInHandEffectType.AddSpecificCardToHand;
    }
    public bool ShowEnergyReduction()
    {
        return modifyEffect == ModifyAllCardsInHandEffectType.ReduceEnergyCost;
    }
    public bool ShowNewEnergyCost()
    {
        return modifyEffect == ModifyAllCardsInHandEffectType.SetEnergyCost;
    }
    public bool ShowRarityFilter()
    {
        return modifyEffect == ModifyAllCardsInHandEffectType.AddNewCardFromLibraryToHand;
    }
    public bool ShowTalentFilter()
    {
        return modifyEffect == ModifyAllCardsInHandEffectType.AddNewCardFromLibraryToHand;
    }

}

public enum SpecificCardType
{
    None = 0,
    Burn = 1,
}

public enum ModifyAllCardsInHandEffectType
{
    None = 0,
    ReduceEnergyCost = 1,
    SetEnergyCost = 2,
    ExpendIt = 3,
    AddNewCardFromLibraryToHand = 4,
    AddRandomBlessingToHand = 5,
    AddSpecificCardToHand = 6,
    DamageAllEnemies = 7,

}

public enum SpecificTypeDrawn
{
    None = 0,
    MeleeAttack = 1,
    RangedAttack = 2,
    ZeroEnergyCost = 3,
}

public enum ModifyDeckCardEffectType
{
    None = 0,
    IncreaseBaseDamage = 1,
    IncreaseBaseBlockGain = 2,
}

public enum RemoveBlockFrom
{
    None = 0,
    Self =1,
    Target =2,
}




