using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class VisualEventManager : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static VisualEventManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Properties
    #region
    private List<VisualEvent> eventQueue = new List<VisualEvent>();
    [SerializeField] private float startDelayExtra;
    [SerializeField] private float endDelayExtra;

    #endregion

    // Misc Logic
    #region
    private void Update()
    {
        if (eventQueue.Count > 0 &&
            !eventQueue[0].isPlaying)
        {
            PlayEventFromQueue(eventQueue[0]);
        }
    }
    #endregion

    // Trigger Visual Events
    #region
    private void PlayEventFromQueue(VisualEvent ve)
    {
        Debug.Log("VisualEventManager.PlayEventFromQueue() called, running function: " + ve.eventFunction.Method.Name);
        ve.isPlaying = true;
        StartCoroutine(PlayEventFromQueueCoroutine(ve));
    }
    private IEnumerator PlayEventFromQueueCoroutine(VisualEvent ve)
    {
        Debug.Log("VisualEventManager.PlayEventFromQueueCoroutine() called, running function: " + ve.eventFunction.Method.Name);

        // Start Delay
        Debug.Log("Awaiting start delay time out. Wait time: " + (ve.startDelay + startDelayExtra).ToString());     
        yield return new WaitForSeconds(ve.startDelay + startDelayExtra);

        // Start coroutine, wait until finished
        Debug.Log("Event invocation started....");
        ve.eventFunction.Invoke();
        if(ve.cData != null)
        {
            yield return new WaitUntil(() => ve.cData.CoroutineCompleted() == true);
        }       
        Debug.Log("Event invocation finished....");

        // End delay
        Debug.Log("Awaiting end delay time out. Wait time: " + (ve.endDelay + endDelayExtra).ToString());
        yield return new WaitForSeconds(ve.endDelay + endDelayExtra);

        // Remove from queue
        RemoveEventFromQueue(ve);

    }
    #endregion

    // Modify Queue
    #region
    private void RemoveEventFromQueue(VisualEvent ve)
    {
        Debug.Log("VisualEventManager.RemoveEventFromQueue() called...");
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
    public void CreateVisualEvent(Action eventFunction, CoroutineData cData, QueuePosition position, float startDelay = 0f, float endDelay = 0f)
    {
        // NOTE: This method requires on argument of 'CoroutineData'.
        // this function is only for visual events that have their sequence
        // triggered over time by way of coroutine.
        // if a visual event has no coroutine and resolves instantly when played from the queue,
        // it should be called using the overload function below this function


        Debug.Log("VisualEventManager.CreateVisualEvent() called...");

        VisualEvent vEvent = new VisualEvent(eventFunction, cData, startDelay, endDelay);

        if(position == QueuePosition.Back)
        {
            AddEventToBackOfQueue(vEvent);
        }
        else if (position == QueuePosition.Front)
        {
            AddEventToFrontOfQueue(vEvent);
        }
    }
    public void CreateVisualEvent(Action eventFunction, QueuePosition position, float startDelay = 0f, float endDelay = 0f)
    {
        Debug.Log("VisualEventManager.CreateVisualEvent() called...");

        VisualEvent vEvent = new VisualEvent(eventFunction, null, startDelay, endDelay);

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

}
public enum QueuePosition
{
    Front,
    Back
}
