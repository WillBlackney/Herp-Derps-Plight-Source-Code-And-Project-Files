using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour
{
    [SerializeField] protected CardViewModel cardVM;
    [SerializeField] protected CardLocationTracker locationTracker;

    public CardViewModel CardVM()
    {
        return cardVM;
    }
    public abstract void OnStartDrag();

    public abstract void OnEndDrag(bool forceFailure = false);

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {
            // prevent dragging a card that was already dragged and played, but 
            // hasnt been removed from hand yet due to visual event delays
            if (cardVM.eventSetting == EventSetting.Combat &&
                cardVM.card != null && 
                cardVM.card.owner != null && 
                !CardController.Instance.DiscoveryScreenIsActive &&
                !CardController.Instance.ChooseCardScreenIsActive &&
                !MainMenuController.Instance.AnyMenuScreenIsActive())               
            {
                return CardController.Instance.IsCardPlayable(cardVM.card, cardVM.card.owner);
            }
            else if (cardVM.eventSetting == EventSetting.Camping &&
               cardVM.campCard != null)
            {
                return CampSiteController.Instance.IsCampCardPlayable(cardVM.campCard);
            }
            else
            {
                return false;
            }
           
        }

    }

    protected abstract bool DragSuccessful();
}
