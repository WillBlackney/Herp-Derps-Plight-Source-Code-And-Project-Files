using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : Singleton<StateController>
{
    // Properties + Components
    #region
    [Header("State Library Properties")]
    [SerializeField] private StateDataSO[] allStateDataScriptableObjects;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    private StateData[] allStateData;

    [Header("Player States")]
    private List<StateData> playerStates;
    #endregion

    // Accessors + Getters
    #region
    public StateData[] AllStateData
    {
        get { return allStateData; }
        private set { allStateData = value; }
    }
    public List<StateData> PlayerStates
    {
        get { return playerStates; }
        private set { playerStates = value; }
    }
    #endregion

    // Conversion Logic
    #region
    public StateData ConvertStateSoToStateData(StateDataSO data)
    {
        StateData newState = new StateData();
        newState.stateName = data.stateName;

        // Keyword Model Data
        newState.keyWordModels = new List<KeyWordModel>();
        foreach (KeyWordModel kwdm in data.keyWordModels)
        {
            newState.keyWordModels.Add(ObjectCloner.CloneJSON(kwdm));
        }

        // Custom string Data
        newState.customDescription = new List<CustomString>();
        foreach (CustomString cs in data.customDescription)
        {
            newState.customDescription.Add(ObjectCloner.CloneJSON(cs));
        }

        return newState;
    }
    #endregion

    // Library Logic
    #region
    public StateData GetStateByName(string name)
    {
        StateData sRet = null;

        foreach(StateData sd in  AllStateData)
        {
            if(sd.stateName == name)
            {
                sRet = sd;
                break;
            }
        }

        if (sRet == null)
            Debug.LogWarning("StateController.GetStateByName() did not find a state matching the search term: " +
                name + ", returning null...");

        return sRet;
    }
    public StateDataSO GetStateDataByName(string name)
    {
        StateDataSO sRet = null;

        foreach (StateDataSO sd in allStateDataScriptableObjects)
        {
            if (sd.stateName == name)
            {
                sRet = sd;
                break;
            }
        }

        if (sRet == null)
            Debug.LogWarning("StateController.GetStateByName() did not find a state matching the search term: " +
                name + ", returning null...");

        return sRet;
    }
    #endregion

    // Save + Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData save)
    {
        save.playerStates = PlayerStates;
    }
    public void SaveMyDataToSaveFile(SaveGameData save)
    {
        PlayerStates = save.playerStates;
    }
    #endregion


}

