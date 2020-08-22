using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActivationManager : Singleton<ActivationManager>
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject activationSlotContentParent;
    public GameObject activationWindowContentParent;
    public GameObject windowStartPos;
    public GameObject activationPanelParent;
    public Canvas activationPanelParentCanvas;
    public GameObject panelArrow;
    public GameObject panelSlotPrefab;
    public GameObject slotHolderPrefab;
    public GameObject windowHolderPrefab;

    [Header("Properties")]
    public List<CharacterEntityModel> activationOrder = new List<CharacterEntityModel>();
    public List<GameObject> panelSlots;
    public CharacterEntityModel entityActivated;
    public bool panelIsMousedOver;
    public bool updateWindowPositions;
    #endregion

    // Setup + Initializaton
    #region 
    public void CreateActivationWindow(CharacterEntityModel entity)
    {
        // Create slot
        GameObject newSlot = Instantiate(panelSlotPrefab, activationSlotContentParent.transform);
        panelSlots.Add(newSlot);

        // Create window
        GameObject newWindow = Instantiate(PrefabHolder.Instance.activationWindowPrefab, activationWindowContentParent.transform);
        newWindow.transform.position = windowStartPos.transform.position;

        // Set up window + connect components
        ActivationWindow newWindowScript = newWindow.GetComponent<ActivationWindow>();
        newWindowScript.InitializeSetup(entity);

        // add character to activation order list
        activationOrder.Add(entity);
        
        // play window idle anim on ucm
        //newWindowScript.myUCM.SetBaseAnim();
    }
    public void CreateSlotAndWindowHolders()
    {
        activationSlotContentParent = Instantiate(slotHolderPrefab, activationPanelParent.transform);
        activationWindowContentParent = Instantiate(windowHolderPrefab, activationPanelParent.transform);
        updateWindowPositions = true;
    }
    #endregion

    // Events
    #region
    public void OnNewCombatEventStarted()
    {
        TurnChangeNotifier.Instance.currentTurnCount = 0;
        SetActivationWindowViewState(true);
        StartNewTurnSequence();
    }
    public void StartNewTurnSequence()
    {
        // Increment turn count
        TurnChangeNotifier.Instance.currentTurnCount++;

        // Resolve each entity's OnNewTurnCycleStarted events       
        foreach(CharacterEntityModel entity in CharacterEntityController.Instance.allCharacters)
        {
            CharacterEntityController.Instance.CharacterOnNewTurnCycleStarted(entity);
        }       

        // Characters roll for initiative
        GenerateInitiativeRolls();

        // Play roll animation sequence
        CoroutineData rollsCoroutine = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => PlayActivationRollSequence(rollsCoroutine), rollsCoroutine, QueuePosition.Back, 0, 0);

        // Play turn change notification
        CoroutineData turnNotificationCoroutine = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => TurnChangeNotifier.Instance.DisplayTurnChangeNotification(turnNotificationCoroutine), turnNotificationCoroutine, QueuePosition.Back, 0, 0);

        // Set all enemy intent images if turn 1
        if(TurnChangeNotifier.Instance.currentTurnCount == 1)
        {
            VisualEventManager.Instance.CreateVisualEvent(()=> EnemyController.Instance.SetAllEnemyIntents(), QueuePosition.Back, 0, 1f);
        }        

        ActivateEntity(activationOrder[0]);
    }
    public void ClearAllWindowsFromActivationPanel()
    {
        updateWindowPositions = false;
        foreach(CharacterEntityModel entity in activationOrder)
        {
            if(entity.characterEntityView.myActivationWindow != null)
            {
                entity.characterEntityView.myActivationWindow.DestroyWindowOnCombatEnd();
            }
            
        }

        activationOrder.Clear();
        panelSlots.Clear();
        Destroy(activationSlotContentParent);
        Destroy(activationWindowContentParent);        
        
    }
    #endregion

    // Logic + Calculations
    #region
    private void Update()
    {
        MoveArrowTowardsEntityActivatedWindow();
    }
    private int CalculateInitiativeRoll(CharacterEntityModel entity)
    {
        return EntityLogic.GetTotalInitiative(entity) + Random.Range(1, 4);
    }
    public void GenerateInitiativeRolls()
    {
        foreach (CharacterEntityModel entity in activationOrder)
        {
            entity.currentInitiativeRoll = CalculateInitiativeRoll(entity);
        }
    }
   
   
    #endregion 

    // Player Input + UI interactions + Visual
    #region
    public void OnEndTurnButtonClicked()
    {
        // only start end turn sequence if all on turn visual events have completed
        if (!ActionManager.Instance.UnresolvedCombatActions() &&
            !Command.CardDrawPending())
        {
            StartCoroutine(OnEndTurnButtonClickedCoroutine());
        }
        
    }
    public IEnumerator OnEndTurnButtonClickedCoroutine()
    {
        Debug.Log("OnEndTurnButtonClickedCoroutine() started...");

        UIManager.Instance.DisableEndTurnButtonInteractions();
        //OldCoroutineData endTurnEvent = LivingEntityManager.Instance.EndEntityActivation(entityActivated);
        // yield return new WaitUntil(() => endTurnEvent.ActionResolved() == true); 
        yield return null;
    }  
    public OldCoroutineData MoveArrowTowardsTargetPanelPos(ActivationWindow window, float moveDelay = 0f, float arrowMoveSpeed = 400)
    {
        Debug.Log("ActivationManager.MoveArrowTowardsTargetPanelPos() called....");
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(MoveArrowTowardsTargetPanelPosCoroutine(window, action, moveDelay, arrowMoveSpeed));
        return action;
    }
    public IEnumerator MoveArrowTowardsTargetPanelPosCoroutine(ActivationWindow window, OldCoroutineData action, float moveDelay = 0, float arrowMoveSpeed = 400)
    {        
        yield return new WaitForSeconds(moveDelay);
        Vector3 destination = new Vector2(window.transform.position.x, panelArrow.transform.position.y);

        while (panelArrow.transform.position != destination)
        {
            panelArrow.transform.position = Vector2.MoveTowards(panelArrow.transform.position, destination, arrowMoveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        action.coroutineCompleted = true;
    }
    public void MoveArrowTowardsEntityActivatedWindow()
    {
        int currentActivationIndex = 0;

        if(activationOrder.Count > 0)
        {
            for (int i = 0; i < activationOrder.Count; i++)
            {
                // Check if GameObject is in the List
                if (activationOrder[i] == entityActivated)
                {
                    // It is. Return the current index
                    currentActivationIndex = i;
                    break;
                }
            }

            if (panelSlots[currentActivationIndex] != null && panelSlots.Count > 0)
            {
                Vector3 destination = new Vector2(panelSlots[currentActivationIndex].transform.position.x, panelArrow.transform.position.y);
                panelArrow.transform.position = Vector2.MoveTowards(panelArrow.transform.position, destination, 400 * Time.deltaTime);
            }
        }
       


    }
    public void SetActivationWindowViewState(bool onOrOff)
    {
        Debug.Log("ActivationManager.SetActivationWindowViewState() called...");
        activationPanelParent.SetActive(onOrOff);
    }
    #endregion

    // Entity / Activation related
    #region   
    public void ActivateEntity(CharacterEntityModel entity)
    {
        Debug.Log("Activating entity: " + entity.myName);
        entityActivated = entity;        

        // Player controlled characters
        if (entity.allegiance == Allegiance.Player &&
            entity.controller == Controller.Player)
        {
            UIManager.Instance.SetEndTurnButtonText("End Activation");
            UIManager.Instance.EnableEndTurnButtonView();
            UIManager.Instance.EnableEndTurnButtonInteractions();
           // entity.defender.SelectDefender();
        }

        // Enemy controlled characters
        else if (entity.allegiance == Allegiance.Enemy &&
                 entity.controller == Controller.AI)
        {
            UIManager.Instance.EnableEndTurnButtonView();
            UIManager.Instance.SetEndTurnButtonText("Enemy Activation...");
            UIManager.Instance.DisableEndTurnButtonInteractions();
        }

        CharacterEntityController.Instance.CharacterOnActivationStart(entity);
       // OldCoroutineData activationStartAction = entity.OnActivationStart();
        //yield return new WaitUntil(() => activationStartAction.ActionResolved() == true);


        /*
        if (entity.enemy)
        {
            //yield return new WaitForSeconds(1f);
            entity.enemy.StartMyActivation();
           // yield return new WaitForSeconds(0.5f);
        }
        */
      
    }  
    public void ActivateNextEntity()
    {
        Debug.Log("ActivationManager.ActivateNextEntity() called...");

        // dont activate next entity if either all defenders or all enemies are dead
        if (EventManager.Instance.currentCombatEndEventTriggered)
        {
            Debug.Log("ActivationManager.ActivateNextEntity() detected that an end combat event has been triggered, " +
                "cancelling next entity activation...");
            return;
        }

        CharacterEntityModel nextEntityToActivate = null;
        if (AllEntitiesHaveActivatedThisTurn())
        {
            StartNewTurnSequence();
        }
        else
        {
            for (int index = 0; index < activationOrder.Count; index++)
            {
                if (true)//(activationOrder[index].inDeathProcess == false &&
                  // (activationOrder[index].hasActivatedThisTurn == false ||
                   // (activationOrder[index].hasActivatedThisTurn == true && activationOrder[index].myPassiveManager.timeWarp)))
                {
                    nextEntityToActivate = activationOrder[index];
                    break;
                }
            }


            if (nextEntityToActivate == null)
            {
                StartNewTurnSequence();
            }
            else
            {
                ActivateEntity(nextEntityToActivate);
            }
        }
        
    }
    public bool IsEntityActivated(CharacterEntityModel entity)
    {
        if(entityActivated == entity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool AllEntitiesHaveActivatedThisTurn()
    {
        bool boolReturned = true;
        foreach (CharacterEntityModel entity in activationOrder)
        {
            if (entity.hasActivatedThisTurn == false)
            {
                boolReturned = false;
            }
        }
        return boolReturned;
    }
    #endregion

    // Visual Events
    #region
    public void PlayActivationRollSequence(CoroutineData cData)
    {
        StartCoroutine(PlayActivationRollSequenceCoroutine(cData));
    }
    private IEnumerator PlayActivationRollSequenceCoroutine(CoroutineData cData)
    {
        // Disable arrow to prevtn blocking numbers
        panelArrow.SetActive(false);

        foreach (CharacterEntityModel entity in activationOrder)
        {
            // start animating their roll number text
            StartCoroutine(PlayRandomNumberAnim(entity.characterEntityView.myActivationWindow));
        }

        yield return new WaitForSeconds(1);

        foreach (CharacterEntityModel entity in activationOrder)
        {
            // stop the number anim
            entity.characterEntityView.myActivationWindow.animateNumberText = false;
            // set the number text as their initiative roll
            entity.characterEntityView.myActivationWindow.rollText.text = entity.currentInitiativeRoll.ToString();
            yield return new WaitForSeconds(0.3f);
        }

        // Re arrange the activation order list based on the entity rolls
        List<CharacterEntityModel> sortedList = activationOrder.OrderBy(entity => entity.currentInitiativeRoll).ToList();
        sortedList.Reverse();
        activationOrder = sortedList;

        // Move activation windows to their new positions
        //ArrangeActivationWindowPositions();
        yield return new WaitForSeconds(1f);

        // Disable roll number text components
        foreach (CharacterEntityModel entity in activationOrder)
        {
            entity.characterEntityView.myActivationWindow.rollText.enabled = false;
        }

        panelArrow.SetActive(true);
        cData.MarkAsCompleted();
    }
    private IEnumerator PlayRandomNumberAnim(ActivationWindow window)
    {
        Debug.Log("PlayRandomNumberAnim() called....");
        int numberDisplayed = 0;
        window.animateNumberText = true;
        window.rollText.enabled = true;

        while (window.animateNumberText == true)
        {
            //Debug.Log("Animating roll number text....");
            numberDisplayed++;
            if (numberDisplayed > 9)
            {
                numberDisplayed = 0;
            }
            window.rollText.text = numberDisplayed.ToString();

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion







}
