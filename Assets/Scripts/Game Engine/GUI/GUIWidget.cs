using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class GUIWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    // Properties + Component References
    #region
    [Header("Event Data")]
    [SerializeField] GUIWidgetEventData[] onClickEvents;
    [SerializeField] GUIWidgetEventData[] mouseEnterEvents;
    [SerializeField] GUIWidgetEventData[] mouseExitEvents;    

    public GUIWidgetEventData[] MouseEnterEvents
    {
        get { return mouseEnterEvents; }
        private set { mouseEnterEvents = value; }
    }
    public GUIWidgetEventData[] OnClickEvents
    {
        get { return onClickEvents; }
        private set { onClickEvents = value; }
    }
    public GUIWidgetEventData[] MouseExitEvents
    {
        get { return mouseExitEvents; }
        private set { mouseExitEvents = value; }
    }
    #endregion

    // Input Listeners
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        GUIWidgetController.Instance.HandleWidgetEvents(OnClickEvents);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GUIWidgetController.Instance.HandleWidgetEvents(MouseEnterEvents);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GUIWidgetController.Instance.HandleWidgetEvents(MouseExitEvents);
    }
    #endregion
}
