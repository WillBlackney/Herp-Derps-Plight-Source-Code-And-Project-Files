using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour
{
    [SerializeField] protected CardViewModel cardVM;
    [SerializeField] protected CardLocationTracker locationTracker;
    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {
            return CardController.Instance.IsCardPlayable(cardVM.card, cardVM.card.owner);
        }

    }

    protected abstract bool DragSuccessful();
}
