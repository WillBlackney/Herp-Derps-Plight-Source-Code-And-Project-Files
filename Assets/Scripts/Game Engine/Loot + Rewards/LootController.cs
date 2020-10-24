using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LootController : Singleton<LootController>
{
    // Properties + Component References
    #region
    [Header("Properties")]
    [SerializeField] private LootResultModel currentLootResultData;
    [SerializeField] private CharacterData currentCharacterSelection;

    [Header("Component References")]
    [SerializeField] private GameObject visualParent;
    [SerializeField] private GameObject frontPageParent;
    [SerializeField] private GameObject chooseCardScreenParent;
    [SerializeField] private LootTab[] cardLootTabs;
    [SerializeField] private LootScreenCardViewModel[] lootCardViewModels;

    public LootResultModel CurrentLootResultData
    {
        get { return currentLootResultData; }
        private set { currentLootResultData = value; }
    }
    #endregion

    // Build Views
    #region
    public void BuildLootScreenElementsFromLootResultData()
    {
        // Enable Choose card buttons
        for (int i = 0; i < CharacterDataController.Instance.allPlayerCharacters.Count; i++)
        {
            ShowCardLootTab(cardLootTabs[i]);
            cardLootTabs[i].descriptionText.text = "Gain new card: " + 
                TextLogic.ReturnColoredText(CharacterDataController.Instance.allPlayerCharacters[i].myName, TextLogic.neutralYellow);
        }
    }
    public void BuildChooseCardScreenCardsFromData(List<CardData> cardData)
    {
        for (int i = 0; i < cardData.Count; i++)
        {
            CardController.Instance.BuildCardViewModelFromCardData(cardData[i], lootCardViewModels[i].cardViewModel);
        }
    }
    #endregion

    // Show, Hide And Transisition Screens
    #region
    public void ShowMainLootView()
    {
        visualParent.SetActive(true);
    }
    public void ShowFrontPageView()
    {
        frontPageParent.SetActive(true);
    }
    public void HideFrontPageView()
    {
        frontPageParent.SetActive(false);
    }
    public void ShowChooseCardScreen()
    {
        chooseCardScreenParent.SetActive(true);
    }
    public void HideChooseCardScreen()
    {
        chooseCardScreenParent.SetActive(false);
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
        currentLootResultData = GenerateNewLootResult();
    }
    public LootResultModel GenerateNewLootResult()
    {
        LootResultModel newLoot = new LootResultModel();
        //newLoot.allCharacterCardChoices = new List<List<CardData>>();

        for(int i = 0; i < CharacterDataController.Instance.allPlayerCharacters.Count; i++)
        {
            Debug.LogWarning("Character count: " + CharacterDataController.Instance.allPlayerCharacters.Count.ToString());
            Debug.LogWarning("allCharacterCardChoices count: " + newLoot.allCharacterCardChoices.Count.ToString());
            newLoot.allCharacterCardChoices.Add(new List<CardData>());
            newLoot.allCharacterCardChoices[i] = GenerateCharacterCardLootChoices(CharacterDataController.Instance.allPlayerCharacters[i]);
        }


        return newLoot;
    }
    #endregion

    // Get Lootable Cards Logic
    #region
    private List<CardData> GenerateCharacterCardLootChoices(CharacterData character)
    {
        List<CardData> listReturned = new List<CardData>();
        List<CardData> validLootableCards = GetAllValidLootableCardsForCharacter(character);        

        // Get 3 random + different cards 
        for(int i = 0; i < 3; i++)
        {
            if(validLootableCards.Count == 0)
            {
                Debug.Log("WARNING: GenerateCharacterCardLootChoices() iterating over an empty card list!!");
            }

            validLootableCards.Shuffle();
            listReturned.Add(validLootableCards[0]);
            validLootableCards.Remove(validLootableCards[0]);
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
            List<CardData> cardChoices = currentLootResultData.allCharacterCardChoices[index];

            // Cache the character, so we know which character to reward a card to if player chooses one
            currentCharacterSelection = CharacterDataController.Instance.allPlayerCharacters[index];

            // Build choose card view models
            BuildChooseCardScreenCardsFromData(cardChoices);

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
        HideCardLootTab(cardLootTabs[CharacterDataController.Instance.allPlayerCharacters.IndexOf(currentCharacterSelection)]);


    }
    public void OnChooseCardScreenBackButtonClicked()
    {
        HideChooseCardScreen();
        ShowFrontPageView();
    }
   
    #endregion
}
