using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class GUIWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    // Properties + Component References
    #region
    [Header("Core Properties")]
    public WidgetInputType inputType;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Event Data")]
    [SerializeField] GUIWidgetEventData[] onClickEvents;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] GUIWidgetEventData[] mouseEnterEvents;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] GUIWidgetEventData[] mouseExitEvents;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] GUIWidgetEventData[] onDisableEvents;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Input State")]
    public bool pointerIsOverMe;
    [HideInInspector] public float timeSinceLastPointerEnter;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

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
    public GUIWidgetEventData[] OnDisableEvents
    {
        get { return onDisableEvents; }
        private set { onDisableEvents = value; }
    }
    #endregion

    // Input Listeners
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        if (inputType == WidgetInputType.IPointer)
        {
            GUIWidgetController.Instance.HandleWidgetEvents(this, OnClickEvents);
        }
       
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inputType == WidgetInputType.IPointer)
        {
            pointerIsOverMe = true;
            timeSinceLastPointerEnter = Time.realtimeSinceStartup;
            GUIWidgetController.Instance.HandleWidgetEvents(this, MouseEnterEvents);
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(inputType == WidgetInputType.IPointer)
        {
            pointerIsOverMe = false;
            GUIWidgetController.Instance.HandleWidgetEvents(this, MouseExitEvents);
        }
       
    }

    public void OnMouseEnter()
    {
        if (inputType == WidgetInputType.Collider)
        {
            pointerIsOverMe = true;
            timeSinceLastPointerEnter = Time.realtimeSinceStartup;
            GUIWidgetController.Instance.HandleWidgetEvents(this, MouseEnterEvents);
        }
    }
    public void OnMouseExit()
    {
        if (inputType == WidgetInputType.Collider)
        {
            pointerIsOverMe = false;
            GUIWidgetController.Instance.HandleWidgetEvents(this, MouseExitEvents);
        }
    }
    public void OnMouseDown()
    {
        if (inputType == WidgetInputType.Collider)
        {
            GUIWidgetController.Instance.HandleWidgetEvents(this, OnClickEvents);
        }
    }
    #endregion

    // Life Cycle Listeners
    #region
    void OnDisable()
    {
        GUIWidgetController.Instance.HandleWidgetEvents(this, OnDisableEvents);
    }
    #endregion
}
