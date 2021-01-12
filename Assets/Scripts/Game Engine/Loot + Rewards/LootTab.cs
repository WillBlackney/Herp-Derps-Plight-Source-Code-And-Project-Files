using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LootTab : MonoBehaviour
{
    [SerializeField] LootTabType tabType;
    public TextMeshProUGUI descriptionText;
    public Image typeImage;
    public LootTabType TabType
    {
        get { return tabType; }
    }
    public void OnLootButtonClicked()
    {
        LootController.Instance.OnLootTabButtonClicked(this);
    }
}
public enum LootTabType
{
    None = 0,
    CardReward = 1,
    GoldReward = 2,
    TrinketReward = 3,
}
