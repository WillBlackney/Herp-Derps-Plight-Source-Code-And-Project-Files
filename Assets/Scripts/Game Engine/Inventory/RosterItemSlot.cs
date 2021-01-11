using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RosterItemSlot : MonoBehaviour
{
    public ItemData itemDataRef;
    public Image itemImage;
    public RosterSlotType slotType;

    public void OnMouseEnter()
    {
        CharacterRosterViewController.Instance.rosterSlotMousedOver = this;

        if(CharacterRosterViewController.Instance.rosterSlotMousedOver != null)
        {
            Debug.LogWarning("MOUSED OVER SLOT: " + CharacterRosterViewController.Instance.rosterSlotMousedOver.gameObject.name);
        }

        if (itemDataRef == null)
            return;

        CharacterRosterViewController.Instance.BuildAndShowCardViewModelPopupFromRosterItem(itemDataRef);
        KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(itemDataRef.keyWordModels);
    }
    public void OnMouseExit()
    {
        if (CharacterRosterViewController.Instance.rosterSlotMousedOver == this)
            CharacterRosterViewController.Instance.rosterSlotMousedOver = null;

        CharacterRosterViewController.Instance.HidePreviewItemCardInRoster();
        KeyWordLayoutController.Instance.FadeOutMainView();
    }


}
public enum RosterSlotType
{
    None = 0,
    TrinketOne = 1,
    TrinketTwo = 4,
    MainHand = 2,
    OffHand = 3,
}
