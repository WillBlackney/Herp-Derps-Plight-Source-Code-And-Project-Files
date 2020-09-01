using UnityEngine;
using System.Collections;

public abstract class DraggingActionsTest : MonoBehaviour {

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {            
            return true;
        }
    }

    protected abstract bool DragSuccessful();
}
