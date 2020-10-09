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
    public CharacterEntityModel myCharacter;
    public bool animateNumberText;
    #endregion

    // Mouse + Pointer Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnMouseEnter called...");
        CharacterEntityController.Instance.OnCharacterMouseEnter(myCharacter.characterEntityView);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnMouseExit called...");
        CharacterEntityController.Instance.OnCharacterMouseExit(myCharacter.characterEntityView);
    }
    
    #endregion

}
