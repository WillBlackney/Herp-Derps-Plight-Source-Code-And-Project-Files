using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : Singleton<ScoreManager>
{
    // Properties + Components
    #region
    [Header("Game Over Screen Components")]
    [SerializeField] GameObject scoreScreenVisualParent;
    [SerializeField] CanvasGroup scoreScreenCg;
    [SerializeField] ScoreTabView[] allScoreTabs;
    [SerializeField] TextMeshProUGUI ribbonText;

    private PlayerScoreData currentScoreData;
    #endregion

    // Getters + Accessors
    #region
    public PlayerScoreData CurrentScoreData
    {
        get { return currentScoreData; }
        private set { currentScoreData = value; }
    }
    #endregion

    // Persistency 
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        CurrentScoreData = saveFile.scoreData;
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        saveFile.scoreData = CurrentScoreData;
    }
    #endregion

    // Score Screen Logic
    #region
    public void HandleGameOverSequence(GameOverEventType type)
    {
        HandleShowScoreScreenSequence(type);
    }
    private void HandleShowScoreScreenSequence(GameOverEventType type)
    {
        // Is victory or defeat??
        if (type == GameOverEventType.Defeat)
            ribbonText.text = "Defeated...";
        if (type == GameOverEventType.Victory)
            ribbonText.text = "Victory!";

        // Play victory or defeat music effect + stop combat music and related vfx + sfx


        // build all score tabs
        // enable and fade in main view
        // play score tabs one by one like StS
        // animate total/final score text with rolling number maybe?
    }
    private void BuildAllScoreTabs(PlayerScoreData dataSet)
    {

    }
    private void HideAllScoreTabs()
    {
        foreach (ScoreTabView tab in allScoreTabs)
            tab.gameObject.SetActive(false);
    }
    #endregion
}

public enum GameOverEventType
{
    Defeat = 0,
    Victory = 1,
}
