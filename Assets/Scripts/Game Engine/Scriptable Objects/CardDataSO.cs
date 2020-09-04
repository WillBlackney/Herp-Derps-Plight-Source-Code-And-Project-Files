using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New CardDataSO", menuName = "CardDataSO", order = 52)]
[Serializable]
public class CardDataSO : ScriptableObject
{
    [BoxGroup("General Info")]
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

public enum TalentSchool
{
    None, Neutral, Brawler, Duelist, Assassination, Guardian, Pyromania, Cyromancy, Ranger, Manipulation,
    Divinity, Shadowcraft, Corruption, Naturalism,
}
