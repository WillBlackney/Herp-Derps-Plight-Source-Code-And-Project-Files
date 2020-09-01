using UnityEngine;
using System.Collections;

public class DamageAllOpponentCreatures : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            cl.Health -= specialAmount;
        }
    }
}
