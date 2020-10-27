using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveGameData 
{
    public int currentJourneyPosition;
    public List<CharacterData> characters = new List<CharacterData>();

    public SaveCheckPoint saveCheckPoint;

    // Combat event data
    public EncounterData currentEncounter;
    public string currentEnemyWave;

    // Recruit event data
    public List<CharacterData> recruitCharacterChoices = new List<CharacterData>();

    // Loot data
    public LootResultModel currentLootResult;
   
}
public enum SaveCheckPoint
{
    None = 0,
    CombatStart = 1,
    CombatEnd = 2,
    KingsBlessingStart = 3,
    RecruitCharacterStart = 4,


}
