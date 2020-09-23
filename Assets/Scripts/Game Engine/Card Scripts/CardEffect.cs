using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CardEffect
{
    [LabelWidth(200)]
    public CardEffectType cardEffectType;
    [LabelWidth(200)]
    public CardWeaponRequirement weaponRequirement;

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

}




