using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Draggable : MonoBehaviour 
{
    // Properties + Component References
    #region
    // a flag to know if we are currently dragging this GameObject
    private bool dragging = false;
    private bool touchFingerIsOverMe = false;

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

    // Mobile properties
    private Vector3 previousTouchPosition;
    #endregion

    // Follow Mouse Logic
    #region
    void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            da.OnDraggingInUpdate();
        }
    }
    #endregion

    // Input Hooks
    #region
    void OnMouseDown()
    {
        Debug.LogWarning("Draggable.OnMouseDown() called...");

        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive)
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
                dragging = true;
                // when we are dragging something, all previews should be off
                HoverPreview.PreviewsAllowed = false;
                _draggingThis = this;
                da.OnStartDrag();
                zDisplacement = -CameraManager.Instance.MainCamera.transform.position.z + transform.position.z;
                pointerDisplacement = -transform.position + MouseInWorldCoords();
            }
            /*
            else if (GlobalSettings.Instance.deviceMode == DeviceMode.Mobile &&
                mobileTouching == false)
            {
                mobileTouching = true;
                dragging = true;
                // when we are dragging something, all previews should be off
                HoverPreview.PreviewsAllowed = false;
                _draggingThis = this;
                da.OnStartDrag();
                zDisplacement = -CameraManager.Instance.MainCamera.transform.position.z + transform.position.z;
                pointerDisplacement = -transform.position + MouseInWorldCoords();
            }
            */

        }
    }   
	
    void OnMouseUp()
    {
        Debug.Log("Draggable.OnMouseUp() called...");
        
        // prevent clicking through an active UI screen
        if (CardController.Instance.DiscoveryScreenIsActive || CardController.Instance.ChooseCardScreenIsActive)
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
        else if (GlobalSettings.Instance.deviceMode == DeviceMode.Mobile &&
            touchFingerIsOverMe == true)
        {
            if (dragging)
            {
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
        if(GlobalSettings.Instance.deviceMode == DeviceMode.Mobile)
        {
            Vector3 currentTouchPos = MouseInWorldCoords();
            float deltaY = currentTouchPos.y - previousTouchPosition.y;
            touchFingerIsOverMe = true;

            if (deltaY > GlobalSettings.Instance.mouseDragSensitivity && 
                dragging == false && 
                _draggingThis == null)
            {
                dragging = true;
                // when we are dragging something, all previews should be off
                HoverPreview.PreviewsAllowed = false;
                _draggingThis = this;
                da.OnStartDrag();
                zDisplacement = -CameraManager.Instance.MainCamera.transform.position.z + transform.position.z;
                pointerDisplacement = -transform.position + MouseInWorldCoords();
            }

            // get mobile input data
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // did player lift the finger off the screen?
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    // they did, start handling drag/drop logic
                    if (dragging)
                    {
                        dragging = false;
                        touchFingerIsOverMe = false;
                        // turn all previews back on
                        HoverPreview.PreviewsAllowed = true;
                        _draggingThis = null;
                        da.OnEndDrag();
                    }
                }
            }          
            

            previousTouchPosition = currentTouchPos;
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
    #endregion

}
