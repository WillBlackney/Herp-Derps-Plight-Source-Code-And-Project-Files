using UnityEngine;
using System.Collections;

public class DrawACardCommand : Command {

    private Player p;
    private CharacterEntityModel character;
    private Defender d;
    private CardLogic cl;
    private Card c;
    private bool fast;
    private bool fromDeck;

    public DrawACardCommand(CardLogic cl, Player p, bool fast, bool fromDeck)
    {        
        this.cl = cl;
        this.p = p;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }
    public DrawACardCommand(CardLogic cl, Defender d, bool fast, bool fromDeck)
    {
        this.cl = cl;
        this.d = d;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }
    public DrawACardCommand(Card c, Defender d, bool fast, bool fromDeck)
    {
        Debug.Log("DrawACardCommand() constructor called...");
        this.c = c;
        this.d = d;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }
    public DrawACardCommand(Card c, CharacterEntityModel character, bool fast, bool fromDeck)
    {
        Debug.Log("DrawACardCommand() constructor called...");
        this.c = c;
        this.character = character;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }


    public override void StartCommandExecution()
    {
        character.characterEntityView.handVisual.GivePlayerACard(c, fast, fromDeck);
    }
 

}
