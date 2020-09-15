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

    [ShowIf("cardEventListenerFunction", CardEventListenerFunction.ReduceCardEnergyCost)]
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

}
