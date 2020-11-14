using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

public class CampSiteController : Singleton<CampSiteController>
{
    // Properties + Component References
    #region
    [Header("Camp Card Library Properties")]
    [SerializeField] private CampCardDataSO[] allCampCardSoData;
    private CampCardData[] allCampCardData;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Player Starting Camp Stats")]
    [SerializeField] private int baseCampDraw = 5;
    [SerializeField] private int baseCampPointRegren;

    [Header("Player Starting Camp Deck Properties")]   
    [SerializeField] private CampCardDataSO[] startingCampDeckData;

    [Header("Player Current Camp Stats")]
    private int currentCampDraw;
    private int currentCampPoints;
    private int currentCampPointRegen;

    [Header("View Parent References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private GameObject charactersViewParent;
    [SerializeField] private GameObject campGuiViewParent;
    [SerializeField] private GameObject nodesParent;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup campGuiCg;
    [SerializeField] private CanvasGroup nodesCg;

    [Header("Character View References")]
    [SerializeField] private CampSiteCharacterView[] allCampSiteCharacterViews;

    [Header("Nodes + Location References")]
    [SerializeField] private CampSiteNode[] campSiteNodes;
    [SerializeField] private Transform offScreenStartPosition;

    [Header("Current Camp Deck Properties")]
    private List<CampCardData> campDeck = new List<CampCardData>();
    private List<CampCard> campHand = new List<CampCard>();
    private List<CampCard> campDrawPile = new List<CampCard>();

    [Header("Card GUI Components")]
    [SerializeField] private HandVisual handVisual;
    [SerializeField] private TextMeshProUGUI drawPileCountText;
    [SerializeField] private TextMeshProUGUI campPointText;

    [Header("Misc Properties")]
    private bool continueButtonEnabled = false;
    private bool awaitingCardUpgradeChoice = false;
    private CampSiteCharacterView selectedCharacterView = null;
    public CardData selectedUpgradeCard = null;
    #endregion

    // Getters 
    #region
    public HandVisual HandVisual
    {
        get { return handVisual; }
        private set { handVisual = value; }
    }
    public CampSiteCharacterView[] AllCampSiteCharacterViews
    {
        get { return allCampSiteCharacterViews; }
    }
    public bool AwaitingCardUpgradeChoice
    {
        get { return awaitingCardUpgradeChoice; }
        private set { awaitingCardUpgradeChoice = value; }
    }
    public CampSiteCharacterView SelectedCharacterView
    {
        get { return selectedCharacterView; }
        private set { selectedCharacterView = value; }
    }

    #endregion

    // Enable + Disable Core Views
    #region
    public void EnableCharacterViewParent()
    {
        charactersViewParent.SetActive(true);
    }
    public void DisableCharacterViewParent()
    {
        charactersViewParent.SetActive(false);
    }
    public void EnableCampGuiViewParent()
    {
        continueButtonEnabled = true;
        campGuiViewParent.SetActive(true);
    }
    public void DisableCampGuiViewParent()
    {
        campGuiViewParent.SetActive(false);
        ForceDestroyAllCampCardViewModels();
    }

    #endregion

    // Fade + Transition Core Views
    #region
    public void FadeInCampGui()
    {
        campGuiCg.alpha = 0;
        campGuiCg.DOFade(1f, 0.5f);
    }
    public void FadeOutCampGui()
    {
        campGuiCg.alpha = 1;
        campGuiCg.DOFade(0f, 0.5f);
    }
    public void FadeInNodes()
    {
        nodesParent.SetActive(true);
        nodesCg.DOFade(1f, 0.5f);
    }
    public void FadeOutNodes()
    {
        nodesParent.SetActive(true);
        Sequence s = DOTween.Sequence();
        s.Append(nodesCg.DOFade(0f, 0.5f));
        s.OnComplete(() => nodesParent.SetActive(false));
    }
    public void FadeInAllCharacterGUI()
    {
        for (int i = 0; i < allCampSiteCharacterViews.Length; i++)
        {
            if (allCampSiteCharacterViews[i].myCharacterData != null)
            {
                FadeInCharacterGUI(allCampSiteCharacterViews[i]);
            }
        }
    }
    private void FadeInCharacterGUI(CampSiteCharacterView view)
    {
        CharacterEntityController.Instance.FadeInCharacterWorldCanvas(view.characterEntityView, null, 1f);
    }
    public void FadeOutAllCharacterGUI()
    {
        for (int i = 0; i < allCampSiteCharacterViews.Length; i++)
        {
            if (allCampSiteCharacterViews[i].myCharacterData != null)
            {
                FadeOutCharacterGUI(allCampSiteCharacterViews[i]);
            }
        }
    }
    private void FadeOutCharacterGUI(CampSiteCharacterView view)
    {
        CharacterEntityController.Instance.FadeOutCharacterWorldCanvas(view.characterEntityView, null, 0.5f);
    }
    #endregion

    // Initialization + Library Logic
    #region
    protected override void Awake()
    {
        base.Awake();
        BuildCampCardLibrary();
    }
    private void BuildCampCardLibrary()
    {
        Debug.LogWarning("CampSiteController.BuildCampCardLibrary() called...");

        List<CampCardData> tempList = new List<CampCardData>();

        foreach (CampCardDataSO dataSO in allCampCardSoData)
        {
            tempList.Add(BuildCampCardDataFromScriptableObjectData(dataSO));
        }

        allCampCardData = tempList.ToArray();
    }
    public void BuildPropertiesFromStandardSettings()
    {
        BuildStartingCampSiteDeck(startingCampDeckData);
        SetStartingCampPointRegenStat(baseCampPointRegren);
        SetStartingDrawStat(baseCampDraw);
    }
    #endregion

    // Save + Load Logic
    #region
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        BuildStartingCampSiteDeck(saveFile.campDeck);
        SetStartingCampPointRegenStat(saveFile.campPointRegen);
        SetStartingDrawStat(saveFile.campCardDraw);
    }
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        saveFile.campCardDraw = currentCampDraw;
        saveFile.campDeck = campDeck;
        saveFile.campPointRegen = currentCampPointRegen;
    }
    #endregion

    // Input Listners + Mouse events
    #region
    public void OnCampCharacterMouseEnter(CampSiteCharacterView view)
    {
        // Cancel if no character on the node
        if(view.myCharacterData == null)
        {
            return;
        }

        // Mouse over SFX
        AudioManager.Instance.PlaySound(Sound.GUI_Button_Mouse_Over);

        // Find + Highlight character's level node
        for(int i = 0; i < campSiteNodes.Length; i++)
        {
            if(view == allCampSiteCharacterViews[i])
            {
                LevelManager.Instance.SetMouseOverViewState(campSiteNodes[i].LevelNode, true);
                break;
            }
        }

    }
    public void OnCampCharacterMouseExit(CampSiteCharacterView view)
    {
        // Cancel if no character on the node
        if (view.myCharacterData == null)
        {
            return;
        }

        // Find + Highlight character's level node
        for (int i = 0; i < campSiteNodes.Length; i++)
        {
            if (view == allCampSiteCharacterViews[i])
            {
                LevelManager.Instance.SetMouseOverViewState(campSiteNodes[i].LevelNode, false);
                break;
            }
        }
    }
    #endregion

    // Set Starting player properites
    #region
    public void BuildStartingCampSiteDeck(CampCardDataSO[] data)
    {
        Debug.LogWarning("CampSiteController.BuildStartingCampSiteDeck() called...");

        campDeck.Clear();
        foreach (CampCardDataSO dso in data)
        {
            AddCardToCampDeck(BuildCampCardDataFromScriptableObjectData(dso));
        }
    }
    public void BuildStartingCampSiteDeck(List<CampCardData> data)
    {
        Debug.LogWarning("CampSiteController.BuildStartingCampSiteDeck() called...");

        campDeck.Clear();
        foreach (CampCardData card in data)
        {
            AddCardToCampDeck(card);
        }
    }
    public void SetStartingDrawStat(int baseDraw)
    {
        Debug.LogWarning("CampSiteController.SetStartingDrawStat() called...");
        currentCampDraw = baseDraw;
    }
    public void SetStartingCampPointRegenStat(int basePointRegen)
    {
        Debug.LogWarning("CampSiteController.SetStartingCampPointRegenStat() called...");
        currentCampPointRegen = basePointRegen;
    }
    #endregion

    // Data Conversion + Serialization Logic
    #region
    public CampCardData BuildCampCardDataFromScriptableObjectData(CampCardDataSO d)
    {
        Debug.Log("CampSiteController.BuildCampCardDataFromScriptableObjectData() called on card data SO: " + d.cardName);
        Debug.Log("CampSiteController.BuildCampCardDataFromScriptableObjectData() called on card data SO: " + d.cardName);

        CampCardData c = new CampCardData();

        c.cardName = d.cardName;
        c.cardEnergyCost = d.cardEnergyCost;
        c.targettingType = d.targettingType;

        c.innate = d.innate;
        c.expend = d.expend;

        // Requirements
        c.targetRequirements = new List<CampCardTargettingCondition>();
        foreach (CampCardTargettingCondition ce in d.targetRequirements)
        {
            c.targetRequirements.Add(ObjectCloner.CloneJSON(ce));
        }

        // Card effects
        c.cardEffects = new List<CampCardEffect>();
        foreach (CampCardEffect ce in d.cardEffects)
        {
            c.cardEffects.Add(ObjectCloner.CloneJSON(ce));
        }

        // Keyword Model Data
        c.keyWordModels = new List<KeyWordModel>();
        foreach (KeyWordModel kwdm in d.keyWordModels)
        {
            c.keyWordModels.Add(ObjectCloner.CloneJSON(kwdm));
        }

        // Custom string Data
        c.customDescription = new List<CustomString>();
        foreach (CustomString cs in d.customDescription)
        {
            c.customDescription.Add(ObjectCloner.CloneJSON(cs));
        }

        return c;
    }
    public CampCard BuildCampCardFromCampCardData(CampCardData data)
    {
        Debug.Log("CampSiteController.BuildCampCardFromCampCardData() called on card data: " + data.cardName);

        CampCard card = new CampCard();

        // Connect reference to card data in persistent camp deck
        card.myCampDeckCardRef = data;

        // Core data
        card.cardName = data.cardName;
        card.cardEnergyCost = data.cardEnergyCost;
        card.targettingType = data.targettingType;

        // key words
        card.expend = data.expend;
        card.innate = data.innate;

        // Collections
        card.customDescription.AddRange(data.customDescription);
        card.targetRequirements.AddRange(data.targetRequirements);
        card.cardEffects.AddRange(data.cardEffects);
        card.keyWordModels.AddRange(data.keyWordModels);

        return card;
    }
    public Sprite GetCampCardSpriteByName(string cardName)
    {
        Debug.Log("CampSiteController.GetCampCardSpriteByName() called, search term: " + cardName);

        Sprite sprite = null;

        foreach (CampCardDataSO data in allCampCardSoData)
        {
            if (data.cardName == cardName)
            {
                sprite = data.cardSprite;
                break;
            }
        }

        return sprite;
    }
    #endregion

    // Build Views + Setup
    #region
    public void BuildAllCampSiteCharacterViews(List<CharacterData> characters)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            BuildCampSiteCharacterView(allCampSiteCharacterViews[i], characters[i]);
        }
    }
    private void BuildCampSiteCharacterView(CampSiteCharacterView view, CharacterData data)
    {
        Debug.LogWarning("CampSiteController.BuildCampSiteCharacterView() called, character data: " + data.myName);

        // Connect data to view
        view.myCharacterData = data;

        // Enable shadow
        view.ucmShadowParent.SetActive(true);

        // Set health gui
        UpdateHealthGUIElements(view.characterEntityView, data.health, data.maxHealth);

        // Build UCM
        CharacterModelController.BuildModelFromStringReferences(view.characterEntityView.ucm, data.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(data.itemManager, view.characterEntityView.ucm);
    }
    #endregion

    // Move Characters + Positioning
    #region
    public void MoveAllCharactersToStartPosition()
    {
        for(int i = 0; i < allCampSiteCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allCampSiteCharacterViews[i];
            // Cancel if data ref is null (happens if player has less then 3 characters
            if (character.myCharacterData == null)
            {
                return;
            }

            // Dead characters dont walk on screen, they start at their node
            if (character.myCharacterData.health <= 0)
            {
                character.characterEntityView.ucmMovementParent.transform.position = campSiteNodes[i].transform.position;
                CharacterEntityController.Instance.PlayLayDeadAnimation(character.characterEntityView);
            }
            // Alive characters start off screen, then walk to their node on event start
            else
            {
                character.characterEntityView.ucmMovementParent.transform.position = offScreenStartPosition.position;
            }
        }
    }
    public void MoveAllCharactersToTheirNodes()
    {
        StartCoroutine(MoveAllCharactersToTheirNodesCoroutine());
    }
    private IEnumerator MoveAllCharactersToTheirNodesCoroutine()
    {
        for(int i = 0; i < allCampSiteCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allCampSiteCharacterViews[i];

            // Normal move to node
            if (character.myCharacterData != null && character.myCharacterData.health > 0)
            {
                // replace this with new move function in future, this function will make characters run through the camp fire
                CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, campSiteNodes[i].LevelNode, null);
            }

            yield return new WaitForSeconds(0.25f);
        }

    }
    public void MoveCharactersToOffScreenRight(List<CampSiteCharacterView> characters, CoroutineData cData)
    {
        StartCoroutine(MoveCharactersToOffScreenRightCoroutine(characters, cData));
    }
    private IEnumerator MoveCharactersToOffScreenRightCoroutine(List<CampSiteCharacterView> characters, CoroutineData cData)
    {
        int index = 1;
        foreach (CampSiteCharacterView character in characters)
        {      
            // Only move existing living characters
            if(character.myCharacterData != null && 
                character.myCharacterData.health > 0)
            {
                if (index == 1)
                {
                    CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, campSiteNodes[1].LevelNode, null, () =>
                    CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, LevelManager.Instance.EnemyOffScreenNode, null, null, 0f));
                }
                else
                {
                    CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, LevelManager.Instance.EnemyOffScreenNode, null);
                }
            }        
            index++;
        }

        yield return new WaitForSeconds(3f);

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    #endregion

    // Set View starting states
    #region
    public void SetAllCampSiteCharacterViewStartStates()
    {
        for(int i = 0; i < allCampSiteCharacterViews.Length; i++)
        {
            SetCampSiteCharacterViewStartingState(allCampSiteCharacterViews[i]);
        }
    }
    private void SetCampSiteCharacterViewStartingState(CampSiteCharacterView view)
    {
        view.characterEntityView.worldSpaceCanvasParent.gameObject.SetActive(false);
        view.characterEntityView.worldSpaceCG.alpha = 0;
    }
    #endregion

    // Continue Button Logic
    #region
    public void OnContinueButtonClicked()
    {
        if (continueButtonEnabled && 
            VisualEventManager.Instance.PendingCardDrawEvent() == false && 
            !MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            continueButtonEnabled = false;
            HandleContinueButtonClicked();
        }
    }
    private void HandleContinueButtonClicked()
    {
        EventSequenceController.Instance.HandleLoadNextEncounter();
    }
    #endregion

    // Set starting player properties
    #region
    public void AddCardToCampDeck(CampCardData card)
    {
        campDeck.Add(card);
    }
    public void RemoveCardToCampDeck(CampCardData card)
    {
        campDeck.Remove(card);
    }   
    #endregion

    // On New camp site event start logic
    #region    
    public void GainCampPointsOnNewCampEventStart()
    {
        Debug.Log("CampSiteController.GainCampPointsOnNewCampEventStart() called...");
        // Reset camp points
        currentCampPoints = 0;

        // Regen camp points
        ModifyCurrentCampPoints(currentCampPointRegen);
    }   
    public void DrawCampCardsOnCampEventStart()
    {
        Debug.Log("CampSiteController.DrawCampCardsOnCampEventStart() called...");

        // Shuffle draw pile
        campDrawPile.Shuffle();

        for (int i = 0; i < currentCampDraw; i++)
        {
            // try find an innate card
            CampCard cardDrawn = null;
            foreach (CampCard card in campDrawPile)
            {
                if (card.innate)
                {
                    cardDrawn = card;
                    break;
                }
            }

            // did we find an innate card?
            if (cardDrawn != null)
            {
                // we did, draw it
                DrawACardFromDrawPile(cardDrawn);
            }
            else
            {
                // we didnt, draw the first card from top of draw pile instead
                DrawACardFromDrawPile(0);
            }

        }
    }
    public void PopulateDrawPile()
    {
        foreach (CampCardData cd in campDeck)
        {
            AddCardToDrawPile(BuildCampCardFromCampCardData(cd));
        }
    }
    private void ModifyCurrentCampPoints(int gainedOrLost)
    {
        currentCampPoints += gainedOrLost;
        campPointText.text = currentCampPoints.ToString();
        AutoUpdateCardsInHandGlowOutlines();
    }
    #endregion

    // Condition Checks + Bools
    #region
    public bool IsCampCardPlayable(CampCard card)
    {
        return card.cardEnergyCost <= currentCampPoints;
    }
    private bool IsCardDrawValid()
    {
        Debug.Log("CampSiteController.IsCardDrawValid() called...");
        bool bReturned = false;
        if (campDrawPile.Count > 0)
        {
            bReturned = true;
        }

        return bReturned;

    }
    public bool IsTargetValid(CampSiteCharacterView target, CampCard card)
    {
        bool bReturned = true;

        if(target.myCharacterData == null)
        {
            Debug.Log("IsTargetValid() detected camp character's character data ref is null, returning false");
            return false;
        }

        foreach (CampCardTargettingCondition condition in card.targetRequirements)
        {
            if (!IsTargettingConditionMet(condition, target))
            {
                bReturned = false;
                break;
            }
        }

        return bReturned;

    }
    private bool IsTargettingConditionMet(CampCardTargettingCondition condition, CampSiteCharacterView target)
    {
        bool bReturned = false;

        if (condition.targettingConditionType == TargettingConditionType.TargetIsAlive &&
            target.myCharacterData.health > 0)
        {
            bReturned = true;
        }
        else if (condition.targettingConditionType == TargettingConditionType.TargetIsDead &&
            target.myCharacterData.health == 0)
        {
            bReturned = true;
        }
        else if (condition.targettingConditionType == TargettingConditionType.TargetHasUpgradeableCards &&
            CardController.Instance.GetUpgradeableCardsFromCollection(target.myCharacterData.deck).Count > 0)
        {
            bReturned = true;
        }

        return bReturned;
    }
    #endregion

    // Manipulate player card collections
    #region
    private CampCard DrawACardFromDrawPile(int drawPileIndex = 0)
    {
        Debug.Log("CampSiteController.DrawACardFromDrawPile() called...");
        CampCard cardDrawn = null;

        if (IsCardDrawValid())
        {
            // Get card and remove from deck
            cardDrawn = campDrawPile[drawPileIndex];
            RemoveCardFromDrawPile(cardDrawn);

            // Add card to hand
            AddCardToHand(cardDrawn);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => DrawCardFromDeckVisualEvent(cardDrawn), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);
        }       

        return cardDrawn;
    }
    private CampCard DrawACardFromDrawPile(CampCard cardDrawn)
    {
        Debug.Log("CampSiteController.DrawACardFromDrawPile() called...");

        if (IsCardDrawValid())
        {
            // Get card and remove from deck
            RemoveCardFromDrawPile(cardDrawn);

            // Add card to hand
            AddCardToHand(cardDrawn);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => DrawCardFromDeckVisualEvent(cardDrawn), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);
        }

        return cardDrawn;
    }
    private void RemoveCardFromDrawPile(CampCard card)
    {
        campDrawPile.Remove(card);
        string drawPileCount = campDrawPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => {
            drawPileCountText.text = drawPileCount; 
        });
    }
    private void RemoveCardFromHand(CampCard card)
    {
        if (campHand.Contains(card))
        {
            campHand.Remove(card);
        }
    }
    private void AddCardToHand(CampCard card)
    {
        Debug.Log("CampSiteController.AddCardToHand() called...");
        campHand.Add(card);
    }
    private void AddCardToDrawPile(CampCard card)
    {
        campDrawPile.Add(card);
        drawPileCountText.text = campDrawPile.Count.ToString();
    }
    public void ForceDestroyAllCampCardViewModels()
    {
        HandVisual.ForceDestroyAllCards();
        campHand.Clear();
    }
    #endregion

    // Build + Destroy + Set up cards
    #region
    public CardViewModel BuildCardViewModelFromCard(CampCard card, Vector3 position)
    {
        Debug.Log("CampSiteController.BuildCardViewModelFromCard() called...");

        CardViewModel cardVM = null;
        if (card.targettingType == CampTargettingType.NoTarget)
        {
            cardVM = Instantiate(PrefabHolder.Instance.noTargetCard, position, Quaternion.identity).GetComponentInChildren<CardViewModel>();
        }
        else
        {
            cardVM = Instantiate(PrefabHolder.Instance.targetCard, position, Quaternion.identity).GetComponentInChildren<CardViewModel>();
        }

        // Disable unused view elements
        cardVM.cardTypeParent.SetActive(false);
        cardVM.talentSchoolParent.SetActive(false);

        // Cache references
        ConnectCardWithCardViewModel(card, cardVM);

        // Set up appearance, texts and sprites
        CardController.Instance.SetUpCardViewModelAppearanceFromCard(cardVM, card);

        // Set glow outline
        AutoUpdateCardGlowOutline(card);

        return cardVM;
    }
    private void ConnectCardWithCardViewModel(CampCard card, CardViewModel cardVM)
    {
        card.cardVM = cardVM;
        cardVM.campCard = card;
        cardVM.eventSetting = EventSetting.Camping;
    }
    private void DisconnectCardAndCardViewModel(CampCard card, CardViewModel cardVM)
    {
        if (card != null)
        {
            card.cardVM = null;
        }
        if (cardVM != null)
        {
            cardVM.campCard = null;
        }

    }
    #endregion

    // Play Camp Cards Logic
    #region
    private void DiscardCardFromHandToDrawPile(CampCard card)
    {
        Debug.Log("CampSiteController.DiscardCardFromHandToDrawPile() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;

        // remove from hand
        RemoveCardFromHand(card);

        // place on top of discard pile
        AddCardToDrawPile(card);

        // does the card have a cardVM linked to it?
        if (cvm)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => PlayACardFromHandVisualEvent(cvm), QueuePosition.Back, 0f, 0.1f, EventDetail.CardDraw);
        }
    }
    public void PlayCampCardFromHand(CampCard card, CampSiteCharacterView target = null)
    {
        // Pay energy cost, remove from hand, etc
        OnCardPlayedStart(card);

        // Remove references between card and its view
        DisconnectCardAndCardViewModel(card, card.cardVM);

        foreach(CampCardEffect cce in card.cardEffects)
        {
            TriggerEffectFromCard(card, cce, target);
        }
    }
    private void TriggerEffectFromCard(CampCard card, CampCardEffect cardEffect, CampSiteCharacterView view)
    {
        CharacterData cData = null;
        if(view != null)
        {
            cData = view.myCharacterData;
        }

        // Queue starting anims and particles        
        foreach (AnimationEventData vEvent in cardEffect.visualEventsOnStart)
        {
            AnimationEventController.Instance.PlayAnimationEvent(vEvent, view);
        }        

        // Heal Target
        if (cardEffect.cardEffectType == CampCardEffectType.Heal)
        {
            int healAmount = 0;

            if (cardEffect.healingType == HealingType.PercentageOfMaxHealth)
            {
                // Calculate heal amount
                healAmount = (int)(cData.maxHealth * 0.5f);
                Debug.LogWarning("Heal amount = " + healAmount.ToString());
            }

            else if (cardEffect.healingType == HealingType.FlatAmount)
            {
                // Calculate heal amount
                healAmount = cardEffect.flatHealAmount;
                Debug.LogWarning("Heal amount = " + healAmount.ToString());
            }

            // Modify health
            HandleHealEffect(view, healAmount);

            // Heal VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateHealEffect(view.characterEntityView.WorldPosition));

            // Create heal text effect
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateDamageEffect(view.characterEntityView.WorldPosition, healAmount, true));

            // Create SFX
            VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(Sound.Passive_General_Buff));
        }

        // Heal All
        if (cardEffect.cardEffectType == CampCardEffectType.HealAllCharacters)
        {
            foreach(CampSiteCharacterView campCharacter in allCampSiteCharacterViews)
            {
                if (campCharacter.myCharacterData == null)
                {
                    return;
                }

                cData = campCharacter.myCharacterData;

                // does each character meet the healing requirements?
                if (IsTargetValid(campCharacter, card))
                {
                    int healAmount = 0;

                    if (cardEffect.healingType == HealingType.PercentageOfMaxHealth)
                    {
                        // Calculate heal amount
                        healAmount = (int)(cData.maxHealth * 0.5f);
                        Debug.Log("Heal amount = " + healAmount.ToString());
                    }

                    else if (cardEffect.healingType == HealingType.FlatAmount)
                    {
                        // Calculate heal amount
                        healAmount = cardEffect.flatHealAmount;
                        Debug.Log("Heal amount = " + healAmount.ToString());
                    }

                    // Modify health
                    HandleHealEffect(campCharacter, healAmount);

                    // Heal VFX
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                        VisualEffectManager.Instance.CreateHealEffect(campCharacter.characterEntityView.WorldPosition));

                    // Create heal text effect
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateDamageEffect(campCharacter.characterEntityView.WorldPosition, healAmount, true));

                    // Create SFX
                    VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(Sound.Passive_General_Buff));
                }
            }
           
        }

        // Increase max health target
        if (cardEffect.cardEffectType == CampCardEffectType.IncreaseMaxHealth)
        {
            int maxHpGainAmount = cardEffect.maxHealthGained;

            // Increase character's max health
            CharacterDataController.Instance.SetCharacterMaxHealth(cData, cData.maxHealth + maxHpGainAmount);

            // Update health GUI visual event
            VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(view.characterEntityView, cData.health, cData.maxHealth), QueuePosition.Back, 0, 0);

            // Heal VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateHealEffect(view.characterEntityView.WorldPosition));

            // Create SFX
            VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(Sound.Passive_General_Buff));
        }

        // Increase max health all
        if (cardEffect.cardEffectType == CampCardEffectType.IncreaseMaxHealthAll)
        {
            foreach (CampSiteCharacterView campCharacter in allCampSiteCharacterViews)
            {
                if(campCharacter.myCharacterData == null)
                {
                    return;
                }

                cData = campCharacter.myCharacterData;

                // does each character meet the healing requirements?
                if (IsTargetValid(campCharacter, card))
                {
                    int maxHpGainAmount = cardEffect.maxHealthGained;

                    // Increase character's max health
                    CharacterDataController.Instance.SetCharacterMaxHealth(cData, cData.maxHealth + maxHpGainAmount);

                    // Update health GUI visual event
                    VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(campCharacter.characterEntityView, cData.health, cData.maxHealth), QueuePosition.Back, 0, 0);

                    // Heal VFX
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                        VisualEffectManager.Instance.CreateHealEffect(campCharacter.characterEntityView.WorldPosition));

                    // Create SFX
                    VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(Sound.Passive_General_Buff));
                }
            }
        }

        // Apply passive
        if (cardEffect.cardEffectType == CampCardEffectType.ApplyPassive)
        {
            // Setup
            string passiveName = TextLogic.SplitByCapitals(cardEffect.passivePairing.passiveData.ToString());
            int stacks = cardEffect.passivePairing.passiveStacks;

            // Apply passive to character data
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(cData.passiveManager, passiveName, stacks, false);

            // Visual notification event
            VisualEventManager.Instance.CreateVisualEvent(() => 
            VisualEffectManager.Instance.CreateStatusEffect(view.characterEntityView.WorldPosition, passiveName + " +" +stacks.ToString()));
        }

        // Modify Core Attribute
        if (cardEffect.cardEffectType == CampCardEffectType.ModifyCoreAttribute)
        {
            // Power
            if(cardEffect.attributeChanged == CoreAttribute.Power)
            {
                CharacterDataController.Instance.ModifyPower(cData, cardEffect.attributeAmountGained);

                // Visual notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.characterEntityView.WorldPosition, cardEffect.attributeChanged.ToString() + 
                " +" + cardEffect.attributeAmountGained.ToString()));
            }

            // Dexterity
            else if (cardEffect.attributeChanged == CoreAttribute.Dexterity)
            {
                CharacterDataController.Instance.ModifyDexterity(cData, cardEffect.attributeAmountGained);

                // Visual notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.characterEntityView.WorldPosition, cardEffect.attributeChanged.ToString() +
                " +" + cardEffect.attributeAmountGained.ToString()));
            }

            // Stamina
            else if (cardEffect.attributeChanged == CoreAttribute.Stamina)
            {
                CharacterDataController.Instance.ModifyStamina(cData, cardEffect.attributeAmountGained);

                // Visual notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.characterEntityView.WorldPosition, cardEffect.attributeChanged.ToString() +
                " +" + cardEffect.attributeAmountGained.ToString()));
            }

            // Initiative
            else if (cardEffect.attributeChanged == CoreAttribute.Initiative)
            {
                CharacterDataController.Instance.ModifyInitiative(cData, cardEffect.attributeAmountGained);

                // Visual notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.characterEntityView.WorldPosition, cardEffect.attributeChanged.ToString() +
                " +" + cardEffect.attributeAmountGained.ToString()));
            }

            // Draw
            else if (cardEffect.attributeChanged == CoreAttribute.Draw)
            {
                CharacterDataController.Instance.ModifyDraw(cData, cardEffect.attributeAmountGained);

                // Visual notification event
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(view.characterEntityView.WorldPosition, cardEffect.attributeChanged.ToString() +
                " +" + cardEffect.attributeAmountGained.ToString()));
            }
        }

        // Shuffle Hand into draw pile
        if (cardEffect.cardEffectType == CampCardEffectType.ShuffleHandIntoDrawPile)
        {
            CampCard[] cardsToDiscard = campHand.ToArray();
            Debug.Log("Discarding " + cardsToDiscard.Length.ToString() + " cards");

            foreach (CampCard handCard in cardsToDiscard)
            {
                DiscardCardFromHandToDrawPile(handCard);
            }

            // Shuffle draw pile
            campDrawPile.Shuffle();
        }

        // Draw cards
        if (cardEffect.cardEffectType == CampCardEffectType.DrawCards)
        {
            int totalDraws = cardEffect.cardsDrawn;

            for(int i = 0; i < totalDraws; i++)
            {
                DrawACardFromDrawPile();
            }


        }

        // Upgrade Card
        if (cardEffect.cardEffectType == CampCardEffectType.UpgradeCard)
        {
            HandleUpgradeCardStartEvent(view);
        }
    }
    private void HandleUpgradeCardStartEvent(CampSiteCharacterView character)
    {
        AwaitingCardUpgradeChoice = true;
        SelectedCharacterView = character;
        CardController.Instance.CreateNewUpgradeCardInDeckPopup(character.myCharacterData);
        CardController.Instance.DisableCardGridScreenBackButton();
    }
    public void HandleUpgradeCardChoiceMade(CardData card)
    {
        // Setup
        CharacterData character = SelectedCharacterView.myCharacterData;
        List<CardData> cList = new List<CardData>();
        cList.Add(CardController.Instance.FindUpgradedCardData(card));

        // Close Grid view Screen
        CardController.Instance.HideCardGridScreen();
        CardController.Instance.HideCardUpgradePopupScreen();

        // Create add card to character visual event
        CardController.Instance.StartNewShuffleCardsScreenVisualEvent(SelectedCharacterView.characterEntityView, cList);

        // Add new upgraded card to character data deck
        CardController.Instance.HandleUpgradeCardInCharacterDeck(card, character);

        // Finish
        AwaitingCardUpgradeChoice = false;
        SelectedCharacterView = null;
    }

    public void HandleHealEffect(CampSiteCharacterView character, int healthGainedOrLost)
    {
        CharacterData cData = character.myCharacterData;
        int originalHealth = cData.health;
        int finalHealthValue = cData.health;

        finalHealthValue += healthGainedOrLost;

        // prevent health increasing over maximum
        if (finalHealthValue > cData.maxHealth)
        {
            finalHealthValue = cData.maxHealth;
        }

        // prevent health going less then 0
        if (finalHealthValue < 0)
        {
            finalHealthValue = 0;
        }

        // Apply health change to character data
        CharacterDataController.Instance.SetCharacterHealth(cData, finalHealthValue);

        // Update health GUI visual event
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateHealthGUIElements(character.characterEntityView, finalHealthValue, cData.maxHealth), QueuePosition.Back, 0, 0);

    }
    private void OnCardPlayedStart(CampCard card)
    {
        // Pay Energy Cost
        ModifyCurrentCampPoints(-card.cardEnergyCost);

        // Where should this card be sent to?
        if (card.expend)
        {
           ExpendCard(card);
        }

        else
        {
            // Do normal 'play from hand' stuff
            CardViewModel cardVM = card.cardVM;

            RemoveCardFromHand(card);
            AddCardToDrawPile(card);

            if (card.cardVM)
            {
                PlayACardFromHandVisualEvent(cardVM);
            }

        }
    } 
    private void ExpendCard(CampCard card)
    {
        Debug.Log("CampSiteController.ExpendCard() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;

        // Remove card from which ever collection its in
        if (campHand.Contains(card))
        {
            RemoveCardFromHand(card);
        }
        else if (campDrawPile.Contains(card))
        {
            RemoveCardFromDrawPile(card);
        }

        // Remove card from persistent camp deck
        RemoveCardToCampDeck(card.myCampDeckCardRef);


        // does the card have a cardVM linked to it?
        if (cvm)
        {
            ExpendCardVisualEvent(cvm);
        }

    }
    #endregion

    // Glow Outline logic
    #region
    public void AutoUpdateCardsInHandGlowOutlines()
    {
        for (int i = 0; i < campHand.Count; i++)
        {
            AutoUpdateCardGlowOutline(campHand[i]);
        }
    }
    private void AutoUpdateCardGlowOutline(CampCard card)
    {
        if (card.cardVM != null)
        {
            if (IsCampCardPlayable(card))
            {
                EnableCardViewModelGlowOutline(card.cardVM);
            }
            else
            {
                DisableCardViewModelGlowOutline(card.cardVM);
            }
        }
    }
    private void EnableCardViewModelGlowOutline(CardViewModel cvm)
    {
        cvm.glowAnimator.SetTrigger("Glow");
    }
    private void DisableCardViewModelGlowOutline(CardViewModel cvm)
    {
        cvm.glowAnimator.SetTrigger("Off");
    }
    #endregion

    // Visual Events
    #region
    private void DrawCardFromDeckVisualEvent(CampCard card)
    {
        Debug.Log("CampSiteController.DrawCardFromDeckVisualEvent() called...");

        CardViewModel cvm;
        cvm = BuildCardViewModelFromCard(card, handVisual.DeckTransform.position);

        // pass this card to HandVisual class
        handVisual.AddCard(cvm.movementParent.gameObject);

        // Bring card to front while it travels from draw spot to hand
        CardLocationTracker clt = cvm.locationTracker;
        clt.BringToFront();
        clt.Slot = 0;
        clt.VisualState = VisualStates.Transition;

        // Glow outline
        AutoUpdateCardGlowOutline(card);

        // Start SFX
        AudioManager.Instance.PlaySound(Sound.Card_Draw);

        // Get starting scale
        Vector3 originalScale = new Vector3
            (cvm.movementParent.transform.localScale.x, cvm.movementParent.transform.localScale.y, cvm.movementParent.transform.localScale.z);

        // Shrink card
        cvm.movementParent.transform.localScale = new Vector3(0.1f, 0.1f, cvm.movementParent.transform.localScale.z);

        // Scale up
        CardController.Instance.ScaleCardViewModel(cvm, originalScale.x, 0.25f);

        // Move to hand slot
        CardController.Instance.MoveTransformToLocation(cvm.movementParent, handVisual.slots.Children[0].transform.localPosition, 0.25f, true, () => clt.SetHandSortingOrder());

    }
    private void PlayACardFromHandVisualEvent(CardViewModel cvm)
    {
        Debug.Log("CampSiteController.PlayACardFromHandVisualEvent() called...");
        StartCoroutine(PlayACardFromHandVisualEventCoroutine(cvm));
    }
    private IEnumerator PlayACardFromHandVisualEventCoroutine(CardViewModel cvm)
    {
        // Set state and remove from hand visual
        cvm.locationTracker.VisualState = VisualStates.Transition;
        HandVisual.RemoveCard(cvm.movementParent.gameObject);
        cvm.movementParent.SetParent(null);

        // SFX
        AudioManager.Instance.PlaySound(Sound.Card_Discarded);

        // Create Glow Trail
        ToonEffect glowTrail = VisualEffectManager.Instance.CreateGlowTrailEffect(cvm.movementParent.position);

        // Shrink card
        CardController.Instance.ScaleCardViewModel(cvm, 0.1f, 0.5f);

        // Rotate card upside down
        CardController.Instance.RotateCardVisualEvent(cvm, 180, 0.5f);

        // Move card + glow outline to quick lerp spot
        CardController.Instance.MoveTransformToQuickLerpPosition(cvm.movementParent, 0.25f);
        CardController.Instance.MoveTransformToQuickLerpPosition(glowTrail.transform, 0.25f);
        yield return new WaitForSeconds(0.25f);

        // Move card
        CardController.Instance.MoveTransformToLocation(cvm.movementParent, HandVisual.DeckTransform.position, 0.5f, false, () => { Destroy(cvm.movementParent.gameObject); });
        CardController.Instance.MoveTransformToLocation(glowTrail.transform, HandVisual.DeckTransform.position, 0.5f, false, () =>
        {
            glowTrail.StopAllEmissions();
            Destroy(glowTrail, 3);
        });
    }
    private void ExpendCardVisualEvent(CardViewModel cvm)
    {
        if (cvm == null)
        {
            Debug.Log("ExpendCardVisualEvent() was given a null card view model...");
        }

        // remove from hand visual
        HandVisual.RemoveCard(cvm.movementParent.gameObject);
        cvm.movementParent.SetParent(null);

        // SFX
        AudioManager.Instance.PlaySound(Sound.Explosion_Fire_1);

        // TO DO: fade out card canvas gradually
        CardController.Instance.FadeOutCardViewModel(cvm, null, () => { Destroy(cvm.movementParent.gameObject); });

        // Create smokey effect
        VisualEffectManager.Instance.CreateExpendEffect(cvm.movementParent.transform.position);
    }
    private void UpdateHealthGUIElements(CharacterEntityView character, int health, int maxHealth)
    {
        // Convert health int values to floats
        float currentHealthFloat = health;
        float currentMaxHealthFloat = maxHealth;
        float healthBarFloat = currentHealthFloat / currentMaxHealthFloat;

        // Modify health bar slider + health texts
        character.healthBar.value = healthBarFloat;
        character.healthText.text = health.ToString();
        character.maxHealthText.text = maxHealth.ToString();

    }
    #endregion
       


   

}
