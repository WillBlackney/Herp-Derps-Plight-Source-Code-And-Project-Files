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
    public EncounterData currentEncounter;
    public string currentEnemyWave;
   
}
public enum SaveCheckPoint
{
    None = 0,
    CombatStart = 1,
    CombatEnd = 2,
    KingsBlessingStart = 3,
    RecruitCharacterStart = 4,


}
