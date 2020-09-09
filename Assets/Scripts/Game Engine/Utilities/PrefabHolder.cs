using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : Singleton<PrefabHolder>
{
    // Prefabs References
    #region
    [Header("Buttons + UI")]
    public GameObject PassiveIconViewPrefab;
    public GameObject activationWindowPrefab;
    public GameObject statePrefab;

    [Header("Character Entity Prefabs")]
    public GameObject characterEntityModel;

    [Header("Cards Prefabs")]
    public GameObject noTargetCard;
    public GameObject targetCard;

    [Header("Activation Window Related")]
    public GameObject panelSlotPrefab;
    public GameObject slotHolderPrefab;
    public GameObject windowHolderPrefab;
    #endregion


}
