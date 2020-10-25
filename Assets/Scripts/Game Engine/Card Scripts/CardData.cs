using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[Serializable]
public class CardData 
{
    // General Properties
    public string cardName;
    public Sprite cardSprite;
    public string cardDescription;
    public bool xEnergyCost;
    public int cardBaseEnergyCost;
    public CardType cardType;
    public Rarity rarity;
    public bool racialCard;
    public CharacterRace originRace;
    public TargettingType targettingType;
    public TalentSchool talentSchool;
    public List<CardEffect> cardEffects = new List<CardEffect>();
    public List<CardEventListener> cardEventListeners = new List<CardEventListener>();
    public List<KeyWordModel> keyWordModels = new List<KeyWordModel>();
    public List<CustomString> cardDescriptionTwo = new List<CustomString>();

    // Key words
    public bool expend;
    public bool innate;
    public bool fleeting;
    public bool unplayable;
    public bool lifeSteal;
    public bool blessing;

}
