using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private CharacterEntityModel character;
    private Card c;

    public PlayASpellCardCommand(CharacterEntityModel _character, Card card)
    {
        this.c = card;
        this.character = _character;
    }

    public override void StartCommandExecution()
    {
        character.characterEntityView.handVisual.PlayASpellFromHand(c.cardVM);
    }

}
