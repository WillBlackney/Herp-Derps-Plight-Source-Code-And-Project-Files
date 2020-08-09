using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonAssassin : Enemy
{
    public override EnemyAction DetermineNextAction()
    {
        int randomNum = Random.Range(0, 2);
        EnemyAction actionReturned = null;

        if(randomNum == 0)
        {
            actionReturned = myKnownActions[0];
        }
        else if(randomNum == 1)
        {
            actionReturned = myKnownActions[1];
        }

        Debug.Log("SkeletonAssassin.DetermineNextAction() returning: " + actionReturned.actionName);
        return actionReturned;
    }



    public override void SetBaseProperties()
    {
        myName = "Skeleton Assassin";
        base.SetBaseProperties();        
     
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Action actionEvent = EnemyController.Instance.ExecuteEnemyNextAction(this);
        yield return new WaitUntil(() => actionEvent.ActionResolved() == true);
        LivingEntityManager.Instance.EndEntityActivation(this);
    }
}
