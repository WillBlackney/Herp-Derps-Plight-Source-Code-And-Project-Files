using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Spriter2UnityDX;
using System.Collections;

public class RecruitCharacterController : Singleton<RecruitCharacterController>
{
    // Properties + Component References
    #region
    [Header("Properties")]
    [SerializeField] private CharacterData selectedCharacter;
    [HideInInspector] public List<CharacterData> currentChoices = new List<CharacterData>();

    [Header("Core Components")]
    [SerializeField] private GameObject recruitCharacterVisualParent;
    [SerializeField] private RecruitCharacterWindow[] recruitCharacterWindows;
    [SerializeField] private Button confirmButton;

    [Header("Character Info Popup Components")]
    [SerializeField] private GameObject popUpVisualParent;
    [SerializeField] private CanvasGroup popUpCg;
    [SerializeField] private Scrollbar popUpScrollBar;
    [SerializeField] private TextMeshProUGUI popUpCharacterNameText;
    [SerializeField] private TextMeshProUGUI popUpCharacterClassNameText;
    [SerializeField] private UniversalCharacterModel popUpCharacterModel;
    [SerializeField] private CardInfoPanel racialCardInfoPanel;
    [SerializeField] private TextMeshProUGUI racialNameText;
    [SerializeField] private TextMeshProUGUI racialDescriptionText;
    [SerializeField] private RectTransform characterInfoRect;


    [Header("Card + Panel Components")]
    [SerializeField] private CardInfoPanel[] cardInfoPanels;
    [SerializeField] private TalentInfoPanel[] talentInfoPanels;
    [SerializeField] private CanvasGroup previewCardCg;
    [SerializeField] private CardViewModel previewCardVM;
    #endregion

    // Show + Hide Views
    #region
    public void ResetAllViews()
    {
        // Hide parent
        HideRecruitCharacterScreen();

        // Disable all glows on windows
        foreach (RecruitCharacterWindow w in recruitCharacterWindows)
        {
            w.DisableGlow();
        }

        // Disable character pop up
        HideCharacterInfoPopupWindow();

        // Clear choice
        ClearCharacterChoice();

        // Disable button
        confirmButton.interactable = false;
    }
    public void ShowRecruitCharacterScreen()
    {
        recruitCharacterVisualParent.SetActive(true);
    }
    public void HideRecruitCharacterScreen()
    {
        recruitCharacterVisualParent.SetActive(false);
    }
    private void HideCharacterInfoPopupWindow()
    {
        popUpVisualParent.SetActive(false);
    }
    public void ShowCharacterInfoPopupWindow()
    {
        // Enable and fade in screen
        popUpVisualParent.SetActive(true);
        popUpCg.alpha = 0;
        popUpCg.DOFade(1f, 0.35f);

        // Reset scroll window position
        popUpScrollBar.value = 1;

        // Set model alpha to 0
        EntityRenderer er = popUpCharacterModel.GetComponent<EntityRenderer>();
        er.Color = new Color(er.Color.r, er.Color.g, er.Color.b, 0f);

        // Fade in model
        CharacterEntityController.Instance.FadeInEntityRenderer(er, 3f);
    }
    public void HidePreviewCard()
    {
        previewCardCg.gameObject.SetActive(false);
        previewCardCg.alpha = 0;
    }
    #endregion

    // Build Views
    #region
    private void BuildRecruitCharacterPopupInfoWindow(CharacterData data)
    {
        // Enable window
        ShowCharacterInfoPopupWindow();

        // Set Texts
        popUpCharacterClassNameText.text = data.myName;
        popUpCharacterClassNameText.text = "The " + data.myClassName;

        // Build model
        CharacterModelController.BuildModelFromStringReferences(popUpCharacterModel, data.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(data.itemManager, popUpCharacterModel);

        // Build race section
        BuildRacialInfoPanel(data);

        // Build talent info panels
        BuildTalentInfoPanels(data);

        // Build card info panels
        BuildCardInfoPanels(data);

        // Rebuild layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(characterInfoRect);
    }
    private void BuildCardInfoPanels(CharacterData data)
    {
        // Disable + Reset all card info panels
        for (int i = 0; i < cardInfoPanels.Length; i++)
        {
            cardInfoPanels[i].gameObject.SetActive(false);
            cardInfoPanels[i].copiesCount = 0;
            cardInfoPanels[i].cardDataRef = null;
        }

        // Rebuild panels
        for (int i = 0; i < data.deck.Count; i++)
        { 
            CardInfoPanel matchingPanel = null;
            foreach (CardInfoPanel panel in cardInfoPanels)
            {
                if (panel.cardDataRef != null && panel.cardDataRef.cardName == data.deck[i].cardName)
                {
                    matchingPanel = panel;
                    break;
                }
            }

            if (matchingPanel != null)
            {
                matchingPanel.copiesCount++;
                matchingPanel.copiesCountText.text = "x" + matchingPanel.copiesCount.ToString();
            }
            else
            {
                cardInfoPanels[i].gameObject.SetActive(true);
                cardInfoPanels[i].BuildCardInfoPanelFromCardData(data.deck[i]);
            }
        }

    }
    private void BuildTalentInfoPanels(CharacterData data)
    {
        // Disable + Reset all talent info panels
        for (int i = 0; i < talentInfoPanels.Length; i++)
        {
            talentInfoPanels[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < data.talentPairings.Count; i++)
        {
            talentInfoPanels[i].BuildFromTalentPairingModel(data.talentPairings[i]);
        }

    }
    private void BuildRacialInfoPanel(CharacterData data)
    {
        racialNameText.text = data.race.ToString();
        racialCardInfoPanel.BuildCardInfoPanelFromCardData(CardController.Instance.FindRacialCardData(data.race));
        racialDescriptionText.text = KeywordLibrary.Instance.GetRacialData(data.race).raceDescription;
    }
    public void BuildAndShowCardViewModelPopup(CardData data)
    {
        previewCardCg.gameObject.SetActive(true);
        CardController.Instance.BuildCardViewModelFromCardData(data, previewCardVM);
        previewCardCg.alpha = 0;
        previewCardCg.DOFade(1f, 0.25f);
    }
    public void BuildRecruitCharacterWindows()
    {
        // Setup
        List<CharacterData> selectedCharacters = currentChoices;

        // Build window views
        for (int i = 0; i < recruitCharacterWindows.Length; i++)
        {
            recruitCharacterWindows[i].BuildMyViewsFromTemplate(selectedCharacters[i]);
        }
    }
    public List<CharacterData> GetThreeValidRecruitableCharacters()
    {
        List<CharacterData> recruitableCharacters = new List<CharacterData>();
        List<CharacterData> charactersReturned = new List<CharacterData>();

        foreach (CharacterData charTemplate in CharacterDataController.Instance.AllCharacterTemplates)
        {
            if (IsCharacterRecruitable(charTemplate))
            {
                recruitableCharacters.Add(charTemplate);
            }
        }

        // Choose 3 random recruitable characters
        recruitableCharacters.Shuffle();
        for (int i = 0; i < 3; i++)
        {
            charactersReturned.Add(recruitableCharacters[i]);
        }

        return charactersReturned;
    }
    #endregion

    // Input Listeners
    #region
    public void OnRecruitCharacterWindowClicked(RecruitCharacterWindow window)
    {
        // Disable all glows on windows
        foreach(RecruitCharacterWindow w in recruitCharacterWindows)
        {
            w.DisableGlow();
        }

        // Enable selection glow
        window.EnableGlow();

        // Cache choice
        SetCharacterChoice(window.myTemplateData);
    }
    public void OnRecruitCharacterWindowViewInfoButtonClicked(CharacterData data)
    {
        BuildRecruitCharacterPopupInfoWindow(data);
    }
    public void OnCharacterInfoPopupBackButtonClicked()
    {
        HideCharacterInfoPopupWindow();
    }
    public void OnConfirmChoiceButtonClicked()
    {
        if(selectedCharacter != null)
        {
            HandleCharacterChoiceMade();            
        }
    }
    #endregion
    
    // Misc Logic
    #region
    public bool IsCharacterRecruitable(CharacterData character)
    {
        bool boolReturned = true;

        foreach (CharacterData pCharacter in CharacterDataController.Instance.AllPlayerCharacters)
        {
            // Does player already have this character in their roster?
            if (character.myName == pCharacter.myName)
            {
                // they do, return false and break
                boolReturned = false;
                break;
            }
        }

        return boolReturned;
    }
    #endregion

    // Set, Get + Handle Choice
    #region
    private void HandleCharacterChoiceMade()
    {
        // start recruit character process
        currentChoices.Clear();
        CharacterDataController.Instance.AddNewCharacterToPlayerRoster(selectedCharacter);
        EventSequenceController.Instance.HandleLoadNextEncounter();
    }
    private void SetCharacterChoice(CharacterData character)
    {
        selectedCharacter = character;
        confirmButton.interactable = true;
    }
    private void ClearCharacterChoice()
    {
        selectedCharacter = null;
        confirmButton.interactable = false;
    }
    #endregion

    // Save + Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        currentChoices = saveFile.recruitCharacterChoices;
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        saveFile.recruitCharacterChoices = currentChoices;
    }
    #endregion

}
