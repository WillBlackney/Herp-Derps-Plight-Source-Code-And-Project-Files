using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CardEffect
{
    public CardEffectType cardEffectType;

    [ShowIf("cardEffectType", CardEffectType.GainBlock)]
    public int blockGainValue;

    [ShowIf("cardEffectType", CardEffectType.DealDamage)]
    public int baseDamageValue;

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

}

[Serializable]
public enum CardEffectType
{
    None, 
    GainBlock, 
    DealDamage, 
    LoseHealth, 
    GainEnergy, 
    DrawCards, 
    ApplyBurning, 
    ApplyPassiveToSelf, 
    ApplyPassiveToTarget, 
    ApplyPassiveToAllEnemies,
    ApplyPassiveToAllAllies,
}
