using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonAssassin : Enemy
{ 



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
