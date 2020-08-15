using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineData
{
    private bool coroutineCompleted;
    public bool CoroutineCompleted()
    {
        return coroutineCompleted;
    }

    public void MarkAsCompleted()
    {
        coroutineCompleted = true;
    }

    
}
