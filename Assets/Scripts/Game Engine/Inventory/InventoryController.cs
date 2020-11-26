using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    // Properties + Component References
    #region
    [Header("Properties")]
    private List<CardData> cardInventory = new List<CardData>();   

    #endregion

    // Getters + Accessors
    #region
    public List<CardData> CardInventory
    {
        get { return cardInventory; }
        private set { cardInventory = value; }
    }
    
    #endregion


    // Logic
    #region
    public void PopulateInventoryWithMockCardData(int randomCardsAdded = 10)
    {
        List<CardData> allCards = CardController.Instance.GetCardsQuery(CardController.Instance.AllCards);

        for (int i = 0; i < randomCardsAdded; i++)
        {
            CardData randomCard = allCards[RandomGenerator.NumberBetween(0, allCards.Count - 1)];
            AddCardToInventory(randomCard);
        }
    }
    public void AddCardToInventory(CardData card)
    {
        CardInventory.Add(card);
    }
    public void RemoveCardFromInventory(CardData card)
    {
        CardInventory.Remove(card);
    }
    #endregion


}
