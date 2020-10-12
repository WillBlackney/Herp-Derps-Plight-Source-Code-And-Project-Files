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

    [ShowIf("ShowEnergyReduction")]
    public int energyReductionAmount;

    [ShowIf("ShowPassivePairing")]
    public PassivePairingData passivePairing;

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
