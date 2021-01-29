using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseCombatButton : MonoBehaviour, IPointerClickHandler
{
    public Image[] encounterImages;
    public Image difficultyColourImage;
    [HideInInspector] public CombatData combatDataRef;

    public void OnPointerClick(PointerEventData eventData)
    {
        TownViewController.Instance.OnChooseCombatButtonClicked(this);
    }
}
