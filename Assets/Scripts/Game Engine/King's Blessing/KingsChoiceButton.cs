using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class KingsChoiceButton : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public KingsChoicePairingModel myPairingData;
    public TextMeshProUGUI descriptionText;
    public CanvasGroup cg;

    public bool clickable = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        KingsBlessingController.Instance.OnChoiceButtonClicked(this);
    }
}
