using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class GUISoundHelper : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

     /*
     * INFO:
     * Attack this script to any button or 
     * GUI element to trigger sounds FX 
     */

    // Properties + Component References
    #region
    [Header("Sound References")]
    [SerializeField] private Sound hoverSound;
    [SerializeField] private Sound clickSound;

    [Header("Image References")]
    [SerializeField] private GameObject activeOnHover;
    #endregion

    // Input Listeners
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound(clickSound);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound(hoverSound);

        if(activeOnHover != null)
        {
            activeOnHover.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (activeOnHover != null)
        {
            activeOnHover.SetActive(false);
        }
    }
    #endregion
}
