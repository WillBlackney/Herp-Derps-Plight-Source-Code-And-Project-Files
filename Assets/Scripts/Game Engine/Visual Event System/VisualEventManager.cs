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
    public List<VisualEvent> EventQueue
    {
        get { return eventQueue; }
        private set { eventQueue = value; }
    }
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
        if (EventQueue.Count > 0 &&
            EventQueue[0].isPlaying == false &&
            paused == false &&
            currentEventPlaying == false)
        {
            PlayEventFromQueue(EventQueue[0]);
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
        if (EventQueue.Contains(ve))
        {
            EventQueue.Remove(ve);
        }
     
    }
    private void AddEventToFrontOfQueue(VisualEvent ve)
    {
        if(EventQueue.Count > 0)
        {
            EventQueue.Insert(1, ve);
        }
        else
        {
            EventQueue.Insert(0, ve);
        }        
    }
    private void AddEventToBackOfQueue(VisualEvent ve)
    {
        EventQueue.Add(ve);
    }
    private void AddEventAfterBatchedEvent(VisualEvent ve, VisualEvent batchedEvent)
    {
        int index = EventQueue.IndexOf(batchedEvent) + 1;
        EventQueue.Insert(index, ve);
    }
    public void ClearEventQueue()
    {
        EventQueue.Clear();
    }
    public VisualEvent HandleEventQueueTearDown()
    {
        // this function is used to make sure no null ref errors
        // occur when we transistion from game scene to menu scene.
        // errors can occur if we destroy the scene while coroutines
        // are still actively running and operating on scene objects.
        // this function allows the game to safely clear the event
        // queue, wait for the current event to finish, then tear down
        // the game scene.

        // TO DO: just realized a problem: not all corutines running from
        // visual events have a coroutine data tracker. They start their coroutine 
        // and get removed from the queue straight away. If an event like this is playing
        // when we tear down the scene, the logic wont know to wait for it to finish
        // since it isnt being tracked, and we will probably get errors!! This could sort
        // of be fixed by waiting for a second or two when we fade the screen out, since most
        // coroutines that arent tracked dont last longer then 2 seconds.

        VisualEvent handleReturned = null;

        PauseQueue();
        if(EventQueue.Count > 0 && currentEventPlaying == true)
        {
            handleReturned = EventQueue[0];
        }

        ClearEventQueue();
        EnableQueue();

        return handleReturned;
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

        VisualEvent vEvent = new VisualEvent(eventFunction, cData, startDelay, endDelay, eventDetail);

        if(position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
        }
        else if (position == QueuePosition.BatchedEvent)
        {
            AddEventAfterBatchedEvent(vEvent, batchedEvent);
        }

        return vEvent;
    }
    public VisualEvent CreateVisualEvent(Action eventFunction, QueuePosition position = QueuePosition.Back, float startDelay = 0f, float endDelay = 0f, EventDetail eventDetail = EventDetail.None, VisualEvent batchedEvent = null)
    {
        VisualEvent vEvent = new VisualEvent(eventFunction, null, startDelay, endDelay, eventDetail);

        if (position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
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

        return vEvent;
    }
    #endregion

    // Bools and Queue Checks
    #region
    public bool PendingCardDrawEvent()
    {
        bool boolReturned = false;
        foreach(VisualEvent ve in EventQueue)
        {
            if (ve.eventDetail == EventDetail.CardDraw)
            {
                boolReturned = true;
                break;
            }
        }

        return boolReturned;
    }
    public bool PendingDefeatEvent()
    {
        bool boolReturned = false;
        foreach (VisualEvent ve in EventQueue)
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
        bool boolReturned = false;
        foreach (VisualEvent ve in EventQueue)
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

