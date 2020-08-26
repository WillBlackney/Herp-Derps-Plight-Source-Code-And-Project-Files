using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderController : Singleton<DefenderController>
{
    public void EnableDefenderTargetIndicator(CharacterEntityView view)
    {
        Debug.Log("DefenderController.EnableDefenderTargetIndicator() called...");
        view.myTargetIndicator.EnableView();
    }
    public void DisableDefenderTargetIndicator(CharacterEntityView view)
    {
        Debug.Log("DefenderController.DisableDefenderTargetIndicator() called...");
        view.myTargetIndicator.DisableView();
    }
    public void DisableAllDefenderTargetIndicators()
    {
        foreach(CharacterEntityModel defender in CharacterEntityController.Instance.allDefenders)
        {
            DisableDefenderTargetIndicator(defender.characterEntityView);
        }

        // Disable targeting path lines from all nodes
        foreach(LevelNode node in LevelManager.Instance.allLevelNodes)
        {
            node.DisableAllExtraViews();
        }
    }

}
