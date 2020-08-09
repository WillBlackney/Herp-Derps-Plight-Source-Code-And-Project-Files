using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderController : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static DefenderController Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void EnableDefenderTargetIndicator(LivingEntity defender)
    {
        Debug.Log("DefenderController.ActivateDefenderTargetIndicator() called...");
        defender.defender.myTargetIndicator.EnableView();
    }
    public void DisableDefenderTargetIndicator(LivingEntity defender)
    {
        Debug.Log("DefenderController.ActivateDefenderTargetIndicator() called...");
        defender.defender.myTargetIndicator.DisableView();
    }
    public void DisableAllDefenderTargetIndicators()
    {
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            DisableDefenderTargetIndicator(defender);
        }

        // Disable targeting path lines from all nodes
        foreach(LevelNode node in LevelManager.Instance.allLevelNodes)
        {
            node.DisableAllExtraViews();
        }
    }

}
