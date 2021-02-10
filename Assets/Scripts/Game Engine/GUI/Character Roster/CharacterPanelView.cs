using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterPanelView : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
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
    #endregion

    // Drag + Input Listeners
    #region
    public void OnDrag(PointerEventData eventData)
    {
        CharacterBoxDragger.Instance.OnCharacterPanelViewDragStart(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CharacterBoxDragger.Instance.CurrentPanelDragging)
            CharacterBoxDragger.Instance.OnCharacterPanelViewDragEnd();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!beingDragged)
            CharacterPanelViewController.Instance.OnCharacterPanelViewClicked(this);
    }
    #endregion
}
