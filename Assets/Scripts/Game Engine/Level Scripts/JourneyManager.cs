using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyManager : Singleton<JourneyManager>
{
    // Properties + Component Refs
    #region
    [Header("Encounter Sequence Properties")]
    [SerializeField] private List<EncounterData> encounters;

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
}
