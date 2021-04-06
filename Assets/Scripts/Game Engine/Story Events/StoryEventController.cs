using DG.Tweening;
using MapSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryEventController : Singleton<StoryEventController>
{
    // Properties + Components
    #region
    [Header("Core Data")]
    [SerializeField] private List<StoryEventDataSO> allStoryEvents;

    [Header("UI Components")]
    [SerializeField] GameObject mainVisualParent;
    [SerializeField] TextMeshProUGUI eventNameHeaderText;
    [SerializeField] TextMeshProUGUI eventDescriptionText;
    [SerializeField] StoryChoiceButton[] allChoiceButtons;
    [SerializeField] StoryEventCharacterButton[] allCharacterButtons;

    [Header("Choice Button Colours")]
    [SerializeField] Color normalColour;
    [SerializeField] Color mouseOverColour;
    [SerializeField] Color lockedColour;

    // Misc fields
    private List<string> eventsAlreadyEncountered = new List<string>();
    private StoryEventCharacterButton selectedCharacterButton;
    [HideInInspector] public CardData selectedUpgradeCard;


    #endregion

    // Getters + Accessors
    #region
    public List<StoryEventDataSO> AllStoryEvents
    {
        get { return allStoryEvents; }
        private set { allStoryEvents = value; }
    }
    public StoryEventDataSO CurrentStoryEvent { get; private set; }
    public bool AwaitingCardUpgradeChoice { get; private set; }
    public bool AwaitingCardRemovalChoice { get; private set; }
    #endregion

    // Save + Load Logic
    #region
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        if (CurrentStoryEvent != null)
            saveFile.currentStoryEvent = CurrentStoryEvent.storyEventName;

        saveFile.encounteredStoryEvents = eventsAlreadyEncountered;
    }
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        foreach (StoryEventDataSO s in AllStoryEvents)
        {
            if (s.name == saveFile.currentStoryEvent)
            {
                CurrentStoryEvent = s;
                break;
            }
        }

        eventsAlreadyEncountered = saveFile.encounteredStoryEvents;
    }
    public void ClearCurrentStoryEvent()
    {
        CurrentStoryEvent = null;
    }
    #endregion

   

    // Generate Next Event Logic
    #region
    public void GenerateAndCacheNextStoryEventRandomly()
    {
        List<StoryEventDataSO> validEvents = GetValidStoryEvents();
        CurrentStoryEvent = validEvents[RandomGenerator.NumberBetween(0, validEvents.Count - 1)];
        eventsAlreadyEncountered.Add(CurrentStoryEvent.storyEventName);
    }
    public void ForceCacheNextStoryEvent(StoryEventDataSO storyEvent)
    {
        // FUNCTION IS FOR TESTING ONLY, NEVER USE IN GAME
        CurrentStoryEvent = storyEvent;
    }
    public List<StoryEventDataSO> GetValidStoryEvents()
    {
        List<StoryEventDataSO> listReturned = new List<StoryEventDataSO>();
        foreach (StoryEventDataSO s in AllStoryEvents)
        {
            if (IsStoryEventValid(s))
                listReturned.Add(s);
        }

        return listReturned;
    }
    #endregion

    // Main Views + UI Logic
    #region
    private void ShowMainScreen()
    {
        mainVisualParent.SetActive(true);
    }
    public void HideMainScreen()
    {
        mainVisualParent.SetActive(false);
    }
    public void BuildAllViewsOnStoryEventStart(StoryEventDataSO storyEvent)
    {
        ShowMainScreen();
        eventNameHeaderText.text = storyEvent.storyEventName;
        BuildAllCharacterButtons();
        BuildAllViewsFromPage(storyEvent.firstPage);
        HandleNewCharacterSelection(allCharacterButtons[0]);
    }
    private void BuildAllViewsFromPage(StoryEventPageSO page)
    {
        // set description text
        eventDescriptionText.text = page.pageDescription;

        // set up buttons and their views
        BuildChoiceButtonsFromPageData(page);
    }
    private void BuildChoiceButtonsFromPageData(StoryEventPageSO page)
    {
        ResetAllChoiceButtons();
        for(int i = 0; i < page.allChoices.Length; i++)
        {
            BuildChoiceButtonFromChoiceData(allChoiceButtons[i], page.allChoices[i]);
        }
    }
    private void BuildChoiceButtonFromChoiceData(StoryChoiceButton button, StoryEventChoiceSO data)
    {
        button.gameObject.SetActive(true);
        button.myData = data;

        // Build Description Text
        button.activityDescriptionText.text = "";
        if(DoesChoiceHaveRequirement(data, StoryChoiceReqType.TalentLevel))
        {
            StoryChoiceRequirement rd = FindRequirementInChoiceData(data, StoryChoiceReqType.TalentLevel);
            button.activityDescriptionText.text += TextLogic.ReturnColoredText("[" + rd.talent.ToString() + " " + rd.talentLevel.ToString() + "+] ", TextLogic.neutralYellow);
        }
        else if (DoesChoiceHaveRequirement(data, StoryChoiceReqType.AttributeLevel))
        {
            StoryChoiceRequirement rd = FindRequirementInChoiceData(data, StoryChoiceReqType.TalentLevel);
            button.activityDescriptionText.text += TextLogic.ReturnColoredText("[" + rd.attribute.ToString() + " " + rd.attributeLevel.ToString() + "+] ", TextLogic.neutralYellow);
        }

        button.activityDescriptionText.text += ("[" + data.activityDescription + "] ");
        button.activityDescriptionText.text += TextLogic.ReturnColoredText( data.effectDescription, TextLogic.lightGreen);

        // Reset colouring
        button.buttonBG.color = normalColour;
    }
    private void RebuildChoiceButtonsOnNewCharacterSelected(CharacterData character)
    {
        foreach(StoryChoiceButton s in allChoiceButtons)
        {
            if(s.myData != null &&
                DoesCharacterMeetAllChoiceRequirements(character, s.myData.requirements))
            {
                s.locked = false;
                s.buttonBG.color = normalColour;
                s.HideLock();
            }
            else
            {
                s.locked = true;
                s.buttonBG.color = lockedColour;
                s.ShowLock();
            }
        }
    }
    private void ResetAllChoiceButtons()
    {
        foreach (StoryChoiceButton b in allChoiceButtons)
        {
            b.gameObject.SetActive(false);
            b.myData = null;
        }
    }
    #endregion

    // Character Button Logic
    #region
    private void BuildAllCharacterButtons()
    {
        // hide + reset buttons
        foreach(StoryEventCharacterButton b in allCharacterButtons)        
            b.visualParent.SetActive(false);
        
        for(int i = 0; i < CharacterDataController.Instance.AllPlayerCharacters.Count; i++)
        {
            BuildCharacterButtonFromCharacterData(allCharacterButtons[i], CharacterDataController.Instance.AllPlayerCharacters[i]);
        }
    }
    private void BuildCharacterButtonFromCharacterData(StoryEventCharacterButton button, CharacterData data)
    {
        button.visualParent.SetActive(true);
        button.myCharacter = data;

        // build ucm
        CharacterModelController.Instance.BuildModelFromStringReferences(button.myUCM, data.modelParts);
        button.myUCM.SetBaseAnim();
    }
    private void HandleNewCharacterSelection(StoryEventCharacterButton button)
    {
        if (selectedCharacterButton != null)
        {
            selectedCharacterButton.scalingParent.transform.DOKill();
            selectedCharacterButton.scalingParent.transform.DOScale(1, 0.2f).SetEase(Ease.OutSine);
        }

        selectedCharacterButton = button;
        selectedCharacterButton.scalingParent.transform.DOKill();
        selectedCharacterButton.scalingParent.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutSine);

        // refresh + rebuild choices
        RebuildChoiceButtonsOnNewCharacterSelected(selectedCharacterButton.myCharacter);
    }

    #endregion

    // Character Button Input Listeners
    #region
    public void OnCharacterButtonClicked(StoryEventCharacterButton button)
    {
        AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Clicked);
        HandleNewCharacterSelection(button);
    }
    public void OnCharacterButtonMouseEnter(StoryEventCharacterButton button)
    {
        button.myGlowOutline.DOKill();
        button.myGlowOutline.DOFade(0f, 0f);
        button.myGlowOutline.DOFade(1f, 0.2f);
        AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);
    }
    public void OnCharacterButtonMouseExit(StoryEventCharacterButton button)
    {
        button.myGlowOutline.DOKill();
        button.myGlowOutline.DOFade(0f, 0f);
    }
    #endregion

    // Choice Button Input Listeners
    #region
    public void OnChoiceButtonClicked(StoryChoiceButton button)
    {
        if(button.locked == false)
        {
            button.buttonBG.DOKill();
            button.buttonBG.DOColor(normalColour, 0f);
            AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Clicked);

            // EXECUTE CHOICE EFFECTS!
            HandleChoiceEffects(button.myData);
        }
      
    }
    public void OnChoiceButtonMouseEnter(StoryChoiceButton button)
    {
        if (button.locked == false)
        {
            button.buttonBG.DOKill();
            button.buttonBG.DOColor(mouseOverColour, 0.2f);
            AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);
        }           
    }
    public void OnChoiceButtonMouseExit(StoryChoiceButton button)
    {
        if (button.locked == false)
        {
            button.buttonBG.DOKill();
            button.buttonBG.DOColor(normalColour, 0f);
        }
          
    }
    #endregion

    // Handle Choice Effects Logic
    #region
    private void HandleChoiceEffects(StoryEventChoiceSO choice)
    {
        foreach(StoryChoiceEffect s in choice.effects)
        {
            TriggerChoiceEffect(s, selectedCharacterButton.myCharacter);
        }
    }
    private void TriggerChoiceEffect(StoryChoiceEffect effect, CharacterData character = null)
    {
        // Setup targets of effect
        List<CharacterData> targets = new List<CharacterData>();
        targets.Add(selectedCharacterButton.myCharacter);
        if (effect.target == ChoiceEffectTarget.AllCharacters)
            targets = CharacterDataController.Instance.AllPlayerCharacters;

        // Load page
        if (effect.effectType == StoryChoiceEffectType.LoadPage)
        {
            BuildAllViewsFromPage(effect.pageToLoad);
            HandleNewCharacterSelection(selectedCharacterButton);
        }

        // Finish event
        else if (effect.effectType == StoryChoiceEffectType.FinishEvent)
        {
            MapPlayerTracker.Instance.UnlockMap();
            MapView.Instance.OnWorldMapButtonClicked();
        }

        // Upgrade a card
        else if (effect.effectType == StoryChoiceEffectType.UpgradeCard)
        {
            AwaitingCardUpgradeChoice = true;
            CardController.Instance.CreateNewUpgradeCardInDeckPopup(character, "Upgrade A Card!");
            CardController.Instance.DisableCardGridScreenBackButton();
        }

        // Remove a card
        else if (effect.effectType == StoryChoiceEffectType.RemoveCard)
        {
            AwaitingCardRemovalChoice = true;
            CardController.Instance.CreateNewRemoveCardInDeckPopup(character, "Remove A Card!");
            CardController.Instance.DisableCardGridScreenBackButton();
        }

        // Heal
        else if (effect.effectType == StoryChoiceEffectType.GainHealth)
        {
            foreach (CharacterData c in targets)
            {
                // Heal to max health
                if(effect.healType == HealType.HealMaximum)
                {
                    int healAmount = c.MaxHealthTotal - c.health;
                    CharacterDataController.Instance.SetCharacterHealth(selectedCharacterButton.myCharacter, healAmount);

                    // Heal VFX
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                        VisualEffectManager.Instance.CreateHealEffect(selectedCharacterButton.transform.position, 5000, 2f));

                    // Damage Text Effect VFX
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateDamageEffect(selectedCharacterButton.transform.position, healAmount, true, false));

                    // Create SFX
                    VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff));
                }
            }
        }
        else if (effect.effectType == StoryChoiceEffectType.GainItem)
        {
            List<ItemData> itemsGained = new List<ItemData>();

            if(effect.itemRewardType == ItemRewardType.SpecificItem)
            {
                for (int i = 0; i < effect.totalItemsGained; i++)
                    itemsGained.Add(ItemController.Instance.GetItemDataByName(effect.itemGained.itemName));
            }
            else if (effect.itemRewardType == ItemRewardType.RandomItem)
            {
                for (int i = 0; i < effect.totalItemsGained; i++)
                    itemsGained.Add(ItemController.Instance.GetRandomLootableItem(Rarity.Common));
            }

            // Get random item
            List<ItemData> vEventList = new List<ItemData>();
            vEventList.AddRange(itemsGained);

            // Add to inventory
            foreach(ItemData i in itemsGained)
                InventoryController.Instance.AddItemToInventory(i);

            // Add item to inventory visual event pop up
            CardController.Instance.StartNewShuffleCardsScreenVisualEvent(TopBarController.Instance.CharacterRosterButton.transform.position, vEventList);
        }
    }
    public void HandleUpgradeCardChoiceMade(CardData card)
    {
        // Setup
        List<CardData> cList = new List<CardData>();
        cList.Add(CardController.Instance.FindUpgradedCardData(card));

        // Close Grid view Screen
        CardController.Instance.HideCardGridScreen();
        CardController.Instance.HideCardUpgradePopupScreen();

        // Create add card to character visual event
        CardController.Instance.StartNewShuffleCardsScreenVisualEvent(selectedCharacterButton.myUCM, cList);

        // Add new upgraded card to character data deck
        CardController.Instance.HandleUpgradeCardInCharacterDeck(card, selectedCharacterButton.myCharacter);

        // Finish
        AwaitingCardUpgradeChoice = false;
        selectedUpgradeCard = null;
    }
    public void HandleRemoveCardChoiceMade(CardData card)
    {
        // Setup
        List<CardData> cList = new List<CardData>();
        cList.Add(card);

        // Close Grid view Screen
        CardController.Instance.HideCardGridScreen();
        CardController.Instance.HideCardUpgradePopupScreen();

        // Create add card to character visual event
        CardController.Instance.StartNewShuffleCardsScreenExpendVisualEvent(cList);

        // Remove the card from character deck persistency
        CharacterDataController.Instance.RemoveCardFromCharacterDeck(selectedCharacterButton.myCharacter, card);

        // Finish
        AwaitingCardRemovalChoice = false;
    }
    #endregion

    // Conditional Checks + Bools
    #region
    private bool IsStoryEventValid(StoryEventDataSO storyEvent)
    {
        if (IsStoryEventInStageRound(storyEvent) &&
             DoesStoryEventMeetItsRequirements(storyEvent) &&
             eventsAlreadyEncountered.Contains(storyEvent.storyEventName) == false)
        {
            return true;
        }
        else
            return false;
    }
    private bool IsStoryEventInStageRound(StoryEventDataSO storyEvent)
    {
        int count = CharacterDataController.Instance.AllPlayerCharacters.Count;

        if ((storyEvent.stageOne && count == 1 ) ||
            (storyEvent.stageTwo && count == 2) ||
            (storyEvent.stageThree && count == 3))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool DoesStoryEventMeetItsRequirements(StoryEventDataSO storyEvent)
    {
        // TO DO: some events will require non stage related things, for example
        // at least 1 character with 30+ health, or player must have 100+ gold, etc
        return true;
    }
    private bool DoesChoiceHaveRequirement(StoryEventChoiceSO choice, StoryChoiceReqType requirement)
    {
        bool bRet = false;
        foreach(StoryChoiceRequirement scr in choice.requirements)
        {
            if(scr.requirementType == requirement)
            {
                bRet = true;
                break;
            }
        }

        return bRet;
    }
    private StoryChoiceRequirement FindRequirementInChoiceData(StoryEventChoiceSO choice, StoryChoiceReqType requirement)
    {
        StoryChoiceRequirement bRet = null;
        foreach (StoryChoiceRequirement scr in choice.requirements)
        {
            if (scr.requirementType == requirement)
            {
                bRet = scr;
                break;
            }
        }

        return bRet;
    }
    private bool DoesCharacterMeetChoiceRequirement(CharacterData character, StoryChoiceRequirement req)
    {
        bool bRet = false;

        if(req.requirementType == StoryChoiceReqType.AttributeLevel)
        {
            if( 
                (req.attribute == CoreAttribute.Strength && character.strength >= req.attributeLevel) ||
                (req.attribute == CoreAttribute.Intelligence && character.intelligence >= req.attributeLevel) ||
                (req.attribute == CoreAttribute.Dexterity && character.dexterity >= req.attributeLevel) ||
                (req.attribute == CoreAttribute.Wits && character.wits >= req.attributeLevel) ||
                (req.attribute == CoreAttribute.Constitution && character.constitution >= req.attributeLevel)
                )
            {
                bRet = true;
            }
        }

        else if (req.requirementType == StoryChoiceReqType.TalentLevel &&
                CharacterDataController.Instance.GetTalentLevel(character, req.talent) >= req.talentLevel)
        {
            bRet = true;
        }

        return bRet;
    }
    private bool DoesCharacterMeetAllChoiceRequirements(CharacterData character, StoryChoiceRequirement[] reqs)
    {
        bool bRet = true;

        foreach(StoryChoiceRequirement r in reqs)
        {
            if (DoesCharacterMeetChoiceRequirement(character, r) == false)
                return false;
        }

        return bRet;
    }

    #endregion

    


}
