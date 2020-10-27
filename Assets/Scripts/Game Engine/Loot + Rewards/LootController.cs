using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LootController : Singleton<LootController>
{
    // Properties + Component References
    #region
    [Header("Properties")]
    [SerializeField] private LootResultModel currentLootResultData;
    [SerializeField] private CharacterData currentCharacterSelection;

    [Header("Component References")]
    [SerializeField] private GameObject visualParent;
    [SerializeField] private CanvasGroup visualParentCg;
    [SerializeField] private GameObject frontPageParent;
    [SerializeField] private CanvasGroup frontPageCg;
    [SerializeField] private GameObject chooseCardScreenParent;
    [SerializeField] private CanvasGroup chooseCardScreenCg;
    [SerializeField] private LootTab[] cardLootTabs;
    [SerializeField] private LootScreenCardViewModel[] lootCardViewModels;

    [Header("Character Deck Quick View References")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private CardInfoPanel[] cardPanels;
    [SerializeField] private CardViewModel previewCardVM;
    [SerializeField] private CanvasGroup previewCardCg;

    public LootResultModel CurrentLootResultData
    {
        get { return currentLootResultData; }
        private set { currentLootResultData = value; }
    }
    public bool LootScreenIsActive()
    {
        return visualParent.activeSelf;
    }
    #endregion

    // Build Views
    #region
    public void CloseAndResetAllViews()
    {
        HideChooseCardScreen();
        HideFrontPageView();
        HideMainLootView();

        foreach(LootTab lt in cardLootTabs)
        {
            HideCardLootTab(lt);
        }

    }
    public void BuildLootScreenElementsFromLootResultData()
    {
        // Enable Choose card buttons
        for (int i = 0; i < CharacterDataController.Instance.AllPlayerCharacters.Count; i++)
        {
            ShowCardLootTab(cardLootTabs[i]);
            cardLootTabs[i].descriptionText.text = "Gain new card: " + 
                TextLogic.ReturnColoredText(CharacterDataController.Instance.AllPlayerCharacters[i].myName, TextLogic.neutralYellow);
        }
    }
    public void BuildChooseCardScreenCardsFromData(List<CardData> cardData)
    {
        for (int i = 0; i < cardData.Count; i++)
        {
            // Build card views
            CardController.Instance.BuildCardViewModelFromCardData(cardData[i], lootCardViewModels[i].cardViewModel);

            // Cache the card data
            lootCardViewModels[i].myDataRef = cardData[i];
        }
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
            // Build new tab from card data
            cardPanels[i].gameObject.SetActive(true);
            cardPanels[i].BuildCardInfoPanelFromCardData(data[i]);

            // Hide text count
            cardPanels[i].copiesCountText.gameObject.SetActive(false);
        }
    }
    public void BuildAndShowCardViewModelPopup(CardData data)
    {
        previewCardCg.gameObject.SetActive(true);
        CardData cData = CardController.Instance.GetCardDataFromLibraryByName(data.cardName);
        CardController.Instance.BuildCardViewModelFromCardData(cData, previewCardVM);
        previewCardCg.alpha = 0;
        previewCardCg.DOFade(1f, 0.25f);
    }
    public void HidePreviewCard()
    {
        previewCardCg.gameObject.SetActive(false);
        previewCardCg.alpha = 0;
    }
    #endregion

    // Show, Hide And Transisition Screens
    #region
    public void ShowMainLootView()
    {
        visualParentCg.alpha = 0;
        visualParent.SetActive(true);
        visualParentCg.DOFade(1f, 0.35f);
    }
    public void HideMainLootView()
    {
        visualParent.SetActive(false);
    }
    public void ShowFrontPageView()
    {
        frontPageCg.alpha = 0;
        frontPageParent.SetActive(true);
        frontPageCg.DOFade(1f, 0.35f);
    }
    public void HideFrontPageView()
    {
        frontPageParent.SetActive(false);
    }
    public void ShowChooseCardScreen()
    {
        chooseCardScreenCg.alpha = 0;
        chooseCardScreenParent.SetActive(true);
        chooseCardScreenCg.DOFade(1f, 0.35f);
    }
    public void HideChooseCardScreen()
    {
        chooseCardScreenParent.SetActive(false);

        // Reset card scales
        foreach(LootScreenCardViewModel card in lootCardViewModels)
        {
            card.ResetSelfOnEventComplete();
        }
    }
    public void ShowCardLootTab(LootTab tab)
    {
        tab.gameObject.SetActive(true);
    }
    public void HideCardLootTab(LootTab tab)
    {
        tab.gameObject.SetActive(false);
    }
    #endregion

    // Generate Loot Result Logic
    #region
    public void SetAndCacheNewLootResult()
    {
        CurrentLootResultData = GenerateNewLootResult();
    }
    public LootResultModel GenerateNewLootResult()
    {
        LootResultModel newLoot = new LootResultModel();

        for(int i = 0; i < CharacterDataController.Instance.AllPlayerCharacters.Count; i++)
        {
            newLoot.allCharacterCardChoices.Add(new List<CardData>());
            newLoot.allCharacterCardChoices[i] = GenerateCharacterCardLootChoices(CharacterDataController.Instance.AllPlayerCharacters[i]);
        }


        return newLoot;
    }
    #endregion

    // Get Lootable Cards Logic
    #region
    private List<CardData> GenerateCharacterCardLootChoices(CharacterData character)
    {
        // Set up + get all valid lootable cards
        List<CardData> listReturned = new List<CardData>();
        List<CardData> validLootableCards = GetAllValidLootableCardsForCharacter(character);

        // Seprate common, rare and epic cards
        List<CardData> commonCards = new List<CardData>();
        List<CardData> rareCards = new List<CardData>();
        List<CardData> epicCards = new List<CardData>();

        for(int i = 0; i < validLootableCards.Count; i++)
        {
            if(validLootableCards[i].rarity == Rarity.Common)
            {
                commonCards.Add(validLootableCards[i]);
            }
            else if (validLootableCards[i].rarity == Rarity.Rare)
            {
                rareCards.Add(validLootableCards[i]);
            }
            else if (validLootableCards[i].rarity == Rarity.Epic)
            {
                epicCards.Add(validLootableCards[i]);
            }
        }

        // Get 3 random + different cards 
        for (int i = 0; i < 3; i++)
        {
            // Roll for rarity (5% Epic, 15% Rare, 80% Common probabilities)
            int roll = RandomGenerator.NumberBetween(1, 100);

            // Epic
            if(epicCards.Count > 0 && IsNumberBetweenValues(roll, GlobalSettings.Instance.epicCardLowerLimitProbability, GlobalSettings.Instance.epicCardUpperLimitProbability))
            {
                epicCards.Shuffle();
                listReturned.Add(epicCards[0]);
                epicCards.Remove(epicCards[0]);
            }

            // Rare
            else if (rareCards.Count > 0 && IsNumberBetweenValues(roll, GlobalSettings.Instance.rareCardLowerLimitProbability, GlobalSettings.Instance.rareCardUpperLimitProbability))
            {
                rareCards.Shuffle();
                listReturned.Add(rareCards[0]);
                rareCards.Remove(rareCards[0]);
            }

            // Common
            else
            {
                commonCards.Shuffle();
                listReturned.Add(commonCards[0]);
                commonCards.Remove(commonCards[0]);
            }
        }

        return listReturned;
    }
    private List<CardData> GetAllValidLootableCardsForCharacter(CharacterData character)
    {
        List<CardData> validLootableCards = new List<CardData>();

        // Get a list of all possible lootable cards
        foreach (CardData card in CardController.Instance.AllCards)
        {
            if (IsCardLootableByCharacter(character, card))
            {
                validLootableCards.Add(card);
            }
        }

        return validLootableCards;
    }
    private bool IsCardLootableByCharacter(CharacterData character, CardData card)
    {
        bool bReturned = false;

        foreach(TalentPairingModel tp in character.talentPairings)
        {
            // Does the character have the required talent school?
            if((tp.talentSchool == card.talentSchool && card.rarity == Rarity.Common) ||
                (tp.talentSchool == card.talentSchool && tp.talentLevel == 2 && (card.rarity == Rarity.Rare || card.rarity == Rarity.Epic)))
            {
                bReturned = true;
                break;
            }
        }

        return bReturned;
    }
    #endregion

    // On Click Events
    #region
    public void OnLootTabButtonClicked(LootTab buttonClicked)
    {        
        if(buttonClicked.TabType == LootTabType.CardReward)
        {
            // Get the card reward buttons index for using later
            int index = 0;
            for (int i = 0; i < cardLootTabs.Length; i++)
            {
                if (cardLootTabs[i] == buttonClicked)
                {
                    index = i;
                    break;
                }
            }

            // Get the predetermined card loot result for the character
            List<CardData> cardChoices = CurrentLootResultData.allCharacterCardChoices[index];

            // Cache the character, so we know which character to reward a card to if player chooses one
            currentCharacterSelection = CharacterDataController.Instance.AllPlayerCharacters[index];
            characterNameText.text = currentCharacterSelection.myName;

            // Build choose card view models
            BuildChooseCardScreenCardsFromData(cardChoices);
            BuildCardInfoPanels(currentCharacterSelection.deck);

            // Hide front screen, show choose card screen
            HideFrontPageView();
            ShowChooseCardScreen();
        }
        

    }
    public void OnLootCardViewModelClicked(LootScreenCardViewModel cardClicked)
    {
        // Add card to character deck
        CharacterDataController.Instance.AddCardToCharacterDeck(currentCharacterSelection, cardClicked.myDataRef);

        // Close choose card window,  reopen front screen
        HideChooseCardScreen();
        ShowFrontPageView();

        // TO DO: find a better way to find the matching card tab
        // hide add card to deck button
        HideCardLootTab(cardLootTabs[CharacterDataController.Instance.AllPlayerCharacters.IndexOf(currentCharacterSelection)]);


    }
    public void OnChooseCardScreenBackButtonClicked()
    {
        HideChooseCardScreen();
        ShowFrontPageView();
    }
    public void OnContinueButtonClicked()
    {
        if (VisualEventManager.Instance.EventQueue.Count == 0)
        {
            CloseAndResetAllViews();
            EventSequenceController.Instance.HandleLoadNextEncounter();
        }
    }

    #endregion

    // Misc Roll Stuff
    #region
    public bool IsNumberBetweenValues(int number, int lowerLimit, int higherLimit)
    {
        if(number >= lowerLimit && number <= higherLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
