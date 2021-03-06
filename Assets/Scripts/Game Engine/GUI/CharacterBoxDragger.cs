﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBoxDragger : Singleton<CharacterBoxDragger>
{
    // Properties + Components
    #region
    [Header("View Components")]
    [SerializeField] GameObject visualParent;
    [SerializeField] Transform followMouseTransform;
    [SerializeField] UniversalCharacterModel ucm;

    [Header("Properties")]
    private CharacterPanelView currentPanelDragging;
    private RecruitCharacterTab currentRecruitDragging;
    private ChooseCombatCharacterSlot slotMousedOver;


    #endregion

    // Getters + Accessors
    #region
    public ChooseCombatCharacterSlot SlotMousedOver
    {
        get { return slotMousedOver; }
        private set { slotMousedOver = value; }
    }
    public CharacterPanelView CurrentPanelDragging
    {
        get { return currentPanelDragging; }
    }
    #endregion

    // Folow Mouse Logic
    #region
    private void Update()
    {
        FollowMouse();
    }
    private void FollowMouse()
    {
        if (currentPanelDragging || currentRecruitDragging)
        {
            Vector3 mousePos = CameraManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(mousePos.x, mousePos.y, 0);
            followMouseTransform.position = newPos;
        }          
    }
    #endregion

    // Main View Logic
    #region
    private void ShowBox()
    {
        visualParent.SetActive(true);
    }
    private void HideBox()
    {
        visualParent.SetActive(false);
    }

    private void BuildBoxViewFromCharacterData(CharacterData data)
    {
        CharacterModelController.Instance.BuildModelMugShotFromStringReferences(ucm, data.modelParts);
    }
    #endregion


    // Drag + Input Logic
    #region
    public void OnCharacterPanelViewDragStart(CharacterPanelView panel)
    {
        currentPanelDragging = panel;
        BuildBoxViewFromCharacterData(panel.characterDataRef);
        ShowBox();
    }
    public void OnCharacterPanelViewDragEnd()
    {    
        if (IsCharacterPanelDragDropValid())
        {
            // Check if slot is already occupied
            if(SlotMousedOver.characterDataRef != null)
            {
                TownViewController.Instance.RemoveCharacterFromSelectedCombatCharacters(SlotMousedOver.characterDataRef);
            }
            TownViewController.Instance.AddCharacterToSelectedCombatCharacters(currentPanelDragging.characterDataRef);
            BuildCombatSlotFromCharacterData(currentPanelDragging.characterDataRef, SlotMousedOver);
        }

        currentPanelDragging = null;
        HideBox();
    }
    public void OnRecruitCharacterTabDragStart(RecruitCharacterTab tab)
    {
        currentRecruitDragging = tab;
        BuildBoxViewFromCharacterData(tab.characterDataRef);
        ShowBox();
    }
    public void OnRecruitCharacterTabDragEnd()
    {
        if (IsRecruitCharacterDragDropValid())        
            CharacterDataController.Instance.HandleRecruitCharacter(currentRecruitDragging.characterDataRef);        

        currentRecruitDragging = null;
        HideBox();
    }
    public void OnChooseCombatSlotMouseEnter(ChooseCombatCharacterSlot slot)
    {
        SlotMousedOver = slot;
    }
    public void OnChooseCombatSlotMouseExit(ChooseCombatCharacterSlot slot)
    {
        if (SlotMousedOver == slot)
            SlotMousedOver = null;
    }
    public void OnChooseCombatSlotMouseClick(ChooseCombatCharacterSlot slot)
    {       
        TownViewController.Instance.RemoveCharacterFromSelectedCombatCharacters(slot.characterDataRef);
        TownViewController.Instance.ResetChooseCombatSlot(slot);
    }
    private bool IsCharacterPanelDragDropValid()
    {
        return SlotMousedOver != null;

        // TO DO: need a lot of validation checks in future, e.g.
        /*
         * is character high enough level for the selected combat?
         * is the character actually alive?
         * is the character being dragged already on a combat slot?
         * is the slot being dropped on already occupied by another character?
         * how to handle these things?
         * 
         */
    }
    private bool IsRecruitCharacterDragDropValid()
    {
        if (CharacterPanelViewController.Instance.MouseIsOverRoster &&
            CharacterDataController.Instance.CurrentMaxRosterSize >
            CharacterDataController.Instance.AllPlayerCharacters.Count)        
            return true;
        
        else
            return false;
        
    }
    #endregion

    // Combat Slots Logic
    #region
    private void BuildCombatSlotFromCharacterData(CharacterData data, ChooseCombatCharacterSlot slot)
    {
        slot.characterDataRef = data;
        slot.ucmVisualParent.SetActive(true);
        CharacterModelController.Instance.BuildModelMugShotFromStringReferences(slot.ucm, data.modelParts);
    }
    #endregion

}
