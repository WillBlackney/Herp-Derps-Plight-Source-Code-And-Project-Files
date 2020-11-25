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
        //DOTween.Kill(cardViewModel.movementParent);
        //cardViewModel.movementParent.DOKill();
        cardViewModel.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);

        if (myDataRef != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(myDataRef.keyWordModels);
        }
        else
        {
            Debug.LogWarning("data ref is null");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //DOTween.Kill(cardViewModel.movementParent);
        //cardViewModel.movementParent.DOKill();
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
        //cardViewModel.movementParent.DOKill();
        DOTween.Kill(cardViewModel.movementParent);
        cardViewModel.movementParent.localScale = new Vector3(originalScale, originalScale, 1f);
    }
}
