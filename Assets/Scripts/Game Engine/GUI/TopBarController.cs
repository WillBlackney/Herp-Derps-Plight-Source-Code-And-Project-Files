using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TopBarController : Singleton<TopBarController>
{
    // Properties + Components
    #region
    [Header("Core Components")]
    [SerializeField] private GameObject visualParent;

    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI currentGoldText;    
    [SerializeField] private TextMeshProUGUI currentEncounterText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Misc Components")]
    [SerializeField] private GameObject goldTopBarImage;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Navigation Button Components")]
    [SerializeField] private GameObject navigationButtonVisualParent;
    [SerializeField] private TextMeshProUGUI navigationButtonText;
    #endregion

    // Getters + Accessors
    #region
    public TextMeshProUGUI CurrentGoldText
    {
        get { return currentGoldText; }
        set { currentGoldText = value; }
    }
    public GameObject GoldTopBarImage
    {
        get { return goldTopBarImage; }
        set { goldTopBarImage = value; }
    }
    public TextMeshProUGUI CurrentDayText
    {
        get { return currentEncounterText; }
        set { currentEncounterText = value; }
    }    
 
    #endregion

    // Core Functions
    #region
    public void ShowNavigationButton()
    {
        navigationButtonVisualParent.SetActive(true);
    }
    public void HideNavigationButton()
    {
        navigationButtonVisualParent.SetActive(false);
    }
    public void SetNavigationButtonText(string newText)
    {
        navigationButtonText.text = newText;
    }
    public void ShowTopBar()
    {
        visualParent.SetActive(true);
    }
    public void HideTopBar()
    {
        visualParent.SetActive(false);
    }
    #endregion
}
