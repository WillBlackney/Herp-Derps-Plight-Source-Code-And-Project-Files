using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class CardPassiveEffect 
{
    public CardPassiveEffectType cardPassiveEffectType;

    [ShowIf("ShowPassive")]
    public Passive passive;

    public bool ShowPassive()
    {
        return cardPassiveEffectType == CardPassiveEffectType.EnergyCostReducedByCurrentPassive;
    }

}
public enum CardPassiveEffectType
{
    None = 0,
    WhileHoldingCertainCard = 1,
    EnergyCostReducedByCurrentPassive = 2,
}
