using UnityEngine;
using System.Collections;

public class GameOverCommand : Command{

    private Player looser;

    public GameOverCommand(Player looser)
    {
        this.looser = looser;
    }

    public override void StartCommandExecution()
    {
        looser.PArea.Portrait.Explode();
    }
}
