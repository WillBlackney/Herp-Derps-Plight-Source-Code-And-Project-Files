using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JourneyManager : Singleton<JourneyManager>
{
    // Properties + Component Refs
    #region
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI currentEncounterText;
    [SerializeField] private TextMeshProUGUI maxEncounterText;

    [Header("General Properties")]
    [SerializeField] private bool allowSameEnemyWaveMultipleTimes;

    [Header("Encounter Sequence Properties")]
    [SerializeField] private List<EncounterData> encounters;
    private List<EnemyWaveSO> enemyWavesAlreadyEncountered = new List<EnemyWaveSO>();

    [Header("Current Player Position + Encounter Properties")]
    private int currentJourneyPosition = 0;

    public EncounterData CurrentEncounter { get; private set; }
    public EnemyWaveSO CurrentEnemyWave { get; private set; }
    public SaveCheckPoint CheckPointType { get; private set; }
    public void SetCheckPoint(SaveCheckPoint type)
    {
        CheckPointType = type;
    }

    #endregion

    // Getters
    #region
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

    // Initialization + Save/Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveData)
    {
        CurrentJourneyPosition = saveData.currentJourneyPosition;
        CurrentEncounter = saveData.currentEncounter;
        CheckPointType = saveData.saveCheckPoint;

        foreach (EnemyWaveSO eWave in CurrentEncounter.possibleEnemyEncounters)
        {
            if(eWave.encounterName == saveData.currentEnemyWave)
            {
                CurrentEnemyWave = eWave;
            }
        }

        UpdateCurrentEncounterText();
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        saveFile.currentJourneyPosition = CurrentJourneyPosition;
        saveFile.currentEncounter = CurrentEncounter;
        if(CurrentEnemyWave != null)
        {
            saveFile.currentEnemyWave = CurrentEnemyWave.encounterName;
        }        
        saveFile.saveCheckPoint = CheckPointType;
    }

    #endregion

    // Modify Player position
    #region
    public void SetNextEncounterAsCurrentLocation()
    {
        CurrentJourneyPosition++;
        CurrentEncounter = Encounters[CurrentJourneyPosition];
        UpdateCurrentEncounterText();
    }
    private void UpdateCurrentEncounterText()
    {
        currentEncounterText.text = (CurrentJourneyPosition + 1).ToString();
        maxEncounterText.text = encounters.Count.ToString();
    }
    #endregion
    
    // Get + Set Enemy Waves
    #region
    public void SetCurrentEnemyWaveData(EnemyWaveSO wave)
    {
        CurrentEnemyWave = wave;
    }
    public EnemyWaveSO GetRandomEnemyWaveFromEncounterData(EncounterData encounter)
    {
        EnemyWaveSO waveReturned = null;

        if (AllowSameEnemyWaveMultipleTimes)
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
