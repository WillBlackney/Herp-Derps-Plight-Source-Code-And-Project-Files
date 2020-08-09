using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New CardDataSO", menuName = "CardDataSO", order = 52)]
[Serializable]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    [TextArea(2, 3)]
    public string cardDescription; 
    public Sprite cardSprite;
    public int cardEnergyCost;
    public CardType cardType;
    public TargettingType targettingType;
    public List<CardEffect> cardEffects;
}

public enum CardType
{
    None,
    Skill,
    MeleeAttack,
    RangedAttack,
    Power
};

public enum TargettingType
{
    NoTarget,
    Ally,
    AllyOrSelf,
    Enemy,
    AllCharacters
};
