using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class StateCardBox : MonoBehaviour
{
    [Header("Components")]
    public GameObject visualParent;
    public CardViewModel cvm;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    [Header("Properties")]
    [HideInInspector] public StateData myStateData;
    [SerializeField] float originalScale;
    [SerializeField] float endScale;
    [SerializeField] float scaleSpeed;
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]

    // Input Listeners
    #region
    public void OnCardMouseEnter()
    {
        // DOTween.Kill(cvm.movementParent);
        cvm.movementParent.DOKill();
        cvm.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);
        
        if (myStateData != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(myStateData.keyWordModels);
        }
        
    }
    public void OnCardMouseExit()
    {
        cvm.movementParent.DOKill();
        cvm.movementParent.DOScale(originalScale, scaleSpeed).SetEase(Ease.OutQuint);
        KeyWordLayoutController.Instance.FadeOutMainView();
    }
    public void OnCardMouseClick()
    {
        /*
        if (ShopController.Instance.MainCg.alpha < 1)
        {
            return;
        }

        ShopController.Instance.OnShopItemBoxClicked(this);
        */

        ChooseStateWindowController.Instance.HandleStateCardInShrineEventClick(this);
    }
    #endregion
}
