using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBarbarian : Enemy
{
    public override EnemyAction DetermineNextAction()
    {
        EnemyAction actionReturned = myKnownActions[0];
        Debug.Log("SkeletonBarbarian.DetermineNextAction() returning: " + actionReturned.actionName);
        return actionReturned;
    }
    public override void SetBaseProperties()
    {
        myName = "Skeleton Barbarian";
        base.SetBaseProperties();      

        CharacterModelController.SetUpAsSkeletonBarbarianPreset(myModel);

        myPassiveManager.ModifyEnrage(2);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Battle Axe");

        // Learn actions
       // myKnownActions.Add(new EnemyAction("Strike", ActionType.AttackTarget, 5));
    }       
    
    public override IEnumerator StartMyActivationCoroutine()
    {
        Action actionEvent = EnemyController.Instance.ExecuteEnemyNextAction(this);
        yield return new WaitUntil(() => actionEvent.ActionResolved() == true);
        LivingEntityManager.Instance.EndEntityActivation(this);
    }

    
}
