using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    // Properties + Component References
    #region
    [Header("Gold Properties + Components")]
    private int currentGold;

    [Header("Gold Text Animation Properties")]
    private bool animIsActive = false;
    #endregion

    // Getters + Accessors
    #region
    public int CurrentGold
    {
        get { return currentGold; }
        private set { currentGold = value; }
    }
   
    #endregion

    // Save + Load + Persistency Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData loadData)
    {
        // Set gold to 0
        ModifyCurrentGold(-CurrentGold);

        // Gain gold from load data
        ModifyCurrentGold(loadData.currentGold);
    }
    public void SaveMyDataToSaveFile(SaveGameData loadData)
    {
        loadData.currentGold = CurrentGold;
    }
    #endregion

    // Modify Gold
    #region
    public void ModifyCurrentGold(int gainedOrLost, bool playTextAnim = false, bool updateScoring = false)
    {
        // Increment gold value
        CurrentGold += gainedOrLost;

        // Prevent gold going negativa
        if(CurrentGold < 0)
        {
            CurrentGold = 0;
        }

        // Prevent gold going over max limit
        else if(CurrentGold > 999)
        {
            CurrentGold = 999;
        }

        // Update top bar gold text
        SetCurrentGoldText(CurrentGold.ToString(), playTextAnim);

        if (updateScoring && gainedOrLost > 0)
        {
            ScoreManager.Instance.IncrementGoldGained(gainedOrLost);
        }
    }
    private void SetCurrentGoldText(string newValue, bool playTextAnim = false)
    {
        if (playTextAnim)
        {
            if(TopBarController.Instance.CurrentGoldText.text == "")
            {
                TopBarController.Instance.CurrentGoldText.text = "0";
            }

            int from = Convert.ToInt32(TopBarController.Instance.CurrentGoldText.text);
            DoRollingGoldTextAnimation(from, CurrentGold);
        }
        else
        {
            TopBarController.Instance.CurrentGoldText.text = newValue;
        }
       
    }
    #endregion

    // Visual Logic + Events
    #region
    public void DoRollingGoldTextAnimation(int from, int to)
    {
        StartCoroutine(DoRollingGoldTextAnimationCoroutine(from, to));
    }
    private IEnumerator DoRollingGoldTextAnimationCoroutine(int from, int to)
    {
        // TO DO!: if gaining gold, create 'CHA CHING' kind of SFX
        animIsActive = false;
        SetCurrentGoldText(from.ToString());
        int current = from;

        yield return null;
        animIsActive = true;

        if(from > to)
        {
            while (animIsActive && current != to)
            {
                SetCurrentGoldText(current.ToString());
                current--;
                yield return null;
            }
        }

        else if (from < to)
        {
            while (animIsActive && current != to)
            {
                SetCurrentGoldText(current.ToString());
                current++;
                yield return null;
            }
        }



        if (animIsActive)
        {
            SetCurrentGoldText(current.ToString());
        }
      
    }
    #endregion
}
