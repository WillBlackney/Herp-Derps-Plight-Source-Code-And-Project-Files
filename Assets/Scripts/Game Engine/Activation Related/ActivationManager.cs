using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;

public class ActivationManager : Singleton<ActivationManager>
{
    // Properties + Component References
    #region
    [Header("Component References")]
    [SerializeField] private GameObject windowStartPos;
    [SerializeField] private GameObject activationPanelParent;
    [SerializeField] private GameObject panelArrow;
    [SerializeField] private GameObject activationSlotContentParent;
    [SerializeField] private GameObject activationWindowContentParent; 

    [Header("Turn Change Component References")]
    [SerializeField] private TextMeshProUGUI whoseTurnText;
    [SerializeField] private CanvasGroup visualParentCG;
    [SerializeField] private RectTransform startPos;
    [SerializeField] private RectTransform endPos;
    [SerializeField] private RectTransform middlePos;

    [Header("Turn Change Properties")]
    [SerializeField] private float alphaChangeSpeed;

    [Header("Variables")]
    private List<CharacterEntityModel> activationOrder = new List<CharacterEntityModel>();
    private List<GameObject> panelSlots = new List<GameObject>();
    private CharacterEntityModel entityActivated;
    private int currentTurn;
    #endregion

    // Properties + Accessors
    #region
    public CharacterEntityModel EntityActivated
    {
        get
        {
            return entityActivated;
        }
        private set
        {
            entityActivated = value;
        }
    }
    public int CurrentTurn
    {
        get { return currentTurn; }
        private set { currentTurn = value; }
    }
    public void RemoveEntityFromActivationOrder(CharacterEntityModel entity)
    {
        if (activationOrder.Contains(entity))
        {
            activationOrder.Remove(entity);
        }
    }
    public void AddEntityToActivationOrder(CharacterEntityModel entity)
    {
        activationOrder.Add(entity);
    }
    #endregion

    // Setup + Initializaton
    #region 
    public void CreateActivationWindow(CharacterEntityModel entity)
    {
        // Create slot
        GameObject newSlot = Instantiate(PrefabHolder.Instance.panelSlotPrefab, activationSlotContentParent.transform);
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
        AddEntityToActivationOrder(entity);

        // Build window UCM
        CharacterModelController.BuildModelFromModelClone(newWindowScript.myUCM, entity.characterEntityView.ucm);
        
        // play window idle anim on ucm
        newWindowScript.myUCM.SetBaseAnim();
        
    } 
    #endregion

    // Turn Events
    #region
    public void OnNewCombatEventStarted()
    {
        CurrentTurn = 0;
        SetActivationWindowsParentViewState(true);
        CombatLogic.Instance.SetCombatState(CombatGameState.CombatActive);
        StartNewTurnSequence();
    }
    private void StartNewTurnSequence()
    {
        // Disable arrow
        VisualEventManager.Instance.CreateVisualEvent(() => SetPanelArrowViewState(false));

        // Disable End turn button
        VisualEventManager.Instance.CreateVisualEvent(() => UIManager.Instance.DisableEndTurnButtonView());

        // Move windows to start positions if combat has only just started
        if (CurrentTurn == 0)
        {
            List<CharacterEntityModel> characters = new List<CharacterEntityModel>();
            characters.AddRange(activationOrder);
            VisualEventManager.Instance.CreateVisualEvent(() => MoveAllWindowsToStartPositions(characters), QueuePosition.Back, 0f, 0.5f);
        }

        // Increment turn count
        CurrentTurn++;

        // Resolve each entity's OnNewTurnCycleStarted events       
        foreach(CharacterEntityModel entity in CharacterEntityController.Instance.AllCharacters)
        {
            CharacterEntityController.Instance.CharacterOnNewTurnCycleStarted(entity);
        }       

        // Characters roll for initiative
        GenerateInitiativeRolls();
        SetActivationOrderBasedOnCurrentInitiativeRolls();

        // Remove temp initiative
        foreach (CharacterEntityModel entity in CharacterEntityController.Instance.AllCharacters)
        {
            if (entity.pManager.temporaryBonusInitiativeStacks > 0)
            {
                PassiveController.Instance.ModifyTemporaryInitiative(entity.pManager, -entity.pManager.temporaryBonusInitiativeStacks, true);
            }
        }            

        // Play roll animation sequence
        CoroutineData rollsCoroutine = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => PlayActivationRollSequence(rollsCoroutine), rollsCoroutine, QueuePosition.Back, 0, 0);

        // Move windows to new positions
        VisualEventManager.Instance.CreateVisualEvent(() => UpdateWindowPositions(), QueuePosition.Back, 0, 1);

        // Play turn change notification
        CoroutineData turnNotificationCoroutine = new CoroutineData();
        VisualEventManager.Instance.CreateVisualEvent(() => DisplayTurnChangeNotification(turnNotificationCoroutine), turnNotificationCoroutine, QueuePosition.Back, 0, 0);

        // Set all enemy intent images if turn 1
        if(CurrentTurn == 1)
        {
            CharacterEntityController.Instance.SetAllEnemyIntents(); 
        }

        // Need to set activation button view state to enemy or player
        // otherwise it gets a bit glitchy when it turns on
        if(activationOrder[0].controller == Controller.Player)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => UIManager.Instance.SetPlayerTurnButtonState());
        }
        else
        {
            VisualEventManager.Instance.CreateVisualEvent(() => UIManager.Instance.SetEnemyTurnButtonState());
        }

        // Enable button visual event
        VisualEventManager.Instance.CreateVisualEvent(() => UIManager.Instance.EnableEndTurnButtonView());

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
        //Destroy(activationSlotContentParent);
       // Destroy(activationWindowContentParent);        
        
    }
    #endregion

    // Logic + Calculations
    #region
    private int CalculateInitiativeRoll(CharacterEntityModel entity)
    {
        return EntityLogic.GetTotalInitiative(entity) + RandomGenerator.NumberBetween(1, 3);
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

        // wait until all card draw visual events have completed
        // prevent function if game over sequence triggered
        if (VisualEventManager.Instance.PendingCardDrawEvent() == false &&
            CombatLogic.Instance.CurrentCombatState == CombatGameState.CombatActive)
        {
            // Mouse click SFX
            AudioManager.Instance.PlaySound(Sound.GUI_Button_Clicked);

            // Trigger character on activation end sequence and events
            CharacterEntityController.Instance.CharacterOnActivationEnd(EntityActivated);
        }        
    }    
    public void SetActivationWindowsParentViewState(bool onOrOff)
    {
        Debug.Log("ActivationManager.SetActivationWindowsParentViewState() called...");
        activationPanelParent.SetActive(onOrOff);
    }
    #endregion

    // Entity / Activation related
    #region   
    private void ActivateEntity(CharacterEntityModel entity)
    {
        Debug.Log("Activating entity: " + entity.myName);
        EntityActivated = entity;
        entity.hasActivatedThisTurn = true;

        // Player controlled characters
        if (entity.controller == Controller.Player)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => UIManager.Instance.SetPlayerTurnButtonState());
        }

        // Enemy controlled characters
        else if (entity.allegiance == Allegiance.Enemy &&
                 entity.controller == Controller.AI)
        {
            VisualEventManager.Instance.CreateVisualEvent(() => UIManager.Instance.SetEnemyTurnButtonState());
        }

        // Move arrow to point at activated enemy
        VisualEventManager.Instance.CreateVisualEvent(() => MoveActivationArrowTowardsEntityWindow(entity), QueuePosition.Back);

        // Start character activation
        CharacterEntityController.Instance.CharacterOnActivationStart(entity);

    }  
    public void ActivateNextEntity()
    {
        Debug.Log("ActivationManager.ActivateNextEntity() called...");

        // Setup
        CharacterEntityModel nextEntityToActivate = null;

        // dont activate next entity if either all defenders or all enemies are dead
        if (CombatLogic.Instance.CurrentCombatState != CombatGameState.CombatActive)
        {
            Debug.Log("ActivationManager.ActivateNextEntity() detected that an end combat event has been triggered, " +
                "cancelling next entity activation...");
            return;
        }       

        // Start a new turn if all characters have activated
        if (AllEntitiesHaveActivatedThisTurn())
        {
            StartNewTurnSequence();
        }
        else
        {
            // check each entity to see if they should activate, start search from front of activation order list
            /*
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
            */

            foreach(CharacterEntityModel entity in activationOrder)
            {
                // check if the character is alive, and not yet activated this turn cycle
                if (entity.livingState == LivingState.Alive &&
                    entity.hasActivatedThisTurn == false)
                {
                    nextEntityToActivate = entity;
                    break;
                }
            }            

            if(nextEntityToActivate != null)
            {
                ActivateEntity(nextEntityToActivate);
            }
            else
            {
                StartNewTurnSequence();
            }
            
        }      
    }
    private bool AllEntitiesHaveActivatedThisTurn()
    {
        Debug.Log("ActivationManager.AllEntitiesHaveActivatedThisTurn() called...");
        bool boolReturned = true;
        foreach (CharacterEntityModel entity in activationOrder)
        {
            if (entity.hasActivatedThisTurn == false)
            {
                boolReturned = false;
                break;
            }
        }
        return boolReturned;
    }
    #endregion

    // Visual Events
    #region

    // Number roll sequence visual events
    #region
    private void PlayActivationRollSequence(CoroutineData cData)
    {
        StartCoroutine(PlayActivationRollSequenceCoroutine(cData));
    }
    private IEnumerator PlayActivationRollSequenceCoroutine(CoroutineData cData)
    {
        // Disable arrow to prevtn blocking numbers
        //panelArrow.SetActive(false);
        SetPanelArrowViewState(false);

        // start number rolling sfx
        AudioManager.Instance.PlaySound(Sound.GUI_Rolling_Bells);

        foreach (CharacterEntityModel entity in activationOrder)
        {
            // start animating their roll number text
            StartCoroutine(PlayRandomNumberAnim(entity.characterEntityView.myActivationWindow));
        }

        yield return new WaitForSeconds(1);

        foreach (CharacterEntityModel entity in activationOrder)
        {
            // cache window
            ActivationWindow window = entity.characterEntityView.myActivationWindow;

            // stop the number anim
            window.animateNumberText = false;

            // set the number text as their initiative roll
            window.rollText.text = entity.currentInitiativeRoll.ToString();

            // chime ping SFX
            AudioManager.Instance.PlaySound(Sound.GUI_Chime_1);

            // do breath effect on window
            float currentScale = window.rollText.transform.localScale.x;
            float endScale = currentScale * 1.5f;
            float animSpeed = 0.25f;
            window.rollText.transform.DOScale(endScale, animSpeed).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(animSpeed);
            window.rollText.transform.DOScale(currentScale, animSpeed).SetEase(Ease.OutQuint);
            
            // brief yield before animating next window
            yield return new WaitForSeconds(0.1f);
        }

        // stop rolling sfx
        AudioManager.Instance.StopSound(Sound.GUI_Rolling_Bells);

        // brief yield
        yield return new WaitForSeconds(1f);

        // Disable roll number text components
        foreach (CharacterEntityModel entity in activationOrder)
        {
            entity.characterEntityView.myActivationWindow.rollText.enabled = false;
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
       
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

    // Destroy activation window visual events
    #region
    private void FadeOutAndDestroyActivationWindow(ActivationWindow window, CoroutineData cData)
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
                // Make sure the slot is found and destroyed if it exists still
                GameObject slotDestroyed = panelSlots[panelSlots.Count - 1];
                if (activationOrder.Contains(window.myCharacter))
                {
                    RemoveEntityFromActivationOrder(window.myCharacter);
                }

                // Remove slot from list and destroy
                panelSlots.Remove(slotDestroyed);
                Destroy(slotDestroyed);
            }
            yield return new WaitForEndOfFrame();
        }

        // Destroy window GO
        DestroyActivationWindow(window);

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }
       
    }
    private void DestroyActivationWindow(ActivationWindow window)
    {
        Destroy(window.gameObject);
    }
    public void OnCharacterKilledVisualEvent(ActivationWindow window, CharacterEntityModel currentlyActivated, CoroutineData cData)
    {
        // Need to cache the currently activated entity in a new variable called 'currentlyActivated'.
        // this makes sure the arrow points to the window of the character that is VISUALLY activated,
        // but not activated in the logic side.
        StartCoroutine(OnCharacterKilledVisualEventCoroutine(window, currentlyActivated, cData));
    }
    private IEnumerator OnCharacterKilledVisualEventCoroutine(ActivationWindow window, CharacterEntityModel currentlyActivated, CoroutineData cData)
    {
        FadeOutAndDestroyActivationWindow(window, null);
        yield return new WaitForSeconds(0.5f);

        UpdateWindowPositions();

        // If the entity that just died wasn't killed during its activation, do this
        if (activationOrder.Contains(currentlyActivated))
        {
            MoveActivationArrowTowardsEntityWindow(activationOrder[activationOrder.IndexOf(currentlyActivated)]);
        }
        else if (activationOrder.Contains(EntityActivated))
        {
            MoveActivationArrowTowardsEntityWindow(activationOrder[activationOrder.IndexOf(EntityActivated)]);
        }

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }
    #endregion

    // Update window position visual events
    #region
    private void UpdateWindowPositions()
    {
        foreach (CharacterEntityModel character in activationOrder)
        {
            MoveWindowTowardsSlotPositionCoroutine(character);
        }
    }
    private void MoveWindowTowardsSlotPositionCoroutine(CharacterEntityModel character)
    {
        Debug.Log("ActivationWindow.MoveWindowTowardsSlotPositionCoroutine() called for character: " + character.myName);

        // Get panel slot
        GameObject panelSlot = panelSlots[activationOrder.IndexOf(character)];

        // cache window
        ActivationWindow window = character.characterEntityView.myActivationWindow;

        // do we have everything needed to move?
        if (panelSlot && window)
        {
            // move the window
            Sequence s = DOTween.Sequence();
            s.Append(window.transform.DOMoveX(panelSlot.transform.position.x, 0.3f));
        }
    }
    private void MoveAllWindowsToStartPositions(List<CharacterEntityModel> characters)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            // move the window
            Sequence s = DOTween.Sequence();
            s.Append(characters[i].characterEntityView.myActivationWindow.transform.DOMoveX(panelSlots[i].transform.position.x, 0.3f));
        }
    }
    #endregion

    // Arrow pointer visual events
    #region
    private void SetPanelArrowViewState(bool onOrOff)
    {
        panelArrow.SetActive(onOrOff);
    }
    private void MoveActivationArrowTowardsEntityWindow(CharacterEntityModel character)
    {
        Debug.Log("ActivationManager.MoveActivationArrowTowardsPosition() called...");

        Debug.Log("PANEL SLOTS COUNT: " + panelSlots.Count);
        Debug.Log("ACTIVATION ORDER COUNT: " + activationOrder.Count);

        GameObject panelSlot = panelSlots[activationOrder.IndexOf(character)];

        if (panelSlot)
        {
            // Activate arrow view
            SetPanelArrowViewState(true);

            // move the arrow
            Sequence s = DOTween.Sequence();
            s.Append(panelArrow.transform.DOMoveX(panelSlot.transform.position.x, 0.2f));
        }

    }
    #endregion

    // Turn Change Notification visual events
    #region
    private void DisplayTurnChangeNotification(CoroutineData cData)
    {
        StartCoroutine(DisplayTurnChangeNotificationCoroutine(cData));
    }
    private IEnumerator DisplayTurnChangeNotificationCoroutine(CoroutineData cData)
    {
        // Get move transform
        RectTransform parent = visualParentCG.gameObject.GetComponent<RectTransform>();

        // Set starting view state values
        visualParentCG.gameObject.SetActive(true);
        parent.position = startPos.position;
        visualParentCG.alpha = 0;
        whoseTurnText.text = "Turn " + CurrentTurn.ToString();

        // Start SFX
        AudioManager.Instance.FadeInSound(Sound.Events_Turn_Change_Notification, 0.2f);

        // Start fade in
        StartCoroutine(FadeInTurnChangeNotification());

        // Move to centre
        Sequence moveToCentre = DOTween.Sequence();
        moveToCentre.Append(parent.DOMoveX(middlePos.position.x, 0.5f));

        // Pause at centre screen
        yield return new WaitForSeconds(2);

        // Start fade out
        StartCoroutine(FadeOutTurnChangeNotification());

        // Move off screen
        Sequence moveOffScreen = DOTween.Sequence();
        moveOffScreen.Append(parent.DOMoveX(endPos.position.x, 0.5f));

        // Fade out SFX
        AudioManager.Instance.FadeOutSound(Sound.Events_Turn_Change_Notification, 1f);

        yield return new WaitForSeconds(1);

        // Hide main view
        visualParentCG.alpha = 0;
        visualParentCG.gameObject.SetActive(false);

        // Resolve
        if (cData != null)
        {
            cData.MarkAsCompleted();
        }

    }
    private IEnumerator FadeInTurnChangeNotification()
    {
        while(visualParentCG.alpha < 1)
        {
            visualParentCG.alpha += alphaChangeSpeed * Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator FadeOutTurnChangeNotification()
    {
        while (visualParentCG.alpha > 0)
        {
            visualParentCG.alpha -= alphaChangeSpeed * Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #endregion

}
