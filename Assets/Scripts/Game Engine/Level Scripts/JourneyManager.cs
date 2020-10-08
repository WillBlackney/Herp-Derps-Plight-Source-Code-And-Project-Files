using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyManager : Singleton<JourneyManager>
{
    // Properties + Component Refs
    #region
    [Header("General Properties")]
    [SerializeField] private bool allowSameEnemyWaveMultipleTimes;

    [Header("Encounter Sequence Properties")]
    [SerializeField] private List<EncounterData> encounters;
    private List<EnemyWaveSO> enemyWavesAlreadyEncountered = new List<EnemyWaveSO>();

    [Header("Player Position Properties")]
    private int currentJourneyPosition = 0;

    // Getters
    public List<EncounterData> Encounters
    {
        get { return encounters; }
        private set { encounters = value; }
    }
    public int CurrentJourneyPosition
    {
        get { return currentJourneyPosition; }
        private set { currentJourneyPosition = value; }
    }
    public bool AllowSameEnemyWaveMultipleTimes
    {
        get { return allowSameEnemyWaveMultipleTimes; }
        private set { allowSameEnemyWaveMultipleTimes = value; }
    }
    #endregion

    // Initialization
    #region
    public void BuildDataFromSaveFile(SaveGameData saveData)
    {
        CurrentJourneyPosition = saveData.currentJourneyPosition;
    }

    #endregion

    // Modify Player position
    #region
    public void SetJourneyPosition(int newPosition)
    {
        CurrentJourneyPosition = newPosition;
    }
    #endregion

    // Get Encounters
    #region
    public EncounterData GetCurrentEncounter()
    {
        return Encounters[CurrentJourneyPosition];
    }
    public EncounterData GetNextEncounter()
    {
        return Encounters[CurrentJourneyPosition + 1];
    }
    #endregion

    // Get Enemy Waves
    #region
    public EnemyWaveSO GetRandomEnemyWaveFromEncounterData(EncounterData encounter)
    {
        EnemyWaveSO waveReturned = null;

        if (allowSameEnemyWaveMultipleTimes)
        {
            if (encounter.possibleEnemyEncounters.Count == 1)
            {
                waveReturned = encounter.possibleEnemyEncounters[0];
            }
            else
            {
                waveReturned = encounter.possibleEnemyEncounters[RandomGenerator.NumberBetween(0, encounter.possibleEnemyEncounters.Count - 1)];
            }
        }
        else
        {
            bool foundUnique = false;
            int loops = 0;

            while (foundUnique == false && loops < 20)
            {
                if (encounter.possibleEnemyEncounters.Count == 1)
                {
                    waveReturned = encounter.possibleEnemyEncounters[0];
                }
                else
                {
                    waveReturned = encounter.possibleEnemyEncounters[RandomGenerator.NumberBetween(0, encounter.possibleEnemyEncounters.Count - 1)];
                }

                if (enemyWavesAlreadyEncountered.Contains(waveReturned) == false)
                {
                    foundUnique = true;
                }

                // prevent an endless loop, can only try find a unique encounter 20 times max.
                loops++;
            }
        }

        return waveReturned;
    }
    public void AddEnemyWaveToAlreadyEncounteredList(EnemyWaveSO wave)
    {
        enemyWavesAlreadyEncountered.Add(wave);
    }
    #endregion
}
