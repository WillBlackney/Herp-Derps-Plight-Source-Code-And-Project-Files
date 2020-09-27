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
    private bool currentEventPlaying;
    #endregion

    // Misc Logic
    #region
    private void Update()
    {
        PlayNextEventFromQueue();
    }
    #endregion

    // Trigger Visual Events
    #region
    private void PlayEventFromQueue(VisualEvent ve)
    {        
        ve.isPlaying = true;
        StartCoroutine(PlayEventFromQueueCoroutine(ve));
    }
    private void PlayNextEventFromQueue()
    {
        if (eventQueue.Count > 0 &&
            eventQueue[0].isPlaying == false &&
            paused == false &&
            currentEventPlaying == false)
        {
            PlayEventFromQueue(eventQueue[0]);
        }
    }
    private IEnumerator PlayEventFromQueueCoroutine(VisualEvent ve)
    {
        currentEventPlaying = true;

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
        currentEventPlaying = false;

        // Start next event
        PlayNextEventFromQueue();

    }
    #endregion

    // Modify Queue
    #region
    private void RemoveEventFromQueue(VisualEvent ve)
    {
        eventQueue.Remove(ve);
    }
    private void AddEventToFrontOfQueue(VisualEvent ve)
    {
        if(eventQueue.Count > 0)
        {
            eventQueue.Insert(1, ve);
        }
        else
        {
            eventQueue.Insert(0, ve);
        }        
    }
    private void AddEventToBackOfQueue(VisualEvent ve)
    {
        eventQueue.Add(ve);
    }
    private void AddEventAfterNextCoroutine(VisualEvent ve)
    {
        int index = 0;

        for(int i  = 0; i < eventQueue.Count; i++)
        {
            if(eventQueue[i].cData != null)
            {
                index = i;
                break;
            }
        }

        Debug.LogWarning("Inserting event at index: " + (index + 1).ToString());

        eventQueue.Insert(index + 1, ve);
    }
    private void AddEventAfterBatchedEvent(VisualEvent ve, VisualEvent batchedEvent)
    {
        int index = 0;

        for (int i = 0; i < eventQueue.Count; i++)
        {
            if (eventQueue[i] == batchedEvent)
            {
                index = i;
                break;
            }
        }

        eventQueue.Insert(index + 1, ve);
    }
    #endregion

    // Create Events
    #region
    public VisualEvent CreateVisualEvent(Action eventFunction, CoroutineData cData, QueuePosition position = QueuePosition.Back, float startDelay = 0f, float endDelay = 0f, EventDetail eventDetail = EventDetail.None, VisualEvent batchedEvent = null)
    {
        // NOTE: This method requires on argument of 'CoroutineData'.
        // this function is only for visual events that have their sequence
        // triggered over time by way of coroutine.
        // if a visual event has no coroutine and resolves instantly when played from the queue,
        // it should be called using the overload function below this function


        //Debug.Log("VisualEventManager.CreateVisualEvent() called, current queue count = " + (eventQueue.Count + 1).ToString());

        VisualEvent vEvent = new VisualEvent(eventFunction, cData, startDelay, endDelay, eventDetail);

        if(position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
        }
        else if (position == QueuePosition.AfterNextCoroutine)
        {
            AddEventAfterNextCoroutine(vEvent);
        }
        else if (position == QueuePosition.BatchedEvent)
        {
            AddEventAfterBatchedEvent(vEvent, batchedEvent);
        }

        return vEvent;
    }
    public VisualEvent CreateVisualEvent(Action eventFunction, QueuePosition position = QueuePosition.Back, float startDelay = 0f, float endDelay = 0f, EventDetail eventDetail = EventDetail.None, VisualEvent batchedEvent = null)
    {
        //Debug.Log("VisualEventManager.CreateVisualEvent() called, current queue count = " + (eventQueue.Count + 1).ToString());

        VisualEvent vEvent = new VisualEvent(eventFunction, null, startDelay, endDelay, eventDetail);

        if (position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
        }
        else if (position == QueuePosition.AfterNextCoroutine)
        {
            AddEventAfterNextCoroutine(vEvent);
        }
        else if (position == QueuePosition.BatchedEvent)
        {
            AddEventAfterBatchedEvent(vEvent, batchedEvent);
        }

        return vEvent;

    }
    #endregion

    // Custom Events
    #region
    public VisualEvent InsertTimeDelayInQueue(float delayDuration, QueuePosition position = QueuePosition.Back)
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
        else if (position == QueuePosition.AfterNextCoroutine)
        {
            AddEventAfterNextCoroutine(vEvent);
        }

        return vEvent;
    }
    #endregion

    // Bools and Queue Checks
    #region
    public bool PendingCardDrawEvent()
    {
        //Debug.Log("VisualEventManager.PendingCardDrawEvent() called...");
        bool boolReturned = false;
        foreach(VisualEvent ve in eventQueue)
        {
            if (ve.eventDetail == EventDetail.CardDraw)
            {
                boolReturned = true;
                break;
            }
        }

        //Debug.Log("VisualEventManager.PendingCardDrawEvent() returning: " + boolReturned.ToString());
        return boolReturned;
    }
    public bool PendingDefeatEvent()
    {
        //Debug.Log("VisualEventManager.PendingDefeatEvent() called...");
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
        //Debug.Log("VisualEventManager.PendingVictoryEvent() called...");
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

