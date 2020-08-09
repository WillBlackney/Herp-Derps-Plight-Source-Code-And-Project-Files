using UnityEngine;
using System.Collections;
using DG.Tweening;

// this class should be attached to the deck
// generates new cards and places them into the hand
public class PlayerDeckVisual : MonoBehaviour {

    public AreaPosition owner;
    public float HeightOfOneCard = 0.012f;

    void Start()
    {
        CardsInDeck = GlobalSettings.Instance.Players[owner].deck.cards.Count;
    }

    private int cardsInDeck = 0;
    public int CardsInDeck
    {
        get{ return cardsInDeck; }

        set
        {
            cardsInDeck = value;
            transform.position = new Vector3(transform.position.x, transform.position.y, - HeightOfOneCard * value);
        }
    }
   
}
