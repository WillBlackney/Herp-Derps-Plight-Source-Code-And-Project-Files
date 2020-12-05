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

    [ShowIf("ShowMaxHealthGained")]
    public int maxHealthGained;

    [ShowIf("ShowEnergyReduction")]
    public int energyReductionAmount;

    [ShowIf("ShowPassivePairing")]
    public PassivePairingData passivePairing;

    [ShowIf("ShowEnergyGainedOrLost")]
    public int energyGainedOrLost;

    [ShowIf("ShowCertainCardNames")]
    public List<string> certainCardNames;

    [Header("Holding Certain Card Effects")]
    [ShowIf("ShowCertainCardNames")]
    public bool cardCostsZero = false;

    public bool ShowCertainCardNames()
    {
        return cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard;

    }
    public bool ShowMaxHealthGained()
    {
        return cardEventListenerFunction == CardEventListenerFunction.ModifyMaxHealth;
      
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
