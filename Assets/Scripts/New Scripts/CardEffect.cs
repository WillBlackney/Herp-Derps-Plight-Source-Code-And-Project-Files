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
    public AbilityDataSO.DamageType damageType;

}

[Serializable]
public enum CardEffectType
{
    None, GainBlock, DealDamage
}
