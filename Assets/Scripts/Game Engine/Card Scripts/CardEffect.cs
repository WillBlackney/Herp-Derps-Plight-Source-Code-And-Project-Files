using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CardEffect
{
    public CardEffectType cardEffectType;
    public CardWeaponRequirement weaponRequirement;

    [ShowIf("ShowBlockGainValue")]
    public int blockGainValue;

    [ShowIf("ShowBaseDamageValue")]
    public int baseDamageValue;

    [ShowIf("ShowDrawDamageFromBlock")]
    public bool drawBaseDamageFromCurrentBlock;

    [ShowIf("ShowDrawDamageFromTargetPoisoned")]
    public bool drawBaseDamageFromTargetPoisoned;

    [ShowIf("ShowDamageType")]
    public DamageType damageType;

    [ShowIf("cardEffectType", CardEffectType.LoseHP)]
    public int healthLost;

    [ShowIf("cardEffectType", CardEffectType.GainEnergy)]
    public int energyGained;

    [ShowIf("cardEffectType", CardEffectType.DrawCards)]
    public int cardsDrawn;

    [ShowIf("cardEffectType", CardEffectType.DrawCards)]
    public ExtraDrawEffect extraDrawEffect;

    [ShowIf("extraDrawEffect", ExtraDrawEffect.ReduceEnergyCostThisCombat)]
    public int cardEnergyReduction;

    [ShowIf("ShowPassivePairing")]
    public PassivePairingData passivePairing;

    [ShowIf("cardEffectType", CardEffectType.AddCardsToHand)]
    public CardDataSO cardAdded;

    [ShowIf("cardEffectType", CardEffectType.AddCardsToHand)]
    public int copiesAdded;

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

}

[Serializable]
public enum CardEffectType
{
    None = 0,
    AddCardsToHand = 1,
    ApplyPassiveToSelf = 2,
    ApplyPassiveToTarget = 3,
    ApplyPassiveToAllEnemies = 4,
    ApplyPassiveToAllAllies = 5,
    DamageTarget = 6,
    DamageSelf = 7,
    DamageAllEnemies = 8,
    DrawCards = 9,
    GainBlockSelf = 10,
    GainBlockTarget = 19,
    GainBlockAllAllies = 12,
    GainEnergy = 13,
    LoseHP = 14,
    RemoveAllPoisonedFromSelf = 15,
    RemoveAllPoisonedFromTarget = 16,
    TauntTarget = 17,
    TauntAllEnemies = 18,
    
    

}
[Serializable]
public enum ExtraDrawEffect
{
    None,
    ReduceEnergyCostThisCombat,
    SetEnergyCostToZeroThisCombat,
}


