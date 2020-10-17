using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card
{
    // Data references
    public CharacterEntityModel owner;
    public CardData myCharacterDeckCard;    

    // General Properties
    public CardViewModel cardVM;
    public string cardName;
    public Sprite cardSprite;
    public string cardDescription;
    public bool xEnergyCost;
    public int cardBaseEnergyCost;
    public CardType cardType;
    public Rarity rarity;
    public TargettingType targettingType;
    public TalentSchool talentSchool;
    public List<CardEffect> cardEffects = new List<CardEffect>();
    public List<CardEventListener> cardEventListeners = new List<CardEventListener>();
    public List<KeyWordModel> keyWordModels = new List<KeyWordModel>();
    public List<CustomString> cardDescriptionTwo = new List<CustomString>();

    // Energy reduction propeties
    public int energyReductionUntilPlayed;
    public int energyReductionThisCombatOnly;
    public int energyReductionThisActivationOnly;

    // Key words
    public bool expend;
    public bool innate;
    public bool fleeting;
    public bool unplayable;
    public bool lifeSteal;
    public bool blessing;

    // misc getters
    public CharacterData myCharacterData()
    {
        return owner.characterData;
    }

}
