using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ActivationWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    #endregion

    // Mouse + Pointer Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnPointerEnter() called...");
        myGlowOutline.SetActive(true);

        if(myCharacter != null)
        {
            // Set character highlight color
            CharacterEntityController.Instance.SetCharacterColor(myCharacter.characterEntityView, CharacterEntityController.Instance.highlightColour);

            // Set character's level node mouse over state
            if (myCharacter.levelNode != null)
            {
                myCharacter.levelNode.SetMouseOverViewState(true);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnMouseEnter called...");
        myGlowOutline.SetActive(false);

        if (myCharacter != null)
        {
            // Set character highlight color
            CharacterEntityController.Instance.SetCharacterColor(myCharacter.characterEntityView, CharacterEntityController.Instance.normalColour);

            // Set character's level node mouse over state
            if (myCharacter.levelNode != null)
            {
                myCharacter.levelNode.SetMouseOverViewState(false);
            }
        }
    }
    #endregion

}
