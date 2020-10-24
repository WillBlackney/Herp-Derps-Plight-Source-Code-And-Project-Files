using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class LootScreenCardViewModel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardViewModel.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
        AudioManager.Instance.PlaySound(Sound.Card_Discarded);

        if (myDataRef != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(myDataRef.keyWordModels);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardViewModel.movementParent.DOScale(originalScale, scaleSpeed).SetEase(Ease.OutQuint);
        KeyWordLayoutController.Instance.FadeOutMainView();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        KeyWordLayoutController.Instance.FadeOutMainView();
        LootController.Instance.OnLootCardViewModelClicked(this);
    }

    public void ResetSelfOnEventComplete()
    {
        myDataRef = null;
        cardViewModel.movementParent.localScale = new Vector3(originalScale, originalScale, 1f);
        gameObject.SetActive(false);
    }
}
