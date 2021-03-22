using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Draggable : MonoBehaviour 
{
    // Properties + Component References
    #region
    // a flag to know if we are currently dragging this GameObject
    private bool dragging = false;
    private bool lockedOn = false;
   

    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 pointerDisplacement;

    // distance from camera to mouse on Z axis 
    private float zDisplacement;

    // reference to DraggingActions script. Dragging Actions should be attached to the same GameObject.
    [SerializeField] private DraggingActions da;

    // STATIC property that returns the instance of Draggable that is currently being dragged
    private static Draggable _draggingThis;
    public static Draggable DraggingThis
    {
        get{ return _draggingThis;}
    }

    public DraggingActions Da
    {
        get { return da; }
        private set { da = value; }
    }

    // Mobile properties
    private bool initialTouchSet = false;
    private Vector3 initialTouchPos;
    private bool touchFingerIsOverMe = false;
    #endregion

    // Follow Mouse Logic
    #region
    void Update()
    {       

        if (dragging && !lockedOn && Da is DragSpellNoTarget)
        {
            Vector3 mousePos = MouseInWorldCoords();
            float distance = Vector3.Distance(transform.position, mousePos);
            transform.DOKill();
            transform.DOMove(new Vector3(mousePos.x, mousePos.y, transform.position.z), 0.1f);
            da.OnDraggingInUpdate();

            if (transform.position == mousePos)
                lockedOn = true;
        }
        else if (dragging && lockedOn && Da is DragSpellNoTarget)
        {
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            da.OnDraggingInUpdate();
        }
    }
    #endregion

    // Input Hooks
    #region
    public void TriggerOnMouseDown()
    {
        OnMouseDown();
    }
    public void TriggerOnMouseUp(bool forceFailure = false)
    {
        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive ||
            CardController.Instance.ChooseCardScreenIsActive ||
            MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            return;
        }

        if (GlobalSettings.Instance.deviceMode == DeviceMode.Desktop)
        {
            if (dragging)
            {
                dragging = false;
                // turn all previews back on
                HoverPreview.PreviewsAllowed = true;
                _draggingThis = null;
                da.OnEndDrag(forceFailure);
            }
        }
        else if (GlobalSettings.Instance.deviceMode == DeviceMode.Mobile)
        {
            if (dragging)
            {
                initialTouchSet = false;
                touchFingerIsOverMe = false;
                dragging = false;

                // turn all previews back on
                HoverPreview.PreviewsAllowed = true;
                _draggingThis = null;
                da.OnEndDrag(forceFailure);
            }
        }
    }
    void OnMouseDown()
    {
        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive ||
            MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            return;
        }

        if (CardController.Instance.ChooseCardScreenIsActive)
        {
            CardController.Instance.HandleChooseScreenCardSelection(da.CardVM().card);
        }

        if (da!=null && da.CanDrag)
        {
            if(GlobalSettings.Instance.deviceMode == DeviceMode.Desktop)
            {
                lockedOn = false;
                dragging = true;
                // when we are dragging something, all previews should be off
                HoverPreview.PreviewsAllowed = false;
                _draggingThis = this;
                da.OnStartDrag();
                zDisplacement = -CameraManager.Instance.MainCamera.transform.position.z + transform.position.z;
                pointerDisplacement = -transform.position + MouseInWorldCoords();
            }
        }
    }   
	
    void OnMouseUp()
    {
        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive || 
            CardController.Instance.ChooseCardScreenIsActive ||
            MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            return;
        }

        if (GlobalSettings.Instance.deviceMode == DeviceMode.Desktop)
        {
            if (dragging)
            {
                dragging = false;
                // turn all previews back on
                HoverPreview.PreviewsAllowed = true;
                _draggingThis = null;
                da.OnEndDrag();
            }
        }
        else if (GlobalSettings.Instance.deviceMode == DeviceMode.Mobile)
        {
            if (dragging)
            {
                initialTouchSet = false;
                touchFingerIsOverMe = false;
                dragging = false;

                // turn all previews back on
                HoverPreview.PreviewsAllowed = true;
                _draggingThis = null;
                da.OnEndDrag();
            }
        }
    }   

    void OnMouseOver()
    {
        if(GlobalSettings.Instance.deviceMode == DeviceMode.Mobile &&
            Input.touchCount > 0)
        {
            touchFingerIsOverMe = true;

            if (initialTouchSet == false)
            {
                initialTouchSet = true;
                initialTouchPos = TouchInWorldCoords();
            }       
            
            Vector3 currentTouchPos = TouchInWorldCoords();
            float deltaY = currentTouchPos.y - initialTouchPos.y;

            if (deltaY >= GlobalSettings.Instance.mouseDragSensitivity && 
                dragging == false && 
                _draggingThis == null &&
                da != null && 
                da.CanDrag)
            {
                lockedOn = false;
                dragging = true;

                // when we are dragging something, all previews should be off
                HoverPreview.PreviewsAllowed = false;
                _draggingThis = this;
                da.OnStartDrag();
                zDisplacement = -CameraManager.Instance.MainCamera.transform.position.z + transform.position.z;
                pointerDisplacement = -transform.position + MouseInWorldCoords();
            }

            Touch touch = Input.GetTouch(0);

            // did player lift the finger off the screen?
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // they did, start handling drag/drop logic
                if (dragging)
                {
                    initialTouchSet = false;
                    dragging = false;
                    touchFingerIsOverMe = false;

                    // turn all previews back on
                    HoverPreview.PreviewsAllowed = true;
                    _draggingThis = null;
                    da.OnEndDrag();
                }
            }            
        }
    }

    private void OnMouseExit()
    {
        if (GlobalSettings.Instance.deviceMode == DeviceMode.Mobile)
        {
            initialTouchSet = false;
            touchFingerIsOverMe = false;
        }
    }
    #endregion

    // Misc Functions
    #region
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        return CameraManager.Instance.MainCamera.ScreenToWorldPoint(screenMousePos);
    }
    private Vector3 TouchInWorldCoords()
    {
        Vector3 screenMousePos = Input.GetTouch(0).position;
        screenMousePos.z = zDisplacement;
        return CameraManager.Instance.MainCamera.ScreenToWorldPoint(screenMousePos);

    }
    #endregion

}
