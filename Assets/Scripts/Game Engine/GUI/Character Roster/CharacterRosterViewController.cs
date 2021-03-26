using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using MapSystem;
using Sirenix.OdinInspector;

public class CharacterRosterViewController : Singleton<CharacterRosterViewController>
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
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Character Model Box Components")]
    [SerializeField] private UniversalCharacterModel characterModel;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI currentXpText;
    [SerializeField] private TextMeshProUGUI maxXpText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private RosterItemSlot mainHandItemSlot;
    [SerializeField] private RosterItemSlot offHandItemSlot;
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

    [Header("Inventory Box Components")]
    [SerializeField] private GameObject inventoryVisualParent;
    [SerializeField] private InventoryCardSlot[] inventoryCardSlots;
    [SerializeField] private InventoryItemSlot[] inventoryItemSlots;
    [SerializeField] private CanvasGroup previewCardInventoryCg;
    [SerializeField] private CardViewModel previewInventoryCardVM;
    [SerializeField] private CanvasGroup previewCardInventoryItemCg;
    [SerializeField] private CardViewModel previewCardInventoryItemVM;
    [SerializeField] private CanvasGroup previewCardRosterItemCg;
    [SerializeField] private CardViewModel previewCardRosterItemVM;
    [SerializeField] private Transform dragParent;
    [SerializeField] private GameObject cardInventoryParent;
    [SerializeField] private GameObject itemInventoryParent;
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
    [SerializeField] private TextMeshProUGUI attributeRollCountText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    [SerializeField] private TextMeshProUGUI witsText;
    [SerializeField] private TextMeshProUGUI constitutionText;

    [Header("Increase Attribute Page Components")]
    [SerializeField] private GameObject increaseAttributePageVisualParent;
    [SerializeField] private CanvasGroup increaseAttributePageCg;
    [SerializeField] private AttributePlusButton[] attributePlusButtons;
    private List<AttributePlusButton> selectedAttributes = new List<AttributePlusButton>();

    #endregion

    // Getters + Accessors
    #region
    public UniversalCharacterModel CharacterModel
    {
        get { return characterModel; }
        private set { characterModel = value; }
    }
    public InventoryCardSlot[] InventoryCardSlots
    {
        get { return inventoryCardSlots; }
        private set { inventoryCardSlots = value; }
    }
    public InventoryItemSlot[] InventoryItemSlots
    {
        get { return inventoryItemSlots; }
        private set { inventoryItemSlots = value; }
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
        HideAttributeBoxView();

        // Enable default boxes
        ShowDeckBoxView();
        ShowInventoryBoxView();
        ShowCardInventory();
        HideItemInventory();

        // Set name header text
        topBarCharacterNameText.text = character.myName;

        // Build Character Model box views
        BuildCharacterModelBoxFromData(currentCharacterViewing);

        // Build item slots
        BuildCharacterItemSlotsFromData(character);

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
        else if (MainVisualParent.activeSelf == false)
        {
            EnableMainView();
            FadeInMainView();
            BuildFrontPageDefaultViewState(CharacterDataController.Instance.AllPlayerCharacters[0]);

            if (MapView.Instance.MasterMapParent.activeSelf)
            {
                MapView.Instance.HideMainMapView();
            }
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
        HideAttributeBoxView();

        // Enable deck related boxes
        ShowDeckBoxView();
        ShowInventoryBoxView();
        ShowCardInventory();
        HideItemInventory();
    }
    public void OnTalentPageButtonClicked()
    {
        // Hide unused boxes
        HideDeckBoxView();
        HideInventoryBoxView();

        // Enable talent + attribute  boxes
        ShowTalentBoxView();
        ShowAttributeBoxView();

        // Build all views
        BuildTalentBoxFromData(currentCharacterViewing);
        BuildAttributeBoxFromData(CurrentCharacterViewing);
    }
    public void OnItemInventoryButtonClicked()
    {
        if(itemInventoryParent.activeSelf == false)
        {            
            HideCardInventory();
            ShowItemInventory();
            BuildInventoryItemSlots();
        }
    }
    public void OnCardInventoryButtonClicked()
    {
        if (cardInventoryParent.activeSelf == false)
        {
            HideItemInventory();
            ShowCardInventory();            
        }
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
        foreach(RosterItemSlot ris in allItemSlots)
        {
            ris.itemDataRef = null;
            ris.itemImage.gameObject.SetActive(false);
        }

        if(data.itemManager.mainHandItem != null)
        {
            BuildCharacterItemSlotFromItemData(mainHandItemSlot, data.itemManager.mainHandItem);
        }
        if (data.itemManager.offHandItem != null)
        {
            BuildCharacterItemSlotFromItemData(offHandItemSlot, data.itemManager.offHandItem);
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
    public void BuildInventoryItemSlots()
    {
        // Disable + Reset all inventory slots
        for (int i = 0; i < InventoryItemSlots.Length; i++)
        {
            // Reset position, reassign parent
            InventoryItemSlots[i].myItem.transform.SetParent(InventoryItemSlots[i].transform);
            InventoryItemSlots[i].myItem.transform.localPosition = Vector3.zero;

            // Disable view and clear data
            InventoryItemSlots[i].gameObject.SetActive(false);
            InventoryItemSlots[i].myItem.itemDataRef = null;
        }

        // Rebuild all panels based on current card inventory
        for (int i = 0; i < InventoryController.Instance.ItemInventory.Count; i++)
        {
            InventoryItemSlots[i].gameObject.SetActive(true);
            InventoryItemSlots[i].myItem.gameObject.SetActive(true);
            InventoryItemSlots[i].myItem.BuildInventoryItemFromItemData(InventoryController.Instance.ItemInventory[i]);
            //InventoryItemSlots[i].myItem.myInventorySlot = InventoryItemSlots[i];
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
    public void BuildAndShowCardViewModelPopupFromInventoryItem(ItemData item)
    {
        previewCardInventoryItemCg.gameObject.SetActive(true);
        CardController.Instance.BuildCardViewModelFromItemData(item, previewCardInventoryItemVM);
        previewCardInventoryItemCg.alpha = 0;
        previewCardInventoryItemCg.DOFade(1f, 0.25f);
    }
    public void BuildAndShowCardViewModelPopupFromRosterItem(ItemData item)
    {
        previewCardRosterItemCg.gameObject.SetActive(true);
        CardController.Instance.BuildCardViewModelFromItemData(item, previewCardRosterItemVM);
        previewCardRosterItemCg.alpha = 0;
        previewCardRosterItemCg.DOFade(1f, 0.25f);
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
    public void HidePreviewItemCardInInventory()
    {
        previewCardInventoryItemCg.gameObject.SetActive(false);
        previewCardInventoryItemCg.alpha = 0;
    }
    public void HidePreviewItemCardInRoster()
    {
        previewCardRosterItemCg.gameObject.SetActive(false);
        previewCardRosterItemCg.alpha = 0;
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
        if(item == null)
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
        ItemType itemType = item.itemDataRef.itemType;

        if (itemType == ItemType.Trinket && (slot.slotType == RosterSlotType.TrinketOne || slot.slotType == RosterSlotType.TrinketTwo))
            bRet = true;
        else if (itemType == ItemType.OneHandMelee && (slot.slotType == RosterSlotType.MainHand || slot.slotType == RosterSlotType.OffHand))
            bRet = true;
        else if (itemType == ItemType.Shield && slot.slotType == RosterSlotType.OffHand)
            bRet = true;
        else if (itemType == ItemType.TwoHandMelee && slot.slotType == RosterSlotType.MainHand)
            bRet = true;
        else if (itemType == ItemType.TwoHandRanged && slot.slotType == RosterSlotType.MainHand)
            bRet = true;

        Debug.LogWarning("IsItemValidOnSlot() returning " + bRet.ToString());

        return bRet;
            
    }
    private void ShowInventoryBoxView()
    {
        inventoryVisualParent.SetActive(true);
    }
    private void HideInventoryBoxView()
    {
        inventoryVisualParent.SetActive(false);
    }
    private void ShowCardInventory()
    {
        cardInventoryParent.SetActive(true);
    }
    private void HideCardInventory()
    {
        cardInventoryParent.SetActive(false);
    }
    private void ShowItemInventory()
    {
        itemInventoryParent.SetActive(true);
    }
    private void HideItemInventory()
    {
        itemInventoryParent.SetActive(false);
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

        if(currentCharacterViewing.talentPoints == 0)
        {
            foreach(TalentPanelCharacterRoster tpc in allTalentPanels)
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

            // Should enable plus button?
            if(character.talentPoints > 0 && talentLevel < 2)
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
        SetAttributeRollCountText(character.attributeRollResults.Count.ToString());
    }
    private void BuildAttributePanelsFromCharacterData(CharacterData character)
    {
        strengthText.text = character.strength.ToString();
        if (character.strength > 20)        
            strengthText.text = TextLogic.ReturnColoredText(character.strength.ToString(), TextLogic.neutralYellow);          
        else if (character.strength < 20)        
            strengthText.text = TextLogic.ReturnColoredText(character.strength.ToString(), TextLogic.redText);        

        intelligenceText.text = character.intelligence.ToString();
        if (character.intelligence > 20)        
            intelligenceText.text = TextLogic.ReturnColoredText(character.intelligence.ToString(), TextLogic.neutralYellow);              
        else if (character.intelligence < 20)
            intelligenceText.text = TextLogic.ReturnColoredText(character.intelligence.ToString(), TextLogic.redText);        

        dexterityText.text = character.dexterity.ToString();
        if (character.dexterity > 20)        
            dexterityText.text = TextLogic.ReturnColoredText(character.dexterity.ToString(), TextLogic.neutralYellow);          
        else if (character.dexterity < 20)
            dexterityText.text = TextLogic.ReturnColoredText(character.dexterity.ToString(), TextLogic.redText);        

        witsText.text = character.wits.ToString();
        if (character.wits > 20)        
            witsText.text = TextLogic.ReturnColoredText(character.wits.ToString(), TextLogic.neutralYellow);            
        else if (character.wits < 20)
            witsText.text = TextLogic.ReturnColoredText(character.wits.ToString(), TextLogic.redText);        

        constitutionText.text = character.constitution.ToString();
        if (character.constitution > 20)        
            constitutionText.text = TextLogic.ReturnColoredText(character.constitution.ToString(), TextLogic.neutralYellow);        
        else if (character.constitution < 20)
            constitutionText.text = TextLogic.ReturnColoredText(character.constitution.ToString(), TextLogic.redText);
        
    }   
    
    #endregion

    // Attribute Level Up Screen logic
    #region
    private void SetAttributeRollCountText(string text)
    {
        attributeRollCountText.text = text;
    }
    private void ShowAttributeLevelUpPage()
    {
        increaseAttributePageVisualParent.SetActive(true);
        increaseAttributePageCg.DOKill();
        increaseAttributePageCg.alpha = 0;
        increaseAttributePageCg.DOFade(1, 0.25f);
    }
    private void HideAttributeLevelUpPage()
    {
        increaseAttributePageCg.DOKill();
        increaseAttributePageCg.alpha = 1;
        Sequence s = DOTween.Sequence();
        s.Append(increaseAttributePageCg.DOFade(0, 0.25f));
        s.OnComplete(() => increaseAttributePageVisualParent.SetActive(false));
    }
    public void OnConfirmAttributeSelectionButtonClicked()
    {
        // check if player has selected at least 2 stats
        if (selectedAttributes.Count != 2)
            return;

        // cache refs
        CharacterData character = CurrentCharacterViewing;
        AttributeRollResult roll = character.attributeRollResults[0];

        // Apply attribute changes to character
        foreach (AttributePlusButton button in selectedAttributes)
        {
            if (button.MyAttribute == CoreAttribute.Strength)
                CharacterDataController.Instance.ModifyStrength(character, roll.strengthRoll);
            else if (button.MyAttribute == CoreAttribute.Intelligence)
                CharacterDataController.Instance.ModifyIntelligence(character, roll.intelligenceRoll);
            else if (button.MyAttribute == CoreAttribute.Dexterity)
                CharacterDataController.Instance.ModifyDexterity(character, roll.dexterityRoll);
            else if (button.MyAttribute == CoreAttribute.Wits)
                CharacterDataController.Instance.ModifyWits(character, roll.witsRoll);
            else if (button.MyAttribute == CoreAttribute.Constitution)
                CharacterDataController.Instance.ModifyConstitution(character, roll.constitutionRoll);
        }

        // remove roll result from character
        character.attributeRollResults.Remove(roll);
        selectedAttributes.Clear();

        // close and reset GUI views
        SetAttributeRollCountText(character.attributeRollResults.Count.ToString());
        HideAttributeLevelUpPage();
        ResetAllAttributePlusButtons();
        BuildAttributeBoxFromData(character);
        BuildCharacterModelBoxFromData(character);
    }
    public void OnCancelAttributeSelectionButtonClicked()
    {
        selectedAttributes.Clear();
        ResetAllAttributePlusButtons();
        HideAttributeLevelUpPage();
    }
    public void OnAttributePlusButtonClicked(AttributePlusButton button)
    {
        if (button.isPressed == false)
        {
            if (selectedAttributes.Count == 2)
                return;

            selectedAttributes.Add(button);
            button.isPressed = true;
            button.buttonImage.color = button.pressedColor;

            VisualEffectManager.Instance.CreateSmallMeleeImpact(button.transform.position, 27000);
            AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff);
        }
        else
        {
            selectedAttributes.Remove(button);
            button.isPressed = false;
            button.buttonImage.color = button.normalColor;
        }


    }
    public void OnAttributeRollCountTextClicked()
    {
        if (CurrentCharacterViewing.attributeRollResults.Count > 0)
        {
            Debug.Log("Current character viewing = " + CurrentCharacterViewing.myName +
                ", total unused rolls = " + CurrentCharacterViewing.attributeRollResults.Count.ToString());
            selectedAttributes.Clear();
            BuildAttributePlusButtonsFromRollResult(CurrentCharacterViewing.attributeRollResults[0]);
            ShowAttributeLevelUpPage();
        }
    }
    private void BuildAttributePlusButtonsFromRollResult(AttributeRollResult roll)
    {
        ResetAllAttributePlusButtons();

        Debug.Log("Roll build: Strength = " + roll.strengthRoll.ToString() +
           ", Intelligence = " + roll.intelligenceRoll.ToString() + ", Dexterity = " + roll.dexterityRoll.ToString() +
           ", Wits = " + roll.witsRoll.ToString() + ", Constitution = " + roll.constitutionRoll.ToString());

        GetButtonByAttribute(CoreAttribute.Strength).pointsText.text = "+" + roll.strengthRoll.ToString();
        GetButtonByAttribute(CoreAttribute.Intelligence).pointsText.text = "+" + roll.intelligenceRoll.ToString();
        GetButtonByAttribute(CoreAttribute.Dexterity).pointsText.text = "+" + roll.dexterityRoll.ToString();
        GetButtonByAttribute(CoreAttribute.Wits).pointsText.text = "+" + roll.witsRoll.ToString();
        GetButtonByAttribute(CoreAttribute.Constitution).pointsText.text = "+" + roll.constitutionRoll.ToString();
    }
    private AttributePlusButton GetButtonByAttribute(CoreAttribute attribute)
    {
        Debug.Log("CharacterSheetViewController() called, searching for " + attribute.ToString() + " button");
        AttributePlusButton button = null;
        foreach (AttributePlusButton apb in attributePlusButtons)
        {
            if (apb.MyAttribute == attribute)
            {
                button = apb;
                break;
            }
        }

        return button;
    }
    private void ResetAllAttributePlusButtons()
    {
        foreach (AttributePlusButton apb in attributePlusButtons)
        {
            apb.buttonImage.color = apb.normalColor;
            apb.isPressed = false;
        }
    }
    #endregion
}
