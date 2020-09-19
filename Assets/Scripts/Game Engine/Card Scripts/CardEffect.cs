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

    [ShowIf("ShowDrawStacksFromOverload")]
    public bool drawStacksFromOverload;

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

    public AnimationEventData animationEventData;

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

}




