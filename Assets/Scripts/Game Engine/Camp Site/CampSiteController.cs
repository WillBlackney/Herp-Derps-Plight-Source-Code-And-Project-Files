using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class CampSiteController : Singleton<CampSiteController>
{
    // Properties + Component References
    #region
    [Header("Character References")]
    [SerializeField] private CampSiteCharacterView[] allCampSiteCharacterViews;

    [Header("Transform + Location References")]
    [SerializeField] private CampSiteNode[] campSiteNodes;
    [SerializeField] private Transform offScreenStartPosition;

    [Header("Node View Components")]
    [SerializeField] private GameObject nodesParent;
    [SerializeField] private CanvasGroup nodesCg;

    [Header("Camp Point Properties")]
    [SerializeField] private int currentCampPoints;
    [SerializeField] private int campPointRegen;

    [Header("Camp Draw Properties")]
    [SerializeField] private int baseCampDraw = 5;
    [SerializeField] private int currentCampDraw;

    [Header("Camp Card Library Properties")]
    [SerializeField] private CampCardDataSO[] allCampCardSoData;
    private CampCardData[] allCampCardData;

    [Header("Camp Deck Properties")]
    [SerializeField] private CampCardDataSO[] startingCampDeckData;
    private List<CampCardData> campDeck = new List<CampCardData>();
    private List<CampCard> campHand = new List<CampCard>();
    private List<CampCard> campDrawPile = new List<CampCard>();

    [Header("Card GUI Components")]
    [SerializeField] private HandVisual handVisual;
    [SerializeField] private TextMeshProUGUI drawPileCountText;
    [SerializeField] private TextMeshProUGUI campPointText;
    #endregion

    // Getters 
    #region
    public HandVisual HandVisual
    {
        get { return handVisual; }
        private set { handVisual = value; }
    }
    #endregion

    // Library Logic
    #region
    protected override void Awake()
    {
        base.Awake();
        BuildCampCardLibrary();
        BuildPropertiesFromStandardSettings();
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
    private void BuildPropertiesFromStandardSettings()
    {
        BuildStartingCampSiteDeck(startingCampDeckData);
        SetStartingCampPointRegenStat(campPointRegen);
        SetStartingDrawStat(baseCampDraw);
    }
    #endregion

    // Data Conversion + Serialization Logic
    #region
    public CampCardData BuildCampCardDataFromScriptableObjectData(CampCardDataSO d)
    {
        Debug.LogWarning("CampSiteController.BuildCampCardDataFromScriptableObjectData() called on card data SO: " + d.cardName);

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
        Debug.LogWarning("CampSiteController.BuildCampCardFromCampCardData() called on card data: " + data.cardName);

        CampCard card = new CampCard();

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
        Debug.LogWarning("CampSiteController.GetCampCardSpriteByName() called, search term: " + cardName);

        Sprite sprite = null;

        foreach (CampCardDataSO data in allCampCardSoData)
        {
            if (data.cardName == cardName)
            {
                Debug.LogWarning("Found sprite match");
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
            // Cancel if data ref is null (happens if player has less then 3 characters
            if(allCampSiteCharacterViews[i].myCharacterData == null)
            {
                return;
            }

            // Dead characters dont walk on screen, they start at their node
            if (allCampSiteCharacterViews[i].myCharacterData.health <= 0)
            {
                allCampSiteCharacterViews[i].characterEntityView.ucmMovementParent.transform.position = campSiteNodes[i].transform.position;
            }
            // Alive characters start off screen, then walk to their node on event start
            else
            {
                allCampSiteCharacterViews[i].characterEntityView.ucmMovementParent.transform.position = offScreenStartPosition.position;
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
            if (character.myCharacterData != null && character.myCharacterData.health > 0)
            {
                // replace this with new move function in future, this function will make characters run through the camp fire
                CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, campSiteNodes[i].LevelNode, null);
            }

            yield return new WaitForSeconds(0.25f);
        }

    }
    #endregion

    // Modify View Elements
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
    #endregion

    // Set starting Camp Site Deck + draw + camp regen 
    #region

    public void BuildStartingCampSiteDeck(CampCardDataSO[] data)
    {
        Debug.LogWarning("CampSiteController.BuildStartingCampSiteDeck() called...");

        campDeck.Clear();
        foreach(CampCardDataSO dso in data)
        {
            campDeck.Add(BuildCampCardDataFromScriptableObjectData(dso));
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
        campPointRegen = basePointRegen;
    }
    #endregion

    // On New camp site event start logic
    #region
    public void PopulateDrawPile()
    {
        foreach(CampCardData cd in campDeck)
        {
            AddCardToDrawPile(BuildCampCardFromCampCardData(cd));
        }
    }
    public void GainCampPointsOnNewCampEventStart()
    {
        Debug.LogWarning("CampSiteController.GainCampPointsOnNewCampEventStart() called...");
        ModifyCurrentCampPoints(campPointRegen);
    }
    private void ModifyCurrentCampPoints(int gainedOrLost)
    {
        currentCampPoints += gainedOrLost;
        campPointText.text = currentCampPoints.ToString();
    }
    public void DrawCampCardsOnCampEventStart()
    {
        Debug.LogWarning("CampSiteController.DrawCampCardsOnCampEventStart() called...");
        for (int i = 0; i < currentCampDraw; i++)
        {
            DrawACardFromDrawPile(0);
        }
    }

    // gain camp site points
    // convert deck cards to camp cards, then add to draw pile
    // shuffle draw pile
    // draw cards equal to camp draw stat


    #endregion

    // Cards
    #region
    public bool IsCampCardPlayable(CampCard card)
    {
        return card.cardEnergyCost <= currentCampPoints;
    }
    private bool IsCardDrawValid()
    {
        Debug.LogWarning("CampSiteController.IsCardDrawValid() called...");
        bool bReturned = false;
        if(campDrawPile.Count > 0)
        {
            bReturned = true;
        }

        return bReturned;

    }
    public bool IsTargetValid(CampSiteCharacterView target, CampCard card)
    {
        bool bReturned = true;

        foreach(CampCardTargettingCondition condition in card.targetRequirements)
        {
            if(!IsTargettingConditionMet(condition, target))
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

        if(condition.targettingConditionType == TargettingConditionType.TargetIsAlive &&
            target.myCharacterData.health > 0)
        {
            bReturned = true;
        }
        else if (condition.targettingConditionType == TargettingConditionType.TargetIsDead &&
            target.myCharacterData.health == 0)
        {
            bReturned = true;
        }

        return bReturned;
    }
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
       // AutoUpdateCardGlowOutline(card);

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

        // Cache references
        ConnectCardWithCardViewModel(card, cardVM);

        // Set up appearance, texts and sprites
        CardController.Instance.SetUpCardViewModelAppearanceFromCard(cardVM, card);

        // Set glow outline
        //AutoUpdateCardGlowOutline(card);

        return cardVM;
    }
    private void ConnectCardWithCardViewModel(CampCard card, CardViewModel cardVM)
    {
        card.cardVM = cardVM;
        cardVM.campCard = card;
        cardVM.eventSetting = EventSetting.Camping;
    }
    #endregion

    // Play Camp Cards Logic
    #region
    public void PlayCampCardFromHand(CampCard card, CampSiteCharacterView target = null)
    {
        // Pay energy cost, remove from hand, etc
        OnCardPlayedStart(card);

        // Remove references between card and its view
        //DisconnectCardAndCardViewModel(card, cardVM);
    }

    private void OnCardPlayedStart(CampCard card)
    {
        // Pay Energy Cost
        ModifyCurrentCampPoints(-card.cardEnergyCost);

        // Where should this card be sent to?
        if (card.expend)
        {
           // ExpendCard(card);
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

    private void PlayACardFromHandVisualEvent(CardViewModel cvm )
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
    #endregion


}
