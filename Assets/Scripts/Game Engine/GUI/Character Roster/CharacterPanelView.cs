using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterPanelView : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    // Properties + Component Refs
    #region
    [Header("Properties")]
    [HideInInspector] public CharacterData characterDataRef;
    [HideInInspector] public bool beingDragged = false;

    [Header("Core Components")]
    public GameObject visualParent;
    public UniversalCharacterModel ucm;
    public TextMeshProUGUI nameText;
   

    [Header("Health Bar Components")]
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public Slider healthBar;

    [Header("XP Bar Components")]
    public TextMeshProUGUI levelText;
    public Slider xpBar;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogWarning("CharacterPanelView.OnBeginDrag() called...");
        CharacterBoxDragger.Instance.OnCharacterPanelViewDragStart(this);
    }
    #endregion

    // Drag + Input Listeners
    #region
    public void OnDrag(PointerEventData eventData)
    {
       // although nothing is done during OnDrag, dragging doesnt work if this interface isn't implemented
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.LogWarning("CharacterPanelView.OnEndDrag() called...");
        CharacterBoxDragger.Instance.OnCharacterPanelViewDragEnd();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!beingDragged)
            CharacterPanelViewController.Instance.OnCharacterPanelViewClicked(this);
    }
    #endregion
}
