using UnityEngine;
using System.Collections;

public class StartATurnCommand : Command {

    private Player p;

    public StartATurnCommand(Player p)
    {
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        TurnManager.Instance.whoseTurn = p;
        // this command is completed instantly
        CommandExecutionComplete();
    }
}
