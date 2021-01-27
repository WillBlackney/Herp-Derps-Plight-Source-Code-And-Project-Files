using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using MapSystem;
using Sirenix.OdinInspector;

public class CharacterSheetViewController : Singleton<CharacterSheetViewController>
{
    // Properties + Component References
    #region
    [Header("Core Properites")]
    private CharacterData currentCharacterViewing;
    private bool mouseIsOverDeckView = false;
    [HideInInspector] public bool currentlyDraggingSomePanel;
    public RosterItemSlot rosterSlotMousedOver = null;

    [Header("Core Components")]
    [SerializeField] private GameObject mainVisualParent;
    [SerializeField] private CanvasGroup mainCg;
    [SerializeField] private TextMeshProUGUI topBarCharacterNameText;
    [SerializeField] private TextMeshProUGUI topBarCharacterClassRaceText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Character Model Box Components")]
    [SerializeField] private UniversalCharacterModel characterModel;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentXpText;
    [SerializeField] private TextMeshProUGUI maxXpText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private RosterItemSlot trinketOneSlot;
    [SerializeField] private RosterItemSlot trinketTwoSlot;
    [SerializeField] private RosterItemSlot[] allItemSlots;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Character Deck Box Components")]
    [SerializeField] private GameObject deckBoxVisualParent;
    [SerializeField] private CardInfoPanel[] cardPanels;
    [SerializeField] private CanvasGroup previewCardCg;
    [SerializeField] private CardViewModel previewCardVM;
    [SerializeField] private CanvasGroup dragDropCg;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Talent Box Components")]
    [SerializeField] private GameObject talentBoxVisualParent;
    [SerializeField] private TextMeshProUGUI talentPointsText;
    [SerializeField] private TalentPanelCharacterRoster[] allTalentPanels;
    [SerializeField] private TalentInfoPanelPopup[] talentPopups;
    [SerializeField] private GameObject talentPopupParent;
    [SerializeField] private CanvasGroup talentPopupCg;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Attribute Box Components")]
    [SerializeField] private GameObject attributeBoxVisualParent;
    [SerializeField] private TextMeshProUGUI attributePointsText;

    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    [SerializeField] private TextMeshProUGUI witsText;
    [SerializeField] private TextMeshProUGUI constitutionText;

    [SerializeField] private GameObject strengthPlusButton;
    [SerializeField] private GameObject intelligencePlusButton;
    [SerializeField] private GameObject dexterityPlusButton;
    [SerializeField] private GameObject witsPlusButton;
    [SerializeField] private GameObject constitutionPlusButton;

    [Header("Traits Box Components")]
    [SerializeField] private GameObject traitsBoxVisualParent;

    #endregion

    // Getters + Accessors
    #region
    public UniversalCharacterModel CharacterModel
    {
        get { return characterModel; }
        private set { characterModel = value; }
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
        MainVisualParent.SetActive(true);
    }
    private void FadeInMainView()
    {
        mainCg.alpha = 0;
        mainCg.DOFade(1, 0.25f);
    }
    public void DisableMainView()
    {
        mainCg.DOKill();
        MainVisualParent.SetActive(false);
    }
    private void BuildFrontPageDefaultViewState(CharacterData character)
    {
        currentCharacterViewing = character;

        // Hide unused boxes
        HideTalentBoxView();
        HideTraitsBoxView();

        // Enable default boxes
        ShowDeckBoxView();
        ShowAttributeBoxView();      

        // Set name header text
        topBarCharacterNameText.text = character.myName;

        // Set class + racial top header text
        topBarCharacterClassRaceText.text = character.race.ToString() + " " + character.myClassName;

        // Build Character Model box views
        BuildCharacterModelBoxFromData(character);

        // Build item slots
        BuildCharacterItemSlotsFromData(character);

        // Build deck box views
        BuildCharacterDeckBoxFromData(character);

        // Build core attributes page
        BuildAttributeBoxFromData(character);

        // Build talents page
        BuildTalentBoxFromData(character);

    }
    #endregion

    // On Button Clicks + Input Listeners
    #region
    public void OnCloseWindowButtonClicked()
    {
        DisableMainView();
    }
    public void OnCharacterPanelViewClicked(CharacterPanelView panel)
    {
        if(MainVisualParent.activeSelf == false)
        {
            EnableMainView();
            FadeInMainView();
        }        
        BuildFrontPageDefaultViewState(panel.characterDataRef);
    }
    public void OnDeckPageButtonClicked()
    {
        HideTalentBoxView();
        ShowDeckBoxView();
    }
    public void OnTalentsPageButtonClicked()
    {
        ShowTalentBoxView();
        HideDeckBoxView();
    }
    public void OnAttributesPageButtonClicked()
    {
        ShowAttributeBoxView();
        HideTraitsBoxView();
    }
    public void OnTraitsPageButtonClicked()
    {
        HideAttributeBoxView();
        ShowTraitsBoxView();
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
        CharacterModelController.Instance.BuildModelFromStringReferences(characterModel, data.modelParts);
        CharacterModelController.Instance.ApplyItemManagerDataToCharacterModelView(data.itemManager, characterModel);

        // Set health + xp texts
        currentHealthText.text = data.health.ToString();
        maxHealthText.text = data.MaxHealthTotal.ToString();
        currentXpText.text = data.currentXP.ToString();
        maxXpText.text = data.currentMaxXP.ToString();
        currentLevelText.text = "Level " + data.currentLevel.ToString();
    }
    public void BuildCharacterItemSlotsFromData(CharacterData data)
    {
        // Reset slots
        foreach (RosterItemSlot ris in allItemSlots)
        {
            ris.itemDataRef = null;
            ris.itemImage.gameObject.SetActive(false);
        }
        
        if (data.itemManager.trinketOne != null)
        {
            BuildCharacterItemSlotFromItemData(trinketOneSlot, data.itemManager.trinketOne);
        }
        if (data.itemManager.trinketTwo != null)
        {
            BuildCharacterItemSlotFromItemData(trinketTwoSlot, data.itemManager.trinketTwo);
        }
    }
    private void BuildCharacterItemSlotFromItemData(RosterItemSlot slot, ItemData itemData)
    {
        slot.itemImage.gameObject.SetActive(true);
        slot.itemImage.sprite = itemData.ItemSprite;
        slot.itemDataRef = itemData;
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
   
    public void HidePreviewCardInDeck()
    {
        previewCardCg.gameObject.SetActive(false);
        previewCardCg.alpha = 0;
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

    // Inventory Logic
    #region
    public bool IsItemValidOnSlot(InventoryItem item, RosterItemSlot slot)
    {
        if (item == null)
        {
            Debug.LogWarning("ITEM IS NULL!!!");
            return false;
        }

        if (slot == null)
        {
            Debug.LogWarning("SLOT IS NULL!!!");
            return false;
        }

        bool bRet = false;
        ItemCategory itemType = item.itemDataRef.itemType;

        if (itemType == ItemCategory.Trinket && (slot.slotType == RosterSlotType.TrinketOne || slot.slotType == RosterSlotType.TrinketTwo))
            bRet = true;
        else if (itemType == ItemCategory.OneHandMelee && (slot.slotType == RosterSlotType.MainHand || slot.slotType == RosterSlotType.OffHand))
            bRet = true;
        else if (itemType == ItemCategory.Shield && slot.slotType == RosterSlotType.OffHand)
            bRet = true;
        else if (itemType == ItemCategory.TwoHandMelee && slot.slotType == RosterSlotType.MainHand)
            bRet = true;
        else if (itemType == ItemCategory.TwoHandRanged && slot.slotType == RosterSlotType.MainHand)
            bRet = true;

        Debug.LogWarning("IsItemValidOnSlot() returning " + bRet.ToString());

        return bRet;

    }

    #endregion

    // Traits Box Logic
    #region
    private void ShowTraitsBoxView()
    {
        traitsBoxVisualParent.SetActive(true);
    }
    private void HideTraitsBoxView()
    {
        traitsBoxVisualParent.SetActive(false);
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
        if (currentCharacterViewing.talentPoints > 0)
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
            if (tpm.talentLevel == 2)
            {
                panel.SetPlusButtonActiveState(false);
            }
        }

        if (currentCharacterViewing.talentPoints == 0)
        {
            foreach (TalentPanelCharacterRoster tpc in allTalentPanels)
            {
                tpc.SetPlusButtonActiveState(false);
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
        foreach (TalentPanelCharacterRoster panel in allTalentPanels)
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
                if (panel.TalentSchool == tp.talentSchool)
                {
                    talentLevel = tp.talentLevel;

                    // Set tier text
                    panel.SetTierText(tp.talentLevel.ToString());

                    break;
                }
            }

            // Should enable plus button?
            if (character.talentPoints > 0 && talentLevel < 2)
            {
                panel.SetPlusButtonActiveState(true);
            }
        }
    }
    #endregion

    // Attribute Box Logic
    #region
    private void ShowAttributeBoxView()
    {
        attributeBoxVisualParent.SetActive(true);
    }
    private void HideAttributeBoxView()
    {
        attributeBoxVisualParent.SetActive(false);
    }
    private void BuildAttributeBoxFromData(CharacterData character)
    {
        BuildAttributePanelsFromCharacterData(character);
        SetAttributePointsText(character.attributePoints.ToString());
    }
    private void BuildAttributePanelsFromCharacterData(CharacterData character)
    {
        // Disable all plus buttons
        strengthPlusButton.SetActive(false);
        intelligencePlusButton.SetActive(false);
        dexterityPlusButton.SetActive(false);
        witsPlusButton.SetActive(false);
        constitutionPlusButton.SetActive(false);

        strengthText.text = character.strength.ToString();
        if (character.strength > 10)
        {
            strengthText.text = TextLogic.ReturnColoredText(character.strength.ToString(), TextLogic.neutralYellow);
        }

        intelligenceText.text = character.intelligence.ToString();
        if (character.intelligence > 10)
        {
            intelligenceText.text = TextLogic.ReturnColoredText(character.intelligence.ToString(), TextLogic.neutralYellow);
        }

        dexterityText.text = character.dexterity.ToString();
        if (character.dexterity > 10)
        {
            dexterityText.text = TextLogic.ReturnColoredText(character.dexterity.ToString(), TextLogic.neutralYellow);
        }

        witsText.text = character.wits.ToString();
        if (character.wits > 10)
        {
            witsText.text = TextLogic.ReturnColoredText(character.wits.ToString(), TextLogic.neutralYellow);
        }

        constitutionText.text = character.constitution.ToString();
        if (character.constitution > 10)
        {
            constitutionText.text = TextLogic.ReturnColoredText(character.constitution.ToString(), TextLogic.neutralYellow);
        }

        // Enable plus buttons
        if (character.strength < 20 && character.attributePoints > 0)
            strengthPlusButton.SetActive(true);

        if (character.wits < 20 && character.attributePoints > 0)
            witsPlusButton.SetActive(true);

        if (character.dexterity < 20 && character.attributePoints > 0)
            dexterityPlusButton.SetActive(true);

        if (character.intelligence < 20 && character.attributePoints > 0)
            intelligencePlusButton.SetActive(true);

        if (character.constitution < 20 && character.attributePoints > 0)
            constitutionPlusButton.SetActive(true);
    }
    private void SetAttributePointsText(string text)
    {
        attributePointsText.text = text;
    }
    public void OnStrengthPanelPlusButtonClicked()
    {
        if (currentCharacterViewing.attributePoints > 0 && currentCharacterViewing.strength < 20)
        {
            // VFX + SFX
            VisualEffectManager.Instance.CreateSmallMeleeImpact(strengthPlusButton.transform.position, 27000);
            AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);

            // Deduct talent points
            CharacterDataController.Instance.ModifyCharacterAttributePoints(currentCharacterViewing, -1);

            // Gain new attribute
            CharacterDataController.Instance.ModifyStrength(CurrentCharacterViewing, 1);

            // Update gui            
            attributePointsText.text = currentCharacterViewing.attributePoints.ToString();
            BuildAttributePanelsFromCharacterData(CurrentCharacterViewing);
        }
    }
    public void OnIntelligencePanelPlusButtonClicked()
    {
        if (currentCharacterViewing.attributePoints > 0 && currentCharacterViewing.intelligence < 20)
        {
            // VFX + SFX
            VisualEffectManager.Instance.CreateSmallMeleeImpact(intelligencePlusButton.transform.position, 27000);
            AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);

            // Deduct talent points
            CharacterDataController.Instance.ModifyCharacterAttributePoints(currentCharacterViewing, -1);

            // Gain new attribute
            CharacterDataController.Instance.ModifyIntelligence(CurrentCharacterViewing, 1);

            // Update gui            
            attributePointsText.text = currentCharacterViewing.attributePoints.ToString();
            BuildAttributePanelsFromCharacterData(CurrentCharacterViewing);
        }
    }
    public void OnDexterityPanelPlusButtonClicked()
    {
        if (currentCharacterViewing.attributePoints > 0 && currentCharacterViewing.dexterity < 20)
        {
            // VFX + SFX
            VisualEffectManager.Instance.CreateSmallMeleeImpact(dexterityPlusButton.transform.position, 27000);
            AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);

            // Deduct talent points
            CharacterDataController.Instance.ModifyCharacterAttributePoints(currentCharacterViewing, -1);

            // Gain new attribute
            CharacterDataController.Instance.ModifyDexterity(CurrentCharacterViewing, 1);

            // Update gui            
            attributePointsText.text = currentCharacterViewing.attributePoints.ToString();
            BuildAttributePanelsFromCharacterData(CurrentCharacterViewing);
        }
    }
    public void OnWitsPanelPlusButtonClicked()
    {
        if (currentCharacterViewing.attributePoints > 0 && currentCharacterViewing.wits < 20)
        {
            // VFX + SFX
            VisualEffectManager.Instance.CreateSmallMeleeImpact(witsPlusButton.transform.position, 27000);
            AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);

            // Deduct talent points
            CharacterDataController.Instance.ModifyCharacterAttributePoints(currentCharacterViewing, -1);

            // Gain new attribute
            CharacterDataController.Instance.ModifyWits(CurrentCharacterViewing, 1);

            // Update gui            
            attributePointsText.text = currentCharacterViewing.attributePoints.ToString();
            BuildAttributePanelsFromCharacterData(CurrentCharacterViewing);
        }
    }
    public void OnConstitutionPanelPlusButtonClicked()
    {
        if (currentCharacterViewing.attributePoints > 0 && currentCharacterViewing.constitution < 20)
        {
            // VFX + SFX
            VisualEffectManager.Instance.CreateSmallMeleeImpact(constitutionPlusButton.transform.position, 27000);
            AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);

            // Deduct talent points
            CharacterDataController.Instance.ModifyCharacterAttributePoints(currentCharacterViewing, -1);

            // Gain new attribute
            CharacterDataController.Instance.ModifyConstitution(CurrentCharacterViewing, 1);

            // Update gui            
            maxHealthText.text = currentCharacterViewing.MaxHealthTotal.ToString();
            attributePointsText.text = currentCharacterViewing.attributePoints.ToString();
            BuildAttributePanelsFromCharacterData(CurrentCharacterViewing);
        }
    }
    #endregion
}
