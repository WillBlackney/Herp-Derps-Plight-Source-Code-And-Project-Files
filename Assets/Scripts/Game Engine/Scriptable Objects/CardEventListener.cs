using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class CardEventListener
{
    public CardEventListenerType cardEventListenerType;
    public CardEventListenerFunction cardEventListenerFunction;
    public CardWeaponRequirement weaponRequirement = CardWeaponRequirement.None;

    [ShowIf("ShowWhileHoldingEffect")]
    public WhileHoldingCardPassiveEffect whileHoldingEffect;

    [ShowIf("ShowCardEnergyCostIncrease")]
    public int cardEnergyCostIncrease;
    
   

    [ShowIf("ShowMaxHealthGained")]
    public int maxHealthGained;

    [ShowIf("ShowEnergyReduction")]
    public int energyReductionAmount;

    [ShowIf("ShowPassivePairing")]
    public PassivePairingData passivePairing;

    [ShowIf("ShowHealthLost")]
    public int healthLost;

    [ShowIf("ShowEnergyGainedOrLost")]
    public int energyGainedOrLost;

    [ShowIf("ShowCertainCardNames")]
    public List<string> certainCardNames;

    [ShowIf("ShowWhileHoldingBlessing")]
    public bool whileHoldingBlessing;

    [Header("Holding Certain Card Effects")]
    [ShowIf("ShowCertainCardNames")]
    public bool cardCostsZero = false;

    public bool ShowCardEnergyCostIncrease()
    {
        return cardEventListenerType == CardEventListenerType.WhileHoldingThis &&
            whileHoldingEffect == WhileHoldingCardPassiveEffect.CardsCostMoreEnergy;
    }
    public bool ShowWhileHoldingEffect()
    {
        return cardEventListenerType == CardEventListenerType.WhileHoldingThis;
    }
    public bool ShowCertainCardNames()
    {
        return cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard;

    }
    public bool ShowWhileHoldingBlessing()
    {
        return cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard;

    }
    public bool ShowMaxHealthGained()
    {
        return cardEventListenerFunction == CardEventListenerFunction.ModifyMaxHealth;      
    }
    public bool ShowHealthLost()
    {
        return cardEventListenerFunction == CardEventListenerFunction.LoseHealth;

    }
    public bool ShowEnergyGainedOrLost()
    {
        return cardEventListenerFunction == CardEventListenerFunction.ModifyEnergy;

    }
    public bool ShowPassivePairing()
    {
        if (cardEventListenerFunction == CardEventListenerFunction.ApplyPassiveToSelf)
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
        if (cardEventListenerFunction == CardEventListenerFunction.ReduceCardEnergyCost ||
            cardEventListenerFunction == CardEventListenerFunction.ReduceCardEnergyCostThisActivation)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
