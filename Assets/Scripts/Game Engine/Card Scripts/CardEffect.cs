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

    [ShowIf("cardEffectType", CardEffectType.DamageTarget)]
    public DamageType damageType;

    [ShowIf("cardEffectType", CardEffectType.LoseHP)]
    public int healthLost;

    [ShowIf("cardEffectType", CardEffectType.GainEnergy)]
    public int energyGained;

    [ShowIf("cardEffectType", CardEffectType.DrawCards)]
    public int cardsDrawn;

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
        if (cardEffectType != CardEffectType.DamageTarget ||
            ((cardEffectType == CardEffectType.DamageTarget && drawBaseDamageFromCurrentBlock )||
             (cardEffectType == CardEffectType.DamageTarget && drawBaseDamageFromTargetPoisoned))
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
        if (cardEffectType == CardEffectType.DamageTarget &&
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
        if (cardEffectType == CardEffectType.DamageTarget &&
            drawBaseDamageFromCurrentBlock == false)
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
    None,
    DamageTarget,
    DamageSelf,
    DamageAllEnemies,
    GainBlockSelf,
    GainBlockTarget,
    GainBlockAllAllies,    
    LoseHP, 
    GainEnergy, 
    DrawCards, 
    ApplyPassiveToSelf, 
    ApplyPassiveToTarget, 
    ApplyPassiveToAllEnemies,
    ApplyPassiveToAllAllies,
    TauntTarget,
    TauntAllEnemies,
    AddCardsToHand,
    RemoveAllPoisonedFromSelf,
    RemoveAllPoisonedFromTarget,

}
