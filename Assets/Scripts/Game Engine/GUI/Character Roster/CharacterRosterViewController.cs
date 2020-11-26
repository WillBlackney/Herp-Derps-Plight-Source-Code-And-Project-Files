﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CharacterRosterViewController : Singleton<CharacterRosterViewController>
{
    // Properties + Component References
    #region
    [Header("Core Properites")]
    private CharacterData currentCharacterViewing;
    private bool mouseIsOverDeckView = false;
    [HideInInspector] public bool currentlyDraggingSomePanel;

    [Header("Core Components")]
    [SerializeField] private GameObject mainVisualParent;
    [SerializeField] private CanvasGroup mainCg;
    [SerializeField] private TextMeshProUGUI topBarCharacterNameText;

    [Header("Character Model Box Components")]
    [SerializeField] private UniversalCharacterModel characterModel;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentXpText;
    [SerializeField] private TextMeshProUGUI maxXpText;
    [SerializeField] private TextMeshProUGUI currentLevelText;

    [Header("Character Deck Box Components")]
    [SerializeField] private GameObject deckBoxVisualParent;
    [SerializeField] private CardInfoPanel[] cardPanels;
    [SerializeField] private CanvasGroup previewCardCg;
    [SerializeField] private CardViewModel previewCardVM;
    [SerializeField] private CanvasGroup dragDropCg;

    [Header("Card Inventory Box Components")]
    [SerializeField] private GameObject cardInventoryVisualParent;
    [SerializeField] private InventoryCardSlot[] inventoryCardSlots;
    [SerializeField] private CanvasGroup previewCardInventoryCg;
    [SerializeField] private CardViewModel previewInventoryCardVM;
    [SerializeField] private Transform dragParent;

    [Header("Talent Box Components")]
    [SerializeField] private GameObject talentBoxVisualParent;
    [SerializeField] private TextMeshProUGUI talentPointsText;
    [SerializeField] private TalentPanelCharacterRoster[] allTalentPanels;
    [SerializeField] private TalentInfoPanelPopup[] talentPopups;
    [SerializeField] private GameObject talentPopupParent;
    [SerializeField] private CanvasGroup talentPopupCg;


    #endregion

    // Getters + Accessors
    #region
    public InventoryCardSlot[] InventoryCardSlots
    {
        get { return inventoryCardSlots; }
        private set { inventoryCardSlots = value; }
    }
    public Transform DragParent
    {
        get { return dragParent; }
        private set { dragParent = value; }
    }
    public GameObject MainVisualParent
    {
        get { return mainVisualParent; }
        private set { mainVisualParent = value; }
    }
    public GameObject DeckBoxVisualParent
    {
        get { return deckBoxVisualParent; }
        private set { deckBoxVisualParent = value; }
    }
    public bool MouseIsOverDeckView
    {
        get { return mouseIsOverDeckView; }
        private set { mouseIsOverDeckView = value; }
    }
    public CharacterData CurrentCharacterViewing
    {
        get { return currentCharacterViewing; }
        private set { currentCharacterViewing = value; }
    }
    #endregion


    // Enable + Fade Main View and Default View
    #region
    private void EnableMainView()
    {
        mainVisualParent.SetActive(true);
    }
    private void FadeInMainView()
    {
        mainCg.alpha = 0;
        mainCg.DOFade(1, 0.25f);
    }
    private void DisableMainView()
    {
        mainCg.DOKill();
        mainVisualParent.SetActive(false);
    }
    private void BuildFrontPageDefaultViewState(CharacterData character)
    {
        currentCharacterViewing = character;

        // Hide unused boxes
        HideTalentBoxView();

        // Enable default boxes
        ShowDeckBoxView();
        ShowCardInventoryBoxView();

        // Set name header text
        topBarCharacterNameText.text = character.myName;

        // Build Character Model box views
        BuildCharacterModelBoxFromData(currentCharacterViewing);

        // Build deck box views
        BuildCharacterDeckBoxFromData(currentCharacterViewing);

        // Build card invetory box views
        BuildInventoryCardsBoxView();
    }
    #endregion

    // On Button Clicks + Input Listeners
    #region
    public void OnCharacterRosterButtonClicked()
    {
        if(mainVisualParent.activeSelf == true)
        {
            DisableMainView();         
        }
        else if (mainVisualParent.activeSelf == false)
        {
            EnableMainView();
            FadeInMainView();
            BuildFrontPageDefaultViewState(CharacterDataController.Instance.AllPlayerCharacters[0]);
        }
    }
    public void OnNextCharacterButtonClicked()
    {
        List<CharacterData> allCharacters = CharacterDataController.Instance.AllPlayerCharacters;
        CharacterData nextCharacter = null;

        // Do nothing if player only has 1 character
        if (allCharacters.Count == 1)
        {
            return;
        }
        else if (allCharacters.Count > 1)
        {
            int index = allCharacters.IndexOf(currentCharacterViewing);

            if(index == allCharacters.Count - 1)
            {
                nextCharacter = allCharacters[0];
            }
            else
            {
                nextCharacter = allCharacters[index + 1];
            }
        }

        BuildFrontPageDefaultViewState(nextCharacter);
    }
    public void OnPreviousCharacterButtonClicked()
    {
        List<CharacterData> allCharacters = CharacterDataController.Instance.AllPlayerCharacters;
        CharacterData previousCharacter = null;

        // Do nothing if player only has 1 character
        if (allCharacters.Count == 1)
        {
            return;
        }
        else if (allCharacters.Count > 1)
        {
            int index = allCharacters.IndexOf(currentCharacterViewing);

            if (index == 0)
            {
                previousCharacter = allCharacters[allCharacters.Count -1];
            }
            else
            {
                previousCharacter = allCharacters[index - 1];
            }
        }

        BuildFrontPageDefaultViewState(previousCharacter);
    }
    public void OnDeckPageButtonClicked()
    {
        // Hide unused boxes
        HideTalentBoxView();

        // Enable deck related boxes
        ShowDeckBoxView();
        ShowCardInventoryBoxView();
    }
    public void OnTalentPageButtonClicked()
    {
        // Hide unused boxes
        HideDeckBoxView();
        HideCardInventoryBoxView();

        // Enable talent related boxes
        ShowTalentBoxView();

        // Build all views
        BuildTalentBoxFromData(currentCharacterViewing);
    }
    public void OnMouseEnterDeckViewBox()
    {
        mouseIsOverDeckView = true;
    }
    public void OnMouseExitDeckViewBox()
    {
        mouseIsOverDeckView = false;
    }
    #endregion

    // Model Box Logic
    #region
    private void BuildCharacterModelBoxFromData(CharacterData data)
    {
        // Build character model
        CharacterModelController.BuildModelFromStringReferences(characterModel, data.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(data.itemManager, characterModel);

        // Set health + xp texts
        currentHealthText.text = data.health.ToString();
        maxHealthText.text = data.maxHealth.ToString();
        currentXpText.text = data.currentXP.ToString();
        maxXpText.text = data.currentMaxXP.ToString();
        currentLevelText.text = "Level " + data.currentLevel.ToString();
    }
    #endregion

    // Deck Box Logic
    #region
    private void ShowDeckBoxView()
    {
        deckBoxVisualParent.SetActive(true);
    }
    private void HideDeckBoxView()
    {
        deckBoxVisualParent.SetActive(false);
    }
    private void BuildInventoryCardsBoxView()
    {
        BuildInventoryCardSlots();
    }
    private void BuildInventoryCardSlots()
    {
        // Disable + Reset all inventory slots
        for (int i = 0; i < InventoryCardSlots.Length; i++)
        {
            // Reset position, reassign parent
            InventoryCardSlots[i].cardInfoPanel.transform.SetParent(InventoryCardSlots[i].transform);
            InventoryCardSlots[i].cardInfoPanel.transform.localPosition = Vector3.zero;

            // Disable view and clear data
            InventoryCardSlots[i].gameObject.SetActive(false);
            InventoryCardSlots[i].cardInfoPanel.cardDataRef = null;
        }

        // Rebuild all panels based on current card inventory
        for(int i = 0; i < InventoryController.Instance.CardInventory.Count; i++)
        {
            InventoryCardSlots[i].gameObject.SetActive(true);
            InventoryCardSlots[i].cardInfoPanel.BuildCardInfoPanelFromCardData(InventoryController.Instance.CardInventory[i]);
            InventoryCardSlots[i].cardInfoPanel.myInventorySlot = InventoryCardSlots[i];
        }
       
    }
    public void BuildCharacterDeckBoxFromData(CharacterData data)
    {
        BuildCardInfoPanels(data.deck);
    }
    private void BuildCardInfoPanels(List<CardData> data)
    {
        // Disable + Reset all card info panels
        for (int i = 0; i < cardPanels.Length; i++)
        {
            cardPanels[i].gameObject.SetActive(false);
            cardPanels[i].copiesCount = 0;
            cardPanels[i].cardDataRef = null;
        }

        // Rebuild panels
        for (int i = 0; i < data.Count; i++)
        {
            CardInfoPanel matchingPanel = null;
            foreach (CardInfoPanel panel in cardPanels)
            {
                if (panel.cardDataRef != null && panel.cardDataRef.cardName == data[i].cardName)
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
                cardPanels[i].gameObject.SetActive(true);
                cardPanels[i].BuildCardInfoPanelFromCardData(data[i]);
            }
        }
    }
    public void BuildAndShowCardViewModelPopupFromDeck(CardData data)
    {
        previewCardCg.gameObject.SetActive(true);
        CardData cData = CardController.Instance.GetCardDataFromLibraryByName(data.cardName);
        CardController.Instance.BuildCardViewModelFromCardData(cData, previewCardVM);
        previewCardCg.alpha = 0;
        previewCardCg.DOFade(1f, 0.25f);
    }
    public void BuildAndShowCardViewModelPopupFromInventory(CardData data)
    {
        previewCardInventoryCg.gameObject.SetActive(true);
        CardData cData = CardController.Instance.GetCardDataFromLibraryByName(data.cardName);
        CardController.Instance.BuildCardViewModelFromCardData(cData, previewInventoryCardVM);
        previewCardInventoryCg.alpha = 0;
        previewCardInventoryCg.DOFade(1f, 0.25f);
    }
    public void HidePreviewCardInDeck()
    {
        previewCardCg.gameObject.SetActive(false);
        previewCardCg.alpha = 0;
    }
    public void HidePreviewCardInInventory()
    {
        previewCardInventoryCg.gameObject.SetActive(false);
        previewCardInventoryCg.alpha = 0;
    }
    public void StartDragDropAnimation()
    {
        dragDropCg.gameObject.SetActive(true);
        dragDropCg.alpha = 0;
        dragDropCg.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void StopDragDropAnimation()
    {
        dragDropCg.DOKill();
        dragDropCg.alpha = 0;
        dragDropCg.gameObject.SetActive(false);
    }
    #endregion

    // Card Inventory Logic
    #region

    private void ShowCardInventoryBoxView()
    {
        cardInventoryVisualParent.SetActive(true);
    }
    private void HideCardInventoryBoxView()
    {
        cardInventoryVisualParent.SetActive(false);
    }
    #endregion

    // Talent Box Logic
    #region
    public void OnTalentPanelMouseEnter(TalentPanelCharacterRoster panel)
    {
        BuildAndShowTalentPopups(panel.TalentSchool);
    }
    public void OnTalentPanelMouseExit(TalentPanelCharacterRoster panel)
    {
        HideTalentPopups();
    }
    private void BuildAndShowTalentPopups(TalentSchool talentSchool)
    {
        // Reset view state
        talentPopupCg.DOKill();
        talentPopupCg.alpha = 0;
        talentPopupParent.SetActive(true);      

        // Build panel views
        talentPopups[0].BuildMe(TextLogic.GetTalentPairingTierOneDescriptionText(talentSchool));
        talentPopups[1].BuildMe(TextLogic.GetTalentPairingTierTwoDescriptionText(talentSchool));

        // Fade in
        talentPopupCg.DOFade(1, 0.25f);
    }
    private void HideTalentPopups()
    {
        // Reset view state
        talentPopupCg.DOKill();
        talentPopupCg.alpha = 0;
        talentPopupParent.SetActive(false);
    }
    public void OnTalentPanelPlusButtonClicked(TalentPanelCharacterRoster panel)
    {
        if(currentCharacterViewing.talentPoints > 0)
        {
            // VFX + SFX
            VisualEffectManager.Instance.CreateSmallMeleeImpact(panel.PlusButtonParent.transform.position, 27000);
            AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);           

            // Deduct talent points
            CharacterDataController.Instance.ModifyCharacterTalentPoints(currentCharacterViewing, -1);

            // Gain new talent
            TalentPairingModel tpm = CharacterDataController.Instance.HandlePlayerGainTalent(currentCharacterViewing, panel.TalentSchool, 1);

            // Update gui            
            talentPointsText.text = currentCharacterViewing.talentPoints.ToString();
            panel.SetTierText(tpm.talentLevel.ToString());
            if(tpm.talentLevel == 2)
            {
                panel.SetPlusButtonActiveState(false);
            }
        }      

    }
    private void ShowTalentBoxView()
    {
        talentBoxVisualParent.SetActive(true);
    }
    private void HideTalentBoxView()
    {
        talentBoxVisualParent.SetActive(false);
    }
    private void BuildTalentBoxFromData(CharacterData character)
    {
        BuildTalentPanelsFromCharacterData(character);
        SetTalentPointsText(character.talentPoints.ToString());
    }
    private void SetTalentPointsText(string text)
    {
        talentPointsText.text = text;
    }
    private void BuildTalentPanelsFromCharacterData(CharacterData character)
    {
        foreach(TalentPanelCharacterRoster panel in allTalentPanels)
        {
            // Reset views
            panel.SetTierText("0");
            panel.SetPlusButtonActiveState(false);

            // Set talent image 
            panel.SetMyImage(SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(panel.TalentSchool));

            // Set name text
            panel.SetTalentNameText(panel.TalentSchool.ToString());

            // cache talent level
            int talentLevel = 0;

            // Find the character's matching talent pairing, if they have it
            foreach (TalentPairingModel tp in character.talentPairings)
            {
                if(panel.TalentSchool == tp.talentSchool)
                {
                    talentLevel = tp.talentLevel;

                    // Set tier text
                    panel.SetTierText(tp.talentLevel.ToString());

                    break;
                }
            }

            // SHould enable plus button?
            if(character.talentPoints > 0 && talentLevel < 2)
            {
                panel.SetPlusButtonActiveState(true);
            }
        }
    }
    #endregion

}
