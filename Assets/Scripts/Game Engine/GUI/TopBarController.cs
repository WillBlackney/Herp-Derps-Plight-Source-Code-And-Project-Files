using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class TopBarController : Singleton<TopBarController>
{
    // Properties + Components
    #region
    [Header("Core Components")]
    [SerializeField] private GameObject visualParent;

    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI currentGoldText;    
    [SerializeField] private TextMeshProUGUI currentEncounterText;
    [SerializeField] private TextMeshProUGUI maxEncounterText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Main Button Components")]
    [SerializeField] private GameObject characterRosterButton;
    [SerializeField] private Image characterRosterButtonGlow;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Misc Components")]
    [SerializeField] private GameObject goldTopBarImage;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    private bool charRosterGlowIsPlaying = false;
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
    public TextMeshProUGUI CurrentEncounterText
    {
        get { return currentEncounterText; }
        set { currentEncounterText = value; }
    }
    public TextMeshProUGUI MaxEncounterText
    {
        get { return maxEncounterText; }
        set { maxEncounterText = value; }
    }
    public GameObject CharacterRosterButton
    {
        get { return characterRosterButton; }
        set { characterRosterButton = value; }
    }
    #endregion

    // Core Functions
    #region
    public void ShowTopBar()
    {
        visualParent.SetActive(true);
    }
    public void HideTopBar()
    {
        visualParent.SetActive(false);
    }
    public void ShowCharacterRosterButtonGlow()
    {
        if(charRosterGlowIsPlaying == false)
        {
            charRosterGlowIsPlaying = true;
            characterRosterButtonGlow.DOKill();
            characterRosterButtonGlow.DOFade(0.33f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
      
    }
    public void HideCharacterRosterButtonGlow()
    {
        charRosterGlowIsPlaying = false;
        characterRosterButtonGlow.DOKill();
        characterRosterButtonGlow.DOFade(0, 0);
    }
    #endregion
}
