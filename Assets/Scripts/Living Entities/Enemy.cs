using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Enemy : LivingEntity
{
    [Header("Enemy Components")]
    [HideInInspector] public EnemyDataSO enemyData;
    public EnemyInfoPanel myInfoPanel;
    [HideInInspector] public GameObject freeStrikeIndicator;
    [HideInInspector] public LivingEntity currentActionTarget;
    [HideInInspector] public EnemyAction myNextAction;
    public IntentViewModel myIntentViewModel;
    [HideInInspector] public List<EnemyAction> myPreviousActionLog = new List<EnemyAction>();

    // Initialization + Setup
    #region
    public override void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {              
        EnemyManager.Instance.allEnemies.Add(this);        
        base.InitializeSetup(startingGridPosition, startingTile);
        myInfoPanel.InitializeSetup(this);
        
    }
    public override void InitializeSetup(LevelNode position)
    {
        EnemyManager.Instance.allEnemies.Add(this);
        base.InitializeSetup(position);
        myInfoPanel.InitializeSetup(this);

    }
    public override void SetBaseProperties()
    {
        DifficultyManager.Instance.ApplyActTwoModifiersToLivingEntity(this);
        EnemyController.Instance.BuildEnemyFromEnemyData(this, enemyData);
        base.SetBaseProperties();
    }
    #endregion

    // Activation + Related
    #region
    public virtual void StartMyActivation()
    {
        if (!inDeathProcess)
        {
            EnemyController.Instance.StartEnemyActivation(this);
        }
        else
        {
            Debug.Log("Enemy.StartMyActivation() on " + enemy.myName + " detected that bool 'inDeathProcess' is true, stopping activation from starting...");
        }
    }
    public virtual IEnumerator StartMyActivationCoroutine()
    {
        OldCoroutineData actionEvent = EnemyController.Instance.ExecuteEnemyNextAction(this);
        yield return new WaitUntil(() => actionEvent.ActionResolved() == true);
        LivingEntityManager.Instance.EndEntityActivation(this);
    }    
    #endregion

    // AI Targeting Logic
    #region
    public void SetTargetDefender(LivingEntity target)
    {
        myCurrentTarget = target;
    }
    
    #endregion    

    // Mouse + Click Events
    #region
    public void OnMouseDown()
    {
        Debug.Log("Enemy click detected");
        EnemyManager.Instance.SelectEnemy(this);
    }
    public void OnMouseOver()
    {
    }
    public override void OnMouseEnter()
    {
        Debug.Log("Enemy.OnMouseEnter() called...");
        base.OnMouseEnter();
        EnemyController.Instance.OnEnemyMouseEnter(this);
    }
    public override void OnMouseExit()
    {
        Debug.Log("Enemy.OnMouseExit() called...");
        base.OnMouseExit();
        EnemyController.Instance.OnEnemyMouseExit(this);
    }

    // View + UI Logic
    public void SetFreeStrikeIndicatorViewState(bool onOrOff)
    {
        if(onOrOff == true)
        {
            freeStrikeIndicator.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            freeStrikeIndicator.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    #endregion


}
