using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadCardDragger : Singleton<KeypadCardDragger>
{
    private CardViewModel currentlyDragged;

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
     };
    public void ClearCurrentlyDragged()
    {
        currentlyDragged = null;
    }
    private CharacterEntityModel EntityActivated
    {
        get { return ActivationManager.Instance.EntityActivated; }
    }

    private void Update()
    {
        if(GlobalSettings.Instance.deviceMode == DeviceMode.Desktop)
        {
            ListenForRightMouseDown();
            ListenForLeftMouseDown();
            ListenForNewNumberKeyPressed();
        }
       
    }
    private void ListenForNewNumberKeyPressed()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                int numberPressed = i + 1;
                HandleKeyPressedStart(i + 1);
                Debug.Log("Pressed " + numberPressed);
                break;
            }
        }
    }
    private void ListenForRightMouseDown()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && currentlyDragged)
        {
            currentlyDragged.draggable.TriggerOnMouseUp(true);
            ClearCurrentlyDragged();
        }
    }
    private void ListenForLeftMouseDown()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentlyDragged)
        {
            currentlyDragged.draggable.TriggerOnMouseUp();
            ClearCurrentlyDragged();
        }
    }
    private void HandleKeyPressedStart(int key)
    {
        if (!EntityIsValid())
            return;

        // Check number pad key pressed is out of range
        if (EntityActivated.hand.Count < key)
            return;

        // Get card + cvm
        Card c = EntityActivated.hand[key - 1];
        if (c == null)
            return;

        CardViewModel cvm = c.cardVM;
        if (cvm == null ||
           (cvm != null && cvm.draggingActions.CanDrag == false))
            return;

        if (currentlyDragged)
        {
            currentlyDragged.draggable.TriggerOnMouseUp(true);
        }

        currentlyDragged = cvm;
        currentlyDragged.draggable.TriggerOnMouseDown();
    }

    private bool EntityIsValid()
    {
        bool bRet = false;
        if (EntityActivated != null &&
           EntityActivated.controller == Controller.Player)
        {
            bRet = true;
        }

        return bRet;
    }
    

}
