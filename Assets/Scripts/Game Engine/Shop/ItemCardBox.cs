using UnityEngine;
using TMPro;
using DG.Tweening;
public class ItemCardBox : MonoBehaviour
{
    [Header("Components")]
    public GameObject visualParent;
    public CardViewModel cvm;
    public TextMeshProUGUI goldCostText;
    public GameObject onSaleVisualParent;

    [Header("Properties")]
    public ItemPricePairing ippData;
    [SerializeField] float originalScale;
    [SerializeField] float endScale;
    [SerializeField] float scaleSpeed;

    public void OnCardMouseEnter()
    {
        if (ShopController.Instance.MainCg.alpha < 1)
        {
            return;
        }

        DOTween.Kill(cvm.movementParent);
        cvm.movementParent.DOScale(endScale, scaleSpeed).SetEase(Ease.OutQuint);
        AudioManager.Instance.PlaySoundPooled(Sound.Card_Discarded);

        if (ippData.itemData != null)
        {
            KeyWordLayoutController.Instance.BuildAllViewsFromKeyWordModels(ippData.itemData.keyWordModels);
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

        ShopController.Instance.OnShopItemBoxClicked(this);
    }
}
public class ItemPricePairing
{
    public int goldCost;
    public ItemData itemData;
    public bool onSale = false;

    public ItemPricePairing(ItemData data)
    {
        itemData = data;
        if (data.itemRarity == Rarity.Common)
        {
            goldCost = RandomGenerator.NumberBetween(GlobalSettings.Instance.commonItemCostLowerLimit, GlobalSettings.Instance.commonItemCostUpperLimit);
        }
        else if (data.itemRarity == Rarity.Rare)
        {
            goldCost = RandomGenerator.NumberBetween(GlobalSettings.Instance.rareItemCostLowerLimit, GlobalSettings.Instance.rareItemCostUpperLimit);
        }
        else if (data.itemRarity == Rarity.Epic)
        {
            goldCost = RandomGenerator.NumberBetween(GlobalSettings.Instance.epicItemCostLowerLimit, GlobalSettings.Instance.epicItemCostUpperLimit);
        }
    }
}
