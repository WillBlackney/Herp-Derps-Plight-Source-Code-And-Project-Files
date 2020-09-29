using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DiscoveryCardViewModel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [Header("Component References")]
    public CardViewModel cardViewModel;

    [Header("Scaling Properties")]
    [SerializeField] float originalScale;
    [SerializeField] float endScale;
    [SerializeField] float scaleSpeed;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.LogWarning("OnPointerEnter");
        cardViewModel.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.LogWarning("OnPointerExit");
        cardViewModel.movementParent.DOScale(originalScale, scaleSpeed).SetEase(Ease.OutQuint);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogWarning("OnPointerClick");
        CardController.Instance.OnDiscoveryCardClicked(this);
    }
}
