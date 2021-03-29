using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spriter2UnityDX;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using MapSystem;

public class ShopController : Singleton<ShopController>
{
    // Properties + Components
    #region
    [Header("Properties")]
    private ShopContentResultModel currentShopContentResultData;
    private bool shopIsInteractable = false;
    private bool continueButtonIsInteractable = false;

    [Header("Main View Components")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private GameObject mainVisualParent;
    [SerializeField] private CanvasGroup mainCg;
    [SerializeField] private ScrollRect mainPanelScrollRect;

    [Header("Character View References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CampSiteCharacterView[] allShopCharacterViews;
    [SerializeField] private GameObject charactersViewParent;

    [Header("Nodes + Location References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CampSiteNode[] shopNodes;
    [SerializeField] private Transform offScreenStartPosition;

    [Header("Merchant Properties + Components")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private UniversalCharacterModel merchantUcm;
    [SerializeField] private List<string> merchantBodyParts;
    [SerializeField] private Image[] merchantAccesoryImages;
    [SerializeField] private Color merchantAccessoriesNormalColour;
    [SerializeField] private Color merchantNormalColour;
    [SerializeField] private Color merchantHightlightColour;

    [Header("Shop Card Components")]
    [SerializeField] private GameObject cardRowThreeParent;
    [SerializeField] private ShopCardBox[] allShopCards;
    [SerializeField] private ItemCardBox[] allShopItems;

    [Header("Continue Button Components")]
    [SerializeField] private GameObject continueButtonVisualParent;
    [SerializeField] private CanvasGroup continueButtonCg;

    [Header("Speech Bubble Components")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CanvasGroup bubbleCg;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private string[] bubbleGreetings;
    [SerializeField] private string[] bubbleFarewells;
    #endregion

    // Getters
    #region
    public ShopContentResultModel CurrentShopContentResultData
    {
        get { return currentShopContentResultData; }
        private set { currentShopContentResultData = value; }
    }
    public CanvasGroup MainCg
    {
        get { return mainCg; }
        private set { mainCg = value; }
    }
    #endregion

    // Show + Hide Views
    #region
    private void ShowMainPanelView()
    {
        mainPanelScrollRect.verticalScrollbar.value = 1;
        mainCg.DOKill();
        mainCg.alpha = 0;
        mainVisualParent.SetActive(true);      
        mainCg.DOFade(1f, 0.25f);
    }
    private void HideMainPanelView()
    {
      
        mainCg.DOKill();
        mainVisualParent.SetActive(true);
        Sequence s = DOTween.Sequence();
        s.Append(mainCg.DOFade(0, 0.25f));
        s.OnComplete(() => { 
            mainVisualParent.SetActive(false);
            mainPanelScrollRect.verticalScrollbar.value = 1;
        });
    }
    public void EnableCharacterViewParent()
    {
        charactersViewParent.SetActive(true);
    }
    public void DisableCharacterViewParent()
    {
        charactersViewParent.SetActive(false);
    }
    public void ShowShopCardBox(ShopCardBox box)
    {
        box.visualParent.SetActive(true);
    }
    public void ShowShopItemBox(ItemCardBox box)
    {
        box.visualParent.SetActive(true);
    }
    public void HideShopCardBox(ShopCardBox box)
    {
        box.onSaleVisualParent.SetActive(false);
        box.visualParent.SetActive(false);
    }
    public void HideItemCardBox(ItemCardBox box)
    {
        box.onSaleVisualParent.SetActive(false);
        box.visualParent.SetActive(false);
    }
    private void HideAllShopCardBoxes()
    {
        foreach(ShopCardBox scb in allShopCards)
        {
            HideShopCardBox(scb);
        }
    }
    private void HideAllShopItemBoxes()
    {
        foreach (ItemCardBox icb in allShopItems)
        {
            HideItemCardBox(icb);
        }
    }
    #endregion

    // Save + Load Logic
    #region
    public void SaveMyDataToSaveFile(SaveGameData saveFile)
    {
        saveFile.shopData = CurrentShopContentResultData;
    }
    public void BuildMyDataFromSaveFile(SaveGameData saveFile)
    {
        CurrentShopContentResultData = saveFile.shopData;
    }
    
    #endregion

    // Build Views
    #region
    public void BuildAllShopCharacterViews(List<CharacterData> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            BuildShopCharacterView(allShopCharacterViews[i], characters[i]);
        }

        CharacterModelController.Instance.BuildModelFromStringReferences(merchantUcm, merchantBodyParts);
    }
    private void BuildShopCharacterView(CampSiteCharacterView view, CharacterData data)
    {
        Debug.LogWarning("CampSiteController.BuildShopCharacterView() called, character data: " + data.myName);

        // Connect data to view
        view.myCharacterData = data;

        // Enable shadow
        view.ucmShadowParent.SetActive(true);

        // Build UCM
        CharacterModelController.Instance.BuildModelFromStringReferences(view.characterEntityView.ucm, data.modelParts);
        CharacterModelController.Instance.ApplyItemManagerDataToCharacterModelView(data.itemManager, view.characterEntityView.ucm);
    }
    public void BuildAllShopContentFromDataSet(ShopContentResultModel dataSet)
    {
        // Reset 
        cardRowThreeParent.SetActive(false);
        HideAllShopCardBoxes();
        HideAllShopItemBoxes();

        // Build cards
        BuildAllShopCardBoxesFromDataSet(dataSet);

        // Build Items
        BuildAllShopItemBoxesFromDataSet(dataSet);
    }
    private void BuildAllShopCardBoxesFromDataSet(ShopContentResultModel dataSet)
    {
        for(int i = 0; i < dataSet.cardsData.Count; i++)
        {
            BuildShopCardBox(allShopCards[i], dataSet.cardsData[i]);
        }
        if(dataSet.cardsData.Count > 10)
            cardRowThreeParent.SetActive(true);
    }
    private void BuildAllShopItemBoxesFromDataSet(ShopContentResultModel dataSet)
    {
        for (int i = 0; i < dataSet.itemsData.Count; i++)
        {
            BuildShopItemBox(allShopItems[i], dataSet.itemsData[i]);
        }
    }
    private void BuildShopCardBox(ShopCardBox box, CardPricePairing data)
    {
        ShowShopCardBox(box);
        box.cppData = data;
        AutoColourShopCardCostText(box);
        CardController.Instance.BuildCardViewModelFromCardData(data.cardData, box.cvm);

        if (data.onSale)
        {
            // enable on sale views
            box.onSaleVisualParent.SetActive(true);
        }
    }
    private void BuildShopItemBox(ItemCardBox box, ItemPricePairing data)
    {
        ShowShopItemBox(box);
        box.ippData = data;
        AutoColourShopItemCostText(box);
        CardController.Instance.BuildCardViewModelFromItemData(data.itemData, box.cvm);

        if (data.onSale)
        {
            // enable on sale views
            box.onSaleVisualParent.SetActive(true);
        }
    }
    private void AutoColourShopCardCostText(ShopCardBox box)
    {
        if(box.cppData.goldCost > PlayerDataManager.Instance.CurrentGold)        
            box.goldCostText.text = "<color=#E52B2B>" + box.cppData.goldCost.ToString();
        
        else if (box.cppData.goldCost <= PlayerDataManager.Instance.CurrentGold && box.cppData.onSale)        
            box.goldCostText.text = "<color=#51C3FF>" + box.cppData.goldCost.ToString();
        
        else        
            box.goldCostText.text = "<color=#FFFFFF>" + box.cppData.goldCost.ToString();        
    }
    private void AutoColourShopItemCostText(ItemCardBox box)
    {
        if (box.ippData.goldCost > PlayerDataManager.Instance.CurrentGold)        
            box.goldCostText.text = "<color=#E52B2B>" + box.ippData.goldCost.ToString();
        
        else if (box.ippData.goldCost <= PlayerDataManager.Instance.CurrentGold && box.ippData.onSale)        
            box.goldCostText.text = "<color=#51C3FF>" + box.ippData.goldCost.ToString();
        
        else        
            box.goldCostText.text = "<color=#FFFFFF>" + box.ippData.goldCost.ToString();
        
    }
    public void SetShopKeeperInteractionState(bool onOrOff)
    {
        shopIsInteractable = onOrOff;
    }
    public void SetContinueButtonInteractionState(bool onOrOff)
    {
        continueButtonIsInteractable = onOrOff;
    }
    #endregion

    // Generate cards and content
    #region
    public void SetAndCacheNewShopContentDataSet()
    {
        CurrentShopContentResultData = GenerateNewShopContentDataSet();
    }
    public void ClearShopContentDataSet()
    {
        CurrentShopContentResultData = null;
    }
    private ShopContentResultModel GenerateNewShopContentDataSet()
    {
        ShopContentResultModel scr = new ShopContentResultModel();

        // Generate cards
        scr.cardsData = GenerateShopCardDataSet();

        // Generate items
        scr.itemsData = GenerateShopItemDataSet();

        return scr;

    }
    private List<CardPricePairing> GenerateShopCardDataSet()
    {
        List<CardPricePairing> listReturned = new List<CardPricePairing>();

        // Get ALL valid cards 
        List<CardData> allValidLootableCards = new List<CardData>();
        List<CardData> validCommons = new List<CardData>();
        List<CardData> validRares = new List<CardData>();
        List<CardData> validEpics = new List<CardData>();

        List<CardData> chosenCards = new List<CardData>();

        // Get all valid lootable cards for every character the player has
        foreach (CharacterData character in CharacterDataController.Instance.AllPlayerCharacters)
        {
            foreach (CardData cd in LootController.Instance.GetAllValidLootableCardsForCharacter(character))
            {
                // Prevent doubling up on cards
                if (allValidLootableCards.Contains(cd) == false)
                    allValidLootableCards.Add(cd);
            }
        }

        // Divide cards by rarity
        foreach (CardData cd in allValidLootableCards)
        {
            if (cd.rarity == Rarity.Common)
                validCommons.Add(cd);

            else if (cd.rarity == Rarity.Rare)
                validRares.Add(cd);

            else if (cd.rarity == Rarity.Epic)
                validEpics.Add(cd);
        }

        int commons = 5;
        int rares = 3;
        int epics = 2;

        if (CharacterDataController.Instance.AllPlayerCharacters.Count > 1)
        {
            commons = 8;
            rares = 4;
            epics = 3;
        }

        // Randomly pick 5 commons, 3 rares and 2 epics
        for (int i = 0; i < commons; i++)
        {
            validCommons.Shuffle();
            chosenCards.Add(validCommons[0]);
            validCommons.Remove(validCommons[0]);
        }
        for (int i = 0; i < rares; i++)
        {
            validRares.Shuffle();
            chosenCards.Add(validRares[0]);
            validRares.Remove(validRares[0]);
        }
        for (int i = 0; i < epics; i++)
        {
            validEpics.Shuffle();
            chosenCards.Add(validEpics[0]);
            validEpics.Remove(validEpics[0]);
        }

        // Create new pairings and randomized prices
        foreach (CardData card in chosenCards)
        {
            listReturned.Add(new CardPricePairing(card));
        }

        // Put a random card on sale
        CardPricePairing randomCard = listReturned[RandomGenerator.NumberBetween(0, listReturned.Count - 1)];
        randomCard.onSale = true;
        randomCard.goldCost = randomCard.goldCost / 2;

        return listReturned;
    }
    private List<ItemPricePairing> GenerateShopItemDataSet()
    {
        List<ItemPricePairing> listReturned = new List<ItemPricePairing>();

        // Get ALL valid cards 
        List<ItemData> allValidLootableItems = ItemController.Instance.GetAllLootableItems();
        List<ItemData> validCommons = new List<ItemData>();
        List<ItemData> validRares = new List<ItemData>();
        List<ItemData> validEpics = new List<ItemData>();

        List<ItemData> chosenItems = new List<ItemData>();

        // Divide cards by rarity
        foreach (ItemData i in allValidLootableItems)
        {
            if (i.itemRarity == Rarity.Common)
                validCommons.Add(i);

            else if (i.itemRarity == Rarity.Rare)
                validRares.Add(i);

            else if (i.itemRarity == Rarity.Epic)
                validEpics.Add(i);
        }

        int commons = 2;
        int rares = 2;
        int epics = 1;


        // Randomly pick 2 commons, 2 rares and 1 epic
        for (int i = 0; i < commons; i++)
        {
            validCommons.Shuffle();
            chosenItems.Add(validCommons[0]);
            validCommons.Remove(validCommons[0]);
        }
        for (int i = 0; i < rares; i++)
        {
            validRares.Shuffle();
            chosenItems.Add(validRares[0]);
            validRares.Remove(validRares[0]);
        }
        for (int i = 0; i < epics; i++)
        {
            validEpics.Shuffle();
            chosenItems.Add(validEpics[0]);
            validEpics.Remove(validEpics[0]);
        }

        // Create new pairings and randomized prices
        foreach (ItemData i in chosenItems)
        {
            listReturned.Add(new ItemPricePairing(i));
        }

        // Put a random card on sale
        ItemPricePairing randomItem = listReturned[RandomGenerator.NumberBetween(0, listReturned.Count - 1)];
        randomItem.onSale = true;
        randomItem.goldCost = randomItem.goldCost / 2;

        return listReturned;
    }
    #endregion

    // Handle Buy Stuff Logic
    #region
    public void OnShopCardBoxClicked(ShopCardBox box)
    {
        if(PlayerDataManager.Instance.CurrentGold >= box.cppData.goldCost)
        {
            KeyWordLayoutController.Instance.FadeOutMainView();

            // Pay gold price
            PlayerDataManager.Instance.ModifyCurrentGold(-box.cppData.goldCost, true);           

            // Cha ching SFX + coin explosion VFX
            AudioManager.Instance.PlaySoundPooled(Sound.Gold_Cha_Ching);
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateGoldCoinExplosion(box.transform.position, 10000, 2));

            // Sparkle glow trail towards inventory
            LootController.Instance.CreateGreenGlowTrailEffect(box.transform.position, TopBarController.Instance.CharacterRosterButton.transform.position);

            // Hide card box
            HideShopCardBox(box);

            // add card to player card inventory
            InventoryController.Instance.AddCardToInventory(CardController.Instance.CloneCardDataFromCardData(box.cppData.cardData));

            // Update shop card box texts on gold value changed
            UpdateGoldTextColouringsOnGoldValueChanged();
        }        
    }
    public void OnShopItemBoxClicked(ItemCardBox box)
    {
        if (PlayerDataManager.Instance.CurrentGold >= box.ippData.goldCost)
        {
            KeyWordLayoutController.Instance.FadeOutMainView();

            // Pay gold price
            PlayerDataManager.Instance.ModifyCurrentGold(-box.ippData.goldCost, true);

            // Cha ching SFX + coin explosion VFX
            AudioManager.Instance.PlaySoundPooled(Sound.Gold_Cha_Ching);
            VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateGoldCoinExplosion(box.transform.position, 10000, 2));

            // Sparkle glow trail towards inventory
            LootController.Instance.CreateGreenGlowTrailEffect(box.transform.position, TopBarController.Instance.CharacterRosterButton.transform.position);

            // Hide card box
            HideItemCardBox(box);

            // add item to inventory
            InventoryController.Instance.AddItemToInventory(box.ippData.itemData);

            // Update gold texts
            UpdateGoldTextColouringsOnGoldValueChanged();
        }
    }
    private void UpdateGoldTextColouringsOnGoldValueChanged()
    {
        // Update shop card box texts on gold value changed
        foreach (ShopCardBox scb in allShopCards)
        {
            if (scb.cppData != null)
                AutoColourShopCardCostText(scb);
        }

        // Update shop item box texts on gold value changed
        foreach (ItemCardBox icb in allShopItems)
        {
            if (icb.ippData != null)
                AutoColourShopItemCostText(icb);
        }
    }
    #endregion

    // On Button Click Listeners
    #region
    public void OnMerchantCharacterClicked()
    {
        Debug.LogWarning("OnMerchantCharacterClicked()");
        if (mainVisualParent.activeSelf == false && shopIsInteractable)
        {
            SetMerchantColor(merchantUcm.GetComponent<EntityRenderer>(), merchantNormalColour);
            SetMerchantAccessoriesColor(merchantNormalColour);
            AudioManager.Instance.PlaySound(Sound.GUI_Button_Clicked);
            ShowMainPanelView();
        }
    }
    public void OnMerchantCharacterMouseEnter()
    {
        if (mainVisualParent.activeSelf == false && shopIsInteractable)
        {
            AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);
            SetMerchantColor(merchantUcm.GetComponent<EntityRenderer>(), merchantHightlightColour);
            SetMerchantAccessoriesColor(merchantHightlightColour);
        }
    }
    public void OnMerchantCharacterMouseExit()
    {
        if (mainVisualParent.activeSelf == false && shopIsInteractable)
        {
            SetMerchantColor(merchantUcm.GetComponent<EntityRenderer>(), merchantNormalColour);
            SetMerchantAccessoriesColor(merchantAccessoriesNormalColour);
        }
    }
    public void OnMainPanelBackButtonClicked()
    {
        HideMainPanelView();
    }
    #endregion

    // Colouring
    #region
    public void SetMerchantColor(EntityRenderer view, Color newColor)
    {
        if (view != null)
        {
            view.Color = new Color(newColor.r, newColor.g, newColor.b, view.Color.a);
        }
    }
    public void SetMerchantAccessoriesColor(Color newColor)
    {
        foreach (Image i in merchantAccesoryImages)
        {
            i.DOKill();
            i.DOColor(newColor, 0.2f);
        }
    }
    #endregion

    // Character Movement Events
    #region
    public void MoveAllCharactersToStartPosition()
    {
        for (int i = 0; i < allShopCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allShopCharacterViews[i];

            // Cancel if data ref is null (happens if player has less then 3 characters
            if (character.myCharacterData == null)
            {
                return;
            }

            character.characterEntityView.ucmMovementParent.transform.position = offScreenStartPosition.position;
        }
    }
    public void MoveAllCharactersToTheirNodes()
    {
        StartCoroutine(MoveAllCharactersToTheirNodesCoroutine());
    }
    private IEnumerator MoveAllCharactersToTheirNodesCoroutine()
    {
        for (int i = 0; i < allShopCharacterViews.Length; i++)
        {
            CampSiteCharacterView character = allShopCharacterViews[i];

            // Normal move to node
            if (character.myCharacterData != null && character.myCharacterData.health > 0)
            {
                // replace this with new move function in future, this function will make characters run through the camp fire
                CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, shopNodes[i].LevelNode, null);
            }

            yield return new WaitForSeconds(0.25f);
        }

    }
    public void MoveCharactersToOffScreenRight()
    {
        StartCoroutine(MoveCharactersToOffScreenRightCoroutine());
    }
    private IEnumerator MoveCharactersToOffScreenRightCoroutine()
    {
        foreach (CampSiteCharacterView character in allShopCharacterViews)
        {
            // Only move existing living characters
            if (character.myCharacterData != null &&
                character.myCharacterData.health > 0)
            {
                CharacterEntityController.Instance.MoveEntityToNodeCentre(character.characterEntityView, LevelManager.Instance.EnemyOffScreenNode, null);
            }
        }

        yield return new WaitForSeconds(3f);
    }
    #endregion

    // Greeting Visual Logic
    #region
    public void DoMerchantGreeting()
    {
        StartCoroutine(DoMerchantGreetingCoroutine());
    }
    private IEnumerator DoMerchantGreetingCoroutine()
    {
        bubbleCg.DOKill();
        bubbleText.text = bubbleGreetings[RandomGenerator.NumberBetween(0, bubbleGreetings.Length - 1)];
        bubbleCg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(3.5f);
        bubbleCg.DOFade(0, 0.5f);
    }
    public void DoMerchantFarewell()
    {
        StartCoroutine(DoMerchantFarewellCoroutine());
    }
    private IEnumerator DoMerchantFarewellCoroutine()
    {
        bubbleCg.DOKill();
        bubbleText.text = bubbleFarewells[RandomGenerator.NumberBetween(0, bubbleFarewells.Length - 1)];
        bubbleCg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1.75f);
        bubbleCg.DOFade(0, 0.5f);
    }
    #endregion

    // Continue Button Logic
    #region
    public void OnContinueButtonClicked()
    {
        if (continueButtonIsInteractable)
        {
            MapPlayerTracker.Instance.UnlockMap();
            MapView.Instance.OnWorldMapButtonClicked();
        }
    }

    public void ShowContinueButton()
    {
        continueButtonCg.DOKill();
        continueButtonCg.alpha = 0;
        continueButtonVisualParent.SetActive(true);
        continueButtonCg.DOFade(1, 0.5f);
    }
    public void HideContinueButton()
    {
        continueButtonCg.DOKill();
        continueButtonCg.alpha = 1;
        continueButtonVisualParent.SetActive(true);

        Sequence s = DOTween.Sequence();
        s.Append(continueButtonCg.DOFade(0, 0.5f));
        s.OnComplete(() => { continueButtonVisualParent.SetActive(false); });
    }

    #endregion
}
