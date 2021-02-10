using System.Collections;
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

    #endregion

    // Getters + Accessors
    #region
    public CharacterPanelView CurrentPanelDragging
    {
        get { return currentPanelDragging; }
    }
    #endregion

    // Folow Mouse Logic
    #region
    private void Update()
    {
       // FollowMouse();
    }
    private void FollowMouse()
    {
        if(currentPanelDragging)
            followMouseTransform.position = Input.mousePosition;
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


    // Drag Logic
    #region
    public void OnCharacterPanelViewDragStart(CharacterPanelView panel)
    {
        currentPanelDragging = panel;
        BuildBoxViewFromCharacterData(panel.characterDataRef);
        ShowBox();

    }
    public void OnCharacterPanelViewDragEnd()
    {       
        currentPanelDragging = null;
        HideBox();
    }
    #endregion


}
