using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoryEventCharacterButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject scalingParent;
    public Image myGlowOutline;
    public CanvasGroup myCanvasGroup;
    public UniversalCharacterModel myUCM;

    // Properties
    [HideInInspector] public CharacterData myCharacter;


    #endregion

    // Input Listeners
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        StoryEventController.Instance.OnCharacterButtonClicked(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StoryEventController.Instance.OnCharacterButtonMouseEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StoryEventController.Instance.OnCharacterButtonMouseExit(this);
    }
    #endregion
}