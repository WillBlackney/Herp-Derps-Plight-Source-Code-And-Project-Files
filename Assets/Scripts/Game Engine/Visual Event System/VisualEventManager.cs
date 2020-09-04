using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class VisualEventManager : Singleton<VisualEventManager>
{
    // Properties
    #region
    private List<VisualEvent> eventQueue = new List<VisualEvent>();
    [SerializeField] private float startDelayExtra;
    [SerializeField] private float endDelayExtra;
    private bool paused;
    #endregion

    // Misc Logic
    #region
    private void Update()
    {
        if (eventQueue.Count > 0 &&
            !eventQueue[0].isPlaying && 
            paused == false)
        {
            PlayEventFromQueue(eventQueue[0]);
        }
    }
    #endregion

    // Trigger Visual Events
    #region
    private void PlayEventFromQueue(VisualEvent ve)
    {
        if(ve.eventFunction != null)
        {
            Debug.Log("VisualEventManager.PlayEventFromQueue() called, running function: " + ve.eventFunction.Method.Name);
        }       
        ve.isPlaying = true;
        StartCoroutine(PlayEventFromQueueCoroutine(ve));
    }
    private IEnumerator PlayEventFromQueueCoroutine(VisualEvent ve)
    {
        if (ve.eventFunction != null)
        {
            Debug.Log("VisualEventManager.PlayEventFromQueueCoroutine() called, running function: " + ve.eventFunction.Method.Name);
        }            

        // Start Delay    
        if(ve.startDelay > 0)
        {
            yield return new WaitForSeconds(ve.startDelay + startDelayExtra);
        }       

        // Execute function 
        if(ve.eventFunction != null)
        {
            ve.eventFunction.Invoke();
        }

        // Wait until execution finished finished
        if (ve.cData != null)
        {
            yield return new WaitUntil(() => ve.cData.CoroutineCompleted() == true);
        } 

        // End delay
        if(ve.endDelay > 0)
        {
            yield return new WaitForSeconds(ve.endDelay + endDelayExtra);
        }       

        // Remove from queue
        RemoveEventFromQueue(ve);

    }
    #endregion

    // Modify Queue
    #region
    private void RemoveEventFromQueue(VisualEvent ve)
    {
        Debug.Log("VisualEventManager.RemoveEventFromQueue() called, current queue count = " + (eventQueue.Count - 1).ToString());
        eventQueue.Remove(ve);
    }
    private void AddEventToFrontOfQueue(VisualEvent ve)
    {
        Debug.Log("VisualEventManager.AddEventToFrontOfQueue() called...");
        eventQueue.Insert(0, ve);
    }
    private void AddEventToBackOfQueue(VisualEvent ve)
    {
        Debug.Log("VisualEventManager.AddEventToBackOfQueue() called...");
        eventQueue.Add(ve);
    }
    #endregion

    // Create Events
    #region
    public void CreateVisualEvent(Action eventFunction, CoroutineData cData, QueuePosition position = QueuePosition.Back, float startDelay = 0f, float endDelay = 0f, EventDetail eventDetail = EventDetail.None)
    {
        // NOTE: This method requires on argument of 'CoroutineData'.
        // this function is only for visual events that have their sequence
        // triggered over time by way of coroutine.
        // if a visual event has no coroutine and resolves instantly when played from the queue,
        // it should be called using the overload function below this function


        Debug.Log("VisualEventManager.CreateVisualEvent() called, current queue count = " + (eventQueue.Count + 1).ToString());

        VisualEvent vEvent = new VisualEvent(eventFunction, cData, startDelay, endDelay, eventDetail);

        if(position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
        }
    }
    public void CreateVisualEvent(Action eventFunction, QueuePosition position = QueuePosition.Back, float startDelay = 0f, float endDelay = 0f, EventDetail eventDetail = EventDetail.None)
    {
        Debug.Log("VisualEventManager.CreateVisualEvent() called, current queue count = " + (eventQueue.Count + 1).ToString());

        VisualEvent vEvent = new VisualEvent(eventFunction, null, startDelay, endDelay, eventDetail);

        if (position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
        }
    }
    #endregion

    // Custom Events
    #region
    public void InsertTimeDelayInQueue(float delayDuration, QueuePosition position = QueuePosition.Back)
    {
        VisualEvent vEvent = new VisualEvent(null, null, 0, delayDuration, EventDetail.None);

        if (position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
        }
    }
    #endregion

    // Bools and Queue Checks
    #region
    public bool PendingCardDrawEvent()
    {
        Debug.Log("VisualEventManager.PendingCardDrawEvent() called...");
        bool boolReturned = false;
        foreach(VisualEvent ve in eventQueue)
        {
            if (ve.eventDetail == EventDetail.CardDraw)
            {
                boolReturned = true;
                break;
            }
        }

        Debug.Log("VisualEventManager.PendingCardDrawEvent() returning: " + boolReturned.ToString());
        return boolReturned;
    }
    public bool PendingDefeatEvent()
    {
        Debug.Log("VisualEventManager.PendingDefeatEvent() called...");
        bool boolReturned = false;
        foreach (VisualEvent ve in eventQueue)
        {
            if (ve.eventDetail == EventDetail.GameOverDefeat)
            {
                boolReturned = true;
                break;
            }
        }

        Debug.Log("VisualEventManager.PendingDefeatEvent() returning: " + boolReturned.ToString());
        return boolReturned;
    }
    public bool PendingVictoryEvent()
    {
        Debug.Log("VisualEventManager.PendingVictoryEvent() called...");
        bool boolReturned = false;
        foreach (VisualEvent ve in eventQueue)
        {
            if (ve.eventDetail == EventDetail.GameOverVictory)
            {
                boolReturned = true;
                break;
            }
        }

        Debug.Log("VisualEventManager.PendingVictoryEvent() returning: " + boolReturned.ToString());
        return boolReturned;
    }
    #endregion

    // Disable + Enable Queue 
    #region
    public void EnableQueue()
    {
        paused = false;
    }
    public void PauseQueue()
    {
        paused = true;
    }
    #endregion
}
public enum QueuePosition
{
    Front,
    Back
}
