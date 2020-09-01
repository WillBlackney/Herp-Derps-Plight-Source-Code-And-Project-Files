using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : Singleton<PrefabHolder>
{
    // Prefabs References
    #region
    [Header("Buttons + UI")]
    public GameObject PassiveIconViewPrefab;
    public GameObject AbilityButtonPrefab;
    public GameObject abilityPageAbility;
    public GameObject consumable;
    public GameObject AttributeTab;
    public GameObject enemyPanelAbilityTab;
    public GameObject activationWindowPrefab;
    public GameObject statePrefab;
    public GameObject afflicationOnPanelPrefab;

    [Header("Living Entity Prefabs")]
    public GameObject characterEntityModel;
    public GameObject defenderPrefab;
    public GameObject enemyPrefab;

    [Header("Cards Prefabs")]
    public GameObject noTargetCard;
    public GameObject targetCard;

    [Header("Loot Related")]
    public GameObject GoldRewardButton;
    public GameObject ConsumableRewardButton;
    public GameObject ItemRewardButton;
    public GameObject stateRewardButton;
    public GameObject ArtifactGO;
    public GameObject ItemCard;
    public GameObject StateCard;
    public GameObject InventoryItem;
    public GameObject AbilityTomeInventoryCard;
    public GameObject TreasureChest;

    [Header("Activation Window Related")]
    public GameObject panelSlotPrefab;
    public GameObject slotHolderPrefab;
    public GameObject windowHolderPrefab;
    #endregion


}
