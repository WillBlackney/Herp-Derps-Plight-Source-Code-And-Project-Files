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

    [ShowIf("cardEffectType", CardEffectType.DealDamage)]
    public DamageType damageType;

    [ShowIf("cardEffectType", CardEffectType.LoseHealth)]
    public int healthLost;

    [ShowIf("cardEffectType", CardEffectType.GainEnergy)]
    public int energyGained;

    [ShowIf("cardEffectType", CardEffectType.DrawCards)]
    public int cardsDrawn;

    [ShowIf("cardEffectType", CardEffectType.ApplyBurning)]
    public int burningApplied;

    [ShowIf("ShowPassivePairing")]
    public PassivePairingData passivePairing;

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
        if (cardEffectType != CardEffectType.DealDamage ||
            (cardEffectType == CardEffectType.DealDamage && drawBaseDamageFromCurrentBlock)
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
        if (cardEffectType == CardEffectType.DealDamage)
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
    DealDamage,
    GainBlockSelf,
    GainBlockTarget,
    GainBlockAllAllies,    
    LoseHealth, 
    GainEnergy, 
    DrawCards, 
    ApplyBurning, 
    ApplyPassiveToSelf, 
    ApplyPassiveToTarget, 
    ApplyPassiveToAllEnemies,
    ApplyPassiveToAllAllies,
}
