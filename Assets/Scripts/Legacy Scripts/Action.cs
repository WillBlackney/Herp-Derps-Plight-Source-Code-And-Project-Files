
public class OldCoroutineData 
{
    // This class is used by IEnumerator/Coroutines to send 'yield wait until' instructions back up the stack
    public bool combatAction;

    public bool coroutineCompleted;
    public bool ActionResolved()
    {
        if(coroutineCompleted == true)
        {
            ActionManager.Instance.RemoveActionFromQueue(this);
            return true;
        }
        else
        {
            return false;
        }
    }

    public OldCoroutineData(bool _combatAction = false)
    {
        combatAction = _combatAction;
        if (combatAction)
        {
            ActionManager.Instance.AddActionToQueue(this);
        }

    }
}
