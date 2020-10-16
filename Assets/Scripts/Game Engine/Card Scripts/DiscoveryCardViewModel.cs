using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DiscoveryCardViewModel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [Header("Component References")]
    public CardViewModel cardViewModel;
    public Transform scalingParent;

    [Header("Scaling Properties")]
    [SerializeField] float originalScale;
    [SerializeField] float endScale;
    [SerializeField] float scaleSpeed;

    [Header("Card References")]
    public CardData myDataRef;
    public Card myCardRef;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CardController.Instance.DiscoveryScreenIsActive ||
             MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            return;
        }
        cardViewModel.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
        AudioManager.Instance.PlaySound(Sound.Card_Discarded);

        if(myDataRef != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(myDataRef.keyWordModels);
        }
        else if(myCardRef != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(myCardRef.keyWordModels);
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CardController.Instance.DiscoveryScreenIsActive ||
             MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            return;
        }

        cardViewModel.movementParent.DOScale(originalScale, scaleSpeed).SetEase(Ease.OutQuint);
        KeyWordLayoutController.Instance.FadeOutMainView();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CardController.Instance.DiscoveryScreenIsActive ||
             MainMenuController.Instance.AnyMenuScreenIsActive())
        {
            return;
        }

        KeyWordLayoutController.Instance.FadeOutMainView();
        CardController.Instance.OnDiscoveryCardClicked(this);
    }

    public void ResetSelfOnEventComplete()
    {
        myCardRef = null;
        myDataRef = null;
        cardViewModel.movementParent.localScale = new Vector3(originalScale, originalScale, 1f);
        gameObject.SetActive(false);
    }
}
