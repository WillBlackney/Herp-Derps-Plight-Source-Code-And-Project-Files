using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[Serializable]
public class CardData 
{
    // General Properties
   // public CardDataSO myCardDataSO;
   // public CharacterData myCharacterDataOwner;
    public string cardName;
    public Sprite cardSprite;
    public string cardDescription;
    public int cardBaseEnergyCost;
    public CardType cardType;
    public Rarity rarity;
    public TargettingType targettingType;
    public TalentSchool talentSchool;
    public List<CardEffect> cardEffects = new List<CardEffect>();
    public List<CardEventListener> cardEventListeners = new List<CardEventListener>();

    // Key words
    public bool expend;
    public bool innate;
    public bool fleeting;
    public bool unplayable;
    public bool blessing;

}
