using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveGameData 
{
    // Character data
    public List<CharacterData> characters = new List<CharacterData>();

    // Journey data
    public int currentJourneyPosition;
    public int dayNumber;
    public SaveCheckPoint saveCheckPoint;
    public string map;

    // Player data
    public int currentGold;
    public int maxRosterSize;

    // Combat event data
    public CombatData currentCombatData;
    public List<CharacterData> chosenCombatCharacters = new List<CharacterData>();
    // to do: save and cache player characters in the combat some how.

    // Recruit event data
    public List<CharacterData> recruitCharacterChoices = new List<CharacterData>();

    // Loot data
    public LootResultModel currentLootResult;
    public int timeSinceLastEpic;
    public int timeSinceLastRare;
    public int timeSinceLastUpgrade;

    // Camp site data
    public int campPointRegen;
    public int campCardDraw;
    public List<CampCardData> campDeck = new List<CampCardData>();

    // KBC Data
    public List<KingsChoicePairingModel> kbcChoices = new List<KingsChoicePairingModel>();

    // Shop
    public ShopContentResultModel shopData;

    // Inventory data
    public List<CardData> cardInventory = new List<CardData>();
    public List<ItemData> itemInventory = new List<ItemData>();

    // States
    public List<StateData> playerStates = new List<StateData>();
    public ShrineStateResult currentShrineStates;

    //cache daily conbats
    public CombatChoicesResult dailyCombatChoices;
}
public enum SaveCheckPoint
{
    None = 0,
    CombatStart = 1,
    CombatEnd = 2,
    TownDayStart = 3,


}
