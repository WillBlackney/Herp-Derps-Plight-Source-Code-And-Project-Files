using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class RecruitCharacterTab : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    // Properties + Components
    #region
    [Header("Properties")]
    [HideInInspector] public CharacterData characterDataRef;
    [HideInInspector] public bool beingDragged;

    [Header("Components")]
    public UniversalCharacterModel ucm;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI raceAndClassText;
    #endregion

    // Drag + Input Listeners
    #region
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogWarning("RecruitCharacterTab.OnBeginDrag() called...");
        CharacterBoxDragger.Instance.OnRecruitCharacterTabDragStart(this);
    }  
    public void OnDrag(PointerEventData eventData)
    {
        // although nothing is done during OnDrag, dragging doesnt work if this interface isn't implemented
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.LogWarning("RecruitCharacterTab.OnEndDrag() called...");
        CharacterBoxDragger.Instance.OnRecruitCharacterTabDragEnd();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!beingDragged)
            CharacterPanelViewController.Instance.OnRecruitCharacterTabClicked(this);
    }
    #endregion
}
