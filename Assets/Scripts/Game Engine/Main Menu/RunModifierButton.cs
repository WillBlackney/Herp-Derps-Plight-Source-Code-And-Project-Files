using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunModifierButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tickParent;
    public GameObject crossParent;
    public GameObject descriptionWindowParent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindowParent.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindowParent.SetActive(false);
    }

    public void TickMe()
    {
        tickParent.SetActive(true);
        crossParent.SetActive(false);
    }
    public void CrossMe()
    {
        tickParent.SetActive(false);
        crossParent.SetActive(true);
    }
}
