using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ActivationWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public TextMeshProUGUI rollText;
    public Slider myHealthBar;
    public GameObject myGlowOutline;
    public CanvasGroup myCanvasGroup;
    public UniversalCharacterModel myUCM;

    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public CharacterEntityModel myCharacter;
    public bool animateNumberText;
    public bool dontFollowSlot;
    public bool update;
    #endregion

    // Setup + Initialization
    #region
    public void InitializeSetup(LivingEntity entity)
    {
        myLivingEntity = entity;
        entity.myActivationWindow = this;
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        Debug.Log("Setting up activation window UC Model for " + entity.myName);

        // Set up model   
        
        if (entity.defender)
        {
            CharacterModelController.BuildModelFromModelClone(myUCM, entity.defender.myCharacterData.myCharacterModel);
        }
        else if (entity.enemy)
        {
            //CharacterModelController.BuildModelFromPresetString(myUCM, entity.myName);
            CharacterModelController.BuildModelFromModelClone(myUCM, entity.myModel);
        }
        
        myUCM.SetBaseAnim();
    }
    public void InitializeSetup(CharacterEntityModel entity)
    {
        Debug.Log("Setting up activation window UCM Model for " + entity.myName);

        myCharacter = entity;
        entity.characterEntityView.myActivationWindow = this;
        gameObject.SetActive(false);
        gameObject.SetActive(true);        

        // Set up model   
        /*
        if (entity.defender)
        {
            CharacterModelController.BuildModelFromModelClone(myUCM, entity.defender.myCharacterData.myCharacterModel);
        }
        else if (entity.enemy)
        {
            //CharacterModelController.BuildModelFromPresetString(myUCM, entity.myName);
            CharacterModelController.BuildModelFromModelClone(myUCM, entity.myModel);
        }
        */

        //myUCM.SetBaseAnim();
    }
    #endregion

    private void Update()
    {
        int myCurrentActivationOrderIndex = 0;

        if (ActivationManager.Instance.activationOrder.Count > 0 
            && ActivationManager.Instance.updateWindowPositions == true)
        {
            for (int i = 0; i < ActivationManager.Instance.activationOrder.Count; i++)
            {
                // Check if GameObject is in the List
                if (ActivationManager.Instance.activationOrder[i] == myCharacter)
                {
                    // It is. Return the current index
                    myCurrentActivationOrderIndex = i;
                    break;
                }
            }

            if (ActivationManager.Instance.panelSlots != null &&
                ActivationManager.Instance.panelSlots.Count -1 >= myCurrentActivationOrderIndex &&
                ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex] != null &&
                transform.position != ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex].transform.position)
            {
                MoveTowardsSlotPosition(ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex]);
            }
        }    
        
        
    }
    public OldCoroutineData FadeOutWindow()
    {
        OldCoroutineData action = new OldCoroutineData();
        StartCoroutine(FadeOutWindowCoroutine(action));
        return action;
    }
    public IEnumerator FadeOutWindowCoroutine(OldCoroutineData action)
    {
        while (myCanvasGroup.alpha > 0)
        {
            myCanvasGroup.alpha -= 0.05f;
            if (myCanvasGroup.alpha == 0)
            {
                GameObject slotDestroyed = ActivationManager.Instance.panelSlots[ActivationManager.Instance.panelSlots.Count - 1];
                if (ActivationManager.Instance.activationOrder.Contains(myCharacter))
                {
                    ActivationManager.Instance.activationOrder.Remove(myCharacter);
                }                
                ActivationManager.Instance.panelSlots.Remove(slotDestroyed);                
                Destroy(slotDestroyed);
            }
            yield return new WaitForEndOfFrame();
        }

        action.coroutineCompleted = true;
        Destroy(gameObject);
    }

    public void DestroyWindowOnCombatEnd()
    {
        Destroy(gameObject);
    }
    public void MoveTowardsSlotPosition(GameObject slot)
    {
        if (!dontFollowSlot)
        {
            transform.position = Vector2.MoveTowards(transform.position, slot.transform.position, 10 * Time.deltaTime);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnPointerClick() called...");
        // Clicking on a character's activation window performs the same logic as clicking on the character itself
        if (myLivingEntity.defender)
        {
            myLivingEntity.defender.OnMouseDown();
        }
        else if (myLivingEntity.enemy)
        {
            myLivingEntity.enemy.OnMouseDown();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnPointerEnter() called...");
        ActivationManager.Instance.panelIsMousedOver = true;
        myGlowOutline.SetActive(true);
        if (myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.highlightColour);
            if(myLivingEntity.levelNode != null)
            {
                myLivingEntity.levelNode.SetMouseOverViewState(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnMouseEnter called...");
        ActivationManager.Instance.panelIsMousedOver = false;
        myGlowOutline.SetActive(false);
        if (myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.normalColour);
            if (myLivingEntity.levelNode != null)
            {
                myLivingEntity.levelNode.SetMouseOverViewState(false);
            }
        }
    }    
   
}
