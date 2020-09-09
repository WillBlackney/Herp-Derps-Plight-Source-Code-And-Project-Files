using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New CardDataSO", menuName = "CardDataSO", order = 52)]
[Serializable]
public class CardDataSO : ScriptableObject
{
    [BoxGroup("General Info", true, true)]
    [LabelWidth(100)]
    public string cardName;
    [BoxGroup("General Info")]
    [LabelWidth(100)]
    [TextArea]
    public string cardDescription; 

    [HorizontalGroup("Core Data", 75)]
    [HideLabel]
    [PreviewField(75)]
    public Sprite cardSprite;

    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    public int cardEnergyCost;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    public CardType cardType;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    public TargettingType targettingType;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    public TalentSchool talentSchool;
    [VerticalGroup("Core Data/Stats")]
    [LabelWidth(100)]
    public Rarity rarity;

    [BoxGroup("Key Words", true, true)]
    [LabelWidth(100)]
    public bool expend;
    [BoxGroup("Key Words")]
    [LabelWidth(100)]
    public bool opener;
    [BoxGroup("Key Words")]
    [LabelWidth(100)]
    public bool fleeting;
    [BoxGroup("Key Words")]
    [LabelWidth(100)]
    public bool unplayable;

    public List<CardEffect> cardEffects;

    public List<CardEventListener> cardEventListeners;

}

[Serializable]
public enum CardEventListenerType
{
    None, OnLoseHealth, OnDraw, 
}

[Serializable]
public enum CardEventListenerFunction
{
    None, ReduceCardEnergyCost,
}

[Serializable]
public class CardEventListener
{
    public CardEventListenerType cardEventListenerType;
    public CardEventListenerFunction cardEventListenerFunction;

    [ShowIf("cardEventListenerFunction", CardEventListenerFunction.ReduceCardEnergyCost)]
    public int energyReductionAmount;

}



