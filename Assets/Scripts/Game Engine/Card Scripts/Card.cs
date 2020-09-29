using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // General Properties
    public CharacterEntityModel owner;
    public CardViewModel cardVM;
    public CardDataSO myCardDataSO;
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

    // Energy reduction propeties
    public int energyReductionUntilPlayed;
    public int energyReductionPermanent;
    public int energyReductionThisCombatOnly;

    // Key words
    public bool expend;
    public bool opener;
    public bool fleeting;
    public bool unplayable;
    public bool blessing;

}
