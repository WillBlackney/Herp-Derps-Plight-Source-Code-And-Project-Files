using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : Singleton<CardController>
{
    // Properties + Component References
    #region
    [Header("Card Properties")]
    [SerializeField] private float cardTransistionSpeed;
    [SerializeField] private bool mouseIsOverTable;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Card Library Properties")]
    [SerializeField] private CardDataSO[] allCardScriptableObjects;
    private CardData[] allCards;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Discovery Screen Components")]
    [SerializeField] private DiscoveryCardViewModel[] discoveryCards;
    [SerializeField] private Transform[] discoveryCardSlots;
    [SerializeField] private GameObject discoveryScreenVisualParent;
    [SerializeField] private CanvasGroup discoveryScreenOverlayCg;
    [SerializeField] private Transform[] confettiTransforms;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Discovery Screen Properties")]
    private CardEffect currentDiscoveryEffect;
    private bool discoveryScreenIsActive;

    [Header("Choose Card In Hand Screen Components")]
    [SerializeField] private Transform chooseCardScreenSelectionSpot;
    [SerializeField] private GameObject chooseCardScreenVisualParent;
    [SerializeField] private CanvasGroup chooseCardScreenOverlayCg;
    [SerializeField] private TextMeshProUGUI chooseCardScreenBannerText;
    [SerializeField] private Button chooseCardScreenConfirmButton;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Choose Card In Hand Screen Properties")]
    private CardEffect chooseCardScreenEffectReference;
    private Card currentChooseCardScreenSelection;
    private bool chooseCardScreenIsActive;

    [Header("Shuffle New Cards Screen Components")]
    [SerializeField] private DiscoveryCardViewModel[] shuffleCards;
    [SerializeField] private Transform[] shuffleCardSlots;
    [SerializeField] private GameObject shuffleCardsScreenVisualParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Cards Grid Screen Components")]
    [SerializeField] private GameObject cardGridVisualParent;
    [SerializeField] private CanvasGroup cardGridCg;
    [SerializeField] private TextMeshProUGUI cardGridRibbonText;
    [SerializeField] private GridCardViewModel[] allGridCards;
    [SerializeField] private GameObject cardGridScreenBackButtonParent;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Upgrade Pop Up Components")]
    [SerializeField] private GameObject upgradePopupVisualParent;
    [SerializeField] private CanvasGroup upgradePopupCg;
    [SerializeField] private GridCardViewModel originalCardPopup;
    [SerializeField] private GridCardViewModel upgradedCardPopup;

    [Header("Button Sprites")]
    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Sprite inactiveButtonSprite;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Card Movement Locations")]
    [SerializeField] private Transform[] discardToDrawAnimPath;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    // Getters
    #region
    public CardData[] AllCards
    {
        get { return allCards; }
        private set { allCards = value; }
    }
    public CardDataSO[] AllCardScriptableObjects
    {
        get { return allCardScriptableObjects; }
    }
    public bool DiscoveryScreenIsActive
    {
        get { return discoveryScreenIsActive; }
        private set { discoveryScreenIsActive = value; }
    }
    public bool ChooseCardScreenIsActive
    {
        get { return chooseCardScreenIsActive; }
        private set { chooseCardScreenIsActive = value; }
    }
    public Card CurrentChooseCardScreenSelection
    {
        get { return currentChooseCardScreenSelection; }
        private set { currentChooseCardScreenSelection = value; }
    }
    public bool GridCardScreenIsActive()
    {
        return cardGridVisualParent.activeSelf;
    }
    #endregion

    #endregion

    // Card Library Logic
    #region

    // Initialization + Build Library
    protected override void Awake()
    {
        base.Awake();
        BuildCardLibrary();
    }
    private void BuildCardLibrary()
    {
        Debug.LogWarning("CardController.BuildCardLibrary() called...");

        List<CardData> tempList = new List<CardData>();

        foreach (CardDataSO dataSO in allCardScriptableObjects)
        {
            tempList.Add(BuildCardDataFromScriptableObjectData(dataSO));
        }

        AllCards = tempList.ToArray();
    }

    // Getters
    public CardData GetCardDataFromLibraryByName(string name)
    {
        CardData cardReturned = null;

        foreach (CardData card in AllCards)
        {
            if (card.cardName == name)
            {
                cardReturned = card;
                break;
            }
        }

        if (cardReturned == null)
        {
            Debug.Log("WARNING! CardController.GetCardFromLibraryByName() could not find a card " +
                "with a matching name of " + name + ", returning null...");
        }
        return cardReturned;
    }
    public CardDataSO GetCardDataSOFromLibraryByName(string name)
    {
        CardDataSO cardReturned = null;

        foreach (CardDataSO card in AllCardScriptableObjects)
        {
            if (card.cardName == name)
            {
                cardReturned = card;
                break;
            }
        }

        if (cardReturned == null)
        {
            Debug.Log("WARNING! CardController.GetCardFromLibraryByName() could not find a card " +
                "with a matching name of " + name + ", returning null...");
        }
        return cardReturned;
    }
    public Sprite GetCardSpriteByName(string cardName)
    {
        Sprite sprite = null;

        foreach (CardDataSO data in AllCardScriptableObjects)
        {
            if (data.cardName == cardName)
            {
                sprite = data.cardSprite;
                break;
            }
        }

        return sprite;
    }
    public CardDataSO FindBaseRacialCardDataSO(CharacterRace race)
    {
        CardDataSO dataReturned = null;

        for (int i = 0; i < allCards.Length; i++)
        {
            if (allCardScriptableObjects[i].racialCard &&
                allCardScriptableObjects[i].originRace == race &&
                allCardScriptableObjects[i].upgradeLevel == 0)
            {
                dataReturned = allCardScriptableObjects[i];
                break;
            }
        }

        return dataReturned;
    }
    public CardData FindBaseRacialCardData(CharacterRace race)
    {
        CardData dataReturned = null;

        for (int i = 0; i < allCards.Length; i++)
        {
            if (allCards[i].racialCard &&
                allCards[i].originRace == race &&
                allCards[i].upgradeLevel == 0)
            {
                dataReturned = allCards[i];
                break;
            }
        }

        return dataReturned;
    }

    // Core Queires
    public List<CardData> GetCardsQuery(IEnumerable<CardData> queriedCollection, TalentSchool ts = TalentSchool.None, Rarity r = Rarity.None, bool blessing = false, UpgradeFilter uf = UpgradeFilter.Any)
    {
        Debug.Log("GetCardsQuery() called, query params --- TalentSchool = " + ts.ToString()
            + ", Rarity = " + r.ToString() + ", Blessing = " + blessing.ToString());

        List<CardData> cardsReturned = new List<CardData>();
        cardsReturned.AddRange(queriedCollection);

        if (ts != TalentSchool.None)
        {
            cardsReturned = QueryByTalentSchool(cardsReturned, ts);
        }

        if (r != Rarity.None)
        {
            cardsReturned = QueryByRarity(cardsReturned, r);
        }

        // Filter blessings
        cardsReturned = QueryByBlessing(cardsReturned, blessing);

        // Filter upgrade settings
        if (uf == UpgradeFilter.OnlyNonUpgraded)
        {
            cardsReturned = QueryByNonUpgraded(cardsReturned);
        }
        else if (uf == UpgradeFilter.OnlyUpgraded)
        {
            cardsReturned = QueryByUpgraded(cardsReturned);
        }

        return cardsReturned;
    }
    public List<Card> GetCardsQuery(IEnumerable<Card> queriedCollection, TalentSchool ts = TalentSchool.None, Rarity r = Rarity.None, bool blessing = false)
    {
        //  Debug.LogWarning("GetCardsQuery() called, query params --- TalentSchool = " + ts.ToString()
        //    + ", Rarity = " + r.ToString() + ", Blessing = " + blessing.ToString());

        List<Card> cardsReturned = new List<Card>();
        cardsReturned.AddRange(queriedCollection);

        if (ts != TalentSchool.None)
        {
            cardsReturned = QueryByTalentSchool(cardsReturned, ts);
        }

        if (r != Rarity.None)
        {
            cardsReturned = QueryByRarity(cardsReturned, r);
        }

        // Filter blessings
        cardsReturned = QueryByBlessing(cardsReturned, blessing);

        return cardsReturned;
    }

    // Specific Queries
    public List<CardData> QueryBySourceSpell(IEnumerable<CardData> collectionQueried)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.sourceSpell
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    public List<CardData> QueryByUpgraded(IEnumerable<CardData> collectionQueried)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.upgradeLevel >= 1
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    public List<CardData> QueryByNonUpgraded(IEnumerable<CardData> collectionQueried)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.upgradeLevel == 0
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    public List<CardData> QueryByRacial(IEnumerable<CardData> collectionQueried)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.racialCard == true
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    public List<CardData> QueryByAffliction(IEnumerable<CardData> collectionQueried)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.affliction == true
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    public List<CardData> QueryByRacialSpecific(IEnumerable<CardData> collectionQueried, CharacterRace race)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.originRace == race
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    private List<CardData> QueryByTalentSchool(IEnumerable<CardData> collectionQueried, TalentSchool ts)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.talentSchool == ts
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    private List<Card> QueryByTalentSchool(IEnumerable<Card> collectionQueried, TalentSchool ts)
    {
        List<Card> cardsReturned = new List<Card>();

        var query =
           from cardData in collectionQueried
           where cardData.talentSchool == ts
           select cardData;

        cardsReturned.AddRange(query);

        return cardsReturned;
    }
    private List<CardData> QueryByRarity(IEnumerable<CardData> collectionQueried, Rarity r)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.rarity == r
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    private List<Card> QueryByRarity(IEnumerable<Card> collectionQueried, Rarity r)
    {
        List<Card> cardsReturned = new List<Card>();
        var query =
           from cardData in collectionQueried
           where cardData.rarity == r
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    public List<CardData> QueryByBlessing(IEnumerable<CardData> collectionQueried, bool isBlessing)
    {
        List<CardData> cardsReturned = new List<CardData>();

        var query =
           from cardData in collectionQueried
           where cardData.blessing == isBlessing
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    private List<Card> QueryByBlessing(IEnumerable<Card> collectionQueried, bool blessing)
    {
        List<Card> cardsReturned = new List<Card>();
        var query =
           from cardData in collectionQueried
           where cardData.blessing == blessing
           select cardData;

        cardsReturned.AddRange(query);
        return cardsReturned;
    }
    #endregion

    // Build Cards, Decks, View Models and Data
    #region
    public CardData BuildCardDataFromScriptableObjectData(CardDataSO d)
    {
        CardData c = new CardData();

        // Core data
        c.cardName = d.cardName;
        c.cardDescription = d.cardDescription;
        c.cardSprite = GetCardSpriteByName(d.cardName);
        c.cardBaseEnergyCost = d.cardEnergyCost;
        c.xEnergyCost = d.xEnergyCost;
        c.upgradeable = d.upgradeable;
        c.upgradeLevel = d.upgradeLevel;

        // Types
        c.cardType = d.cardType;
        c.targettingType = d.targettingType;
        c.talentSchool = d.talentSchool;
        c.rarity = d.rarity;
        c.affliction = d.affliction;
        c.racialCard = d.racialCard;
        c.originRace = d.originRace;

        // Key words
        c.expend = d.expend;
        c.innate = d.innate;
        c.fleeting = d.fleeting;
        c.unplayable = d.unplayable;
        c.lifeSteal = d.lifeSteal;
        c.blessing = d.blessing;
        c.sourceSpell = d.sourceSpell;

        // Card effects
        c.cardEffects = new List<CardEffect>();
        foreach (CardEffect ce in d.cardEffects)
        {
            c.cardEffects.Add(ObjectCloner.CloneJSON(ce));
        }

        // Card event listeners
        c.cardEventListeners = new List<CardEventListener>();
        foreach (CardEventListener cel in d.cardEventListeners)
        {
            c.cardEventListeners.Add(ObjectCloner.CloneJSON(cel));
        }

        // Card passive effects
        c.cardPassiveEffects = new List<CardPassiveEffect>();
        foreach (CardPassiveEffect cel in d.cardPassiveEffects)
        {
            c.cardPassiveEffects.Add(ObjectCloner.CloneJSON(cel));
        }

        // Keyword Model Data
        c.keyWordModels = new List<KeyWordModel>();
        foreach (KeyWordModel kwdm in d.keyWordModels)
        {
            c.keyWordModels.Add(ObjectCloner.CloneJSON(kwdm));
        }

        // Custom string Data
        c.cardDescriptionTwo = new List<CustomString>();
        foreach (CustomString cs in d.customDescription)
        {
            c.cardDescriptionTwo.Add(ObjectCloner.CloneJSON(cs));
        }

        return c;
    }
    public CardData CloneCardDataFromCardData(CardData original)
    {
        CardData c = new CardData();

        // Core data
        c.cardName = original.cardName;
        c.cardDescription = original.cardDescription;
        c.cardSprite = GetCardSpriteByName(original.cardName);
        c.cardBaseEnergyCost = original.cardBaseEnergyCost;
        c.xEnergyCost = original.xEnergyCost;
        c.upgradeable = original.upgradeable;
        c.upgradeLevel = original.upgradeLevel;

        // Types
        c.cardType = original.cardType;
        c.targettingType = original.targettingType;
        c.talentSchool = original.talentSchool;
        c.affliction = original.affliction;
        c.rarity = original.rarity;
        c.racialCard = original.racialCard;
        c.originRace = original.originRace;

        // Key words
        c.expend = original.expend;
        c.innate = original.innate;
        c.fleeting = original.fleeting;
        c.unplayable = original.unplayable;
        c.lifeSteal = original.lifeSteal;
        c.blessing = original.blessing;
        c.sourceSpell = original.sourceSpell;

        // Card effects
        c.cardEffects = new List<CardEffect>();
        foreach (CardEffect ce in original.cardEffects)
        {
            c.cardEffects.Add(ObjectCloner.CloneJSON(ce));
        }

        // Card event listeners
        c.cardEventListeners = new List<CardEventListener>();
        foreach (CardEventListener cel in original.cardEventListeners)
        {
            c.cardEventListeners.Add(ObjectCloner.CloneJSON(cel));
        }

        // Card passive effects 
        c.cardPassiveEffects = new List<CardPassiveEffect>();
        foreach (CardPassiveEffect cel in original.cardPassiveEffects)
        {
            c.cardPassiveEffects.Add(ObjectCloner.CloneJSON(cel));
        }

        // Keyword Model Data
        c.keyWordModels = new List<KeyWordModel>();
        foreach (KeyWordModel kwdm in original.keyWordModels)
        {
            c.keyWordModels.Add(ObjectCloner.CloneJSON(kwdm));
        }

        // Custom string Data
        c.cardDescriptionTwo = new List<CustomString>();
        foreach (CustomString cs in original.cardDescriptionTwo)
        {
            c.cardDescriptionTwo.Add(ObjectCloner.CloneJSON(cs));
        }

        return c;
    }
    private Card BuildCardFromCardDataSO(CardDataSO data, CharacterEntityModel owner)
    {
        Debug.Log("CardController.BuildCardFromCardData() called...");

        Card card = new Card();

        // Core data
        card.owner = owner;
        card.cardName = data.cardName;
        card.cardDescription = data.cardDescription;
        card.cardBaseEnergyCost = data.cardEnergyCost;
        card.xEnergyCost = data.xEnergyCost;
        //card.cardSprite = data.cardSprite;
        card.cardSprite = GetCardSpriteByName(data.cardName);
        card.cardType = data.cardType;
        card.rarity = data.rarity;
        card.racialCard = data.racialCard;
        card.originRace = data.originRace;
        card.targettingType = data.targettingType;
        card.talentSchool = data.talentSchool;

        // key words
        card.expend = data.expend;
        card.fleeting = data.fleeting;
        card.innate = data.innate;
        card.lifeSteal = data.lifeSteal;
        card.unplayable = data.unplayable;
        card.blessing = data.blessing;
        card.affliction = data.affliction;
        card.racial = data.racialCard;
        card.sourceSpell = data.sourceSpell;

        // lists
        card.cardEventListeners.AddRange(data.cardEventListeners);
        card.cardPassiveEffects.AddRange(data.cardPassiveEffects);
        card.cardEffects.AddRange(data.cardEffects);
        card.keyWordModels.AddRange(data.keyWordModels);
        card.cardDescriptionTwo.AddRange(data.customDescription);

        return card;
    }
    private Card BuildCardFromCardData(CardData data, CharacterEntityModel owner)
    {
        Debug.Log("CardController.BuildCardFromCardData() called...");

        Card card = new Card();

        // Core data
        card.owner = owner;
        card.cardName = data.cardName;
        card.cardDescription = data.cardDescription;
        card.cardBaseEnergyCost = data.cardBaseEnergyCost;
        card.xEnergyCost = data.xEnergyCost;
        card.cardSprite = GetCardSpriteByName(data.cardName);
        card.cardType = data.cardType;
        card.rarity = data.rarity;
        card.targettingType = data.targettingType;
        card.talentSchool = data.talentSchool;
        card.upgradeLevel = data.upgradeLevel;
        card.upgradeable = data.upgradeable;

        // key words
        card.expend = data.expend;
        card.fleeting = data.fleeting;
        card.innate = data.innate;
        card.unplayable = data.unplayable;
        card.lifeSteal = data.lifeSteal;
        card.blessing = data.blessing;
        card.racial = data.racialCard;
        card.affliction = data.affliction;
        card.sourceSpell = data.sourceSpell;

        // lists
        card.cardEventListeners.AddRange(data.cardEventListeners);
        card.cardPassiveEffects.AddRange(data.cardPassiveEffects);
        card.cardEffects.AddRange(data.cardEffects);
        card.keyWordModels.AddRange(data.keyWordModels);
        card.cardDescriptionTwo.AddRange(data.cardDescriptionTwo);

        return card;
    }
    private Card BuildCardFromCard(Card original, CharacterEntityModel owner)
    {
        // OVER LOAD: inseatd of building from scriptable object, clones
        // an existing card and builds from that
        Debug.Log("CardController.BuildCardFromCard() called...");

        Card card = new Card();

        // Core data
        card.owner = owner;
        card.cardName = original.cardName;
        card.cardDescription = original.cardDescription;
        card.cardSprite = GetCardSpriteByName(original.cardName);
        card.cardType = original.cardType;
        card.rarity = original.rarity;
        card.targettingType = original.targettingType;
        card.talentSchool = original.talentSchool;

        // Energy related
        card.xEnergyCost = original.xEnergyCost;
        card.cardBaseEnergyCost = original.cardBaseEnergyCost;
        card.energyReductionUntilPlayed = original.energyReductionUntilPlayed;
        card.energyReductionThisCombatOnly = original.energyReductionThisCombatOnly;
        card.energyReductionThisActivationOnly = original.energyReductionThisActivationOnly;

        // key words
        card.expend = original.expend;
        card.fleeting = original.fleeting;
        card.innate = original.innate;
        card.unplayable = original.unplayable;
        card.lifeSteal = original.lifeSteal;
        card.blessing = original.blessing;
        card.affliction = original.affliction;
        card.racial = original.racial;
        card.sourceSpell = original.sourceSpell;

        card.cardEffects = new List<CardEffect>();
        foreach (CardEffect ce in original.cardEffects)
        {
            card.cardEffects.Add(ObjectCloner.CloneJSON(ce));
        }

        // Card event listeners
        card.cardEventListeners = new List<CardEventListener>();
        foreach (CardEventListener cel in original.cardEventListeners)
        {
            card.cardEventListeners.Add(ObjectCloner.CloneJSON(cel));
        }

        // Key word models
        card.keyWordModels = new List<KeyWordModel>();
        foreach (KeyWordModel kwdm in original.keyWordModels)
        {
            card.keyWordModels.Add(ObjectCloner.CloneJSON(kwdm));
        }

        return card;

    }
    public CardViewModel BuildCardViewModelFromCard(Card card, Vector3 position)
    {
        Debug.Log("CardController.BuildCardViewModelFromCard() called...");

        CardViewModel cardVM = null;
        if (card.targettingType == TargettingType.NoTarget)
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
        SetUpCardViewModelAppearanceFromCard(cardVM, card);

        // Set glow outline
        AutoUpdateCardGlowOutline(card);

        return cardVM;
    }
    public void BuildCharacterEntityCombatDeckFromDeckData(CharacterEntityModel defender, List<CardData> deckData)
    {
        Debug.Log("CardController.BuildDefenderDeckFromDeckData() called...");

        // Convert each cardDataSO into a card object
        foreach (CardData cardData in deckData)
        {
            Card newCard = BuildCardFromCardData(cardData, defender);
            AddCardToDrawPile(defender, newCard);
            ConnectCombatCardWithCardInCharacterDataDeck(newCard, cardData);
        }

        // Shuffle the characters draw pile
        defender.drawPile.Shuffle();
    }
    public void BuildCharacterEntityCombatDeckFromCardDataSoSet(CharacterEntityModel defender, List<CardDataSO> deckData)
    {
        Debug.Log("CardController.BuildDefenderDeckFromDeckData() called...");

        // Convert each cardDataSO into a card object
        foreach (CardDataSO cardData in deckData)
        {
            Card newCard = BuildCardFromCardDataSO(cardData, defender);
            AddCardToDrawPile(defender, newCard);
            //ConnectCombatCardWithCardInCharacterDataDeck(newCard, cardData);
        }

        // Shuffle the characters draw pile
        defender.drawPile.Shuffle();
    }
    public void SetUpCardViewModelAppearanceFromCard(CardViewModel cardVM, Card card)
    {
        if (cardVM == null)
        {
            Debug.LogWarning("SetUpCardViewModelAppearanceFromCard() CVM IS NULL!!!");
        }

        // Set texts and images
        SetCardViewModelNameText(cardVM, card.cardName, card.upgradeLevel >= 1);
        AutoUpdateCardDescriptionText(card);
        SetCardViewModelDescriptionText(cardVM, TextLogic.ConvertCustomStringListToString(card.cardDescriptionTwo));
        SetCardViewModelEnergyText(card, cardVM, GetCardEnergyCost(card).ToString());
        SetCardViewModelGraphicImage(cardVM, GetCardSpriteByName(card.cardName));
        SetCardViewModelTalentSchoolImage(cardVM, SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(card.talentSchool));
        ApplyCardViewModelTalentColoring(cardVM, ColorLibrary.Instance.GetTalentColor(card.talentSchool));
        ApplyCardViewModelRarityColoring(cardVM, ColorLibrary.Instance.GetRarityColor(card.rarity));
        SetCardViewModelCardTypeImage(cardVM, SpriteLibrary.Instance.GetCardTypeImageFromTypeEnumData(card.cardType));
        SetCardViewModelTypeImageParentVisibility(cardVM, true);

        // Hide talent type parent for blessings, afflictions, etc
        if (card.affliction == true ||
            card.blessing == true ||
            card.talentSchool == TalentSchool.Neutral ||
             card.talentSchool == TalentSchool.None)
        {
            SetCardViewModelTalentImageParentVisibility(cardVM, false);
        }
        else
        {
            SetCardViewModelTalentImageParentVisibility(cardVM, true);
        }

    }
    public void SetUpCardViewModelAppearanceFromCard(CardViewModel cardVM, CampCard card)
    {
        // Set texts and images
        SetCardViewModelNameText(cardVM, card.cardName);
        SetCardViewModelDescriptionText(cardVM, TextLogic.ConvertCustomStringListToString(card.customDescription));
        SetCardViewModelEnergyText(cardVM, card, card.cardEnergyCost.ToString());
        SetCardViewModelGraphicImage(cardVM, CampSiteController.Instance.GetCampCardSpriteByName(card.cardName));
    }
    public void BuildCardViewModelFromCardData(CardData card, CardViewModel cardVM)
    {
        Debug.Log("CardController.BuildCardViewModelFromCardData() called...");

        // Set texts and images
        SetCardViewModelNameText(cardVM, card.cardName, card.upgradeLevel >= 1);
        AutoUpdateCardDescription(card);
        SetCardViewModelDescriptionText(cardVM, TextLogic.ConvertCustomStringListToString(card.cardDescriptionTwo));
        SetCardViewModelEnergyText(null, cardVM, card.cardBaseEnergyCost.ToString());
        SetCardViewModelGraphicImage(cardVM, GetCardSpriteByName(card.cardName));
        SetCardViewModelTalentSchoolImage(cardVM, SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(card.talentSchool));
        ApplyCardViewModelTalentColoring(cardVM, ColorLibrary.Instance.GetTalentColor(card.talentSchool));
        ApplyCardViewModelRarityColoring(cardVM, ColorLibrary.Instance.GetRarityColor(card.rarity));
        SetCardViewModelCardTypeImage(cardVM, SpriteLibrary.Instance.GetCardTypeImageFromTypeEnumData(card.cardType));
        SetCardViewModelTypeImageParentVisibility(cardVM, true);

        // Hide talent type parent for blessings, afflictions, etc
        if (card.affliction == true ||
            card.blessing == true ||
            card.talentSchool == TalentSchool.Neutral ||
             card.talentSchool == TalentSchool.None)
        {
            SetCardViewModelTalentImageParentVisibility(cardVM, false);
        }
        else
        {
            SetCardViewModelTalentImageParentVisibility(cardVM, true);
        }
        

    }
    public void BuildCardViewModelFromItemData(ItemData item, CardViewModel cardVM)
    {
        Debug.Log("CardController.BuildCardViewModelFromCardData() called...");

        // Set texts and images
        SetCardViewModelNameText(cardVM, item.itemName, false);
        SetCardViewModelDescriptionText(cardVM, TextLogic.ConvertCustomStringListToString(item.customDescription));
        SetCardViewModelGraphicImage(cardVM, item.ItemSprite);
        ApplyCardViewModelRarityColoring(cardVM, ColorLibrary.Instance.GetRarityColor(item.itemRarity));
        SetCardViewModelTypeImageParentVisibility(cardVM, false);
        SetCardViewModelTalentImageParentVisibility(cardVM, false);
        cardVM.energyParent.SetActive(false);

    }
    public void BuildCardViewModelFromStateData(StateData state, CardViewModel cardVM)
    {
        Debug.Log("CardController.BuildCardViewModelFromStateData() called...");

        // Set texts and images
        SetCardViewModelNameText(cardVM, TextLogic.SplitByCapitals(state.stateName.ToString()), false);
        SetCardViewModelDescriptionText(cardVM, TextLogic.ConvertCustomStringListToString(state.customDescription));
        SetCardViewModelGraphicImage(cardVM, state.StateSprite);
        ApplyCardViewModelRarityColoring(cardVM, ColorLibrary.Instance.GetRarityColor(state.rarity));
        SetCardViewModelTypeImageParentVisibility(cardVM, false);
        SetCardViewModelTalentImageParentVisibility(cardVM, false);
        cardVM.energyParent.SetActive(false);

    }
    private void ConnectCombatCardWithCardInCharacterDataDeck(Card card, CardData deckDataCard)
    {
        card.myCharacterDeckCard = deckDataCard;
    }
    private void ConnectCardWithCardViewModel(Card card, CardViewModel cardVM)
    {
        card.cardVM = cardVM;
        cardVM.card = card;
    }
    private void DisconnectCardAndCardViewModel(Card card, CardViewModel cardVM)
    {
        if(card != null)
        {
            card.cardVM = null;
        }
        if(cardVM != null)
        {
            cardVM.card = null;
        }       
        
    }
    #endregion

    // Card View Model Specific Logic
    #region
    public void AutoUpdateCardDescriptionText(Card card, CharacterEntityModel target = null)
    {
        // Function is used to automatically update the descriptions
        // of cards in hand when the values that dictate the damage/block 
        // they grant are changed by external factors

        // Damage card logic
        foreach(CustomString cs in card.cardDescriptionTwo)
        {
            // Does the custom string even have a dynamic value?
            if (cs.getPhraseFromCardValue)
            {
                // It does, start searching for a card effect that
                // matches the effect value of the custom string
                CardEffect matchingEffect = null;
                foreach (CardEffect ce in card.cardEffects)
                {
                    if(ce.cardEffectType == cs.cardEffectType)
                    {
                        // Found a match, cache it and break
                        matchingEffect = ce;
                        break;
                    }
                }

                // Which type of value should be inserted into the custom string phrase?

                // Damage Target
                if(cs.cardEffectType == CardEffectType.DamageTarget)
                {                   
                    int damageValue = 0;

                    if(target != null)
                    {
                        if (target.pManager.incorporealStacks > 0)
                            damageValue = 1;
                        else
                            damageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(card.owner, target, matchingEffect.damageType, matchingEffect.baseDamageValue, card, matchingEffect, false);
                    }
                    else
                    {
                        damageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(card.owner, null, matchingEffect.damageType, matchingEffect.baseDamageValue, card, matchingEffect, false);
                    }

                    
                    cs.phrase = damageValue.ToString();
                }

                // Damage All Enemies
                else if (cs.cardEffectType == CardEffectType.DamageAllEnemies)
                {
                    int damageValue = 0;
                    damageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(card.owner, null, matchingEffect.damageType, matchingEffect.baseDamageValue, card, matchingEffect, false);
                    cs.phrase = damageValue.ToString();
                }

                // Modify All Cards In Hand >> Damage All Enemies
                else if (cs.cardEffectType == CardEffectType.ModifyAllCardsInHand)
                {
                    // Find the damage all enemies mod effect
                    ModifyAllCardsInHandEffect damageEffectMod = null;
                    foreach(ModifyAllCardsInHandEffect modEffect2 in matchingEffect.modifyCardsInHandEffects)
                    {
                        if(modEffect2.modifyEffect == ModifyAllCardsInHandEffectType.DamageAllEnemies)
                        {
                            damageEffectMod = modEffect2;
                            break;
                        }
                    }

                    if(damageEffectMod != null)
                    {
                        
                        int damageValue = 0;
                        damageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(card.owner, null, damageEffectMod.damageType, damageEffectMod.baseDamage, card, matchingEffect, CombatLogic.Instance.RollForCritical(card.owner));
                        cs.phrase = damageValue.ToString();
                    }
                   
                }

                // Lose HP
                else if (cs.cardEffectType == CardEffectType.LoseHP)
                {
                    int damageValue = matchingEffect.healthLost;

                    if(card.owner.pManager.barrierStacks > 0)
                    {
                        damageValue = 0;
                    }

                    cs.phrase = damageValue.ToString();
                }

                // Gain Block Target
                else if (cs.cardEffectType == CardEffectType.GainBlockTarget)
                {
                    int blockGainValue = 0;

                    if (target != null)
                    {
                        blockGainValue = CombatLogic.Instance.CalculateBlockGainedByEffect(matchingEffect.blockGainValue, card.owner, target, null, matchingEffect);
                    }
                    else
                    {
                        blockGainValue = CombatLogic.Instance.CalculateBlockGainedByEffect(matchingEffect.blockGainValue, card.owner, card.owner, null, matchingEffect);
                    }
                    
                    cs.phrase = blockGainValue.ToString();
                }

                // Gain Block Self 
                else if (cs.cardEffectType == CardEffectType.GainBlockSelf)
                {
                    int blockGainValue = CombatLogic.Instance.CalculateBlockGainedByEffect(matchingEffect.blockGainValue, card.owner, card.owner, null, matchingEffect);
                    cs.phrase = blockGainValue.ToString();
                }

                // Gain Block All Allies
                else if (cs.cardEffectType == CardEffectType.GainBlockAllAllies)
                {
                    int blockGainValue = CombatLogic.Instance.CalculateBlockGainedByEffect(matchingEffect.blockGainValue, card.owner, null, null, matchingEffect);
                    cs.phrase = blockGainValue.ToString();
                }

                // Draw cards
                else if (cs.cardEffectType == CardEffectType.DrawCards)
                {
                    cs.phrase = matchingEffect.cardsDrawn.ToString();
                }
            }
        }

        // Finally, set the new value on the description text
       // SetCardViewModelDescriptionText(card.cardVM, TextLogic.ConvertCustomStringListToString(card.cardDescriptionTwo));
    }
    public void AutoUpdateCardDescription(CardData card)
    {
        // Function is used to automatically update the descriptions
        // of cards in hand when the values that dictate the damage/block 
        // they grant are changed by external factors

        // Damage card logic
        foreach (CustomString cs in card.cardDescriptionTwo)
        {
            // Does the custom string even have a dynamic value?
            if (cs.getPhraseFromCardValue)
            {
                // It does, start searching for a card effect that
                // matches the effect value of the custom string
                CardEffect matchingEffect = null;
                foreach (CardEffect ce in card.cardEffects)
                {
                    if (ce.cardEffectType == cs.cardEffectType)
                    {
                        // Found a match, cache it and break
                        matchingEffect = ce;
                        break;
                    }
                }

                // Which type of value should be inserted into the custom string phrase?

                // Damage enemies 
                if (cs.cardEffectType == CardEffectType.DamageTarget ||
                    cs.cardEffectType == CardEffectType.DamageAllEnemies)
                {
                    int damageValue = matchingEffect.baseDamageValue;
                    cs.phrase = damageValue.ToString();
                }

                // Modify All Cards In Hand >> Damage All Enemies
                else if (cs.cardEffectType == CardEffectType.ModifyAllCardsInHand)
                {
                    // Find the damage all enemies mod effect
                    ModifyAllCardsInHandEffect damageEffectMod = null;
                    foreach (ModifyAllCardsInHandEffect modEffect2 in matchingEffect.modifyCardsInHandEffects)
                    {
                        if (modEffect2.modifyEffect == ModifyAllCardsInHandEffectType.DamageAllEnemies)
                        {
                            damageEffectMod = modEffect2;
                            break;
                        }
                    }

                    if (damageEffectMod != null)
                    {
                        int damageValue = damageEffectMod.baseDamage;
                        cs.phrase = damageValue.ToString();
                    }

                }

                // Lose HP
                else if (cs.cardEffectType == CardEffectType.LoseHP)
                {
                    int damageValue = matchingEffect.healthLost;
                    cs.phrase = damageValue.ToString();
                }

                // Gain Block Target
                else if (cs.cardEffectType == CardEffectType.GainBlockTarget ||
                    cs.cardEffectType == CardEffectType.GainBlockSelf ||
                    cs.cardEffectType == CardEffectType.GainBlockAllAllies)
                {
                    int blockGainValue = matchingEffect.blockGainValue;
                    cs.phrase = blockGainValue.ToString();
                }

                // Draw cards
                else if (cs.cardEffectType == CardEffectType.DrawCards)
                {
                    cs.phrase = matchingEffect.cardsDrawn.ToString();
                }
            }
        }
    }
    public void SetCardViewModelNameText(CardViewModel cvm, string name, bool upgradeCard = false)
    {
        cvm.nameText.text = name;
        if (upgradeCard)
        {
            Debug.Log("CardController.SetCardViewModelNameText() upgrade detected, change font color...");
            cvm.nameText.color = ColorLibrary.Instance.cardUpgradeFontColor;
        }
        else
        {
            cvm.nameText.color = Color.white;
        }

        if (cvm.myPreviewCard != null)
        {
            Debug.Log("SETTING CARD VIEW MODEL PREVIEW NAME!!");
            SetCardViewModelNameText(cvm.myPreviewCard, name, upgradeCard);
        }
    }
    public void SetCardViewModelDescriptionText(CardViewModel cvm, string description)
    {
        cvm.descriptionText.text = description;
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelDescriptionText(cvm.myPreviewCard, description);
        }
    }
    public void SetCardViewModelEnergyText(Card card, CardViewModel cvm, string energyCost)
    {
        cvm.energyText.text = energyCost;
        cvm.energyText.color = Color.white;

        // color text if cost is more or less then base.
        if(card != null)
        {
            if (card.xEnergyCost)
            {
                cvm.energyText.text = "X";
                energyCost = "X";
            }
            else
            {
                int currentCost = GetCardEnergyCost(card);

                if (currentCost > card.cardBaseEnergyCost)
                {
                    cvm.energyText.color = Color.red;
                }
                else if (currentCost < card.cardBaseEnergyCost)
                {
                    cvm.energyText.color = Color.green;
                }
            }          
        }

        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelEnergyText(card, cvm.myPreviewCard, energyCost);
        }
    }
    public void SetCardViewModelEnergyText(CardViewModel cvm, CampCard card, string energyCost)
    {
        cvm.energyText.text = energyCost;
        cvm.energyText.color = Color.white;

        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelEnergyText(cvm.myPreviewCard, card, energyCost);
        }
    }
    public void SetCardViewModelGraphicImage(CardViewModel cvm, Sprite sprite)
    {
        cvm.graphicImage.sprite = sprite;
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelGraphicImage(cvm.myPreviewCard, sprite);
        }
    }
    public void SetCardViewModelTalentImageParentVisibility(CardViewModel cvm, bool onOrOff)
    {
        cvm.talentSchoolParent.gameObject.SetActive(onOrOff);
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelTalentImageParentVisibility(cvm.myPreviewCard, onOrOff);
        }
    }
    public void SetCardViewModelTalentSchoolImage(CardViewModel cvm, Sprite sprite)
    {
        if (sprite)
        {
            //cvm.talentSchoolParent.SetActive(true);
            cvm.talentSchoolImage.sprite = sprite;
            if (cvm.myPreviewCard != null)
            {
                SetCardViewModelTalentSchoolImage(cvm.myPreviewCard, sprite);
            }
        }
    }
    public void ApplyCardViewModelTalentColoring(CardViewModel cvm, Color color)
    {
        foreach (Image sr in cvm.talentRenderers)
        {
            if(cvm.card != null && cvm.card.racialCard)
            {
                sr.color = ColorLibrary.Instance.racialColor;
            }
            else if (cvm.card != null && cvm.card.affliction)
            {
                sr.color = ColorLibrary.Instance.afflictionColor;
            }
            else
            {
                sr.color = color;
            }
           
        }
        if (cvm.myPreviewCard != null)
        {
            ApplyCardViewModelTalentColoring(cvm.myPreviewCard, color);
        }
    }
    public void ApplyCardViewModelRarityColoring(CardViewModel cvm, Color color)
    {
        foreach (Image sr in cvm.rarityRenderers)
        {
            sr.color = color;
        }
        if (cvm.myPreviewCard != null)
        {
            ApplyCardViewModelRarityColoring(cvm.myPreviewCard, color);
        }
    }
    public void SetCardViewModelTypeImageParentVisibility(CardViewModel cvm, bool onOrOff)
    {
        cvm.cardTypeParent.gameObject.SetActive(onOrOff);
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelTypeImageParentVisibility(cvm.myPreviewCard, onOrOff);
        }
    }
    public void SetCardViewModelCardTypeImage(CardViewModel cvm, Sprite sprite)
    {
        foreach(Image im in cvm.cardTypeImages)
        {
            im.sprite = sprite;
        }

        // do for card preview also
        if (cvm.myPreviewCard != null)
        {
            SetCardViewModelCardTypeImage(cvm.myPreviewCard, sprite);
        }
    }

    #endregion

    // Card draw Logic
    #region
    public Card DrawACardFromDrawPile(CharacterEntityModel defender, int drawPileIndex = 0)
    {
        Debug.Log("CardController.DrawACardFromDrawPile() called...");
        Card cardDrawn = null;

        // Shuffle discard pile back into draw pile if draw pile is empty
        if (IsDrawPileEmpty(defender))
        {
            MoveAllCardsFromDiscardPileToDrawPile(defender);
        }
        if (IsCardDrawValid(defender))
        {
            // Get card and remove from deck
            cardDrawn = defender.drawPile[drawPileIndex];
            RemoveCardFromDrawPile(defender, cardDrawn);

            // Add card to hand
            AddCardToHand(defender, cardDrawn);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => DrawCardFromDeckVisualEvent(cardDrawn, defender), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);

            // Resolve on draw events
            HandleOnThisCardDrawnListenerEvents(cardDrawn);
        }

        return cardDrawn;
    }
    public Card DrawACardFromDrawPile(CharacterEntityModel defender, Card cardDrawn)
    {
        Debug.Log("CardController.DrawACardFromDrawPile() called...");

        // cancel if card draw is invalid
        if(cardDrawn == null || 
            defender.drawPile.Contains(cardDrawn) == false)
        {
            return null;
        }

        if (IsCardDrawValid(defender))
        {
            // Remove from deck
            RemoveCardFromDrawPile(defender, cardDrawn);

            // Add card to hand
            AddCardToHand(defender, cardDrawn);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => DrawCardFromDeckVisualEvent(cardDrawn, defender), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);

            // Resolve on draw events
            HandleOnThisCardDrawnListenerEvents(cardDrawn);
        }

        return cardDrawn;
    }
    public void DrawCardsOnActivationStart(CharacterEntityModel defender)
    {
        Debug.Log("CardController.DrawCardsOnActivationStart() called...");

        int totalDraw = EntityLogic.GetTotalDraw(defender);

        // Check 'Well Laid Plans' state
        if (StateController.Instance.DoesPlayerHaveState(StateName.WellLaidPlans) &&
            ActivationManager.Instance.CurrentTurn == 1)
        {
            totalDraw += 2;
        }

        for (int i = 0; i < totalDraw; i++)
        {
            // Priortitise drawing innate if turn 1
            if(ActivationManager.Instance.CurrentTurn == 1 &&
                GlobalSettings.Instance.innateSetting == InnateSettings.PrioritiseInnate)
            {
                // try find an innate card
                Card cardDrawn = null;
                foreach(Card card in defender.drawPile)
                {
                    if (card.innate)
                    {
                        cardDrawn = card;
                        break;
                    }
                }

                // did we find an innate card?
                if(cardDrawn != null)
                {
                    // we did, draw it
                    DrawACardFromDrawPile(defender, cardDrawn);
                }
                else
                {
                    // we didnt, draw the first card from top of draw pile instead
                    DrawACardFromDrawPile(defender);
                }               
            }

            else
            {
                DrawACardFromDrawPile(defender);
            }
        }

        // Opener: draw extra
        if(ActivationManager.Instance.CurrentTurn == 1 &&
            GlobalSettings.Instance.innateSetting == InnateSettings.DrawInnateCardsExtra)
        {
            // find innate cards
            List<Card> innateCards = new List<Card>();
            foreach(Card card in defender.drawPile)
            {
                if (card.innate)
                {
                    innateCards.Add(card);
                }
            }

            // draw innate cards
            foreach(Card iCard in innateCards)
            {
                DrawACardFromDrawPile(defender, defender.drawPile.IndexOf(iCard));
            }
        }

        // Check 'Gods Chosen' State
        if (StateController.Instance.DoesPlayerHaveState(StateName.GodsChosen))
        {
            Debug.Log("CardController.DrawCardsOnActivationStart() handling card upgrade from 'God's Chosen' state...");
            List<Card> upgradeableCards = new List<Card>();
            Card chosenCard = null;
            foreach(Card c in defender.hand)
            {
                if (IsCardUpgradeable(c))
                {
                    upgradeableCards.Add(c);
                }
            }

            if(upgradeableCards.Count > 0)
                chosenCard = upgradeableCards[RandomGenerator.NumberBetween(0, upgradeableCards.Count - 1)];

            if(chosenCard != null)
            {
                Debug.Log("Found valid upgradeable card for God's chosen state effect, upgrading: "+ chosenCard.cardName);
                HandleUpgradeCardForCharacterEntity(chosenCard);
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
               // VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(chosenCard.cardVM));
            }
                
        }
    }
    #endregion

    // Gain card not from deck logic
    #region
    public Card CreateAndAddNewCardToCharacterHand(CharacterEntityModel defender, CardDataSO data)
    {
        Card cardReturned = null;
        if (!IsHandFull(defender))
        {
            // Get card and remove from deck
            Card newCard = BuildCardFromCardDataSO(data, defender);

            // Add card to hand
            AddCardToHand(defender, newCard);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => CreateAndAddNewCardToCharacterHandVisualEvent(newCard, defender), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);

            // cache card
            cardReturned = newCard;
        }

        return cardReturned;
    }
    public Card CreateAndAddNewCardToCharacterDiscardPile(CharacterEntityModel defender, CardDataSO data)
    {
        Card cardReturned = null;

        // Get card and remove from deck
        Card newCard = BuildCardFromCardDataSO(data, defender);

        // Add card to hand
        AddCardToDiscardPile(defender, newCard);

        // cache card
        cardReturned = newCard;

        return cardReturned;
    }
    public Card CreateAndAddNewCardToCharacterDrawPile(CharacterEntityModel defender, CardDataSO data, bool randomIndex = true)
    {
        Card cardReturned = null;

        // Get card and remove from deck
        Card newCard = BuildCardFromCardDataSO(data, defender);

        // Add card to hand
        AddCardToDrawPile(defender, newCard);

        // randomize position in draw pile
        if (randomIndex)
        {
            defender.drawPile.Remove(newCard);
            int index = RandomGenerator.NumberBetween(0, defender.drawPile.Count);
            defender.drawPile.Insert(index, newCard);
        }

        // cache card
        cardReturned = newCard;

        return cardReturned;
    }
    public Card CreateAndAddNewCardToCharacterHand(CharacterEntityModel defender, CardData data)
    {
        Card cardReturned = null;
        if (!IsHandFull(defender))
        {
            // Get card and remove from deck
            Card newCard = BuildCardFromCardData(data, defender);

            // Add card to hand
            AddCardToHand(defender, newCard);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => CreateAndAddNewCardToCharacterHandVisualEvent(newCard, defender), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);

            // cache card
            cardReturned = newCard;
        }

        return cardReturned;
    }
    public Card CreateAndAddNewCardToCharacterHand(CharacterEntityModel defender, Card data)
    {
        Card cardReturned = null;
        if (!IsHandFull(defender))
        {
            // Get card and remove from deck
            Card newCard = BuildCardFromCard(data, defender);

            // Add card to hand
            AddCardToHand(defender, newCard);

            // Create and queue card drawn visual event
            VisualEventManager.Instance.CreateVisualEvent(() => CreateAndAddNewCardToCharacterHandVisualEvent(newCard, defender), QueuePosition.Back, 0, 0.2f, EventDetail.CardDraw);

            // cache card
            cardReturned = newCard;
        }

        return cardReturned;
    }
    public void CreateAndAddNewRandomBlessingsToCharacterHand(CharacterEntityModel defender, int blessingsAdded, UpgradeFilter uf)
    {
        //List<CardData> blessings = QueryByBlessing(AllCards, true);
        List<CardData> blessings = GetCardsQuery(AllCards, TalentSchool.None, Rarity.None, true, uf);

        for (int i = 0; i < blessingsAdded; i++)
        {
            if (blessings.Count > 0 &&
                defender.livingState == LivingState.Alive &&
                CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
            {
                // Randomize card chosen
                //int randomIndex = RandomGenerator.NumberBetween(0, blessings.Count - 1);
                blessings.Shuffle();
                CardData blessingChosen = blessings[0];

                // Add card to hand
                //CreateAndAddNewCardToCharacterHand(defender, blessings[randomIndex]);
                CreateAndAddNewCardToCharacterHand(defender, blessingChosen);
            }
        }
    }
    #endregion

    // Card Discard + Removal Logic
    #region
    public void ExpendFleetingCardsOnActivationEnd(CharacterEntityModel defender)
    {
        Card[] cardsToDiscard = defender.hand.ToArray();

        foreach (Card card in cardsToDiscard)
        {
            if (card.fleeting)
            {
                ExpendCard(card, true);
            }
            
        }
    }
    public void DiscardHandOnActivationEnd(CharacterEntityModel defender)
    {
        Debug.Log("CardController.DiscardHandOnActivationEnd() called, hand size = " + defender.hand.Count.ToString());

        Card[] cardsToDiscard = defender.hand.ToArray();

        foreach(Card card in cardsToDiscard)
        {
            DiscardCardFromHand(card);
        }
    }
    private void DiscardCardFromHand(Card card)
    {
        Debug.Log("CardController.DiscardCardFromHand() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;
        CharacterEntityModel owner = card.owner;

        // remove from hand
        RemoveCardFromHand(owner, card);

        // place on top of discard pile
        AddCardToDiscardPile(owner, card);

        // does the card have a cardVM linked to it?
        if (cvm)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => DiscardCardFromHandVisualEvent(cvm, owner), QueuePosition.Back, 0f, 0.1f);
        }                         

    }
    private void ExpendCard(Card card, bool fleeting = false, QueuePosition qp = QueuePosition.Back, float startDelay = 0, float endDelay = 0.5f)
    {
        Debug.Log("CardController.ExpendCard() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;
        CharacterEntityModel owner = card.owner;

        // Remove card from which ever collection its in
        if (owner.hand.Contains(card))
        {
            RemoveCardFromHand(owner, card);
        }
        else if (owner.discardPile.Contains(card))
        {
            RemoveCardFromDiscardPile(owner, card);
        }
        else if (owner.drawPile.Contains(card))
        {
            RemoveCardFromDrawPile(owner, card);
        }

        // place in the expend pile
        AddCardToExpendPile(owner, card);

        // does the card have a cardVM linked to it?
        if (cvm)
        {
            if(fleeting == false)
            {
                ExpendCardVisualEvent(cvm, owner);
            }

            else
            {
                VisualEventManager.Instance.CreateVisualEvent(() => ExpendCardVisualEvent(cvm, owner), qp, startDelay, endDelay);
            }
            
        }

        OnCardExpended(card);
    }
    private void DestroyCardViewModel(CardViewModel cvm)
    {
        Debug.Log("CardController.DestroyCardViewModel() called...");

        // Destoy script + GO
        Destroy(cvm.movementParent.gameObject);
    }
    #endregion
       
    // Conditional Checks
    #region
    private bool IsCardDrawValid(CharacterEntityModel defender)
    {
        if(IsDrawPileEmpty(defender))
        {
            return false;
        }
        else if(IsHandFull(defender))
        {
            return false;
        }
        else if (defender.activationPhase == ActivationPhase.NotActivated || defender.activationPhase == ActivationPhase.EndPhase)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool IsCardPlayable(Card card, CharacterEntityModel owner)
    {
        Debug.Log("CardController.IsCardPlayable() called, checking if '" +
            card.cardName + "' is playable by '" + owner.myName +"'");

        bool boolReturned = false;

        if(HasEnoughEnergyToPlayCard(card, owner) &&
            CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive &&
            card.unplayable == false &&
            !IsCardPlayBlockedByDisarmed(card, owner) &&
            !IsCardPlayBlockedBySilenced(card, owner) &&
            !IsCardBlockedByPistolero(card,owner))
        {
            boolReturned = true;
        }

        if (boolReturned == true)
        {
            Debug.Log("CardController.IsCardPlayable() detected that '" +
            card.cardName + "' is playable by '" + owner.myName + "', returning true...");
        }
        else
        {
            Debug.Log("CardController.IsCardPlayable() detected that '" +
            card.cardName + "' is NOT playable by '" + owner.myName + "', returning false...");
        }     

        return boolReturned;
    }
    public bool IsCardPlayBlockedByDisarmed(Card card, CharacterEntityModel owner)
    {
        if(card.cardType == CardType.MeleeAttack &&
            owner.pManager.disarmedStacks > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsCardPlayBlockedBySilenced(Card card, CharacterEntityModel owner)
    {
        if (card.cardType == CardType.Skill &&
            owner.pManager.silencedStacks > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsCardBlockedByPistolero(Card card, CharacterEntityModel owner)
    {
        if (card.cardType != CardType.RangedAttack &&
            owner.pManager.pistoleroStacks > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool HasEnoughEnergyToPlayCard(Card card, CharacterEntityModel owner)
    {
        Debug.Log("CardController.HasEnoughEnergyToPlayCard(), checking '" +
            card.cardName +"' owned by '" + owner.myName +"'");
        return GetCardEnergyCost(card) <= owner.energy;
    }
    private bool IsDrawPileEmpty(CharacterEntityModel character)
    {
        return character.drawPile.Count == 0;
    }
    private bool IsHandFull(CharacterEntityModel character)
    {
        return character.hand.Count >= 8;
    }
    public bool IsCharacterHoldingBlessing(CharacterEntityModel character)
    {
        bool bRet = false;
        foreach(Card card in character.hand)
        {
            if (card.blessing == true)
            {
                bRet = true;
                break;
            }
        }

        return bRet;
    }
    private bool DoesCardEffectMeetWeaponRequirement(CardEffect ce, CharacterEntityModel owner)
    {
        bool boolReturned = false;

        if(ce.weaponRequirement == CardWeaponRequirement.None)
        {
            boolReturned = true;
        }
        else if(ce.weaponRequirement == CardWeaponRequirement.DualWield &&
            ItemController.Instance.IsDualWielding(owner.iManager))
        {
            boolReturned = true;
        }
        else if (ce.weaponRequirement == CardWeaponRequirement.Shielded &&
            ItemController.Instance.IsShielded(owner.iManager))
        {
            boolReturned = true;
        }
        else if (ce.weaponRequirement == CardWeaponRequirement.TwoHanded &&
            ItemController.Instance.IsTwoHanding(owner.iManager))
        {
            boolReturned = true;
        }
        else if (ce.weaponRequirement == CardWeaponRequirement.Ranged &&
           ItemController.Instance.IsRanged(owner.iManager))
        {
            boolReturned = true;
        }

        return boolReturned;
    }
    private bool DoesCardEffectMeetTargetRequirement(CardEffect ce, CharacterEntityModel target)
    {
        bool bRet = false;

        if(target == null || ce.targetRequirement == TargetRequirement.None)
        {
            bRet = true;
        }
        else if (ce.targetRequirement == TargetRequirement.IsVulnerable && target.pManager.vulnerableStacks > 0)
        {
            bRet = true;
        }
        else if (ce.targetRequirement == TargetRequirement.IsWeakened && target.pManager.weakenedStacks > 0)
        {
            bRet = true;
        }

        return bRet;
    }
    private bool DoesCardEventListenerMeetWeaponRequirement(CardEventListener cel, CharacterEntityModel owner)
    {
        bool boolReturned = false;

        if (cel.weaponRequirement == CardWeaponRequirement.None)
        {
            boolReturned = true;
        }
        else if (cel.weaponRequirement == CardWeaponRequirement.DualWield &&
            ItemController.Instance.IsDualWielding(owner.iManager))
        {
            boolReturned = true;
        }
        else if (cel.weaponRequirement == CardWeaponRequirement.Shielded &&
            ItemController.Instance.IsShielded(owner.iManager))
        {
            boolReturned = true;
        }
        else if (cel.weaponRequirement == CardWeaponRequirement.TwoHanded &&
            ItemController.Instance.IsTwoHanding(owner.iManager))
        {
            boolReturned = true;
        }
        else if (cel.weaponRequirement == CardWeaponRequirement.Ranged &&
           ItemController.Instance.IsRanged(owner.iManager))
        {
            boolReturned = true;
        }

        return boolReturned;
    }

    #endregion

    // Playing Cards Logic
    #region
    private void OnCardPlayedStart(Card card, CharacterEntityModel target = null)
    {
        // Setup
        CharacterEntityModel owner = card.owner;
        int sourceReduction = 0;

        // Check dark bargain
        if(card.owner.pManager.darkBargainStacks > 0)
        {
            CombatLogic.Instance.HandleDamage(2, owner, owner, card, DamageType.None, true);
        }

        // Check 'Soulless' state
        if (card.cardType == CardType.Power &&
            StateController.Instance.DoesPlayerHaveState(StateName.Soulless))
        {
            CombatLogic.Instance.HandleDamage(2, owner, owner, card, DamageType.None, true);
        }

        // Calculate energy reduction amount from source (if source spell card)
        if (card.sourceSpell == true)
        {
            sourceReduction = GetCardEnergyCost(card) - GetCardEnergyCost(card, false);
        }

        // Pay Energy Cost
        CharacterEntityController.Instance.ModifyEnergy(owner, -GetCardEnergyCost(card));

        // Remove/Pay Source cost if Source Spell card
        if(card.sourceSpell == true)
        {
            PassiveController.Instance.ModifySource(owner.pManager, sourceReduction);
        }

        // Remove Melee Attack reduction passive
        if (card.cardType == CardType.MeleeAttack)
        {
            card.owner.meleeAttacksPlayedThisActivation++;

            if (owner.pManager.plantedFeetStacks > 0)
            {
                PassiveController.Instance.ModifyPlantedFeet(owner.pManager, -owner.pManager.plantedFeetStacks, false);
            }

            HandleOnMeleeAttackCardPlayedListeners(owner);

            if(owner.pManager.flurryStacks > 0)
            {
                VisualEvent batchedEvent = VisualEventManager.Instance.InsertTimeDelayInQueue(0f);

                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateAoEMeleeArc(owner.characterEntityView.WorldPosition);
                    VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.WorldPosition, "Flurry!");
                }, QueuePosition.BatchedEvent, 0f, 0f, EventDetail.None, batchedEvent);

                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                {
                    // Calculate damage
                    DamageType damageType = DamageType.Physical;
                    int baseDamage = card.owner.pManager.flurryStacks;

                    // Calculate the end damage value
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, enemy, damageType, baseDamage);

                    // Create blood explosion
                    VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateBloodExplosion(enemy.characterEntityView.WorldPosition), QueuePosition.BatchedEvent, 0f, 0f, EventDetail.None, batchedEvent);

                    // Start damage sequence
                    CombatLogic.Instance.HandleDamage(finalDamageValue, owner, enemy, damageType, batchedEvent);
                }

                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }

            if (owner.pManager.balancedStanceStacks > 0)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.WorldPosition, "Balanced Stance!"));

                CharacterEntityController.Instance.GainBlock(owner, CombatLogic.Instance.CalculateBlockGainedByEffect(owner.pManager.balancedStanceStacks, owner, owner, null, null));

                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }

        }

        // Remove Ranged Attack reduction passive
        if (card.cardType == CardType.RangedAttack)
        {
            card.owner.rangedAttacksPlayedThisActivation++;
            if (owner.pManager.takenAimStacks > 0)
            {
                PassiveController.Instance.ModifyTakenAim(owner.pManager, -owner.pManager.takenAimStacks, false);
            }
        }

        // Blessing listener
        if (card.blessing == true)
        {
            HandleOnBlessingCardPlayedListeners(owner);
        }

        // Fire Ball specific effects
        if (card.cardName == "Fire Ball" ||
            card.cardName == "Fire Ball +1")
        {
            HandleOnFireBallCardPlayedListeners(owner);
        }

        // Arcane Bolt specific effects
        if (card.cardName == "Arcane Bolt" ||
             card.cardName == "Arcane Bolt +1")
        {
            HandleOnArcaneBoltCardPlayedListeners(owner);
        }

        // Check 'Pumped Up!' state
        if(card.cardType == CardType.Power && StateController.Instance.DoesPlayerHaveState(StateName.PumpedUp))
        {
            // Gain block.
            CharacterEntityController.Instance.GainBlock
                    (owner, CombatLogic.Instance.CalculateBlockGainedByEffect(5, owner, owner, null, null));

            // Status notif
            VisualEventManager.Instance.CreateVisualEvent
                (() => VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.WorldPosition, "Pumped Up!"), QueuePosition.Back, 0, 0.5f);

        }

        // Infuriated 
        if (card.cardType == CardType.Skill)
        {
            foreach(CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                if(enemy.pManager.infuriatedStacks > 0)
                {
                    // Status notif
                    VisualEventManager.Instance.CreateVisualEvent
                        (()=> VisualEffectManager.Instance.CreateStatusEffect(enemy.characterEntityView.WorldPosition, "Infuriated!"), QueuePosition.Back, 0, 0.5f);

                    // Gain power
                    PassiveController.Instance.ModifyBonusPower(enemy.pManager, enemy.pManager.infuriatedStacks, true, 0.5f);
                }
            }
        }


        // Blessings
        if (card.blessing)
        {
            // Consecration
            if(card.owner.pManager.consecrationStacks > 0)
            {
                VisualEvent batchedEvent = VisualEventManager.Instance.InsertTimeDelayInQueue(0f);

                // VFX notification
                VisualEventManager.Instance.CreateVisualEvent(() =>
                {
                    VisualEffectManager.Instance.CreateHolyNova(owner.characterEntityView.WorldPosition);
                    VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.WorldPosition, "Consecration!");
                });

                // Apply burning to all enemies
                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                {
                    PassiveController.Instance.ModifyBurning(enemy.pManager, card.owner.pManager.consecrationStacks, true, 0f);
                }

                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }

            // Evangelize
            if (card.owner.pManager.evangelizeStacks > 0)
            {
                VisualEventManager.Instance.CreateVisualEvent(() => 
                VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.WorldPosition, "Evangelize!"), QueuePosition.Back, 0f, 0.5f);

                PassiveController.Instance.ModifyBonusDexterity(card.owner.pManager, card.owner.pManager.evangelizeStacks, true, 0.5f);
            }

            // Check holier than thou passive
            if (owner.pManager.holierThanThouStacks > 0 && target != null)
            {
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.WorldPosition, "Holier Than Thou!"));

                CharacterEntityController.Instance.GainBlock
                    (target, CombatLogic.Instance.CalculateBlockGainedByEffect(owner.pManager.holierThanThouStacks, owner, target, null, null));

                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }
        }

        // Where should this card be sent to?
        if (card.expend ||
            (card.cardType == CardType.Power && GlobalSettings.Instance.onPowerCardPlayedSetting == OnPowerCardPlayedSettings.Expend))
        {
            ExpendCard(card);
        }

        else if(card.cardType == CardType.Power && GlobalSettings.Instance.onPowerCardPlayedSetting == OnPowerCardPlayedSettings.RemoveFromPlay)
        {
            CardViewModel cardVM = card.cardVM;

            if (owner.hand.Contains(card))
            {
                RemoveCardFromHand(owner, card);
            }

            if (cardVM)
            {
                PlayACardFromHandVisualEvent(cardVM, owner.characterEntityView);
            }
        }

        else
        {
            // Do normal 'play from hand' stuff
            CardViewModel cardVM = card.cardVM;
            if (owner.hand.Contains(card))
            {
                RemoveCardFromHand(owner, card);
                AddCardToDiscardPile(owner, card);
            }

            if (cardVM)
            {
                PlayACardFromHandVisualEvent(cardVM, owner.characterEntityView);
            }           
        }
    }
    private void OnCardPlayedFinish(Card card)
    {
        // Ranged attack logic
        if (card.cardType == CardType.RangedAttack)
        {
            //card.owner.rangedAttacksPlayedThisActivation++;

            // Remove Long Draw
            if (card.owner.pManager.longDrawStacks > 0)
            {
                PassiveController.Instance.ModifyLongDraw(card.owner.pManager, -1);
            }
        }

        // Melee attack logic
        if (card.cardType == CardType.MeleeAttack)
        {
            //card.owner.meleeAttacksPlayedThisActivation++;

            if (card.owner.pManager.sharpenBladeStacks > 0)
            {
                PassiveController.Instance.ModifySharpenBlade(card.owner.pManager, -1);
            }

            if(card.owner.pManager.shockingTouchStacks > 0)
            {
                PassiveController.Instance.ModifyOverload(card.owner.pManager, card.owner.pManager.shockingTouchStacks);
            }
        }

        // Resolve 'On this card played listeners
        HandleOnThisCardPlayedListenerEvents(card);

        // Update energy cost text of cards in hand with event listeners
        foreach(Card c in card.owner.hand)
        {
            if(c.cardVM != null)
            SetCardViewModelEnergyText(c, c.cardVM, GetCardEnergyCost(c).ToString());
        }
        
    }
    private void OnCardExpended(Card card)
    {
        CharacterEntityModel owner = card.owner;
        CharacterEntityView view = card.owner.characterEntityView;

        if(owner == null || 
            view == null)
        {
            return;
        }

        // Check 'Recycling' state
        if (StateController.Instance.DoesPlayerHaveState(StateName.Recycling))
        {
            // Apply block gain
            CharacterEntityController.Instance.GainBlock
                (owner, CombatLogic.Instance.CalculateBlockGainedByEffect(2, owner, owner));

            // Notication vfx
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.transform.position, "Recycling!"), QueuePosition.Back, 0, 0.25f);
        }

        // Check 'Adaptation' state
        if (StateController.Instance.DoesPlayerHaveState(StateName.Adaptation) &&
             owner.livingState == LivingState.Alive &&
             CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
        {
            List<CardData> discoverableCards = GetCardsQuery(AllCards);
            CardData chosenCard = discoverableCards[RandomGenerator.NumberBetween(0, discoverableCards.Count - 1)];
            CreateAndAddNewCardToCharacterHand(owner, chosenCard);
        }

        // Corpse Collector Passive
        if (owner.pManager.corpseCollectorStacks > 0)
        {
            // Notification event
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateStatusEffect(view.WorldPosition, "Corpse Collector!"), QueuePosition.Back, 0, 0.5f);

            // Setup
            List<CharacterEntityModel> validEnemies = new List<CharacterEntityModel>();
            CharacterEntityModel targetHit = null;

            // Get valid enemies
            foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                if (enemy.livingState == LivingState.Alive)
                {
                    validEnemies.Add(enemy);
                }
            }

            // Determine target hit
            if (validEnemies.Count == 1)
            {
                targetHit = validEnemies[0];
            }
            else if (validEnemies.Count > 1)
            {
                targetHit = validEnemies[RandomGenerator.NumberBetween(0, validEnemies.Count - 1)];
            }

            if (targetHit != null)
            {
                PassiveController.Instance.ModifyWeakened(targetHit.pManager, owner.pManager.corpseCollectorStacks, owner.pManager, true);
            }
        }
    }
    public void PlayCardFromHand(Card card, CharacterEntityModel target = null)
    {
        Debug.Log("CardController.PlayCardFromHand() called, playing: " + card.cardName);

        // Setup
        CharacterEntityModel owner = card.owner;
        CardViewModel cardVM = card.cardVM;
        int loops = 1;

        // Do effect X times logic
        if (card.xEnergyCost)
        {
            loops = card.owner.energy;
        }

        // Do effect according to loops (e.g. deal damage to a random enemy 3 times type card)
        if(card.cardEffects.Count> 0)
        {
            CardEffect ce = card.cardEffects[0];
            if (ce != null &&
                ce.cardEffectType == CardEffectType.DamageTarget &&
                ce.chooseTargetRandomly)
            {
                loops = ce.damageLoops;
            }
            else if (ce != null &&
                ce.cardEffectType == CardEffectType.ApplyPassiveToTarget &&
                ce.chooseTargetRandomly)
            {
                loops = ce.passiveApplicationLoops;
            }
        }

        // Pay energy cost, remove from hand, etc
        OnCardPlayedStart(card, target);

        // Remove references between card and its view
        DisconnectCardAndCardViewModel(card, cardVM);

        // Trigger all effects on card
        for(int i = 0; i < loops; i++)
        {
            foreach (CardEffect effect in card.cardEffects)
            {
                if (DoesCardEffectMeetWeaponRequirement(effect, owner) &&
                    DoesCardEffectMeetTargetRequirement(effect, target))
                {
                    TriggerEffectFromCard(card, effect, target);
                }
            }
        }       

        // If character moved off node, move back after all card effects resolved
        if (owner.hasMovedOffStartingNode && owner.livingState == LivingState.Alive)
        {
            owner.hasMovedOffStartingNode = false;
            CoroutineData cData = new CoroutineData();
            LevelNode node = owner.levelNode;
            VisualEventManager.Instance.CreateVisualEvent(() => {
                if (owner.QueuedMovements == 0)
                    CharacterEntityController.Instance.MoveEntityToNodeCentre(owner.characterEntityView, node, cData);
                else
                    cData.MarkAsCompleted();
            }, cData, QueuePosition.Back, 0.3f, 0);
        }

        // Brief pause at the end of all effects
        VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);        

        // On end events
        OnCardPlayedFinish(card);
       
    }
    private void TriggerEffectFromCard(Card card, CardEffect cardEffect, CharacterEntityModel target)
    {
        // Handle split card effect
        if (cardEffect.splitTargetEffect)
        {
            // check ally only targetting validity
            if(cardEffect.splitTargetType == TargettingType.Ally &&
                (card.owner.allegiance != target.allegiance || card.owner == target))
            {
                Debug.LogWarning("TriggerEffectFromCard() cancelling card effect: target is not an ally");
                return;
            }

            else if (cardEffect.splitTargetType == TargettingType.AllyOrSelf &&
                card.owner.allegiance != target.allegiance)
            {
                Debug.LogWarning("TriggerEffectFromCard() cancelling card effect: target is not self or ally");
                return;
            }

            else if (cardEffect.splitTargetType == TargettingType.Enemy &&
               card.owner.allegiance == target.allegiance)
            {
                Debug.LogWarning("TriggerEffectFromCard() cancelling card effect: target is not an enemy");
                return;
            }

        }

        // Get random target if marked for a random target
        if (cardEffect.chooseTargetRandomly)
        {
            target = null;

            // Damage random enemy effect
            if (cardEffect.cardEffectType == CardEffectType.DamageTarget)
            {
                List<CharacterEntityModel> viableEnemies = new List<CharacterEntityModel>();
                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(card.owner))
                {
                    if (enemy.livingState != LivingState.Dead)
                    {
                        viableEnemies.Add(enemy);
                    }
                }

                if (viableEnemies.Count == 1)
                {
                    target = viableEnemies[0];
                }
                else if (viableEnemies.Count > 1)
                {
                    target = viableEnemies[RandomGenerator.NumberBetween(0, viableEnemies.Count - 1)];
                }
            }

            // Apply passive to random target effect
            else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToTarget)
            {
                // Get viable targets
                List<CharacterEntityModel> validTargets = new List<CharacterEntityModel>();

                // All characters
                if(cardEffect.validRandomTargetsType == TargettingType.AllCharacters)
                {
                    foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.AllCharacters)
                    {
                        if (enemy.livingState != LivingState.Dead)
                        {
                            validTargets.Add(enemy);
                        }
                    }
                }

                // All enemies
                else if (cardEffect.validRandomTargetsType == TargettingType.Enemy)
                {
                    foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(card.owner))
                    {
                        if (enemy.livingState != LivingState.Dead)
                        {
                            validTargets.Add(enemy);
                        }
                    }
                }

                // All allies
                else if (cardEffect.validRandomTargetsType == TargettingType.Ally)
                {
                    foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllAlliesOfCharacter(card.owner, false))
                    {
                        if (enemy.livingState != LivingState.Dead)
                        {
                            validTargets.Add(enemy);
                        }
                    }
                }

                // All allies and self
                else if (cardEffect.validRandomTargetsType == TargettingType.AllyOrSelf)
                {
                    foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllAlliesOfCharacter(card.owner))
                    {
                        if (enemy.livingState != LivingState.Dead)
                        {
                            validTargets.Add(enemy);
                        }
                    }
                }

                // If only one valid target, choose that
                if (validTargets.Count == 1)
                {
                    target = validTargets[0];
                }

                // if there is more then 1, choose 1 randomly
                else if (validTargets.Count > 1)
                {
                    target = validTargets[RandomGenerator.NumberBetween(0, validTargets.Count - 1)];
                }
            }
        }

        // Stop and return if effect requires a target and that target is dying/dead/no longer valid      
        if(
            (target == null || target.livingState == LivingState.Dead) &&
            (
            cardEffect.cardEffectType == CardEffectType.DamageTarget ||
            cardEffect.cardEffectType == CardEffectType.ApplyPassiveToTarget ||
            cardEffect.cardEffectType == CardEffectType.GainBlockTarget ||
            cardEffect.cardEffectType == CardEffectType.RemoveAllPoisonedFromTarget ||
            cardEffect.cardEffectType == CardEffectType.RemoveAllBurningFromTarget ||
            cardEffect.cardEffectType == CardEffectType.TauntTarget 
            )
            )
        {
            Debug.Log("CardController.TriggerEffectFromCardCoroutine() cancelling: target is no longer valid");
            return;
        }

        // Stop and return if effect requires a an empty summoning node    
        if (cardEffect.cardEffectType == CardEffectType.SummonCharacter &&
             LevelManager.Instance.GetNextAvailableDefenderNode() == null)
        {
            Debug.Log("CardController.TriggerEffectFromCardCoroutine() cancelling: no available summoning positions");
            return;
        }

        Debug.Log("CardController.PlayCardFromHand() called, effect: '" + cardEffect.cardEffectType.ToString() + 
        "' from card: '" + card.cardName);

        CharacterEntityModel owner = card.owner;

        // Stop and return if owner of the card is dead or null  
        if (owner == null || owner.livingState == LivingState.Dead)
        {
            return;
        }

        // Queue starting anims and particles
        foreach (AnimationEventData vEvent in cardEffect.visualEventsOnStart)
        {
            AnimationEventController.Instance.PlayAnimationEvent(vEvent, owner, target);
        }

        // RESOLVE EFFECT LOGIC START!
        // Gain Block Self
        if (cardEffect.cardEffectType == CardEffectType.GainBlockSelf)
        {
            bool didCrit = CombatLogic.Instance.RollForCritical(owner);
            CharacterEntityController.Instance.GainBlock(owner, CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner, owner, null, cardEffect, didCrit));

            // Crit VFX
            if (didCrit)
            {
                // Create critical text effect
                //VisualEventManager.Instance.CreateVisualEvent(() =>
                //VisualEffectManager.Instance.CreateStatusEffect(owner.characterEntityView.WorldPosition, "CRITICAL!", TextLogic.neutralYellow));
            }
        }

        // Summon Character
        else if (cardEffect.cardEffectType == CardEffectType.SummonCharacter)
        {
            // get next available node
            LevelNode node = LevelManager.Instance.GetNextAvailableDefenderNode();

            // Create character
            CharacterEntityModel newSummon = CharacterEntityController.Instance.CreateSummonedPlayerCharacter(cardEffect.characterSummoned, node);

            // Disable activation window until ready
            CharacterEntityView view = newSummon.characterEntityView;
            view.myActivationWindow.gameObject.SetActive(false);
            ActivationManager.Instance.DisablePanelSlotAtIndex(ActivationManager.Instance.ActivationOrder.IndexOf(newSummon));

            // Hide GUI
            CharacterEntityController.Instance.FadeOutCharacterWorldCanvas(view, null, 0);

            // Hide model
            CharacterModelController.Instance.FadeOutCharacterModel(view.ucm, 0);
            CharacterModelController.Instance.FadeOutCharacterShadow(view, 0);
            view.blockMouseOver = true;

            // Enable activation window
            int windowIndex = ActivationManager.Instance.ActivationOrder.IndexOf(newSummon);
            VisualEventManager.Instance.CreateVisualEvent(() =>
            {
                view.myActivationWindow.gameObject.SetActive(true);
                view.myActivationWindow.Show();
                ActivationManager.Instance.EnablePanelSlotAtIndex(windowIndex);
            }, QueuePosition.Back, 0f, 0.1f);

            // Update all window slot positions + activation pointer arrow
            CharacterEntityModel entityActivated = ActivationManager.Instance.EntityActivated;
            VisualEventManager.Instance.CreateVisualEvent(() => ActivationManager.Instance.UpdateWindowPositions());
            VisualEventManager.Instance.CreateVisualEvent(() => ActivationManager.Instance.MoveActivationArrowTowardsEntityWindow(entityActivated), QueuePosition.Back);

            // Fade in model + UI
            VisualEventManager.Instance.CreateVisualEvent(() => CharacterEntityController.Instance.FadeInCharacterWorldCanvas(view, null, cardEffect.uiFadeInSpeed));
            VisualEventManager.Instance.CreateVisualEvent(() =>
            {
                CharacterModelController.Instance.FadeInCharacterModel(view.ucm, cardEffect.modelFadeInSpeed);
                CharacterModelController.Instance.FadeInCharacterShadow(view, 1f, () => view.blockMouseOver = false);
            });

            // Resolve visual events
            foreach (AnimationEventData vEvent in cardEffect.summonedCreatureVisualEvents)
            {
                AnimationEventController.Instance.PlayAnimationEvent(vEvent, newSummon, newSummon);
            }


        }

        // Gain Block Target
        else if (cardEffect.cardEffectType == CardEffectType.GainBlockTarget)
        {
            bool didCrit = CombatLogic.Instance.RollForCritical(owner);
            CharacterEntityController.Instance.GainBlock(target, CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner, target, null, cardEffect, didCrit));
            
            // Crit VFX
            if (didCrit)
            {
                // Create critical text effect
                //VisualEventManager.Instance.CreateVisualEvent(() =>
                //VisualEffectManager.Instance.CreateStatusEffect(target.characterEntityView.WorldPosition, "CRITICAL!", TextLogic.neutralYellow));
            }
        }       

        // Gain Block All Allies
        else if (cardEffect.cardEffectType == CardEffectType.GainBlockAllAllies)
        {
            foreach (CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                bool didCrit = CombatLogic.Instance.RollForCritical(owner);
                CharacterEntityController.Instance.GainBlock(ally, CombatLogic.Instance.CalculateBlockGainedByEffect(cardEffect.blockGainValue, owner, ally, null, cardEffect, didCrit));

                // Crit VFX
                if (didCrit)
                {
                    // Create critical text effect
                    //VisualEventManager.Instance.CreateVisualEvent(() =>
                   // VisualEffectManager.Instance.CreateStatusEffect(ally.characterEntityView.WorldPosition, "CRITICAL!", TextLogic.neutralYellow));
                }
            }            
        }

        // Remove All Block
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllBlock)
        {
            if (cardEffect.removeBlockFrom == RemoveBlockFrom.Self && 
                owner.block > 0)
            {
                // VFX
                int vfxValue = owner.block;
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateLoseBlockEffect(owner.characterEntityView.WorldPosition, vfxValue));

                // Remove Block
                CharacterEntityController.Instance.SetBlock(owner, 0);
            }
            else if (cardEffect.removeBlockFrom == RemoveBlockFrom.Target &&
                target.block > 0)
            {
                // VFX
                int vfxValue = target.block;
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateLoseBlockEffect(target.characterEntityView.WorldPosition, vfxValue));

                // Remove Block
                CharacterEntityController.Instance.SetBlock(target, 0);
            }

        }

        // Deal Damage Target
        else if (cardEffect.cardEffectType == CardEffectType.DamageTarget)
        {
            // Calculate damage
            DamageType damageType = CombatLogic.Instance.GetFinalFinalDamageTypeOfAttack(owner, cardEffect, card);
            int baseDamage;

            // Do normal base damage, or draw base damage from another source?
            if (cardEffect.drawBaseDamageFromCurrentBlock)
            {
                baseDamage = owner.block * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromTargetPoisoned)
            {
                baseDamage = target.pManager.poisonedStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromTargetWeakened)
            {
                baseDamage = target.pManager.weakenedStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromTargetBurning)
            {
                baseDamage = target.pManager.burningStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromMeleeAttacksPlayed)
            {
                baseDamage = (owner.meleeAttacksPlayedThisActivation - 1) * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromOverloadOnSelf)
            {
                baseDamage = owner.pManager.overloadStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromBurningOnSelf)
            {
                baseDamage = owner.pManager.burningStacks * cardEffect.baseDamageMultiplier;
            }
            else
            {
                baseDamage = cardEffect.baseDamageValue;
            }

            // Roll for crit
            bool didCrit = CombatLogic.Instance.RollForCritical(owner);

            // Calculate the end damage value
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, target, damageType, baseDamage, card, cardEffect, didCrit);

            // Start damage sequence
            CombatLogic.Instance.HandleDamage(finalDamageValue, owner, target, card, damageType, false, didCrit);

        }

        // Deal Damage All Enemies
        else if (cardEffect.cardEffectType == CardEffectType.DamageAllEnemies)
        {
            VisualEvent batchedEvent = VisualEventManager.Instance.InsertTimeDelayInQueue(0f);

            foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                // Calculate damage
                DamageType damageType = CombatLogic.Instance.GetFinalFinalDamageTypeOfAttack(owner, cardEffect, card);
                int baseDamage;

                // Do normal base damage, or draw base damage from another source?
                if (cardEffect.drawBaseDamageFromCurrentBlock)
                {
                    baseDamage = owner.block * cardEffect.baseDamageMultiplier;
                }
                else if (cardEffect.drawBaseDamageFromTargetPoisoned)
                {
                    if (cardEffect.useEachIndividualsStackCount)
                    {
                        baseDamage = enemy.pManager.poisonedStacks * cardEffect.baseDamageMultiplier;
                    }
                    else
                    {
                        baseDamage = target.pManager.poisonedStacks * cardEffect.baseDamageMultiplier;
                    }
                   
                }
                else if (cardEffect.drawBaseDamageFromTargetWeakened)
                {
                    if (cardEffect.useEachIndividualsStackCount)
                    {
                        baseDamage = enemy.pManager.weakenedStacks * cardEffect.baseDamageMultiplier;
                    }
                    else
                    {
                        baseDamage = target.pManager.weakenedStacks * cardEffect.baseDamageMultiplier;
                    }

                }
                else if (cardEffect.drawBaseDamageFromTargetBurning)
                {
                    if (cardEffect.useEachIndividualsStackCount)
                    {
                        baseDamage = enemy.pManager.burningStacks * cardEffect.baseDamageMultiplier;
                    }
                    else
                    {
                        baseDamage = target.pManager.burningStacks * cardEffect.baseDamageMultiplier;
                    }
                    
                }
                else if (cardEffect.drawBaseDamageFromMeleeAttacksPlayed)
                {
                    baseDamage = (owner.meleeAttacksPlayedThisActivation - 1) * cardEffect.baseDamageMultiplier;
                }
                else if (cardEffect.drawBaseDamageFromOverloadOnSelf)
                {
                    baseDamage = owner.pManager.overloadStacks * cardEffect.baseDamageMultiplier;
                }
                else if (cardEffect.drawBaseDamageFromBurningOnSelf)
                {
                    baseDamage = owner.pManager.burningStacks * cardEffect.baseDamageMultiplier;
                }
                else
                {
                    baseDamage = cardEffect.baseDamageValue;
                }
                // Roll for crit
                bool didCrit = CombatLogic.Instance.RollForCritical(owner);

                // Calculate the end damage value
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, enemy, damageType, baseDamage, card, cardEffect, didCrit);

                // Start damage sequence
                CombatLogic.Instance.HandleDamage(finalDamageValue, owner, enemy, card, damageType, batchedEvent, false, didCrit);
            }            
        }

        // Deal Damage Self
        else if (cardEffect.cardEffectType == CardEffectType.DamageSelf)
        {
            // Calculate damage
            DamageType damageType = CombatLogic.Instance.GetFinalFinalDamageTypeOfAttack(owner, cardEffect, card);
            int baseDamage;

            // Do normal base damage, or draw base damage from another source?
            if (cardEffect.drawBaseDamageFromCurrentBlock)
            {
                baseDamage = owner.block * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromTargetPoisoned)
            {
                baseDamage = target.pManager.poisonedStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromTargetWeakened)
            {
                baseDamage = target.pManager.weakenedStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromTargetBurning)
            {
                baseDamage = target.pManager.burningStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromMeleeAttacksPlayed)
            {
                baseDamage = (owner.meleeAttacksPlayedThisActivation - 1) * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromOverloadOnSelf)
            {
                baseDamage = owner.pManager.overloadStacks * cardEffect.baseDamageMultiplier;
            }
            else if (cardEffect.drawBaseDamageFromBurningOnSelf)
            {
                baseDamage = owner.pManager.burningStacks * cardEffect.baseDamageMultiplier;
            }
            else
            {
                baseDamage = cardEffect.baseDamageValue;
            }

            // Roll for crit
            bool didCrit = CombatLogic.Instance.RollForCritical(owner);


            // Calculate the end damage value
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, target, damageType, baseDamage, card, cardEffect, didCrit);

            // Start damage sequence
            CombatLogic.Instance.HandleDamage(finalDamageValue, owner, target, card, damageType, false, didCrit);
        }

        // Heal Self
        else if(cardEffect.cardEffectType == CardEffectType.HealSelf)
        {
            // Modify health
            CharacterEntityController.Instance.ModifyHealth(owner, cardEffect.healthRestored);

            // Heal VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateHealEffect(owner.characterEntityView.WorldPosition, cardEffect.healthRestored));

            // Create heal text effect
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateDamageEffect(owner.characterEntityView.WorldPosition, cardEffect.healthRestored, true));

            // Create SFX
            VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff));
        }

        // Heal Target
        else if (cardEffect.cardEffectType == CardEffectType.HealTarget)
        {
            // Modify health
            CharacterEntityController.Instance.ModifyHealth(target, cardEffect.healthRestored);

            // Heal VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateHealEffect(target.characterEntityView.WorldPosition, cardEffect.healthRestored));

            // Create heal text effect
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateDamageEffect(target.characterEntityView.WorldPosition, cardEffect.healthRestored, true));

            // Create SFX
            VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySoundPooled(Sound.Passive_General_Buff));
        }

        // Heal Self And All Allies
        else if (cardEffect.cardEffectType == CardEffectType.HealSelfAndAllies)
        {
            foreach(CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                // Modify health
                CharacterEntityController.Instance.ModifyHealth(target, cardEffect.healthRestored);

                // Heal VFX
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateHealEffect(target.characterEntityView.WorldPosition, cardEffect.healthRestored));

                // Create heal text effect
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateDamageEffect(target.characterEntityView.WorldPosition, cardEffect.healthRestored, true));
            }

            // Create SFX
            VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(Sound.Passive_General_Buff));
        }

        // Lose Health
        else if (cardEffect.cardEffectType == CardEffectType.LoseHP)
        {    
            // Start self damage sequence
            CombatLogic.Instance.HandleDamage(cardEffect.healthLost, owner, owner, card, DamageType.None, true);
        }

        // Gain Energy self
        else if (cardEffect.cardEffectType == CardEffectType.GainEnergySelf)
        {
            if (cardEffect.drawStacksFromOverload)
            {
                CharacterEntityController.Instance.ModifyEnergy(owner, owner.pManager.overloadStacks, true);
            }
            else
            {
                CharacterEntityController.Instance.ModifyEnergy(owner, cardEffect.energyGained, true);
            }
        
        }

        // Gain Energy target
        else if (cardEffect.cardEffectType == CardEffectType.GainEnergyTarget)
        {
            if (cardEffect.drawStacksFromOverload)
            {
                CharacterEntityController.Instance.ModifyEnergy(target, owner.pManager.overloadStacks, true);
            }
            else
            {
                CharacterEntityController.Instance.ModifyEnergy(target, cardEffect.energyGained, true);
            }

        }

        // Draw Cards
        else if (cardEffect.cardEffectType == CardEffectType.DrawCards)
        {
            Card cardDrawn = null;
            int totalDraws = cardEffect.cardsDrawn;

            // Set cards drawn equal to overload?
            if (cardEffect.drawStacksFromOverload)
            {
                totalDraws = owner.pManager.overloadStacks;
            }

            // Draw cards
            for (int draws = 0; draws < totalDraws; draws++)
            {
                // reset cache card variable
                cardDrawn = null;

                // If not specific card criteria is chosen, just draw the first card from draw pile
                if (cardEffect.drawSpecificType == false)
                {
                    cardDrawn = DrawACardFromDrawPile(owner);
                }

                // Draw specific cards by criteria
                else
                {
                    // Draw Melee attack or ranged attack
                    if (cardEffect.specificTypeDrawn == SpecificTypeDrawn.MeleeAttack ||
                        cardEffect.specificTypeDrawn == SpecificTypeDrawn.RangedAttack)
                    {
                        CardType cardType = CardType.None;

                        if(cardEffect.specificTypeDrawn == SpecificTypeDrawn.MeleeAttack)
                        {
                            cardType = CardType.MeleeAttack;
                        }
                        else if (cardEffect.specificTypeDrawn == SpecificTypeDrawn.RangedAttack)
                        {
                            cardType = CardType.RangedAttack;
                        }

                        // Get the first melee attack from draw pile
                        foreach(Card cardInDrawPile in owner.drawPile)
                        {
                            if (cardInDrawPile.cardType == cardType)
                            {
                                // Found one, cache it and break
                                cardDrawn = cardInDrawPile;
                                break;
                            }
                        }                        
                    }

                    else if(cardEffect.specificTypeDrawn == SpecificTypeDrawn.ZeroEnergyCost)
                    {
                        // Get the first 0 cost card
                        foreach (Card cardInDrawPile in owner.drawPile)
                        {
                            if (GetCardEnergyCost(cardInDrawPile) == 0)
                            {
                                // Found one, cache it and break
                                cardDrawn = cardInDrawPile;
                                break;
                            }
                        }
                    }

                    // Draw the card, if was found
                    if (cardDrawn != null)
                    {
                        DrawACardFromDrawPile(owner, cardDrawn);                       
                    }

                }

                // Apply additional effects to card
                if (cardDrawn != null)
                {
                    if (cardEffect.extraDrawEffect == ExtraDrawEffect.ReduceEnergyCostThisCombat)
                    {
                        ReduceCardEnergyCostThisCombat(cardDrawn, cardEffect.cardEnergyReduction);
                    }
                    else if (cardEffect.extraDrawEffect == ExtraDrawEffect.SetEnergyCostToZeroThisCombat)
                    {
                        ReduceCardEnergyCostThisCombat(cardDrawn, cardDrawn.cardBaseEnergyCost);
                    }
                }


            }           
        }

        // Discover cards
        else if (cardEffect.cardEffectType == CardEffectType.DiscoverCards)
        {
            StartCoroutine(StartNewDiscoveryEvent(cardEffect, owner));
        }

        // Choose card in hand
        else if (cardEffect.cardEffectType == CardEffectType.ChooseCardInHand)
        {
            StartNewChooseCardInHandEvent(cardEffect, owner);
        }

        // Modify all cards in hand
        else if (cardEffect.cardEffectType == CardEffectType.ModifyAllCardsInHand)
        {
            // need to iterate over a temp list, not the actual cards in hand list,
            // otherwise expending cards in hand while iterating over them 
            // will cause an invalid operation exception.
           
            // Get all cards in hand
            List<Card> cardsInHand = new List<Card>();
            cardsInHand.AddRange(owner.hand);
            int totalCards = cardsInHand.Count;

            // Or, get specific cards from hand only
            if(cardEffect.modifyCardsInHandEffects[0].onlySpecificCards &&
                cardEffect.modifyCardsInHandEffects[0].specificCardType == SpecificCardType.Burn)
            {
                Debug.LogWarning("Specific card type detected");
                cardsInHand.Clear();
                totalCards = 0;
                foreach(Card ch in owner.hand)
                {
                    if (ch.cardName == "Burn")
                    {
                        totalCards++;
                        cardsInHand.Add(ch);
                    }
                }

                Debug.LogWarning("Found " + totalCards.ToString() +" burn cards");
            }
            
            // Normal mod events
            foreach(ModifyAllCardsInHandEffect modEffect in cardEffect.modifyCardsInHandEffects)
            { 
                foreach (Card c in cardsInHand)
                {
                    if(owner.livingState == LivingState.Alive &&
                        CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
                    {
                        // Reduce cost
                        if (modEffect.modifyEffect == ModifyAllCardsInHandEffectType.ReduceEnergyCost)
                        {
                            ReduceCardEnergyCostThisCombat(c, modEffect.energyReduction);
                        }

                        // Set new cost
                        else if (modEffect.modifyEffect == ModifyAllCardsInHandEffectType.SetEnergyCost)
                        {
                            SetCardEnergyCostThisCombat(c, modEffect.newEnergyCost);
                        }

                        // Expend
                        else if (modEffect.modifyEffect == ModifyAllCardsInHandEffectType.ExpendIt)
                        {
                            ExpendCard(c, false, QueuePosition.Front, 0, 0);
                        }
                    }                  
                }

                // Add new card from library effect
                if (modEffect.modifyEffect == ModifyAllCardsInHandEffectType.AddNewCardFromLibraryToHand)
                {
                    // Get all possible card data
                    List<CardData> discoverableCards = new List<CardData>();
                    discoverableCards = GetCardsQuery(AllCards, modEffect.talentSchoolFilter, modEffect.rarityFilter, false, UpgradeFilter.OnlyNonUpgraded);

                    for(int i = 0; i < totalCards; i++)
                    {
                        if (discoverableCards.Count > 0 &&
                            owner.livingState == LivingState.Alive &&
                            CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
                        {
                            // Randomize card chosen
                            //discoverableCards.Shuffle();
                            int randomIndex = RandomGenerator.NumberBetween(0, discoverableCards.Count - 1);

                            // Add card to hand
                            CreateAndAddNewCardToCharacterHand(owner, discoverableCards[randomIndex]);
                        }
                    }                  
                }

                // Add specific card to hand
                else if (modEffect.modifyEffect == ModifyAllCardsInHandEffectType.AddSpecificCardToHand)
                {
                    for (int i = 0; i < totalCards; i++)
                    {
                        if (owner.livingState == LivingState.Alive &&
                            CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
                        {
                            // Add card to hand
                            CreateAndAddNewCardToCharacterHand(owner, modEffect.cardAdded);
                        }
                    }
                }

                // Add random blessing to hand
                else if (modEffect.modifyEffect == ModifyAllCardsInHandEffectType.AddRandomBlessingToHand)
                {
                    for (int i = 0; i < totalCards; i++)
                    {
                        if (owner.livingState == LivingState.Alive &&
                            CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
                        {
                            // Add card to hand
                            CreateAndAddNewRandomBlessingsToCharacterHand(owner, 1, modEffect.upgradeFilter);
                        }
                    }
                }              
            }

            // Do damage events seperately
            foreach (ModifyAllCardsInHandEffect modEffect in cardEffect.modifyCardsInHandEffects)
            {               

                if (modEffect.modifyEffect == ModifyAllCardsInHandEffectType.DamageAllEnemies)
                {
                    for(int i = 0; i < totalCards; i++)
                    {
                        // Cancel if attacker dies at any point during the looping
                        if(owner.health <= 0 || owner.livingState != LivingState.Alive)
                        {
                            break;
                        }

                        // Queue starting anims and visual events
                        foreach (AnimationEventData vEvent in cardEffect.visualEventsOnDamageLoopStart)
                        {
                            AnimationEventController.Instance.PlayAnimationEvent(vEvent, owner, target);
                        }

                        VisualEvent batchedEvent = VisualEventManager.Instance.InsertTimeDelayInQueue(0f);

                        foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                        {
                            // Cancel if attacker dies at any point during the looping
                            if (owner.health <= 0 || owner.livingState != LivingState.Alive)
                            {
                                break;
                            }

                            //Roll for crit
                            bool didCrit = CombatLogic.Instance.RollForCritical(owner);

                            // Calculate damage
                            DamageType damageType = modEffect.damageType;
                            int baseDamage = modEffect.baseDamage;

                            // Calculate the end damage value
                            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(owner, enemy, damageType, baseDamage, card, cardEffect, didCrit);

                            // Start damage sequence
                            CombatLogic.Instance.HandleDamage(finalDamageValue, owner, enemy, card, damageType, batchedEvent, false, didCrit);
                        }


                        // Stop final anim events if attacker died
                        if (owner.health <= 0 || owner.livingState != LivingState.Alive)
                        {
                            break;
                        }

                        // Queue finishing anims and visual events
                        foreach (AnimationEventData vEvent in cardEffect.visualEventsOnDamageLoopFinish)
                        {
                            AnimationEventController.Instance.PlayAnimationEvent(vEvent, owner, target);
                        }
                    }
                }               
            }            

        }

        // Apply passive to self
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToSelf)
        {
            // draw from overload
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }

            // draw from weakened on all enemies
            else if (cardEffect.drawStacksFromWeakenedOnEnemies)
            {
                foreach(CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                {
                    if(enemy.pManager.weakenedStacks > 0)
                        stacks += 1;
                }                
            }

            string passiveName = TextLogic.SplitByCapitals(cardEffect.passivePairing.passiveData.ToString());
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(owner.pManager, passiveName, stacks, true, 0.5f, owner.pManager);
        }

        // Apply passive to target
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToTarget)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }

            // draw from weakened on all enemies
            else if (cardEffect.drawStacksFromWeakenedOnEnemies)
            {
                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                {
                    if (enemy.pManager.weakenedStacks > 0)
                        stacks += 1;
                }
            }
            string passiveName = TextLogic.SplitByCapitals(cardEffect.passivePairing.passiveData.ToString());
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(target.pManager, passiveName, stacks, true, 0.5f, owner.pManager);
        }

        // Apply passive to all allies
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToAllAllies)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }

            // draw from weakened on all enemies
            else if (cardEffect.drawStacksFromWeakenedOnEnemies)
            {
                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                {
                    if (enemy.pManager.weakenedStacks > 0)
                        stacks += 1;
                }
            }

            foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                string passiveName = TextLogic.SplitByCapitals(cardEffect.passivePairing.passiveData.ToString());
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(enemy.pManager, passiveName, stacks, true, 0f, owner.pManager);
            }

            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
        }

        // Apply passive to all enemies
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToAllEnemies)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }
            // draw from weakened on all enemies
            else if (cardEffect.drawStacksFromWeakenedOnEnemies)
            {
                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                {
                    if (enemy.pManager.weakenedStacks > 0)
                        stacks += 1;
                }
            }

            foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                string passiveName = TextLogic.SplitByCapitals(cardEffect.passivePairing.passiveData.ToString());
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(enemy.pManager, passiveName, stacks, true, 0f, owner.pManager);                
            }

            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
        }

        // Apply passive to all allies
        else if (cardEffect.cardEffectType == CardEffectType.ApplyPassiveToAllEnemies)
        {
            int stacks = cardEffect.passivePairing.passiveStacks;
            if (cardEffect.drawStacksFromOverload)
            {
                stacks = owner.pManager.overloadStacks;
            }

            // draw from weakened on all enemies
            else if (cardEffect.drawStacksFromWeakenedOnEnemies)
            {
                foreach (CharacterEntityModel enemy in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
                {
                    if (enemy.pManager.weakenedStacks > 0)
                        stacks += 1;
                }
            }

            foreach (CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                string passiveName = TextLogic.SplitByCapitals(cardEffect.passivePairing.passiveData.ToString());
                PassiveController.Instance.ModifyPassiveOnCharacterEntity(ally.pManager, passiveName, stacks, true, 0f, owner.pManager);
                VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
            }
        }

        // Remove overload from self
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllOverloadFromSelf)
        {
            PassiveController.Instance.ModifyOverload(owner.pManager, -owner.pManager.overloadStacks, true);
        }

        // Remove burning from self
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllBurningFromSelf)
        {
            PassiveController.Instance.ModifyBurning(owner.pManager, -owner.pManager.burningStacks, true);
        }

        // Remove poisoned from self
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllPoisonedFromSelf)
        {
            PassiveController.Instance.ModifyPoisoned(null, owner.pManager, -owner.pManager.poisonedStacks, true);
        }

        // Remove poisoned from target
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllPoisonedFromTarget)
        {
            PassiveController.Instance.ModifyPoisoned(null, target.pManager, -target.pManager.poisonedStacks, true);
        }

        // Remove burning from target
        else if (cardEffect.cardEffectType == CardEffectType.RemoveAllBurningFromTarget)
        {
            PassiveController.Instance.ModifyBurning(target.pManager, -target.pManager.burningStacks, true);
        }

        // Remove Weakened from self and allies
        else if (cardEffect.cardEffectType == CardEffectType.RemoveWeakenedFromSelfAndAllies)
        {
            foreach (CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                PassiveController.Instance.ModifyWeakened(ally.pManager, -ally.pManager.weakenedStacks, null, false);
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateStatusEffect(ally.characterEntityView.WorldPosition, "Weakened Removed!"));
            }
        }

        // Remove Vulnerable from self and allies
        else if (cardEffect.cardEffectType == CardEffectType.RemoveVulnerableFromSelfAndAllies)
        {
            foreach (CharacterEntityModel ally in CharacterEntityController.Instance.GetAllAlliesOfCharacter(owner))
            {
                PassiveController.Instance.ModifyVulnerable(ally.pManager, -ally.pManager.vulnerableStacks, false);
                VisualEventManager.Instance.CreateVisualEvent(()=> 
                    VisualEffectManager.Instance.CreateStatusEffect(ally.characterEntityView.WorldPosition, "Vulnerable Removed!"));
            }
        }

        // Taunt Target
        else if (cardEffect.cardEffectType == CardEffectType.TauntTarget)
        {
            CharacterEntityController.Instance.HandleTaunt(owner, target);
        }

        // Taunt all enemies
        else if (cardEffect.cardEffectType == CardEffectType.TauntAllEnemies)
        {
            // get all enemies
            foreach(CharacterEntityModel character in CharacterEntityController.Instance.GetAllEnemiesOfCharacter(owner))
            {
                // taunt each enemy
                CharacterEntityController.Instance.HandleTaunt(owner, character);
            }            
        }

        // Add new non deck card to hand
        else if(cardEffect.cardEffectType == CardEffectType.AddCardsToHand)
        {
            for(int i = 0; i < cardEffect.copiesAdded; i++)
            {
                CreateAndAddNewCardToCharacterHand(owner, cardEffect.cardAdded);
            }
        }

        // Add new non deck card to draw pile
        else if (cardEffect.cardEffectType == CardEffectType.AddCardsToDrawPile)
        {
            List<Card> cardsAdded = new List<Card>();
            for (int loops = 0; loops < cardEffect.copiesAdded; loops++)
            {
                Card c = CreateAndAddNewCardToCharacterDrawPile(owner, cardEffect.cardAdded);
                cardsAdded.Add(c);
            }

            StartNewShuffleCardsScreenVisualEvent(cardsAdded);
        }

        // Add random blessings to hand
        else if (cardEffect.cardEffectType == CardEffectType.AddRandomBlessingsToHand)
        {
            for (int i = 0; i < cardEffect.blessingsGained; i++)
            {               
                List<CardData> allBlessings = QueryByBlessing(AllCards, true);
                List<CardData> filteredBlessings = new List<CardData>();

                if(cardEffect.upgradeFilter == UpgradeFilter.OnlyUpgraded)
                {
                    filteredBlessings = QueryByUpgraded(allBlessings);
                }
                else if (cardEffect.upgradeFilter == UpgradeFilter.OnlyNonUpgraded)
                {
                    filteredBlessings = QueryByNonUpgraded(allBlessings);
                }

                CardData randomBlessing = filteredBlessings[RandomGenerator.NumberBetween(0, filteredBlessings.Count - 1)];
                CreateAndAddNewCardToCharacterHand(owner, randomBlessing);
            }
        }

        // Double targets poisoned
        else if(cardEffect.cardEffectType == CardEffectType.DoubleTargetsPoisoned)
        {
            if(target.pManager.poisonedStacks > 0)
            {
                PassiveController.Instance.ModifyPoisoned(owner.pManager, target.pManager, target.pManager.poisonedStacks, true);
            }
            
        }

        // Modify permanent deck card
        else if (cardEffect.cardEffectType == CardEffectType.ModifyMyPermanentDeckCard)
        {
            if (card.myCharacterDeckCard == null)
            {
                Debug.LogWarning("TriggerEffectFromCard() is trying to modify the cards permanent deck card, but it is null, cancelling effect...");
            }

            // Increae damage permanently
            else if (cardEffect.modifyDeckCardEffect == ModifyDeckCardEffectType.IncreaseBaseDamage)
            {
                // Find the damage effect on the card
                CardEffect ce = null;

                foreach(CardEffect cce in card.myCharacterDeckCard.cardEffects)
                {
                    if(cce.cardEffectType == CardEffectType.DamageAllEnemies ||
                        cce.cardEffectType == CardEffectType.DamageTarget)
                    {
                        // Found it, now cache it
                        ce = cce;
                        break;
                    }
                }

                // Did we find the matching effect?
                if(ce != null)
                {
                    // We did, increase its damage permanently
                    ce.baseDamageValue += cardEffect.permanentDamageGain;
                }
            }

            // Increase block gain permanently
            else if (cardEffect.modifyDeckCardEffect == ModifyDeckCardEffectType.IncreaseBaseBlockGain)
            {
                // Find the damage effect on the card
                CardEffect ce = null;

                foreach (CardEffect cce in card.myCharacterDeckCard.cardEffects)
                {
                    if (cce.cardEffectType == CardEffectType.GainBlockAllAllies ||
                        cce.cardEffectType == CardEffectType.GainBlockSelf ||
                        cce.cardEffectType == CardEffectType.GainBlockTarget)
                    {
                        // Found it, now cache it
                        ce = cce;
                        break;
                    }
                }

                // Did we find the matching effect?
                if (ce != null)
                {
                    // We did, increase its damage permanently
                    ce.blockGainValue += cardEffect.permanentBlockGain;
                }
            }
        }


        // CONCLUDING VISUAL EVENTS!
            if (CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive &&
            owner.livingState == LivingState.Alive)
        {
            // cancel if the target was killed
            if(target != null && target.livingState == LivingState.Dead)
            {
                return;
            }

            foreach (AnimationEventData vEvent in cardEffect.visualEventsOnFinish)
            {
                AnimationEventController.Instance.PlayAnimationEvent(vEvent, owner, target);
            }
        }
              
    }
    #endregion

    // Hand, Draw Pile + Discard Pile Functions
    #region
    private List<Card> GetAllCharacterCardsInHandDrawAndDiscard(CharacterEntityModel model)
    {
        Debug.Log("CardController.GetAllCharacterCardsInHandDrawAndDiscard() called for character: " + model.myName);

        List<Card> listReturned = new List<Card>();
        listReturned.AddRange(model.hand);
        listReturned.AddRange(model.drawPile);
        listReturned.AddRange(model.discardPile);

        return listReturned;
    }

    private void MoveAllCardsFromDiscardPileToDrawPile(CharacterEntityModel defender)
    {
        Debug.Log("CardController.MoveAllCardsFromDiscardPileToDrawPile() called for character: " + defender.myName);

        // Create temp list for safe iteration
        Card[] tempDiscardList = defender.discardPile.ToArray();
        //tempDiscardList.AddRange(defender.discardPile);

        // Remove each card from discard pile, then add to draw pile
        foreach (Card card in tempDiscardList)
        {
            RemoveCardFromDiscardPile(defender,card);
            AddCardToDrawPile(defender,card);
        }

        // Re-shuffle the draw pile
        defender.drawPile.Shuffle();

        // Play visual event
        VisualEventManager.Instance.CreateVisualEvent(()=> ShuffleDiscardPileIntoDrawPileVisualEvent());
    }

    private void MoveCardFromDiscardPileToHand(Card card)
    {
        // TO DO: we shouldnt just shuffle the card into the draw pile then draw it...
        // find a better way...
        RemoveCardFromDiscardPile(card.owner, card);
        AddCardToDrawPile(card.owner, card);
        DrawACardFromDrawPile(card.owner, card.owner.drawPile.IndexOf(card));
    }
    private void MoveCardFromExpendPileToHand(Card card)
    {
        // TO DO: we shouldnt just shuffle the card into the draw pile then draw it...
        // find a better way...
        RemoveCardFromExpendPile(card.owner, card);
        AddCardToDrawPile(card.owner, card);
        DrawACardFromDrawPile(card.owner, card.owner.drawPile.IndexOf(card));
    }
    private void AddCardToDrawPile(CharacterEntityModel defender, Card card)
    {
        defender.drawPile.Add(card);
        string drawPileCount = defender.drawPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDrawPileCountText(defender.characterEntityView, drawPileCount));
    }
    private void RemoveCardFromDrawPile(CharacterEntityModel defender, Card card)
    {
        defender.drawPile.Remove(card);
        string drawPileCount = defender.drawPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDrawPileCountText(defender.characterEntityView, drawPileCount));
    }
    private void AddCardToDiscardPile(CharacterEntityModel defender, Card card)
    {
        defender.discardPile.Add(card);
        string discardPileCount = defender.discardPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDiscardPileCountText(defender.characterEntityView, discardPileCount));
    }
    private void RemoveCardFromDiscardPile(CharacterEntityModel defender, Card card)
    {
        defender.discardPile.Remove(card);
        string discardPileCount = defender.discardPile.Count.ToString();
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateDiscardPileCountText(defender.characterEntityView, discardPileCount));
    }
    private void AddCardToHand(CharacterEntityModel defender, Card card)
    {
        defender.hand.Add(card);

        // check while holding
        /*
        for(int i = 0; i < defender.hand.Count; i++)
        {
            if(defender.hand[i].cardEventListeners[0].cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard)
            {
                foreach(Card c in defender.hand)
                {
                    if (defender.hand[i].cardEventListeners[0].certainCardNames.Contains(c.cardName))
                    {
                        SetCardViewModelEnergyText(defender.hand[i], defender.hand[i].cardVM, GetCardEnergyCost(defender.hand[i]).ToString());
                    }
                }
            }
        }
        */

        // update all card energy costs texts for event listener cards
        for(int i = 0; i < defender.hand.Count; i++)
        {
            if (defender.hand[i].cardEventListeners.Count > 0 &&
                defender.hand[i].cardEventListeners[0].cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard &&
                defender.hand[i].cardVM != null)
            {
                SetCardViewModelEnergyText(defender.hand[i], defender.hand[i].cardVM, GetCardEnergyCost(defender.hand[i]).ToString());
            }
        }
    }
    private void RemoveCardFromHand(CharacterEntityModel defender, Card card)
    {
        defender.hand.Remove(card);

        // check while holding 
        /*
        for (int i = 0; i < defender.hand.Count; i++)
        {
            if (defender.hand[i].cardEventListeners[0].cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard)
            {
                foreach (Card c in defender.hand)
                {
                    if (defender.hand[i].cardEventListeners[0].certainCardNames.Contains(c.cardName))
                    {
                        SetCardViewModelEnergyText(defender.hand[i], defender.hand[i].cardVM, GetCardEnergyCost(defender.hand[i]).ToString());
                    }
                }
            }
        }
        */

        // update all card energy costs texts for event listener cards
        for (int i = 0; i < defender.hand.Count; i++)
        {
            if (defender.hand[i].cardEventListeners.Count > 0 &&
                defender.hand[i].cardEventListeners[0].cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard &&
                defender.hand[i].cardVM != null)
            {
                SetCardViewModelEnergyText(defender.hand[i], defender.hand[i].cardVM, GetCardEnergyCost(defender.hand[i]).ToString());
            }
        }
    }
    private void AddCardToExpendPile(CharacterEntityModel defender, Card card)
    {
        defender.expendPile.Add(card);
    }
    private void RemoveCardFromExpendPile(CharacterEntityModel defender, Card card)
    {
        defender.expendPile.Remove(card);
    }
    #endregion

    // Card Event Listener Logic + Passive Listeners
    #region
    private void RunCardEventListenerFunction(Card card, CardEventListener e)
    {
        Debug.Log("CardController.RunCardEventListenerFunction() called...");

        // TO DO: Create a small visual event dotween sequence
        // on card VM's when they trigger on listener event,
        // something like scales up and then back down quickly

        // Cancel if card owner doesnt meet the weapon requirement of the event effect.
        if(card.owner == null ||
            (card.owner != null && DoesCardEventListenerMeetWeaponRequirement(e, card.owner) == false))
        {
            return;
        }

        // Reduce energy cost of card
        if (e.cardEventListenerFunction == CardEventListenerFunction.ReduceCardEnergyCost)
        {
            // Reduce cost this combat
            ReduceCardEnergyCostThisCombat(card, e.energyReductionAmount);
        }

        // Reduce energy cost of card this activation only
        else if (e.cardEventListenerFunction == CardEventListenerFunction.ReduceCardEnergyCostThisActivation)
        {
            // Reduce cost this combat
            ReduceCardEnergyCostThisActivation(card, e.energyReductionAmount);
        }

        // Apply passive
        else if (e.cardEventListenerFunction == CardEventListenerFunction.ApplyPassiveToSelf)
        {
            string passiveName = TextLogic.SplitByCapitals(e.passivePairing.passiveData.ToString());
            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(card.cardVM));
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(card.owner.pManager, passiveName, e.passivePairing.passiveStacks, true, 0.5f, card.owner.pManager);
        }

        // Lose Health
        else if (e.cardEventListenerFunction == CardEventListenerFunction.LoseHealth)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(card.cardVM));

            // Start self damage sequence
            VisualEventManager.Instance.CreateVisualEvent(() => VisualEffectManager.Instance.CreateBloodExplosion(card.owner.characterEntityView.WorldPosition));
            CombatLogic.Instance.HandleDamage(e.healthLost, card.owner, card.owner, card, DamageType.None, true);
            VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);
        }

        // Draw this
        else if (e.cardEventListenerFunction == CardEventListenerFunction.DrawThis)
        {
            // Reduce cost this combat
            if (card.owner.drawPile.Contains(card))
            {
                DrawACardFromDrawPile(card.owner, card);
            }
        }

        // Modify max health
        else if (e.cardEventListenerFunction == CardEventListenerFunction.ModifyMaxHealth)
        {
            CharacterEntityController.Instance.ModifyMaxHealth(card.owner, e.maxHealthGained);
        }

        // Modify Energy
        else if (e.cardEventListenerFunction == CardEventListenerFunction.ModifyEnergy)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(card.cardVM));
            CharacterEntityController.Instance.ModifyEnergy(card.owner, e.energyGainedOrLost);
        }
    }
    private void HandleOnMeleeAttackCardPlayedListeners(CharacterEntityModel character)
    {
        Debug.Log("CardController.HandleOnMeleeAttackCardPlayedListeners() called...");

        foreach (Card card in GetAllCharacterCardsInHandDrawAndDiscard(character))
        {
            foreach (CardEventListener e in card.cardEventListeners)
            {
                if (e.cardEventListenerType == CardEventListenerType.OnMeleeAttackCardPlayed)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    private void HandleOnFireBallCardPlayedListeners(CharacterEntityModel character)
    {
        Debug.Log("CardController.HandleOnFireBallCardPlayedListeners() called...");

        foreach (Card card in GetAllCharacterCardsInHandDrawAndDiscard(character))
        {
            foreach (CardEventListener e in card.cardEventListeners)
            {
                if (e.cardEventListenerType == CardEventListenerType.OnFireBallCardPlayed)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    private void HandleOnBlessingCardPlayedListeners(CharacterEntityModel character)
    {
        Debug.Log("CardController.HandleOnBlessingCardPlayedListeners() called...");

        foreach (Card card in GetAllCharacterCardsInHandDrawAndDiscard(character))
        {
            foreach (CardEventListener e in card.cardEventListeners)
            {
                if (e.cardEventListenerType == CardEventListenerType.OnBlessingCardPlayed)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    private void HandleOnArcaneBoltCardPlayedListeners(CharacterEntityModel character)
    {
        Debug.Log("CardController.HandleOnArcaneBoltCardPlayedListeners() called...");

        foreach (Card card in GetAllCharacterCardsInHandDrawAndDiscard(character))
        {
            foreach (CardEventListener e in card.cardEventListeners)
            {
                if (e.cardEventListenerType == CardEventListenerType.OnArcaneBoltCardPlayed)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    public void HandleOnCharacterDamagedCardListeners(CharacterEntityModel character)
    {
        Debug.Log("CardController.HandleOnCharacterDamagedCardListeners() called...");

        foreach(Card card in GetAllCharacterCardsInHandDrawAndDiscard(character))
        {
            foreach(CardEventListener e in card.cardEventListeners)
            {
                if(e.cardEventListenerType == CardEventListenerType.OnLoseHealth)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    public void HandleOnWeakenedAppliedCardListeners(CharacterEntityModel character)
    {
        Debug.Log("CardController.HandleOnWeakenedAppliedCardListeners() called...");

        foreach (Card card in GetAllCharacterCardsInHandDrawAndDiscard(character))
        {
            foreach (CardEventListener e in card.cardEventListeners)
            {
                if (e.cardEventListenerType == CardEventListenerType.OnWeakenedApplied)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    private void HandleOnThisCardDrawnListenerEvents(Card card)
    {
        Debug.Log("CardController.HandleOnThisCardDrawnListenerEvents() called...");

        foreach (CardEventListener cel in card.cardEventListeners)
        {
            if (cel.cardEventListenerType == CardEventListenerType.OnThisCardDrawn)
            {
                RunCardEventListenerFunction(card, cel);
            }
        }
    }
    private void HandleOnThisCardPlayedListenerEvents(Card card)
    {
        Debug.Log("CardController.HandleOnThisCardDrawnListenerEvents() called...");

        foreach (CardEventListener cel in card.cardEventListeners)
        {
            if (cel.cardEventListenerType == CardEventListenerType.OnThisCardPlayed)
            {
                RunCardEventListenerFunction(card, cel);
            }
        }
    }
    public void HandleOnCharacterActivationEndCardListeners(CharacterEntityModel character)
    {
        foreach (Card card in character.hand)
        {
            foreach (CardEventListener e in card.cardEventListeners)
            {
                if (e.cardEventListenerType == CardEventListenerType.OnActivationEnd)
                {
                    RunCardEventListenerFunction(card, e);
                }
            }
        }
    }
    public void HandleOnTargetKilledEventListeners(Card card)
    {
        foreach (CardEventListener cel in card.cardEventListeners)
        {
            if(cel.cardEventListenerType == CardEventListenerType.OnTargetKilled)
            {
                RunCardEventListenerFunction(card, cel);
            }
        }
    }
    public void OnMeleeAttackReductionModified(CharacterEntityModel model)
    {
        foreach (Card card in model.hand)
        {
            if (card.cardType == CardType.MeleeAttack)
            {
                // Update card vm energy text, if not null
                CardViewModel cvm = card.cardVM;
                int newCostTextValue = GetCardEnergyCost(card);
                if (cvm)
                {
                    // Update energy cost text
                    VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));

                    // Update glow
                    AutoUpdateCardGlowOutline(card);

                    // only play breath if cost of card is reduced, not increased
                    if (model.pManager.plantedFeetStacks > 0)
                    {
                        VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
                    }
                }                
            }
        }
    }
    public void OnSourceModified(CharacterEntityModel model, bool doBreathAnim = true)
    {
        foreach (Card card in model.hand)
        {
            if (card.sourceSpell == true)
            {
                // Update card vm energy text, if not null
                CardViewModel cvm = card.cardVM;
                int newCostTextValue = GetCardEnergyCost(card);
                if (cvm)
                {
                    // Update energy cost text
                    //VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));
                    SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString());

                    // Update glow
                    AutoUpdateCardGlowOutline(card);

                    // only play breath if cost of card is reduced, not increased

                    if (doBreathAnim)
                    {
                        //VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
                        PlayCardBreathAnimationVisualEvent(cvm);

                    }
                }
            }
        }
    }
    public void OnDarkBargainModified(CharacterEntityModel model)
    {
        foreach (Card card in model.hand)
        {
            // Update card vm energy text, if not null
            CardViewModel cvm = card.cardVM;
            int newCostTextValue = GetCardEnergyCost(card);
            if (cvm)
            {
                // Update energy cost text
                VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));

                // Update glow
                AutoUpdateCardGlowOutline(card);

                // only play breath if cost of card is reduced, not increased
                if (model.pManager.darkBargainStacks > 0)
                {
                    VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
                }
            }
        }
    }
    public void OnPistoleroModified(CharacterEntityModel model)
    {
        // Update card glows
        AutoUpdateCardsInHandGlowOutlines(model);

        foreach (Card card in model.hand)
        {
            // Update card vm energy text, if not null
            if(card.cardType == CardType.RangedAttack)
            {
                CardViewModel cvm = card.cardVM;
                int newCostTextValue = GetCardEnergyCost(card);
                if (cvm)
                {
                    // Update energy cost text
                    VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));

                    // only play breath if cost of card is reduced, not increased
                    if (model.pManager.pistoleroStacks > 0)
                    {
                        VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
                    }
                }
            }
            
        }
    }
    public void OnRangedAttackReductionModified(CharacterEntityModel model)
    {
        foreach (Card card in model.hand)
        {
            if (card.cardType == CardType.RangedAttack)
            {
                // Update card vm energy text, if not null
                CardViewModel cvm = card.cardVM;
                int newCostTextValue = GetCardEnergyCost(card);
                if (cvm)
                {
                    // Update energy cost text
                    VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));

                    // Update glow
                    AutoUpdateCardGlowOutline(card);

                    // only play breath if cost of card is reduced, not increased
                    if (model.pManager.takenAimStacks > 0)
                    {
                        VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
                    }
                }
            }
        }
    }
    #endregion

    // Misc + Calculators + Events
    #region
    public int GetCardEnergyCost(Card card, bool includeSourceReduction = true)
    {
        Debug.Log("CardController.GetCardEnergyCost() called for card: " + card.cardName);

        // Spend X energy logic
        if (card.xEnergyCost)
        {
            if(card.owner != null)
            {
                return card.owner.energy;
            }
        }

        // Souless state override
        if (card.cardType == CardType.Power &&
            StateController.Instance.DoesPlayerHaveState(StateName.Soulless))
            return 0;

        // Pistolero override
        if(card.cardType == CardType.RangedAttack &&
            card.owner != null &&
            card.owner.pManager != null &&
            card.owner.pManager.pistoleroStacks > 0)
        {
            return 0;
        }

        // Dark Bargain override
        if (card.owner != null &&
            card.owner.pManager != null &&
            card.owner.pManager.darkBargainStacks > 0)
        {
            return 0;
        }

        // Check while holding listeners
        if(card.cardEventListeners.Count > 0 && 
            card.owner != null)
        {             
            foreach (CardEventListener cel in card.cardEventListeners)
            {
                // does the card have the required listener?
                if(cel.cardEventListenerType == CardEventListenerType.WhileHoldingCertainCard && cel.cardCostsZero == true)
                {
                    // it does, search and see if character is holding the required card for reducing energy cost
                    foreach(Card c in card.owner.hand)
                    {
                        if (cel.certainCardNames.Contains(c.cardName) || (c.blessing == true && cel.whileHoldingBlessing))
                        {
                            // character has the require 'while holding' card, set energy cost to 0
                            return 0;
                        }
                    }
                }
            }
        }      

        // Normal logic
        int costReturned = card.cardBaseEnergyCost;

        costReturned -= card.energyReductionThisActivationOnly;
        costReturned -= card.energyReductionThisCombatOnly;
        costReturned -= card.energyReductionUntilPlayed;

        // Check passive count reduction modifiers
        if (card.cardPassiveEffects.Count > 0)
        {
            foreach (CardPassiveEffect cpe in card.cardPassiveEffects)
            {
                if (cpe.cardPassiveEffectType == CardPassiveEffectType.EnergyCostReducedByCurrentPassive)
                {
                    if (cpe.passive == Passive.Overload)
                    {
                        costReturned -= card.owner.pManager.overloadStacks;
                    }
                }
            }
        }

        // Check 'Planted Feet' passive
        if (card.owner.pManager != null && 
            card.cardType == CardType.MeleeAttack &&
            card.owner.pManager.plantedFeetStacks > 0)
        {
            costReturned -= card.owner.pManager.plantedFeetStacks;
        }

        // Check 'Take Aim' passive
        if (card.owner.pManager != null &&
            card.cardType == CardType.RangedAttack &&
            card.owner.pManager.takenAimStacks > 0)
        {
            costReturned -= card.owner.pManager.takenAimStacks;
        }

        // Check source spell reduction from source
        if (includeSourceReduction == true &&
            card.owner.pManager != null &&
            card.sourceSpell == true &&
            card.owner.pManager.sourceStacks > 0)
        {
            costReturned -= card.owner.pManager.sourceStacks;
        }

        // Prevent cost going negative
        if (costReturned < 0)
        {
            costReturned = 0;
        }

        return costReturned;
    }
    private void ReduceCardEnergyCostThisActivation(Card card, int reductionAmount)
    {
        // Setup
        CardViewModel cvm = card.cardVM;

        // Reduce cost this combat
        card.energyReductionThisActivationOnly += reductionAmount;

        // Update card vm energy text, if not null
        int newCostTextValue = GetCardEnergyCost(card);
        if (cvm)
        {
            // Update glow
            AutoUpdateCardGlowOutline(card);

            // Update energy cost text
            VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));
        }
    }
    public void ReduceCardEnergyCostThisCombat(Card card, int reductionAmount)
    {
        // Setup
        CardViewModel cvm = card.cardVM;

        // Reduce cost this combat
        card.energyReductionThisCombatOnly += reductionAmount;

        // Update card vm energy text, if not null
        int newCostTextValue = GetCardEnergyCost(card);
        if (cvm)
        {
            // Update glow
            AutoUpdateCardGlowOutline(card);

            // Play breath anim
            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));

            // Update energy cost text
            VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));
        }       
    }
    public void SetCardEnergyCostThisCombat(Card card, int newEnergyCost)
    {
        // Setup
        CardViewModel cvm = card.cardVM;

        // get difference
        int reductionAmount = card.cardBaseEnergyCost - newEnergyCost;

        // Reduce cost this combat
        card.energyReductionThisCombatOnly += reductionAmount;

        // Update card vm energy text, if not null
        int newCostTextValue = GetCardEnergyCost(card);
        if (cvm)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
            VisualEventManager.Instance.CreateVisualEvent(() => SetCardViewModelEnergyText(card, cvm, newCostTextValue.ToString()));
        }
    }
    public void ResetAllCardEnergyCostsOnActivationEnd(CharacterEntityModel character)
    {
        List<Card> allCharacterCards = GetAllCharacterCardsInHandDrawAndDiscard(character);

        foreach(Card card in allCharacterCards)
        {
            ReduceCardEnergyCostThisActivation(card, -card.energyReductionThisActivationOnly);
        }
    }
    #endregion

    // Table Logic
    #region
    public bool IsCursorOverTable()
    {
        return mouseIsOverTable;
    }
    private void OnMouseOver()
    {
        mouseIsOverTable = true;
    }
    private void OnMouseExit()
    {
        mouseIsOverTable = false;
    }
    #endregion

    // Choose card in hand Logic 
    #region
    private void StartNewChooseCardInHandEvent(CardEffect ce, CharacterEntityModel owner)
    {
        // cancel if player doesnt have any cards to choose
        if(owner.hand.Count == 0)
        {
            return;
        }

        // cancel if choosing card to upgrade, but no upgradeable cards in hand
        foreach(OnCardInHandChoiceMadeEffect chooseEffect in ce.onChooseCardInHandChoiceMadeEffects)
        {
            if(chooseEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.UpgradeIt &&
                GetUpgradeableCardsFromCollection(owner.hand).Count == 0)
            {
                return;
            }
        }

        ResetChooseCardScreenProperties();
        CurrentChooseCardScreenSelection = null;
        chooseCardScreenEffectReference = ce;
        ChooseCardScreenIsActive = true;
        ShowChooseCardInHandScreen();
        SetChooseCardScreenBannerText(ce);


    }
    public void HandleChooseScreenCardSelection(Card selectedCard)
    {
        AudioManager.Instance.PlaySound(Sound.Card_Discarded);

        // cancel if choice effect is for upgrade, and the card is not upgradeable
        foreach(OnCardInHandChoiceMadeEffect choiceEffect in chooseCardScreenEffectReference.onChooseCardInHandChoiceMadeEffects)
        {
            if(IsCardUpgradeable(selectedCard) == false && choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.UpgradeIt)
            {
                return;
            }
        }

        // move to choice slot
        if (CurrentChooseCardScreenSelection == null)
        {
            CurrentChooseCardScreenSelection = selectedCard;
            selectedCard.cardVM.hoverPreview.SetChooseCardScreenTransistionState(true);
            selectedCard.cardVM.mySlotHelper.ResetAngles();
            MoveTransformToLocation(selectedCard.cardVM.movementParent, chooseCardScreenSelectionSpot.position, 0.25f, false, ()=> selectedCard.cardVM.hoverPreview.SetChooseCardScreenTransistionState(false));

        }
        else if(CurrentChooseCardScreenSelection != null)
        {
            // Set transistion state on both cards
            Card previousCard = CurrentChooseCardScreenSelection;
            previousCard.cardVM.hoverPreview.SetChooseCardScreenTransistionState(true);
            selectedCard.cardVM.hoverPreview.SetChooseCardScreenTransistionState(true);

            // move old card back to hand
            selectedCard.owner.characterEntityView.handVisual.UpdateCardRotationsAndYDrops();
            MoveTransformToLocation(previousCard.cardVM.movementParent, 
                selectedCard.owner.characterEntityView.handVisual.slots.Children[previousCard.cardVM.locationTracker.Slot].gameObject.transform.position,
                0.25f, false, () => previousCard.cardVM.hoverPreview.SetChooseCardScreenTransistionState(false));

            // move new card to centre spot
            CurrentChooseCardScreenSelection = selectedCard;
            selectedCard.cardVM.mySlotHelper.ResetAngles();
            MoveTransformToLocation(selectedCard.cardVM.movementParent, chooseCardScreenSelectionSpot.position, 0.25f, false, () => selectedCard.cardVM.hoverPreview.SetChooseCardScreenTransistionState(false));
        }

        UpdateConfirmChoiceButton();

    }
    private void ResolveChooseCardChoiceEffects()
    {
        // in future we may want to perform extra effects on the cards we create (reduce energy cost, etc)
        // so we can stash them here for now
        List<Card> newCards = new List<Card>();
        bool returnSelctionToHand = true;
        CharacterEntityModel owner = CurrentChooseCardScreenSelection.owner;
       

        if (owner == null)
        {
            owner = ActivationManager.Instance.EntityActivated;
        }

        owner.characterEntityView.handVisual.UpdateCardRotationsAndYDrops();

        foreach (OnCardInHandChoiceMadeEffect choiceEffect in chooseCardScreenEffectReference.onChooseCardInHandChoiceMadeEffects)
        {
            if(choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.AddCopyToHand)
            {
                for(int i = 0; i < choiceEffect.copiesAdded; i++)
                {
                    Card newCard =  CreateAndAddNewCardToCharacterHand(owner, CurrentChooseCardScreenSelection);
                    newCards.Add(newCard);
                }
            }
            else if (choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.ExpendIt)
            {
                ExpendCard(CurrentChooseCardScreenSelection);
                returnSelctionToHand = false;
            }
            else if (choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.UpgradeIt)
            {
                HandleUpgradeCardForCharacterEntity(CurrentChooseCardScreenSelection);
            }

            else if (choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.GainPassive)
            {
                string passiveName = TextLogic.SplitByCapitals(choiceEffect.passivePairing.passiveData.ToString());

                PassiveController.Instance.ModifyPassiveOnCharacterEntity
                    (owner.pManager, passiveName, choiceEffect.passivePairing.passiveStacks, true, 0.5f, owner.pManager);
            }

            else if (choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.GetUpgradedBlessings)
            {
                for(int i = 0; i < choiceEffect.blessingsGained; i++)
                {
                    List<CardData> allBlessings = QueryByBlessing(AllCards, true);
                    List<CardData> filteredBlessings = QueryByUpgraded(allBlessings);

                    CardData randomBlessing = filteredBlessings[RandomGenerator.NumberBetween(0, filteredBlessings.Count - 1)];
                    CreateAndAddNewCardToCharacterHand(owner, randomBlessing);
                }
            }

            // reduce cost of new cards
            else if (choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.ReduceEnergyCost)
            {
                foreach (Card card in newCards)
                {
                    ReduceCardEnergyCostThisCombat(card, choiceEffect.energyReduction);
                }
            }

            // set cost of new cards
            else if (choiceEffect.choiceEffect == OnCardInHandChoiceMadeEffectType.SetEnergyCost)
            {
                foreach (Card card in newCards)
                {
                    SetCardEnergyCostThisCombat(card, choiceEffect.newEnergyCost);
                }
            }

        }

        // Move the card selection back to hand
        if (returnSelctionToHand)
        {
            MoveTransformToLocation(CurrentChooseCardScreenSelection.cardVM.movementParent,
          CurrentChooseCardScreenSelection.owner.characterEntityView.handVisual.slots.Children[CurrentChooseCardScreenSelection.cardVM.locationTracker.Slot].gameObject.transform.position,
          0.25f);
        }  

        // disable screen
        FadeOutChoiceScreenOverlay(() => HideChooseCardInHandScreen());

        // clear propeties and reset for next time
        ResetChooseCardScreenProperties();
    }
    private void ResetChooseCardScreenProperties()
    {
        CurrentChooseCardScreenSelection = null;
        chooseCardScreenEffectReference = null;
        ChooseCardScreenIsActive = false;
    }
    #endregion

    // Choose Card In hand GUI Logic
    #region
    private void ShowChooseCardInHandScreen()
    {
        FadeInChoiceScreenOverlay();
        chooseCardScreenVisualParent.SetActive(true);
        UpdateConfirmChoiceButton();
    }
    private void HideChooseCardInHandScreen()
    {
        chooseCardScreenVisualParent.SetActive(false);
    }
    private void UpdateConfirmChoiceButton()
    {
        if(CurrentChooseCardScreenSelection == null)
        {
            chooseCardScreenConfirmButton.image.sprite = inactiveButtonSprite;
        }
        else
        {
            chooseCardScreenConfirmButton.image.sprite = activeButtonSprite;
        }
    }
    private void SetChooseCardScreenBannerText(CardEffect ce)
    {
        OnCardInHandChoiceMadeEffect data = ce.onChooseCardInHandChoiceMadeEffects[0];
        string newText = "";

        if(data.choiceEffect == OnCardInHandChoiceMadeEffectType.AddCopyToHand)
        {
            newText = "Choose a Card to Copy";
        }
        else if (data.choiceEffect == OnCardInHandChoiceMadeEffectType.ExpendIt)
        {
            newText = "Choose a Card to Expend";
        }
        else if (data.choiceEffect == OnCardInHandChoiceMadeEffectType.UpgradeIt)
        {
            newText = "Choose a Card to Upgrade";
        }
        else if (data.choiceEffect == OnCardInHandChoiceMadeEffectType.ReduceEnergyCost)
        {
            newText = "Choose a Card to Reduce Energy Cost";
        }
        else if (data.choiceEffect == OnCardInHandChoiceMadeEffectType.SetEnergyCost)
        {
            newText = "Choose a Card to Set New Energy Cost";
        }

        chooseCardScreenBannerText.text = newText;
    }
    public void OnConfirmChoiceButtonClicked()
    {
        if(CurrentChooseCardScreenSelection != null)
        {
            ResolveChooseCardChoiceEffects();
        }        
    }
    private void FadeInChoiceScreenOverlay(Action onCompleteCallBack = null)
    {
        chooseCardScreenOverlayCg.alpha = 0f;

        Sequence s = DOTween.Sequence();
        s.Append(chooseCardScreenOverlayCg.DOFade(1f, 0.5f));

        if (onCompleteCallBack != null)
        {
            s.OnComplete(() => onCompleteCallBack.Invoke());
        }
    }
    private void FadeOutChoiceScreenOverlay(Action onCompleteCallBack = null)
    {
        chooseCardScreenOverlayCg.alpha = 1f;

        Sequence s = DOTween.Sequence();
        s.Append(chooseCardScreenOverlayCg.DOFade(0f, 0.5f));

        if (onCompleteCallBack != null)
        {
            s.OnComplete(() => onCompleteCallBack.Invoke());
        }

    }
    #endregion

    // Discovery Logic
    #region
    private IEnumerator StartNewDiscoveryEvent(CardEffect ce, CharacterEntityModel owner)
    {
        // Enable discovery screen
        ShowDiscoveryScreen();
        currentDiscoveryEffect = ce;

        // set up slot positions magic
        
        foreach (Transform t in discoveryCardSlots)
        {
            t.gameObject.SetActive(false);
        }

        foreach (Transform t in discoveryCardSlots)
        {
            t.gameObject.SetActive(true);
        }

        foreach (Transform t in discoveryCardSlots)
        {
            t.gameObject.SetActive(false);
        }

        List<Transform> slotsEnabled = new List<Transform>();
        List<DiscoveryCardViewModel> cardsEnabled = new List<DiscoveryCardViewModel>();
        

        // Discover cards from card data so library
        if (ce.discoveryLocation == CardCollection.CardLibrary)
        {
            List<CardData> discoverableCards = new List<CardData>();
            if (ce.sourceSpellsOnly)
            {
                foreach(CardData c in QueryBySourceSpell(AllCards))
                {
                    if(ce.upgradeFilter == UpgradeFilter.OnlyNonUpgraded && c.upgradeLevel == 0)
                    {
                        discoverableCards.Add(c);
                    }
                    else if (ce.upgradeFilter == UpgradeFilter.OnlyUpgraded && c.upgradeLevel >= 1)
                    {
                        discoverableCards.Add(c);
                    }
                    else if (ce.upgradeFilter == UpgradeFilter.Any)
                    {
                        discoverableCards.Add(c);
                    }
                }
            }
            else
            {
                discoverableCards = GetCardsQuery(AllCards, ce.talentSchoolFilter, ce.rarityFilter, ce.blessing, ce.upgradeFilter);
            }           

            // cancel there are discoverable cards to pick
            if (discoverableCards.Count == 0)
            {
                currentDiscoveryEffect = null;
                discoveryScreenVisualParent.SetActive(false);
                HideDiscoveryScreen();
                yield break;
            }

            // confetti explosion VFX
            CreateConfettiExplosionsOnDiscovery();

            // randomize cards
            discoverableCards.Shuffle();

            // how valid cards were found?
            int discoverChoicesToCreate = discoverableCards.Count;

            // limit choices to 3 or less
            if (discoverChoicesToCreate > 3)
            {
                discoverChoicesToCreate = 3;
            }

            // End if no valid discoverable cards were found
            if (discoverableCards.Count > 0)
            {
                // Build the a discovery card view for each card found
                for (int i = 0; i < discoverChoicesToCreate; i++)
                {
                    // Get discovery card
                    DiscoveryCardViewModel dcvm = discoveryCards[i];

                    // cache ref to data
                    dcvm.myDataRef = discoverableCards[i];
                    //Debug.LogWarning("Discoverable card new data ref = " + dcvm.myDataRef.cardName);

                    // mark slot for enabling
                    slotsEnabled.Add(discoveryCardSlots[i]);

                    // mark card for enabling
                    cardsEnabled.Add(dcvm);

                    // build view model
                    BuildCardViewModelFromCardData(discoverableCards[i], dcvm.cardViewModel);
                }
            }
        }

        // Discover cards from a player collection of card objects
        else
        {
            List<Card> discoverableCards = new List<Card>();

            // Which collection should we discover from?
            List<Card> collectionReference = null;
            if(ce.discoveryLocation == CardCollection.DiscardPile)
            {
                collectionReference = owner.discardPile;
            }
            else if (ce.discoveryLocation == CardCollection.DrawPile)
            {
                collectionReference = owner.drawPile;
            }
            else if (ce.discoveryLocation == CardCollection.Hand)
            {
                collectionReference = owner.hand;
            }
            else if (ce.discoveryLocation == CardCollection.ExpendPile)
            {
                collectionReference = owner.expendPile;
            }

            if(collectionReference == null)
            {
                Debug.LogWarning("StartNewDiscoveryEvent() was given a null collection to discover cards from: you probably" +
                    " forgot to assign a card collection to search in via the inspector!!");
            }

            // Get cards from the chosen collection
            discoverableCards = GetCardsQuery(collectionReference, ce.talentSchoolFilter, ce.rarityFilter, ce.blessing);

            // cancel there are discoverable cards to pick
            if (discoverableCards.Count == 0)
            {
                currentDiscoveryEffect = null;
                discoveryScreenVisualParent.SetActive(false);
                HideDiscoveryScreen();
                yield break;
            }

            // confetti explosion VFX
            CreateConfettiExplosionsOnDiscovery();

            // randomize cards
            discoverableCards.Shuffle();

            // how valid cards were found?
            int discoverChoicesToCreate = discoverableCards.Count;

            // limit choices to 3 or less
            if (discoverChoicesToCreate > 3)
            {
                discoverChoicesToCreate = 3;
            }

            if (discoverableCards.Count > 0)
            {
                // Build the a discovery card view for each card found
                for (int i = 0; i < discoverChoicesToCreate; i++)
                {
                    // Get discovery card
                    DiscoveryCardViewModel dcvm = discoveryCards[i];

                    // cache ref to card
                    dcvm.myCardRef = discoverableCards[i];

                    // mark slot for enabling
                    slotsEnabled.Add(discoveryCardSlots[i]);

                    // mark card for enabling
                    cardsEnabled.Add(dcvm);

                    // build view model
                    SetUpCardViewModelAppearanceFromCard(dcvm.cardViewModel, discoverableCards[i]);

                }
            }
        }        

        // Enable slots
        foreach (Transform t in slotsEnabled)
        {
            t.gameObject.SetActive(true);
        }

        // brief yield to allow the horizontal fitter to correctly 
        // position the slots before moving the cards
        yield return new WaitForEndOfFrame();

        // move the cards
        for(int i = 0; i < cardsEnabled.Count; i++)
        {
            // enable GO
            cardsEnabled[i].gameObject.SetActive(true);

            // Move towards slots
            cardsEnabled[i].transform.DOMove(slotsEnabled[i].position, 0.3f);
        }

    }
    public IEnumerator StartNewDiscoveryEvent(List<CardData> discoverChoices)
    {
        // NOTE: This overload method is used for kings blessing card discovery choices

        // Enable discovery screen
        ShowDiscoveryScreen();

        // set up slot positions magic
        foreach (Transform t in discoveryCardSlots)
        {
            t.gameObject.SetActive(false);
        }

        foreach (Transform t in discoveryCardSlots)
        {
            t.gameObject.SetActive(true);
        }

        foreach (Transform t in discoveryCardSlots)
        {
            t.gameObject.SetActive(false);
        }

        List<Transform> slotsEnabled = new List<Transform>();
        List<DiscoveryCardViewModel> cardsEnabled = new List<DiscoveryCardViewModel>();

        // cancel there are discoverable cards to pick
        if (discoverChoices.Count == 0)
        {
            currentDiscoveryEffect = null;
            discoveryScreenVisualParent.SetActive(false);
            HideDiscoveryScreen();
            yield break;
        }

        // confetti explosion VFX
        CreateConfettiExplosionsOnDiscovery();

        // randomize cards
        discoverChoices.Shuffle();

        // how valid cards were found?
        int discoverChoicesToCreate = discoverChoices.Count;

        // limit choices to 3 or less
        if (discoverChoicesToCreate > 3)
        {
            discoverChoicesToCreate = 3;
        }

        // End if no valid discoverable cards were found
        if (discoverChoices.Count > 0)
        {
            // Build the a discovery card view for each card found
            for (int i = 0; i < discoverChoicesToCreate; i++)
            {
                // Get discovery card
                DiscoveryCardViewModel dcvm = discoveryCards[i];

                // cache ref to data
                dcvm.myDataRef = discoverChoices[i];
                Debug.LogWarning("Discoverable card new data ref = " + dcvm.myDataRef.cardName);

                // mark slot for enabling
                slotsEnabled.Add(discoveryCardSlots[i]);

                // mark card for enabling
                cardsEnabled.Add(dcvm);

                // build view model
                BuildCardViewModelFromCardData(discoverChoices[i], dcvm.cardViewModel);
            }
        }

        // Enable slots
        foreach (Transform t in slotsEnabled)
        {
            t.gameObject.SetActive(true);
        }

        // brief yield to allow the horizontal fitter to correctly 
        // position the slots before moving the cards
        yield return new WaitForEndOfFrame();

        // move the cards
        for (int i = 0; i < cardsEnabled.Count; i++)
        {
            // enable GO
            cardsEnabled[i].gameObject.SetActive(true);

            // Move towards slots
            cardsEnabled[i].transform.DOMove(slotsEnabled[i].position, 0.3f);
        }

    }
    public void OnDiscoveryCardClicked(DiscoveryCardViewModel dcvm)
    {
        if (KingsBlessingController.Instance.AwaitingCardDiscoveryChoice)
        {
            // Handle KBC logic
            KingsBlessingController.Instance.HandleDiscoverCardChoiceMade(dcvm.myDataRef);

            AudioManager.Instance.PlaySound(Sound.GUI_Button_Clicked);

            // disable screen
            HideDiscoveryScreen();

            // reset dcvm's
            foreach (DiscoveryCardViewModel dCard in discoveryCards)
            {
                dCard.ResetSelfOnEventComplete();
            }

            if(dcvm.myDataRef == null)
            {
                Debug.LogWarning("CARD DATA REF IS NULL!!");
            }          

        }
        else
        {
            if (dcvm.myCardRef != null)
            {
                AudioManager.Instance.PlaySound(Sound.GUI_Button_Clicked);
                ResolveDiscoveryCardClicked(dcvm, dcvm.myCardRef);
            }
            else if (dcvm.myDataRef != null)
            {
                AudioManager.Instance.PlaySound(Sound.GUI_Button_Clicked);
                ResolveDiscoveryCardClicked(dcvm, dcvm.myDataRef);
            }

            // disable screen
            HideDiscoveryScreen();

            // clear current d event effect
            currentDiscoveryEffect = null;

            // reset dcvm's
            foreach (DiscoveryCardViewModel dCard in discoveryCards)
            {
                dCard.ResetSelfOnEventComplete();
            }
        }
        
        
    }
    private void ResolveDiscoveryCardClicked(DiscoveryCardViewModel dcvm, Card cardRef)
    {
        List<Card> cards = new List<Card>();

        // TO DO: should probably make a better way to find which player started the discovery process
        CharacterEntityModel owner = ActivationManager.Instance.EntityActivated;

        foreach (OnDiscoveryChoiceMadeEffect effect in currentDiscoveryEffect.onDiscoveryChoiceMadeEffects)
        {
            // Add to hand
            if (effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.AddToHand)
            {
                // From draw pile
                if(currentDiscoveryEffect.discoveryLocation == CardCollection.DrawPile)
                {
                    DrawACardFromDrawPile(owner, owner.drawPile.IndexOf(cardRef));
                    cards.Add(cardRef);
                }

                // From discard pile
                else if (currentDiscoveryEffect.discoveryLocation == CardCollection.DiscardPile)
                {
                    MoveCardFromDiscardPileToHand(cardRef);
                    cards.Add(cardRef);
                }

                // From expend pile
                else if (currentDiscoveryEffect.discoveryLocation == CardCollection.ExpendPile)
                {
                    MoveCardFromExpendPileToHand(cardRef);
                    cards.Add(cardRef);
                }
            }

            // Create copies and add to hand
            else if (effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.AddCopyToHand)
            {
                for (int i = 0; i < effect.copiesAdded; i++)
                {
                    Card newCard = CreateAndAddNewCardToCharacterHand(owner, cardRef);
                    cards.Add(newCard);
                }
            }

            // reduce cost of new cards
            else if (effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.ReduceEnergyCost)
            {
                foreach (Card card in cards)
                {
                    ReduceCardEnergyCostThisCombat(card, effect.energyReduction);
                }
            }

            // set cost of new cards
            else if (effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.SetEnergyCost)
            {
                foreach (Card card in cards)
                {
                    SetCardEnergyCostThisCombat(card, effect.newEnergyCost);
                }
            }
        }
    }
    private void ResolveDiscoveryCardClicked(DiscoveryCardViewModel dcvm, CardData dataRef)
    {
        List<Card> cards = new List<Card>();

        // TO DO: should probably make a better way to find which player started the discovery process
        CharacterEntityModel owner = ActivationManager.Instance.EntityActivated;

        foreach (OnDiscoveryChoiceMadeEffect effect in currentDiscoveryEffect.onDiscoveryChoiceMadeEffects)
        {
            // Add copies to hand
            if (effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.AddCopyToHand ||
                effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.AddToHand)
            {
                for (int i = 0; i < effect.copiesAdded; i++)
                {
                    Card newCard = CreateAndAddNewCardToCharacterHand(owner, dataRef);
                    cards.Add(newCard);
                }
            }

            // reduce cost of new cards
            else if (effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.ReduceEnergyCost)
            {
                foreach (Card card in cards)
                {
                    ReduceCardEnergyCostThisCombat(card, effect.energyReduction);
                }
            }

            // set cost of new cards
            else if (effect.discoveryEffect == OnDiscoveryChoiceMadeEffectType.SetEnergyCost)
            {
                foreach (Card card in cards)
                {
                    SetCardEnergyCostThisCombat(card, effect.newEnergyCost);
                }
            }
        }
    }
    #endregion   

    // Discovery Screen GUI Logic
    #region
    private void ShowDiscoveryScreen()
    {
        DiscoveryScreenIsActive = true;
        discoveryScreenVisualParent.SetActive(true);
        MoveDiscoverySlotsToStartPosition();
        FadeInDiscoveryScreenOverlay();      
    }
    private void HideDiscoveryScreen()
    {
        DiscoveryScreenIsActive = false;
        FadeOutDiscoveryScreenOverlay(() => 
        {
            MoveDiscoverySlotsToStartPosition();
            MoveDiscoveryCardsToStartPosition();
            discoveryScreenVisualParent.SetActive(false);
        });       
    }
    private void CreateConfettiExplosionsOnDiscovery()
    {
        foreach(Transform t in confettiTransforms)
        {
            VisualEffectManager.Instance.CreateConfettiExplosionRainbow(CameraManager.Instance.MainCamera.ScreenToWorldPoint(t.position), 10000);
        }
    }
    private void FadeInDiscoveryScreenOverlay(Action onCompleteCallBack = null)
    {
        discoveryScreenOverlayCg.alpha = 0f;

        Sequence s = DOTween.Sequence();
        s.Append(discoveryScreenOverlayCg.DOFade(1f, 0.5f));

        if (onCompleteCallBack != null)
        {
            s.OnComplete(() => onCompleteCallBack.Invoke());
        }
    }
    private void FadeOutDiscoveryScreenOverlay(Action onCompleteCallBack = null)
    {
        discoveryScreenOverlayCg.alpha = 1f;

        Sequence s = DOTween.Sequence();        
        s.Append(discoveryScreenOverlayCg.DOFade(0f, 0.5f));

        if(onCompleteCallBack != null)
        {
            s.OnComplete(() => onCompleteCallBack.Invoke());
        }
       
    }
    private void MoveDiscoveryCardsToStartPosition()
    {
        foreach(DiscoveryCardViewModel dcvm in discoveryCards)
        {
            dcvm.transform.position = dcvm.transform.parent.transform.position;
        }
    }
    private void MoveDiscoverySlotsToStartPosition()
    {
        // Disable slots
        foreach(Transform t in discoveryCardSlots)
        {
            t.gameObject.SetActive(false);
        }
    }
    #endregion

    // Shuffle Cards Screen Logic
    #region
    public void StartNewShuffleCardsScreenVisualEvent(List<Card> cards)
    {
        // cache cards for visual events
        List<Card> cachedCards = new List<Card>();
        cachedCards.AddRange(cards);
        List<DiscoveryCardViewModel> activeCards = new List<DiscoveryCardViewModel>();

        for(int i = 0; i < cards.Count; i++)
        {
            activeCards.Add(shuffleCards[i]);
        }

        // Get owner / destination
        CharacterEntityView view = cachedCards[0].owner.characterEntityView;

        // Set up main screen V event
        CoroutineData cData = new CoroutineData()
;       VisualEventManager.Instance.CreateVisualEvent(() => SetUpShuffleCardScreen(cachedCards, cData), cData);

        // brief pause so player can view cards
        VisualEventManager.Instance.InsertTimeDelayInQueue(1, QueuePosition.Back);

        // Move each card towards character v Event
        foreach(DiscoveryCardViewModel dcvm in activeCards)
        {
            VisualEventManager.Instance.CreateVisualEvent(() =>
                    MoveShuffleCardTowardsCharacterEntityView(dcvm, view), QueuePosition.Back, 0, 0.2f);
        }

        // Reset Slot Positions
        VisualEventManager.Instance.CreateVisualEvent(() => MoveShuffleSlotsToStartPosition());
    }   
    public void StartNewShuffleCardsScreenVisualEvent(CharacterEntityView view, List<CardData> cards)
    {
        // cache cards for visual events
        List<CardData> cachedCards = new List<CardData>();
        cachedCards.AddRange(cards);
        List<DiscoveryCardViewModel> activeCards = new List<DiscoveryCardViewModel>();

        for (int i = 0; i < cards.Count; i++)
        {
            activeCards.Add(shuffleCards[i]);
        }

        // Set up main screen V event
        CoroutineData cData = new CoroutineData(); 
        VisualEventManager.Instance.CreateVisualEvent(() => SetUpShuffleCardScreen(cachedCards, cData), cData);

        // brief pause so player can view cards
        VisualEventManager.Instance.InsertTimeDelayInQueue(1, QueuePosition.Back);

        // Move each card towards character v Event
        foreach (DiscoveryCardViewModel dcvm in activeCards)
        {
            VisualEventManager.Instance.CreateVisualEvent(() =>
                    MoveShuffleCardTowardsCharacterEntityView(dcvm, view), QueuePosition.Back, 0, 0.2f);
        }

        // Reset Slot Positions
        VisualEventManager.Instance.CreateVisualEvent(() => MoveShuffleSlotsToStartPosition());
    }
    public void StartNewShuffleCardsScreenVisualEvent(UniversalCharacterModel view, List<ItemData> cards)
    {
        // cache cards for visual events
        List<ItemData> cachedCards = new List<ItemData>();
        cachedCards.AddRange(cards);
        List<DiscoveryCardViewModel> activeCards = new List<DiscoveryCardViewModel>();

        for (int i = 0; i < cards.Count; i++)
        {
            activeCards.Add(shuffleCards[i]);
        }

        // Set up main screen V event
        CoroutineData cData = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => SetUpShuffleCardScreen(cachedCards, cData), cData);

        // brief pause so player can view cards
        VisualEventManager.Instance.InsertTimeDelayInQueue(1, QueuePosition.Back);

        // Move each card towards character v Event
        foreach (DiscoveryCardViewModel dcvm in activeCards)
        {
            VisualEventManager.Instance.CreateVisualEvent(() =>
                    MoveShuffleCardTowardsCharacterEntityView(dcvm, view), QueuePosition.Back, 0, 0.2f);
        }

        // Reset Slot Positions
        VisualEventManager.Instance.CreateVisualEvent(() => MoveShuffleSlotsToStartPosition());
    }
    public void StartNewShuffleCardsScreenVisualEvent(UniversalCharacterModel view, List<StateData> states)
    {
        // cache cards for visual events
        List<StateData> cachedStates = new List<StateData>();
        cachedStates.AddRange(states);
        List<DiscoveryCardViewModel> activeCards = new List<DiscoveryCardViewModel>();

        for (int i = 0; i < states.Count; i++)
        {
            activeCards.Add(shuffleCards[i]);
        }

        // Set up main screen V event
        CoroutineData cData = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => SetUpShuffleCardScreen(cachedStates, cData), cData);

        // brief pause so player can view cards
        VisualEventManager.Instance.InsertTimeDelayInQueue(1, QueuePosition.Back);

        // Move each card towards character v Event
        foreach (DiscoveryCardViewModel dcvm in activeCards)
        {
            VisualEventManager.Instance.CreateVisualEvent(() =>
                    MoveShuffleCardTowardsCharacterEntityView(dcvm, view), QueuePosition.Back, 0, 0.2f);
        }

        // Reset Slot Positions
        VisualEventManager.Instance.CreateVisualEvent(() => MoveShuffleSlotsToStartPosition());
    }
    public void StartNewShuffleCardsScreenExpendVisualEvent(List<CardData> cards)
    {
        // cache cards for visual events
        List<CardData> cachedCards = new List<CardData>();
        cachedCards.AddRange(cards);
        List<DiscoveryCardViewModel> activeCards = new List<DiscoveryCardViewModel>();

        for (int i = 0; i < cards.Count; i++)
        {
            activeCards.Add(shuffleCards[i]);
        }

        // Set up main screen V event
        CoroutineData cData = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => SetUpShuffleCardScreen(cachedCards, cData), cData);

        // brief pause so player can view cards
        VisualEventManager.Instance.InsertTimeDelayInQueue(1, QueuePosition.Back);
        
        // Move each card towards character v Event
        foreach (DiscoveryCardViewModel dcvm in activeCards)
        { 
            VisualEventManager.Instance.CreateVisualEvent(() =>
            {
                // SFX
                AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);

                // Fade CVM
                FadeOutCardViewModel(dcvm.cardViewModel, null, () => dcvm.gameObject.SetActive(false));

                // Create smokey/expend effect
                VisualEffectManager.Instance.CreateExpendEffect(dcvm.cardViewModel.movementParent.transform.position, 20000, 1.25f);
            });
            
        }

        VisualEventManager.Instance.InsertTimeDelayInQueue(1f);

        // Reset Slot Positions
        VisualEventManager.Instance.CreateVisualEvent(() => MoveShuffleSlotsToStartPosition());
    }
    public void StartNewShuffleCardsScreenVisualEvent(UniversalCharacterModel view, List<CardData> cards)
    {
        // cache cards for visual events
        List<CardData> cachedCards = new List<CardData>();
        cachedCards.AddRange(cards);
        List<DiscoveryCardViewModel> activeCards = new List<DiscoveryCardViewModel>();

        for (int i = 0; i < cards.Count; i++)
        {
            activeCards.Add(shuffleCards[i]);
        }

        // Set up main screen V event
        CoroutineData cData = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => SetUpShuffleCardScreen(cachedCards, cData), cData);

        // brief pause so player can view cards
        VisualEventManager.Instance.InsertTimeDelayInQueue(1, QueuePosition.Back);

        // Move each card towards character v Event
        foreach (DiscoveryCardViewModel dcvm in activeCards)
        {
            VisualEventManager.Instance.CreateVisualEvent(() =>
                    MoveShuffleCardTowardsCharacterEntityView(dcvm, view), QueuePosition.Back, 0, 0.2f);
        }

        // Reset Slot Positions
        VisualEventManager.Instance.CreateVisualEvent(() => MoveShuffleSlotsToStartPosition());
    }
    private void SetUpShuffleCardScreen(List<Card> cachedCards, CoroutineData cData)
    {
        StartCoroutine(SetUpShuffleCardScreenCoroutine(cachedCards, cData));
    }
    private IEnumerator SetUpShuffleCardScreenCoroutine(List<Card> cachedCards, CoroutineData cData)
    {
        shuffleCardsScreenVisualParent.SetActive(true);
        MoveShuffleSlotsToStartPosition();
        //yield return null;
        MoveShuffleCardsToStartPosition();

        for (int i = 0; i < cachedCards.Count; i++)
        {
            SetUpCardViewModelAppearanceFromCard(shuffleCards[i].cardViewModel, cachedCards[i]);
            shuffleCards[i].gameObject.SetActive(true);
            shuffleCardSlots[i].gameObject.SetActive(true);

            // shrink cards down
            shuffleCards[i].scalingParent.localScale = new Vector3(0.1f, 0.1f);
        }

        yield return null;

        for (int i = 0; i < cachedCards.Count; i++)
        {
            // move card to slot     
            shuffleCards[i].scalingParent.DOScale(1, 0.3f);
            shuffleCards[i].transform.gameObject.transform.DOMove(shuffleCardSlots[i].position, 0.3f);
        }

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    private void SetUpShuffleCardScreen(List<StateData> cachedStates, CoroutineData cData)
    {
        StartCoroutine(SetUpShuffleCardScreenCoroutine(cachedStates, cData));
    }
    private IEnumerator SetUpShuffleCardScreenCoroutine(List<StateData> cachedStates, CoroutineData cData)
    {
        shuffleCardsScreenVisualParent.SetActive(true);
        MoveShuffleSlotsToStartPosition();
        MoveShuffleCardsToStartPosition();

        for (int i = 0; i < cachedStates.Count; i++)
        {
            BuildCardViewModelFromStateData(cachedStates[i], shuffleCards[i].cardViewModel);
            //SetUpCardViewModelAppearanceFromCard(shuffleCards[i].cardViewModel, cachedCards[i]);
            shuffleCards[i].gameObject.SetActive(true);
            shuffleCardSlots[i].gameObject.SetActive(true);

            // shrink cards down
            shuffleCards[i].scalingParent.localScale = new Vector3(0.1f, 0.1f);
        }

        yield return null;

        for (int i = 0; i < cachedStates.Count; i++)
        {
            // move card to slot     
            shuffleCards[i].scalingParent.DOScale(1, 0.3f);
            shuffleCards[i].transform.gameObject.transform.DOMove(shuffleCardSlots[i].position, 0.3f);
        }

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    private void SetUpShuffleCardScreen(List<ItemData> cachedCards, CoroutineData cData)
    {
        StartCoroutine(SetUpShuffleCardScreenCoroutine(cachedCards, cData));
    }
    private IEnumerator SetUpShuffleCardScreenCoroutine(List<ItemData> cachedCards, CoroutineData cData)
    {
        shuffleCardsScreenVisualParent.SetActive(true);
        MoveShuffleSlotsToStartPosition();
        //yield return null;
        MoveShuffleCardsToStartPosition();

        for (int i = 0; i < cachedCards.Count; i++)
        {
            BuildCardViewModelFromItemData(cachedCards[i], shuffleCards[i].cardViewModel);
            //SetUpCardViewModelAppearanceFromCard(shuffleCards[i].cardViewModel, cachedCards[i]);
            shuffleCards[i].gameObject.SetActive(true);
            shuffleCardSlots[i].gameObject.SetActive(true);

            // shrink cards down
            shuffleCards[i].scalingParent.localScale = new Vector3(0.1f, 0.1f);
        }

        yield return null;

        for (int i = 0; i < cachedCards.Count; i++)
        {
            // move card to slot     
            shuffleCards[i].scalingParent.DOScale(1, 0.3f);
            shuffleCards[i].transform.gameObject.transform.DOMove(shuffleCardSlots[i].position, 0.3f);
        }

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    private void SetUpShuffleCardScreen(List<CardData> cachedCards, CoroutineData cData)
    {
        StartCoroutine(SetUpShuffleCardScreenCoroutine(cachedCards, cData));
    }
    private IEnumerator SetUpShuffleCardScreenCoroutine(List<CardData> cachedCards, CoroutineData cData)
    {
        shuffleCardsScreenVisualParent.SetActive(true);
        MoveShuffleSlotsToStartPosition();
        MoveShuffleCardsToStartPosition();

        for (int i = 0; i < cachedCards.Count; i++)
        {
            BuildCardViewModelFromCardData(cachedCards[i], shuffleCards[i].cardViewModel);
            shuffleCards[i].cardViewModel.cg.alpha = 1;
            shuffleCards[i].gameObject.SetActive(true);
            shuffleCardSlots[i].gameObject.SetActive(true);

            // shrink cards down
            shuffleCards[i].scalingParent.localScale = new Vector3(0.1f, 0.1f);
        }

        yield return null;

        for (int i = 0; i < cachedCards.Count; i++)
        {
            // move card to slot     
            shuffleCards[i].scalingParent.DOScale(1, 0.3f);
            shuffleCards[i].transform.gameObject.transform.DOMove(shuffleCardSlots[i].position, 0.3f);
        }

        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
    }
    private void MoveShuffleCardTowardsCharacterEntityView(DiscoveryCardViewModel card, CharacterEntityView character)
    {
        // Setup
        Transform movementParent = card.transform;
        CardViewModel cvm = card.cardViewModel;

        //Vector3 cardDestination = CameraManager.Instance.MainCamera.WorldToScreenPoint(character.WorldPosition);
        Vector3 cardDestination = character.WorldPosition;
        Vector3 glowDestination = character.WorldPosition;

        // SFX
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);

        // Create Glow Trail
        //ToonEffect glowTrail = VisualEffectManager.Instance.CreateGreenGlowTrailEffect
        //    (CameraManager.Instance.MainCamera.ScreenToWorldPoint(movementParent.position));

        ToonEffect glowTrail = VisualEffectManager.Instance.CreateGreenGlowTrailEffect
          (movementParent.position);

        // Shrink card
        ScaleCardViewModel(cvm, 0.1f, 0.5f);

        // Rotate card upside down
        RotateCardVisualEvent(cvm, 180, 0.5f);

        // Move card + glow outline to quick lerp spot

        // Move card
        MoveTransformToLocation(cvm.movementParent, cardDestination, 0.5f, false, () => 
        {
            card.gameObject.SetActive(false);
        });
        MoveTransformToLocation(glowTrail.transform, glowDestination, 0.5f, false, () =>
        {
            glowTrail.StopAllEmissions();
            Destroy(glowTrail, 3);
        });
    }
    private void MoveShuffleCardTowardsCharacterEntityView(DiscoveryCardViewModel card, UniversalCharacterModel character)
    {
        // Setup
        Transform movementParent = card.transform;
        CardViewModel cvm = card.cardViewModel;

        //Vector3 cardDestination = CameraManager.Instance.MainCamera.WorldToScreenPoint(character.transform.position);
        Vector3 cardDestination = character.transform.position;
        Vector3 glowDestination = character.transform.position;

        // SFX
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);

        // Create Glow Trail
        //ToonEffect glowTrail = VisualEffectManager.Instance.CreateGreenGlowTrailEffect
        //    (CameraManager.Instance.MainCamera.ScreenToWorldPoint(movementParent.position));

        // Create Glow Trail
        ToonEffect glowTrail = VisualEffectManager.Instance.CreateGreenGlowTrailEffect(movementParent.position);

        // Shrink card
        ScaleCardViewModel(cvm, 0.1f, 0.5f);

        // Rotate card upside down
        RotateCardVisualEvent(cvm, 180, 0.5f);

        // Move card + glow outline to quick lerp spot

        // Move card
        MoveTransformToLocation(cvm.movementParent, cardDestination, 0.5f, false, () =>
        {
            card.gameObject.SetActive(false);
        });
        MoveTransformToLocation(glowTrail.transform, glowDestination, 0.5f, false, () =>
        {
            glowTrail.StopAllEmissions();
            Destroy(glowTrail, 3);
        });
    }
    private void MoveShuffleSlotsToStartPosition()
    {
        // Disable slots
        foreach (Transform t in shuffleCardSlots)
        {
            //t.gameObject.SetActive(false);
            t.gameObject.SetActive(true);
            t.gameObject.SetActive(false);
        }
    }
    private void MoveShuffleCardsToStartPosition()
    {
        foreach (DiscoveryCardViewModel dcvm in shuffleCards)
        {
            // reset card view propeties
            dcvm.gameObject.SetActive(true);
            dcvm.transform.localPosition = new Vector3(0, 0, 0);
            //dcvm.transform.position 
            dcvm.scalingParent.localPosition = new Vector3(0, 0, 0);
            dcvm.scalingParent.localScale = new Vector3(1, 1, 1);
            dcvm.scalingParent.localRotation = Quaternion.identity;
            dcvm.gameObject.SetActive(false);

        }
    }
    #endregion

    // Card Grid Screen Logic
    #region
    public void OnCloseGridScreenButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        HideCardGridScreen();
    }
    public void CreateNewUpgradeCardInDeckPopup(CharacterData character, string ribbonMessage)
    {
        // enable screen
        ShowCardGridScreen();

        // set text
        cardGridRibbonText.text = ribbonMessage;

        // Build Cards
        BuildGridScreenCards(GetUpgradeableCardsFromCollection(character.deck));
    }
    public void CreateNewRemoveCardInDeckPopup(CharacterData character, string ribbonMessage)
    {
        // enable screen
        ShowCardGridScreen();

        // set text
        cardGridRibbonText.text = ribbonMessage;

        // Build Cards
        BuildGridScreenCards(character.deck);
    }   
    public void CreateNewTransformCardInDeckPopup(CharacterData character, string ribbonMessage)
    {
        // enable screen
        ShowCardGridScreen();

        // set text
        cardGridRibbonText.text = ribbonMessage;

        // Build Cards
        BuildGridScreenCards(character.deck);
    }
    public void CreateNewShowDiscardPilePopup(List<Card> cards)
    {
        // enable screen
        ShowCardGridScreen();

        // Show back button
        EnableCardGridScreenBackButton();

        // set text
        cardGridRibbonText.text = "Discard Pile";

        // Build Cards
        BuildGridScreenCards(cards);
    }
    public void CreateNewShowDrawPilePopup(List<Card> cards)
    {
        // enable screen
        ShowCardGridScreen();

        // Show back button
        EnableCardGridScreenBackButton();

        // set text
        cardGridRibbonText.text = "Draw Pile";

        // Build Cards
        BuildGridScreenCards(cards);
    }
    public void BuildAndShowCardUpgradePopUp(CardData cardData)
    {
        // Enable view
        upgradePopupVisualParent.SetActive(true);

        // Fade in screen pop up
        upgradePopupCg.DOKill();
        upgradePopupCg.alpha = 0;
        upgradePopupCg.DOFade(1, 0.25f);

        // Build pop up card views
        CardData upgradeData = FindUpgradedCardData(cardData);
        originalCardPopup.myCardData = cardData;
        upgradedCardPopup.myCardData = upgradeData;

        BuildCardViewModelFromCardData(cardData, originalCardPopup.cardVM);
        BuildCardViewModelFromCardData(upgradeData, upgradedCardPopup.cardVM);
    }
    public void HideCardUpgradePopupScreen()
    {
        Sequence s = DOTween.Sequence();
        s.Append(upgradePopupCg.DOFade(0, 0.2f));
        s.OnComplete(() => { upgradePopupVisualParent.SetActive(false); });
    }
    public void OnUpgradeCardPopupConfirmButtonClicked()
    {
        if (CampSiteController.Instance.AwaitingCardUpgradeChoice)
        {
            CampSiteController.Instance.HandleUpgradeCardChoiceMade(CampSiteController.Instance.selectedUpgradeCard);
        }
        else if (KingsBlessingController.Instance.AwaitingCardUpgradeChoice)
        {
            KingsBlessingController.Instance.HandleUpgradeCardChoiceMade(KingsBlessingController.Instance.selectedUpgradeCard);
        }

    }
    public void OnUpgradeCardPopupCancelButtonClicked()
    {
        HideCardUpgradePopupScreen();
    }

    private void BuildGridScreenCards(List<Card> cards)
    {
        // Disable all grid cards
        foreach (GridCardViewModel g in allGridCards)
        {
            g.gameObject.SetActive(false);
        }

        // Build new grid cards and views
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            GridCardViewModel gc = allGridCards[i];

            // Get card data
            if(card.myCharacterDeckCard != null)
            {
                gc.myCardData = card.myCharacterDeckCard;
            }
            else
            {
                gc.myCardData = GetCardDataFromLibraryByName(card.cardName);
            }

            gc.gameObject.SetActive(true);
            SetUpCardViewModelAppearanceFromCard(gc.cardVM, card);
        }
    }
    private void BuildGridScreenCards(List<CardData> cards)
    {
        // Disable all grid cards
        foreach (GridCardViewModel g in allGridCards)
        {
            g.gameObject.SetActive(false);
        }

        // Build new grid cards and views
        for (int i = 0; i < cards.Count; i++)
        {
            CardData card = cards[i];
            GridCardViewModel gc = allGridCards[i];

            gc.myCardData = card;
            gc.gameObject.SetActive(true);
            BuildCardViewModelFromCardData(card, gc.cardVM);
        }
    }
    public void ShowCardGridScreen()
    {
        cardGridCg.alpha = 0f;
        cardGridVisualParent.SetActive(true);

        Sequence s = DOTween.Sequence();
        s.Append(cardGridCg.DOFade(1f, 0.25f));
    }
    public void HideCardGridScreen()
    {
        cardGridCg.alpha = 1f;
        Sequence s = DOTween.Sequence();
        s.Append(cardGridCg.DOFade(0f, 0.25f));
        s.OnComplete(() => cardGridVisualParent.SetActive(false));
    }
    public void EnableCardGridScreenBackButton()
    {
        cardGridScreenBackButtonParent.SetActive(true);
    }
    public void DisableCardGridScreenBackButton()
    {
        cardGridScreenBackButtonParent.SetActive(false);
    }
    #endregion

    // Upgrade Cards Logic
    #region
    public CardData FindUpgradedCardData(CardData original)
    {
        CardData upgradeCard = null;
        string searchTerm = "";

        // Generate search term for already upgraded cards
        if(original.upgradeLevel > 0)
        {
            searchTerm = original.cardName;
            searchTerm = searchTerm.Remove(searchTerm.Length - 1);
            searchTerm += (original.upgradeLevel + 1).ToString();
        }

        // Generate search term for non upgraded cards
        else
        {
            searchTerm = original.cardName + " +" + (original.upgradeLevel + 1).ToString();
        }

        Debug.Log("CardController.FindUpgradedCardData() called, searching for upgrade of " + original.cardName +
            ", search term: "+ searchTerm);
       

        for(int i = 0; i < AllCards.Length; i++)
        {
            if(AllCards[i].cardName == searchTerm)
            {
                // Found matching upgrade card
                upgradeCard = AllCards[i];
                break;
            }
        }

        if(upgradeCard == null)
        {
            Debug.LogWarning("CardController.FindUpgradedCardData() called, did not find and upgrade of " + original.cardName +
                ", returning null...");
        }

        return upgradeCard;
    }
    public bool IsCardUpgradeable(CardData card)
    {
        return card.upgradeable;
    }
    public bool IsCardUpgradeable(Card card)
    {
        return card.upgradeable;
    }
    public List<CardData> GetUpgradeableCardsFromCollection(IEnumerable<CardData> collection)
    {
        Debug.Log("CardController.GetUpgradeableCardsFromCollection() called...");

        List <CardData> upgradeableCards = new List<CardData>();

        foreach(CardData card in collection)
        {
            if (IsCardUpgradeable(card))
            {
                upgradeableCards.Add(card);
            }
        }

        Debug.Log("CardController.GetUpgradeableCardsFromCollection() found " + upgradeableCards.Count.ToString() +
            " upgradeable cards");
        return upgradeableCards;
    }
    public List<Card> GetUpgradeableCardsFromCollection(IEnumerable<Card> collection)
    {
        Debug.Log("CardController.GetUpgradeableCardsFromCollection() called...");

        List<Card> upgradeableCards = new List<Card>();

        foreach (Card card in collection)
        {
            if (IsCardUpgradeable(card))
            {
                upgradeableCards.Add(card);
            }
        }

        Debug.Log("CardController.GetUpgradeableCardsFromCollection() found " + upgradeableCards.Count.ToString() +
            " upgradeable cards");
        return upgradeableCards;
    }
    public CardData HandleUpgradeCardInCharacterDeck(CardData card, CharacterData character)
    {
        Debug.Log("CardController.HandleUpgradeCardInCharacterDeck() called, upgrading " +
            card.cardName + " for " + character.myName);

        int index = character.deck.IndexOf(card);

        // Find the upgraded version
        CardData upgradeCardData = FindUpgradedCardData(card);
        CardData upgradedCard = null;

        // Clone the data
        if(upgradeCardData != null)
        {
            upgradedCard = CloneCardDataFromCardData(upgradeCardData);
        }               

        // Did we succesfully find and clone the data?
        if(upgradedCard != null)
        {
            // Add new upgraded card
            CharacterDataController.Instance.AddCardToCharacterDeck(character, upgradeCardData, index);

            // Remove old, unupgraded card
            CharacterDataController.Instance.RemoveCardFromCharacterDeck(character, card);
        }

        return upgradedCard;
    }
    public CardData HandleTransformCardInCharacterDeck(CardData originalCard, CharacterData character)
    {
        Debug.Log("CardController.HandleTransformCardInCharacterDeck() called, transforming " +
            originalCard.cardName + " for " + character.myName);

        int index = character.deck.IndexOf(originalCard);

        // Find the upgraded version
        CardData newTransformedCardData = null;
        CardData transformedCard = null;

        // Get valid transformable cards
        List<CardData> validTransforms = new List<CardData>();
        foreach(CardData c in GetCardsQuery(AllCards))
        {
            if(c.upgradeLevel == 0)
            {
                validTransforms.Add(c);
            }
        }

        // Chose random card to transform into
        validTransforms.Shuffle();
        newTransformedCardData = validTransforms[0];


        // Clone the data
        if (newTransformedCardData != null)
        {
            transformedCard = CloneCardDataFromCardData(newTransformedCardData);
        }

        // Did we succesfully find and clone the data?
        if (transformedCard != null)
        {
            // Add new upgraded card
            CharacterDataController.Instance.AddCardToCharacterDeck(character, transformedCard, index);

            // Remove old, unupgraded card
            CharacterDataController.Instance.RemoveCardFromCharacterDeck(character, originalCard);
        }

        return transformedCard;
    }
    private void HandleUpgradeCardForCharacterEntity(Card card)
    {
        Debug.Log("CardController.HandleUpgradeCardForCharacterEntity() called...");

        // Get handle to the card VM
        CardViewModel cvm = card.cardVM;
        CharacterEntityModel owner = card.owner;

        CardData upgradeData = FindUpgradedCardData(GetCardDataFromLibraryByName(card.cardName));
        Card upgradedCard = BuildCardFromCardData(upgradeData, owner);

        // Remove card from which ever collection its in, then add in the upgraded card
        if (owner.hand.Contains(card))
        {
            Debug.Log("HandleUpgradeCardForCharacterEntity() removing " + card.cardName + " from hand");
            RemoveCardFromHand(owner, card);
            AddCardToHand(owner, upgradedCard);
        }
        else if (owner.discardPile.Contains(card))
        {
            Debug.Log("HandleUpgradeCardForCharacterEntity() removing " + card.cardName + " from discard pile");
            RemoveCardFromDiscardPile(owner, card);
            AddCardToDiscardPile(owner, upgradedCard);
        }
        else if (owner.drawPile.Contains(card))
        {
            Debug.Log("HandleUpgradeCardForCharacterEntity() removing " + card.cardName + " from draw pile");
            RemoveCardFromDrawPile(owner, card);
            AddCardToDrawPile(owner, upgradedCard);
        }
        
        // does the card have a cardVM linked to it?
        if (cvm)
        {
            Debug.Log("HandleUpgradeCardForCharacterEntity() found a linked card view model, updating its view...");

            // Rebuild the old card's vm to the upgraded card.
            ConnectCardWithCardViewModel(upgradedCard, cvm);

            // Set up appearance, texts and sprites
            SetUpCardViewModelAppearanceFromCard(cvm, upgradedCard);

            // Set glow outline
            AutoUpdateCardGlowOutline(upgradedCard);

            VisualEventManager.Instance.CreateVisualEvent(() => PlayCardBreathAnimationVisualEvent(cvm));
        }

        //OnCardExpended(card);
    }
    #endregion

    // Visual Events
    #region
    public void AutoUpdateCardsInHandGlowOutlines(CharacterEntityModel character)
    {
        for (int i = 0; i < character.hand.Count; i++)
        {
            AutoUpdateCardGlowOutline(character.hand[i]);
        }
    }
    private void AutoUpdateCardGlowOutline(Card card)
    {
        if (card.cardVM != null)
        {
            if (IsCardPlayable(card, card.owner))
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
    private void CreateAndAddNewCardToCharacterHandVisualEvent(Card card, CharacterEntityModel character)
    {
        Debug.Log("CardController.CreateAndAddNewCardToCharacterHandVisualEvent() called...");
        CharacterEntityView characterView = character.characterEntityView;

        CardViewModel cvm;
        cvm = BuildCardViewModelFromCard(card, characterView.handVisual.NonDeckCardCreationTransform.position);

        // pass this card to HandVisual class
        characterView.handVisual.AddCard(cvm.movementParent.gameObject);

        // Bring card to front while it travels from draw spot to hand
        CardLocationTracker clt = cvm.locationTracker;
        clt.BringToFront();
        clt.Slot = 0;
        clt.VisualState = VisualStates.Transition;

        // Start SFX
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Draw);

        // Shrink card, then scale up as it moves to hand
        // Get starting scale
        Vector3 originalScale = new Vector3
            (cvm.movementParent.transform.localScale.x, cvm.movementParent.transform.localScale.y, cvm.movementParent.transform.localScale.z);

        // Shrink card
        cvm.movementParent.transform.localScale = new Vector3(0.1f, 0.1f, cvm.movementParent.transform.localScale.z);

        // Scale up
        ScaleCardViewModel(cvm, originalScale.x, cardTransistionSpeed);
        //cvm.mainParent.DOScale(originalScale, cardTransistionSpeed).SetEase(Ease.OutQuint);

        // Card glow
        AutoUpdateCardGlowOutline(card);

        // move card to the hand;
        MoveTransformToLocation(cvm.movementParent, characterView.handVisual.slots.Children[0].transform.localPosition, cardTransistionSpeed, true, () => clt.SetHandSortingOrder());
    }
    private void DrawCardFromDeckVisualEvent(Card card, CharacterEntityModel character)
    {
        Debug.Log("CardController.DrawCardFromDeckVisualEvent() called...");
        CharacterEntityView characterView = character.characterEntityView;

        CardViewModel cvm;        
        cvm = BuildCardViewModelFromCard(card, characterView.handVisual.DeckTransform.position);

        // pass this card to HandVisual class
        characterView.handVisual.AddCard(cvm.movementParent.gameObject);

        // Bring card to front while it travels from draw spot to hand
        CardLocationTracker clt = cvm.locationTracker;
        clt.BringToFront();
        clt.Slot = 0;
        clt.VisualState = VisualStates.Transition;

        // Glow outline
        AutoUpdateCardGlowOutline(card);

        // Start SFX
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Draw);

        // Get starting scale
        Vector3 originalScale = new Vector3
            (cvm.movementParent.transform.localScale.x, cvm.movementParent.transform.localScale.y, cvm.movementParent.transform.localScale.z);

        // Shrink card
        cvm.movementParent.transform.localScale = new Vector3(0.1f, 0.1f, cvm.movementParent.transform.localScale.z);

        // Scale up
        ScaleCardViewModel(cvm, originalScale.x, cardTransistionSpeed);

        // Move to hand slot
        MoveTransformToLocation(cvm.movementParent, characterView.handVisual.slots.Children[0].transform.localPosition, cardTransistionSpeed, true, () => clt.SetHandSortingOrder());
      
    }
    private void DiscardCardFromHandVisualEvent(CardViewModel cvm, CharacterEntityModel character)
    {
        StartCoroutine(DiscardCardFromHandVisualEventCoroutine(cvm, character));
    }
    private IEnumerator DiscardCardFromHandVisualEventCoroutine(CardViewModel cvm, CharacterEntityModel character)
    {
        // Setup 
        CharacterEntityView view = character.characterEntityView;

        // remove from hand visual
        character.characterEntityView.handVisual.RemoveCard(cvm.movementParent.gameObject);

        // SFX
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);

        // Create Glow Trail
        ToonEffect glowTrail = VisualEffectManager.Instance.CreateGreenGlowTrailEffect(cvm.movementParent.position);

        // Shrink card
        ScaleCardViewModel(cvm, 0.1f, 0.5f);

        // Rotate card upside down
        RotateCardVisualEvent(cvm, 180, 0.5f);

        // Move card + glow outline to quick lerp spot
        MoveTransformToQuickLerpPosition(cvm.movementParent, 0.25f);
        MoveTransformToQuickLerpPosition(glowTrail.transform, 0.25f);
        yield return new WaitForSeconds(0.25f);

        // Move card
        MoveTransformToLocation(cvm.movementParent, view.handVisual.DiscardPileTransform.position, 0.5f, false, () => DestroyCardViewModel(cvm));
        MoveTransformToLocation(glowTrail.transform, view.handVisual.DiscardPileTransform.position, 0.5f, false, () =>
        {
            glowTrail.StopAllEmissions();
            Destroy(glowTrail, 3);
        });
    }
    private void ExpendCardVisualEvent(CardViewModel cvm, CharacterEntityModel character)
    {
        if(cvm == null)
        {
            Debug.LogWarning("ExpendCardVisualEvent() was given a null card view model...");
        }

        // remove from hand visual
        character.characterEntityView.handVisual.RemoveCard(cvm.movementParent.gameObject);
        cvm.movementParent.SetParent(null);

        // SFX
        AudioManager.Instance.PlaySoundPooled(Sound.Explosion_Fire_1);

        // TO DO: fade out card canvas gradually
        FadeOutCardViewModel(cvm, null, ()=> DestroyCardViewModel(cvm));

        // Create smokey effect
        VisualEffectManager.Instance.CreateExpendEffect(cvm.movementParent.transform.position);
    }
  
    public void FadeOutCardViewModel(CardViewModel cvm, CoroutineData cData, Action onCompleteCallBack = null)
    {
        StartCoroutine(FadeOutCardViewModelCoroutine(cvm, cData, onCompleteCallBack));
    }
    private IEnumerator FadeOutCardViewModelCoroutine(CardViewModel cvm, CoroutineData cData, Action onCompleteCallBack)
    {
        float fadeSpeed = 1f;

        while(cvm.cg.alpha > 0)
        {
            cvm.cg.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        if(cData != null)
        {
            cData.MarkAsCompleted();
        }

        if (onCompleteCallBack != null)
        {
            onCompleteCallBack.Invoke();
        }
       
    }
    public void MoveCardVMToPlayPreviewSpot(CardViewModel cvm, HandVisual hv)
    {
        MoveTransformToLocation(cvm.movementParent, hv.PlayPreviewSpot.position, 0.25f);
    }
    private void PlayCardBreathAnimationVisualEvent(CardViewModel cvm, float animSpeed = 0.25f, float scaleModifier = 1.25f)
    {
        if (cvm != null)
        {
            float currentScale = cvm.movementParent.localScale.x;
            float endScale = currentScale * scaleModifier;

            Sequence s = DOTween.Sequence();

            s.Append(cvm.movementParent.DOScale(endScale, animSpeed).SetEase(Ease.OutQuint));
            s.OnComplete(() => cvm.movementParent.DOScale(currentScale, animSpeed).SetEase(Ease.OutQuint));
        }

        //StartCoroutine(PlayCardBreathAnimationVisualEventCoroutine(cvm));
    }
    /*
    private IEnumerator PlayCardBreathAnimationVisualEventCoroutine(CardViewModel cvm, float animSpeed = 0.25f, float scaleModifier = 1.5f)
    {
        if(cvm != null)
        {
            float currentScale = cvm.movementParent.localScale.x;
            float endScale = currentScale * scaleModifier;

            Sequence s = DOTween.Sequence();

            s.Append(cvm.movementParent.DOScale(endScale, animSpeed).SetEase(Ease.OutQuint));
            s.OnComplete(() => cvm.movementParent.DOScale(currentScale, animSpeed).SetEase(Ease.OutQuint));

            //cvm.movementParent.DOScale(endScale, animSpeed).SetEase(Ease.OutQuint);
            //yield return new WaitForSeconds(animSpeed);
            //cvm.movementParent.DOScale(currentScale, animSpeed).SetEase(Ease.OutQuint);
        }
        
    }
    */
    private void PlayACardFromHandVisualEvent(CardViewModel cvm, CharacterEntityView view)
    {
        Debug.Log("CardController.PlayACardFromHandVisualEvent() called...");
        StartCoroutine(PlayACardFromHandVisualEventCoroutine(cvm, view));
    }
    private IEnumerator PlayACardFromHandVisualEventCoroutine(CardViewModel cvm, CharacterEntityView view)
    {
        // Set state and remove from hand visual
        cvm.locationTracker.VisualState = VisualStates.Transition;
        view.handVisual.RemoveCard(cvm.movementParent.gameObject);
        cvm.movementParent.SetParent(null);

        // SFX
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);

        // Create Glow Trail
        ToonEffect glowTrail = VisualEffectManager.Instance.CreateGreenGlowTrailEffect(cvm.movementParent.position);

        // Shrink card
        ScaleCardViewModel(cvm, 0.1f, 0.5f);

        // Rotate card upside down
        RotateCardVisualEvent(cvm, 180, 0.5f);

        // Move card + glow outline to quick lerp spot
        MoveTransformToQuickLerpPosition(cvm.movementParent, 0.25f);
        MoveTransformToQuickLerpPosition(glowTrail.transform, 0.25f);
        yield return new WaitForSeconds(0.25f);

        // Move card
        MoveTransformToLocation(cvm.movementParent, view.handVisual.DiscardPileTransform.position, 0.5f, false, ()=> DestroyCardViewModel(cvm));
        MoveTransformToLocation(glowTrail.transform, view.handVisual.DiscardPileTransform.position, 0.5f, false, () =>
        {
            glowTrail.StopAllEmissions();
            Destroy(glowTrail, 3);
        });
    }   
    private void ShuffleDiscardPileIntoDrawPileVisualEvent()
    {
        CharacterEntityView view = ActivationManager.Instance.EntityActivated.characterEntityView;

        // Set path
        Vector3 pos1 = view.handVisual.PlayPreviewSpot.position;
        Vector3 pos2 = view.handVisual.DeckTransform.position;

        // Create Glow Trail at discard pile position
        ToonEffect glowTrail = VisualEffectManager.Instance.CreateGreenGlowTrailEffect(view.handVisual.DiscardPileTransform.position);

        // Move down path
        MoveTransformToLocation(glowTrail.transform, pos1, 0.35f, false, () =>
        {
            MoveTransformToLocation(glowTrail.transform, pos2, 0.35f, false, () =>
            {
                glowTrail.StopAllEmissions();
                Destroy(glowTrail, 3);
            });
        });
    }

    // Dotween Functions
    #region
    public void MoveTransformToLocation(Transform t, Vector3 location, float speed, bool localMove = false, Action onCompleteCallBack = null)
    {        
        Sequence cardSequence = DOTween.Sequence();

        if (localMove)
        {
            cardSequence.Append(t.DOLocalMove(location, speed));
        }
        else
        {
            cardSequence.Append(t.DOMove(location, speed));
        }

        cardSequence.OnComplete(() =>
        {
            if (onCompleteCallBack != null)
            {
                onCompleteCallBack.Invoke();
            }
        });
    }
    private void MoveTransformDownPath(Transform t, Vector3[] locations, float speed, bool localMove = false, Action onCompleteCallBack = null)
    {
        Sequence cardSequence = DOTween.Sequence();

        cardSequence.Append(t.DOPath(locations, speed));

        if (localMove)
        {
            cardSequence.Append(t.DOLocalPath(locations, speed, PathType.CatmullRom));
        }
        else
        {
            cardSequence.Append(t.DOPath(locations, speed));
        }

        cardSequence.OnComplete(() =>
        {
            if (onCompleteCallBack != null)
            {
                onCompleteCallBack.Invoke();
            }
        });
    }
    public void RotateCardVisualEvent(CardViewModel cvm, float endDegrees, float rotationSpeed)
    {
        // Rotate card upside down
        Vector3 endRotation = new Vector3(0, 0, endDegrees);
        cvm.movementParent.DORotate(endRotation, rotationSpeed);
    }
    public void MoveTransformToQuickLerpPosition(Transform t, float speed)
    {
        Vector3 quickLerpSpot = new Vector3(t.position.x - 1, t.position.y + 1, t.position.z);
        t.DOMove(quickLerpSpot, speed);
    }
    public void ScaleCardViewModel(CardViewModel cvm, float endScale, float scaleSpeed)
    {
        cvm.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
    }
    #endregion

    // Text Related Visual Events
    #region
    private void UpdateDiscardPileCountText(CharacterEntityView vm, string newValue)
    {
        if (vm)
        {
            vm.discardPileCountText.text = newValue;
        }
    }
    private void UpdateDrawPileCountText(CharacterEntityView vm, string newValue)
    {
        if (vm)
        {
            vm.drawPileCountText.text = newValue;
        }
    }
    #endregion

    #endregion

}

