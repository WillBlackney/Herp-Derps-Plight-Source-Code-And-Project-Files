using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCardSlot : MonoBehaviour
{

    public CardInfoPanel cardInfoPanel;

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
