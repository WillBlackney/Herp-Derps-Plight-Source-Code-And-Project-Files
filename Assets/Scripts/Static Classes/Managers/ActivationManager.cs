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

        // Set up window + connect component references
        ActivationWindow newWindowScript = newWindow.GetComponent<ActivationWindow>();
        newWindowScript.myCharacter = entity;
        entity.characterEntityView.myActivationWindow = newWindowScript;

        // Enable panel view
        newWindowScript.gameObject.SetActive(false);
        newWindowScript.gameObject.SetActive(true);

        // add character to activation order list
        activationOrder.Add(entity);

        // Build window UCM
        CharacterModelController.BuildModelFromModelClone(newWindowScript.myUCM, entity.characterEntityView.ucm);
        
        // play window idle anim on ucm
        newWindowScript.myUCM.SetBaseAnim();
    }    
    public void CreateSlotAndWindowHolders()
    {
        activationSlotContentParent = Instantiate(slotHolderPrefab, activationPanelParent.transform);
        activationWindowContentParent = Instantiate(windowHolderPrefab, activationPanelParent.transform);
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
        // Move windows to start positions if combat has only just started
        if(TurnChangeNotifier.Instance.currentTurnCount == 0)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => MoveAllWindowsToStartPositions(), QueuePosition.Back, 0f, 0);
        }      

        // Increment turn count
        TurnChangeNotifier.Instance.currentTurnCount++;

        // Resolve each entity's OnNewTurnCycleStarted events       
        foreach(CharacterEntityModel entity in CharacterEntityController.Instance.allCharacters)
        {
            CharacterEntityController.Instance.CharacterOnNewTurnCycleStarted(entity);
        }       

        // Characters roll for initiative
        GenerateInitiativeRolls();
        SetActivationOrderBasedOnCurrentInitiativeRolls();

        // Play roll animation sequence
        CoroutineData rollsCoroutine = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => PlayActivationRollSequence(rollsCoroutine), rollsCoroutine, QueuePosition.Back, 0, 0);

        // Move windows to new positions
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateWindowPositions(), QueuePosition.Back, 0, 1);

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
        foreach(CharacterEntityModel entity in activationOrder)
        {
            if(entity.characterEntityView.myActivationWindow != null)
            {
                // NOTE: maybe this should be a scheduled visual event?
                DestroyActivationWindow(entity.characterEntityView.myActivationWindow);
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
    private int CalculateInitiativeRoll(CharacterEntityModel entity)
    {
        return EntityLogic.GetTotalInitiative(entity) + Random.Range(1, 4);
    }
    private void GenerateInitiativeRolls()
    {
        foreach (CharacterEntityModel entity in activationOrder)
        {
            entity.currentInitiativeRoll = CalculateInitiativeRoll(entity);
        }
    }
    private void SetActivationOrderBasedOnCurrentInitiativeRolls()
    {
        // Re arrange the activation order list based on the entity rolls
        List<CharacterEntityModel> sortedList = activationOrder.OrderBy(entity => entity.currentInitiativeRoll).ToList();
        sortedList.Reverse();
        activationOrder = sortedList;
    }
   
   
    #endregion 

    // Player Input + UI interactions
    #region
    public void OnEndTurnButtonClicked()
    {
        Debug.Log("ActivationManager.OnEndTurnButtonClicked() called...");

        // only start end turn sequence if all on turn visual events have completed
        if (!VisualEventManager.Instance.PendingCardDrawEvent())
        {
            StartEndTurnProcess();
        }
        
    }
    private void StartEndTurnProcess()
    {
        Debug.Log("ActivationManager.StartEndTurnProcess() called...");

        UIManager.Instance.DisableEndTurnButtonInteractions();
        CharacterEntityController.Instance.CharacterOnActivationEnd(entityActivated);
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

        // Move arrow visual event
        CoroutineData moveArrow = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => MoveArrowTowardsEntityActivatedWindow(moveArrow), moveArrow, QueuePosition.Back);
        
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
        if (VisualEventManager.Instance.PendingDefeatEvent() ||
            VisualEventManager.Instance.PendingVictoryEvent())
        {
            Debug.Log("ActivationManager.ActivateNextEntity() detected that an end combat event has been triggered, " +
                "cancelling next entity activation...");
            return;
        }

        CharacterEntityModel nextEntityToActivate = null;

        // Start a new turn if all characters have activated
        if (AllEntitiesHaveActivatedThisTurn())
        {
            StartNewTurnSequence();
        }
        else
        {
            // check each entity to see if they should activate, start search from front of activation order list
            for (int index = 0; index < activationOrder.Count; index++)
            {
                // check if the character is alive, and not yet activated this turn cycle
                if (activationOrder[index].livingState == LivingState.Alive &&
                    activationOrder[index].hasActivatedThisTurn == false )
                {
                    nextEntityToActivate = activationOrder[index];
                    break;
                }
            }

            // Did we find a valid entity?
            if (nextEntityToActivate == null)
            {
                // we didnt, start a new turn sequence
                StartNewTurnSequence();
            }
            else
            {
                // we did, activate that entity
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
        //panelArrow.SetActive(false);
        SetPanelArrowViewState(false);

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

        // Move activation windows to their new positions
        //ArrangeActivationWindowPositions();
        yield return new WaitForSeconds(1f);

        // Disable roll number text components
        foreach (CharacterEntityModel entity in activationOrder)
        {
            entity.characterEntityView.myActivationWindow.rollText.enabled = false;
        }

        SetPanelArrowViewState(true);
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
    public void FadeOutAndDestroyActivationWindow(ActivationWindow window, CoroutineData cData)
    {
        StartCoroutine(FadeOutAndDestroyActivationWindowCoroutine(window, cData));
    }
    private IEnumerator FadeOutAndDestroyActivationWindowCoroutine(ActivationWindow window, CoroutineData cData)
    {
        while (window.myCanvasGroup.alpha > 0)
        {
            window.myCanvasGroup.alpha -= 0.05f;
            if (window.myCanvasGroup.alpha == 0)
            {
                GameObject slotDestroyed = panelSlots[panelSlots.Count - 1];
                if (activationOrder.Contains(window.myCharacter))
                {
                    activationOrder.Remove(window.myCharacter);
                }
                panelSlots.Remove(slotDestroyed);
                Destroy(slotDestroyed);
            }
            yield return new WaitForEndOfFrame();
        }

        DestroyActivationWindow(window);
        cData.MarkAsCompleted();
    }
    private IEnumerator MoveWindowTowardsSlotPositionCoroutine(ActivationWindow window)
    {
        // Setup
        int myCurrentActivationOrderIndex = 0;
        bool shouldMove = false;
        Vector3 destination = new Vector3(0, 0, 0);

        if (activationOrder.Count > 0)
        {
            for (int i = 0; i < activationOrder.Count; i++)
            {
                // Find the window's character's index in the activation order list
                if (activationOrder[i] == window.myCharacter)
                {
                    myCurrentActivationOrderIndex = i;
                    break;
                }
            }

            // Does the corresponding window slot exist?
            if (panelSlots != null &&
                panelSlots.Count - 1 >= myCurrentActivationOrderIndex &&
                panelSlots[myCurrentActivationOrderIndex] != null &&
                window.transform.position != panelSlots[myCurrentActivationOrderIndex].transform.position)
            {
                // it does, signal movement and set destination for window
                shouldMove = true;
                destination = panelSlots[myCurrentActivationOrderIndex].transform.position;
            }

            // are all movement requirements met?
            if (shouldMove)
            {
                // they are, move the window
                while(window.transform.position.x != destination.x)
                {
                    window.transform.position = Vector2.MoveTowards(window.transform.position, destination, 10 * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }    
    private void UpdateWindowPositions()
    {
        foreach(CharacterEntityModel character in activationOrder)
        {
            StartCoroutine(MoveWindowTowardsSlotPositionCoroutine(character.characterEntityView.myActivationWindow));
        }
    }
    private void MoveAllWindowsToStartPositions()
    {
        for(int i = 0; i < activationOrder.Count; i++)
        {
            StartCoroutine(MoveWindowToStartingSlotCoroutine(activationOrder[i].characterEntityView.myActivationWindow, panelSlots[i]));
        }
    }
    private IEnumerator MoveWindowToStartingSlotCoroutine(ActivationWindow window, GameObject startSlotPos)
    {        
        while (window.transform.position.x != startSlotPos.transform.position.x)
        {
            window.transform.position = Vector2.MoveTowards(window.transform.position, startSlotPos.transform.position, 10 * Time.deltaTime);
            yield return null;
        }
        
    }
    private void DestroyActivationWindow(ActivationWindow window)
    {
        Destroy(window.gameObject);
    }
    private void SetPanelArrowViewState(bool onOrOff)
    {
        panelArrow.SetActive(onOrOff);
    }
    public void MoveArrowTowardsEntityActivatedWindow(CoroutineData cData)
    {
        StartCoroutine(MoveArrowTowardsEntityActivatedWindowCoroutine(cData));
    }
    private IEnumerator MoveArrowTowardsEntityActivatedWindowCoroutine(CoroutineData cData)
    {
        // Setup 
        int currentActivationIndex = 0;
        Vector3 destination = new Vector3(0, 0);
        bool destinationFound = false;

        if (activationOrder.Count > 0)
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

            // Calculate destination
            if (panelSlots[currentActivationIndex] != null && panelSlots.Count > 0)
            {
                destination = new Vector2(panelSlots[currentActivationIndex].transform.position.x, panelArrow.transform.position.y);
                destinationFound = true;
            }

            // should the arrow move?
            if (destinationFound)
            {
                // Activate arrow view
                SetPanelArrowViewState(true);

                // Move!
                while (panelArrow.transform.position.x != destination.x)
                {
                    panelArrow.transform.position = Vector2.MoveTowards(panelArrow.transform.position, destination, 10 * Time.deltaTime);
                    yield return null;
                }
            }

        }

        cData.MarkAsCompleted();
    }
    #endregion

}
