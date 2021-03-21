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
    private List<StateData> playerStates = new List<StateData>();

    [Header("State Side Panel Components")]
    [SerializeField] private GameObject statePanelVisualParent;
    [SerializeField] private StateIcon[] allStateIcons;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
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
    public StateData ConvertStateScriptableObjectToStateData(StateDataSO data)
    {
        StateData newState = new StateData();
        newState.stateName = data.stateName;
        newState.rarity = data.rarity;

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
    protected override void Awake()
    {
        base.Awake();
        BuildStateLibrary();
    }
    private void BuildStateLibrary()
    {
        Debug.LogWarning("CardController.BuildStateLibrary() called...");

        List<StateData> tempList = new List<StateData>();

        foreach (StateDataSO dataSO in allStateDataScriptableObjects)
        {
            tempList.Add(ConvertStateScriptableObjectToStateData(dataSO));
        }

        AllStateData = tempList.ToArray();
    }
    public StateData GetStateByName(StateName name)
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
    public StateDataSO GetStateDataByName(StateName name)
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
    public bool DoesPlayerHaveState(StateName state)
    {
        bool bRet = false;
        foreach(StateData s in PlayerStates)
        {
            if(s.stateName == state)
            {
                bRet = true;
                break;
            }
        }
        return bRet;
    }
    public List<StateData> GetAllAvailableStates()
    {
        List<StateData> validStates = new List<StateData>();
        foreach(StateData s in AllStateData)
        {
            if(PlayerStates.Contains(s) == false)
            {
                validStates.Add(s);
            }
        }

        return validStates;
    }
    #endregion

    // State Panel + State Icon Logic
    #region
    private void ShowStatePanel()
    {
        statePanelVisualParent.SetActive(true);
    }
    public void HideStatePanel()
    {
        statePanelVisualParent.SetActive(false);
    }
    public void BuildAndShowStatePanel()
    {
        ResetAllStateIcons();
        ShowStatePanel();
        BuildAllStateIconsFromPlayerStateData();
    }
    private void ResetAllStateIcons()
    {
        foreach(StateIcon icon in allStateIcons)
        {
            icon.visualParent.SetActive(false);
            icon.myStateData = null;
        }
    }
    private void BuildAllStateIconsFromPlayerStateData()
    {
        Debug.Log("StateController.BuildAllStateIconsFromPlayerStateData() called");
        for(int i = 0; i < PlayerStates.Count; i++)
        {
            BuildStateIconFromStateData(allStateIcons[i], PlayerStates[i]);
        }
    }
    private void BuildStateIconFromStateData(StateIcon icon, StateData data)
    {
        Debug.Log("StateController.BuildAllStateIconsFromPlayerStateData() called, building icon from state: " + data.stateName);

        icon.myStateData = data;
        icon.visualParent.SetActive(true);
        icon.stateImage.sprite = data.StateSprite;
        icon.infoPanelDescriptionText.text = TextLogic.ConvertCustomStringListToString(data.customDescription);
        icon.stateNameText.text = TextLogic.SplitByCapitals(data.stateName.ToString());
    }
    #endregion

    // Modify Player States
    #region
    public void GivePlayerState(StateData newState)
    {
        Debug.Log("StateController.GivePlayerState() called, giving state: " + newState.stateName);
        PlayerStates.Add(newState);

        // Rebuild state icon panel
        BuildAllStateIconsFromPlayerStateData();
    }
    #endregion

    // Save + Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData save)
    {
        PlayerStates = save.playerStates;
    }
    public void SaveMyDataToSaveFile(SaveGameData save)
    {
        save.playerStates = PlayerStates;
    }
    #endregion

    // Misc Logic
    #region
    public void BuildPlayerStatesFromGlobalSettingsMockData()
    {
        foreach(StateDataSO state in GlobalSettings.Instance.testingStates)
        {
            GivePlayerState(ConvertStateScriptableObjectToStateData(state));
        }

    }
    #endregion


}

