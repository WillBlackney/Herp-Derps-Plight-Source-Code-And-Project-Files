using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JourneyManager : Singleton<JourneyManager>
{
    // Properties + Component Refs
    #region
    [Header("Components")]   

    [Header("General Properties")]
    [SerializeField] private bool allowSameEnemyWaveMultipleTimes;

    [Header("Encounter Sequence Properties")]
    [SerializeField] private List<EncounterData> encounters;
    private List<EnemyWaveSO> enemyWavesAlreadyEncountered = new List<EnemyWaveSO>();

    [Header("Current Player Position + Encounter Properties")]
    private int currentJourneyPosition = 0;

    [Header("Basic Enemy Encounters ")]
    [SerializeField] private EncounterData stageOneBasicEnemyEncounters;
    [SerializeField] private EncounterData stageTwoBasicEnemyEncounters;
    [SerializeField] private EncounterData stageThreeBasicEnemyEncounters;
    [SerializeField] private EncounterData stageFourBasicEnemyEncounters;
    [SerializeField] private EncounterData stageFiveBasicEnemyEncounters;

    [Header("Elite Enemy Encounters")]
    [SerializeField] private EncounterData stageOneEliteEnemyEncounters;
    [SerializeField] private EncounterData stageTwoEliteEnemyEncounters;
    [SerializeField] private EncounterData stageThreeEliteEnemyEncounters;

    [Header("Boss Enemy Encounters")]
    [SerializeField] private EncounterData stageOneBossEnemyEncounters;
    #endregion

    // Getters + Accessors
    #region
    public EncounterType CurrentEncounter { get; private set; }
    public EnemyWaveSO CurrentEnemyWave { get; private set; }
    public SaveCheckPoint CheckPointType { get; private set; }
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
    public List<EnemyWaveSO> AllEnemyWaves()
    {
        List<EnemyWaveSO> waves = new List<EnemyWaveSO>();
        waves.AddRange(stageOneBasicEnemyEncounters.possibleEnemyEncounters);
        waves.AddRange(stageTwoBasicEnemyEncounters.possibleEnemyEncounters);
        waves.AddRange(stageThreeBasicEnemyEncounters.possibleEnemyEncounters);
        waves.AddRange(stageFourBasicEnemyEncounters.possibleEnemyEncounters);
        waves.AddRange(stageFiveBasicEnemyEncounters.possibleEnemyEncounters);

        waves.AddRange(stageOneEliteEnemyEncounters.possibleEnemyEncounters);
        waves.AddRange(stageTwoEliteEnemyEncounters.possibleEnemyEncounters);
        waves.AddRange(stageThreeEliteEnemyEncounters.possibleEnemyEncounters);

        return waves;
    }
  

    #endregion

    // Initialization + Save/Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveData)
    {
        CurrentJourneyPosition = saveData.currentJourneyPosition;
        SetCurrentEncounterType(saveData.currentEncounter);
        CheckPointType = saveData.saveCheckPoint;

        foreach (EnemyWaveSO eWave in AllEnemyWaves())
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
    public void SetCurrentEncounterType(EncounterType type)
    {
        CurrentEncounter = type;
    }
    public void SetCheckPoint(SaveCheckPoint type)
    {
        CheckPointType = type;
    }
    public void IncrementWorldMapPosition()
    {
        CurrentJourneyPosition++;
        //CurrentEncounter = Encounters[CurrentJourneyPosition];
        UpdateCurrentEncounterText();
    }
    private void UpdateCurrentEncounterText()
    {
        TopBarController.Instance.CurrentEncounterText.text = (CurrentJourneyPosition + 1).ToString();
        TopBarController.Instance.MaxEncounterText.text = encounters.Count.ToString();
    }
    #endregion
    
    // Get + Set Enemy Waves
    #region
    public void SetCurrentEnemyWaveData(EnemyWaveSO wave)
    {
        CurrentEnemyWave = wave;
    }
    public EnemyWaveSO DetermineAndGetNextBasicEnemyWave()
    {
        EnemyWaveSO waveReturned = null;

        if(CurrentJourneyPosition >= 0 && CurrentJourneyPosition <= 2)
        {
            waveReturned = GetRandomEnemyWaveFromEncountersDataSet(stageOneBasicEnemyEncounters);
        }

        // recruit 2nd character here

        else if (CurrentJourneyPosition >= 4 && CurrentJourneyPosition <= 5)
        {
            waveReturned = GetRandomEnemyWaveFromEncountersDataSet(stageTwoBasicEnemyEncounters);
        }
        else if (CurrentJourneyPosition >= 6 && CurrentJourneyPosition <= 7)
        {
            waveReturned = GetRandomEnemyWaveFromEncountersDataSet(stageThreeBasicEnemyEncounters);
        }

        // recruit 3rd character here
        else if (CurrentJourneyPosition >= 9 && CurrentJourneyPosition <= 10)
        {
            waveReturned = GetRandomEnemyWaveFromEncountersDataSet(stageFourBasicEnemyEncounters);
        }

        return waveReturned;
    }
    public EnemyWaveSO DetermineAndGetNextEliteEnemyWave()
    {
        EnemyWaveSO waveReturned = null;

        if (CharacterDataController.Instance.AllPlayerCharacters.Count == 1)
        {
            waveReturned = GetRandomEnemyWaveFromEncountersDataSet(stageOneEliteEnemyEncounters);
        }
        else if (CharacterDataController.Instance.AllPlayerCharacters.Count == 2)
        {
            waveReturned = GetRandomEnemyWaveFromEncountersDataSet(stageTwoEliteEnemyEncounters);
        }
        if (CharacterDataController.Instance.AllPlayerCharacters.Count == 3)
        {
            waveReturned = GetRandomEnemyWaveFromEncountersDataSet(stageThreeEliteEnemyEncounters);
        }

        return waveReturned;
    }
    public EnemyWaveSO DetermineAndGetNextBossEnemyWave()
    {      
        return GetRandomEnemyWaveFromEncountersDataSet(stageOneBossEnemyEncounters);
    }
    public EnemyWaveSO GetRandomEnemyWaveFromEncountersDataSet(EncounterData encounter)
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
        Debug.Log("JourneyManager.AddEnemyWaveToAlreadyEncounteredList() adding " + wave.encounterName + " to already encounter list");
        enemyWavesAlreadyEncountered.Add(wave);
    }
    #endregion
}
