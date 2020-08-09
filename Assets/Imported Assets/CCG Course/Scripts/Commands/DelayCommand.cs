using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DelayCommand : Command 
{
    float delay;

    public DelayCommand(float timeToWait)
    {
        delay = timeToWait;    
    }

    public override void StartCommandExecution()
    {
        Sequence s = DOTween.Sequence();
        s.PrependInterval(delay);
        s.OnComplete(Command.CommandExecutionComplete);
    }
}
