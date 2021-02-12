using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCombatCharacterSlot : MonoBehaviour
{
    // Properties + Components
    #region
    [Header("Properties")]
    [HideInInspector] public CharacterData characterDataRef;

    [Header("Components")]
    public UniversalCharacterModel ucm;
    public GameObject ucmVisualParent;
    #endregion

    // Input Listerner Logic
    #region
    private void OnMouseEnter()
    {
        CharacterBoxDragger.Instance.OnChooseCombatSlotMouseEnter(this);
    }
    private void OnMouseExit()
    {
        CharacterBoxDragger.Instance.OnChooseCombatSlotMouseExit(this);
    }
    void OnMouseDown()
    {
        CharacterBoxDragger.Instance.OnChooseCombatSlotMouseClick(this);
    }
    #endregion
}
