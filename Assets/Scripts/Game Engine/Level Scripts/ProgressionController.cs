using System.Collections;
using System.Collections.Generic;

public class ProgressionController : Singleton<ProgressionController>
{
    // Properties + Component Refs
    #region   
    private int dayNumber = 0;

    #endregion

    // Getters + Accessors
    #region
    public CombatData CurrentCombatData { get; private set; }
    public List<CharacterData> ChosenCombatCharacters { get; private set; }
    public SaveCheckPoint CheckPointType { get; private set; }
    public CombatChoicesResult DailyCombatChoices { get; private set; }
    public int DayNumber { get { return dayNumber; } private set { dayNumber = value; } }
  
    #endregion

    // Initialization + Save/Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveData)
    {
        SetDayNumber(saveData.dayNumber);
        SetCheckPoint(saveData.saveCheckPoint);
        SetCurrentCombat(saveData.currentCombatData);
        SetChosenCombatCharacters(saveData.chosenCombatCharacters);
        SetDailyCombatChoices(saveData.dailyCombatChoices);
        UpdateCurrentDayText();
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        saveFile.dayNumber = DayNumber;
        saveFile.currentCombatData = CurrentCombatData;
        saveFile.chosenCombatCharacters = ChosenCombatCharacters;
        saveFile.saveCheckPoint = CheckPointType;
        saveFile.dailyCombatChoices = DailyCombatChoices;
    }

    #endregion

    // Modify Day + Progression
    #region   
    public void SetCheckPoint(SaveCheckPoint type)
    {
        CheckPointType = type;
    }   
    private void UpdateCurrentDayText()
    {
        TopBarController.Instance.CurrentDayText.text = DayNumber.ToString();
    }
    #endregion
    
    // Get + Set Enemy Waves + etc
    #region
    public void SetChosenCombatCharacters(List<CharacterData> characters)
    {
        ChosenCombatCharacters = characters;
    }
    public void SetCurrentCombat(CombatData wave)
    {
        CurrentCombatData = wave;
    }
    public void SetDayNumber(int newValue)
    {
        DayNumber = newValue;
        UpdateCurrentDayText();
    }
    public void SetDailyCombatChoices(CombatChoicesResult result)
    {
        DailyCombatChoices = result;
    }
   
    #endregion
}
