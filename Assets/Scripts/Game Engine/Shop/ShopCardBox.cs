using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ShopCardBox : MonoBehaviour
{
    [Header("Components")]
    public GameObject visualParent;
    public CardViewModel cvm;
    public TextMeshProUGUI goldCostText;
    public GameObject onSaleVisualParent;

    [Header("Properties")]
    public CardPricePairing cppData;
    [SerializeField] float originalScale;
    [SerializeField] float endScale;
    [SerializeField] float scaleSpeed;

    public void OnCardMouseEnter()
    {
        if(ShopController.Instance.MainCg.alpha < 1)
        {
            return;
        }

        DOTween.Kill(cvm.movementParent);
        cvm.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);

        if (cppData.cardData != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(cppData.cardData.keyWordModels);
        }
    }
    public void OnCardMouseExit()
    {
        if (ShopController.Instance.MainCg.alpha < 1)
        {
            return;
        }

        DOTween.Kill(cvm.movementParent);
        cvm.movementParent.DOScale(originalScale, scaleSpeed).SetEase(Ease.OutQuint);
        KeyWordLayoutController.Instance.FadeOutMainView();
    }
    public void OnCardMouseClick()
    {
        if (ShopController.Instance.MainCg.alpha < 1)
        {
            return;
        }

        ShopController.Instance.OnShopCardBoxClicked(this);
    }

}

public class ShopContentResultModel
{
    public List<CardPricePairing> cardsData = new List<CardPricePairing>();
    public List<ItemPricePairing> itemsData = new List<ItemPricePairing>();
}

public class CardPricePairing
{
    public int goldCost;
    public CardData cardData;
    public bool onSale = false;

    public CardPricePairing(CardData data)
    {
        cardData = data;
        if(data.rarity == Rarity.Common)
        {
            goldCost = RandomGenerator.NumberBetween(GlobalSettings.Instance.commonCardCostLowerLimit, GlobalSettings.Instance.commonCardCostUpperLimit);
        }
        else if (data.rarity == Rarity.Rare)
        {
            goldCost = RandomGenerator.NumberBetween(GlobalSettings.Instance.rareCardCostLowerLimit, GlobalSettings.Instance.rareCardCostUpperLimit);
        }
        else if (data.rarity == Rarity.Epic)
        {
            goldCost = RandomGenerator.NumberBetween(GlobalSettings.Instance.epicCardCostLowerLimit, GlobalSettings.Instance.epicCardCostUpperLimit);
        }
    }
}

