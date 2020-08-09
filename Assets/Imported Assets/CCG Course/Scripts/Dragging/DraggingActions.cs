using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour
{
    protected CardViewModel cardVM;
    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {
            return CardController.Instance.IsCardPlayable(cardVM.card, cardVM.card.owner) &&
                  !ActionManager.Instance.UnresolvedCombatActions() &&
                  !Command.CardDrawPending();
        }

    }

    protected abstract bool DragSuccessful();
}
