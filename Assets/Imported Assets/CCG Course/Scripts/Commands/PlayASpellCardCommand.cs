using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private CardLogic card;
    private Player p;
    //private ICharacter target;
    private Defender d;
    private Card c;

    public PlayASpellCardCommand(Player p, CardLogic card)
    {
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        //p.PArea.handVisual.PlayASpellFromHand(card.UniqueCardID);

        d.handVisual.PlayASpellFromHand(c.cardVM);
    }

    public PlayASpellCardCommand(Defender d, Card c)
    {
        this.c = c;
        this.d = d;
    }
}
