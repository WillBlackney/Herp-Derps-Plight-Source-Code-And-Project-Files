using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellOnTarget : DraggingActions {

    [Header("Properties")]
    private VisualStates tempVisualState;

    public override bool CanDrag
    {
        get
        {
            Debug.Log("DragSpellOnTarget.CanDrag() called...");
            return (base.CanDrag);
        }
    }
    public override void OnStartDrag()
    {
        Debug.Log("DragSpellOnTarget.OnStartDrag() called...");
        tempVisualState = locationTracker.VisualState;
        locationTracker.VisualState = VisualStates.Dragging;

        // enable targetting arrow
        TargettingArrow.Instance.EnableArrow(cardVM);

        // move to play preview spot
        if(cardVM.eventSetting == EventSetting.Combat)
        {
            cardVM.mySlotHelper.ResetAngles();
            CardController.Instance.MoveCardVMToPlayPreviewSpot(cardVM, cardVM.card.owner.characterEntityView.handVisual);
        }
        else if (cardVM.eventSetting == EventSetting.Camping)
        {
            cardVM.mySlotHelper.ResetAngles();
            CardController.Instance.MoveCardVMToPlayPreviewSpot(cardVM, CampSiteController.Instance.HandVisual);
        }

        // play sfx
        AudioManager.Instance.FadeInSound(Sound.Card_Dragging, 0.2f);
    }    
    public override void OnEndDrag(bool forceFailure = false)
    {
        Debug.Log("DragSpellOnTarget.OnEndDrag() called...");
        if(cardVM.eventSetting == EventSetting.Combat)
        {
            // Stop dragging SFX
            AudioManager.Instance.FadeOutSound(Sound.Card_Dragging, 0.2f);

            // Set up
            CharacterEntityModel target = null;
            CharacterEntityModel owner = cardVM.card.owner;
            Card card = cardVM.card;

            // Raycast from cam to mouse
            RaycastHit[] hits;
            hits = Physics.RaycastAll(CameraManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition), 1000.0f);

            // Get Character views from raycast hits
            foreach (RaycastHit h in hits)
            {
                if (h.transform.gameObject.GetComponent<CharacterEntityView>())
                {
                    target = h.transform.gameObject.GetComponent<CharacterEntityView>().character;
                }
            }

            // Check for target validity
            bool targetValid = false;
            if (target != null)
            {
                if (card.targettingType == TargettingType.AllCharacters)
                {
                    targetValid = true;
                }
                else if (card.targettingType == TargettingType.Ally &&
                        target.allegiance == Allegiance.Player &&
                        target != owner)
                {
                    targetValid = true;
                }
                else if (card.targettingType == TargettingType.AllyOrSelf &&
                        target.allegiance == Allegiance.Player)
                {
                    targetValid = true;
                }
                else if (card.targettingType == TargettingType.Enemy &&
                        target.allegiance == Allegiance.Enemy)
                {
                    targetValid = true;
                }

                // check the target isnt already dying
                if (target.livingState == LivingState.Dead)
                {
                    targetValid = false;
                }
            }


            // Did we hit a valid target?
            if (!targetValid || forceFailure)
            {
                // not a valid target, return
                locationTracker.VisualState = tempVisualState;
                locationTracker.SetHandSortingOrder();

                // Move this card back to its slot position
                HandVisual PlayerHand = cardVM.card.owner.characterEntityView.handVisual;
                Vector3 oldCardPos = PlayerHand.slots.Children[locationTracker.Slot].transform.localPosition;
                cardVM.movementParent.DOLocalMove(oldCardPos, 0.25f);
                PlayerHand.UpdateCardRotationsAndYDrops();
            }
            else
            {
                // Valid target, play the card against the target
                CardController.Instance.PlayCardFromHand(card, target);
            }

            TargettingArrow.Instance.DisableArrow();

            // return target and arrow to original position
            // this position is special for spell cards to show the arrow on top       
            transform.localPosition = new Vector3(0f, 0f, -0.1f);
        }

        else if(cardVM.eventSetting == EventSetting.Camping)
        {
            // Stop dragging SFX
            AudioManager.Instance.FadeOutSound(Sound.Card_Dragging, 0.2f);

            // Set up
            CampSiteCharacterView target = null;
            CampCard card = cardVM.campCard;

            // Raycast from cam to mouse
            RaycastHit[] hits;
            hits = Physics.RaycastAll(CameraManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition), 1000.0f);

            // Get Character views from raycast hits
            foreach (RaycastHit h in hits)
            {
                if (h.transform.gameObject.GetComponent<CharacterEntityView>())
                {
                    target = h.transform.gameObject.GetComponentInParent<CampSiteCharacterView>();
                }
            }

            // Check for target validity
            bool targetValid = false;
            if (target != null && target.myCharacterData != null)
            {
                targetValid = CampSiteController.Instance.IsTargetValid(target, card);
            }

            // Did we hit a valid target?
            if (!targetValid)
            {
                // not a valid target, return
                locationTracker.VisualState = tempVisualState;
                locationTracker.SetHandSortingOrder();

                // Move this card back to its slot position
                Vector3 oldCardPos = CampSiteController.Instance.HandVisual.slots.Children[locationTracker.Slot].transform.localPosition;
                cardVM.movementParent.DOLocalMove(oldCardPos, 0.25f);
                CampSiteController.Instance.HandVisual.UpdateCardRotationsAndYDrops();
            }
            else
            {
                // Valid target, play the card against the target
                CampSiteController.Instance.PlayCampCardFromHand(card, target);
            }

            TargettingArrow.Instance.DisableArrow();

            // return target and arrow to original position
            // this position is special for spell cards to show the arrow on top       
            transform.localPosition = new Vector3(0f, 0f, -0.1f);
        }

    }

    // NOT USED IN THIS SCRIPT
    public override void OnDraggingInUpdate()
    {
    }
    protected override bool DragSuccessful()
    {
        return true;
    }
}
