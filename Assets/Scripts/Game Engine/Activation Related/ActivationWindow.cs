using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

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
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Properties")]
    public CharacterEntityModel myCharacter;
    public bool animateNumberText;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
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
