using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements;
using HeroEditor.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface
{
    /// <summary>
    /// High-level shop interface.
    /// </summary>
    public class ShopBase : ItemWorkspace
    {
        public ScrollInventory Trader;
        public ScrollInventory Bag;
        public InputField AmountInput;
        public Button BuyButton;
        public Button SellButton;
        public AudioSource AudioSource;
        public AudioClip TradeSound;
        public AudioClip NoMoney;
        public Character Dummy;
        public bool CanBuy = true;
        public bool CanSell = true;
        public bool ExampleInitialize;

        public string CurrencyId = "Gold";
	    public Action<Item> OnRefresh; // Can be used to customize shop behaviour;
        public Action<Item> OnBuy;
        public Action<Item> OnSell;
        public int Amount;

        public void Start()
        {
            Bag.OnLeftClick = Trader.OnLeftClick = SelectItem;
            Bag.OnRightClick = Trader.OnRightClick = Bag.OnDoubleClick = Trader.OnDoubleClick = QuickAction;

            if (ExampleInitialize)
            {
                var spriteCollection = SpriteCollection.Instances.ContainsKey("FantasyHeroes") ? SpriteCollection.Instances["FantasyHeroes"] : SpriteCollection.Instances.First().Value;

                IconCollection.Active = IconCollection.Instances[spriteCollection.Id]; // This is very important step.

                GenerateRandomItemsJustForExample(spriteCollection, ItemCollection.Instance);

                var trader = ItemCollection.Instance.Dict.Select(i => new Item(i.Key)).ToList();
                var inventory = new List<Item> { new Item("Gold", 1000) };

                Trader.Initialize(ref trader);
                Bag.Initialize(ref inventory);

                if (!Trader.SelectAny() && !Bag.SelectAny())
                {
                    ItemInfo.Reset();
                }
            }
        }

        public void Initialize(ref List<Item> traderItems, ref List<Item> playerItems)
        {
            Trader.Initialize(ref traderItems);
            Bag.Initialize(ref playerItems);

            if (!Trader.SelectAny() && !Bag.SelectAny())
            {
                ItemInfo.Reset();
            }
        }

        public int GetPrice(Item item) // Modify this function to fit your needs!
        {
            var trader = Trader.Items.Contains(item);
            var price = item.Params.Price * Amount;

            if (trader)
            {
                price *= GetTraderMarkup(item);
            }

            return price;
        }

        public static int GetTraderMarkup(Item item) // Modify this function to fit your needs!
        {
            if (item.Params.Rarity > ItemRarity.Common) return 2;

            switch (item.Params.Type)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                case ItemType.Helmet:
                case ItemType.Shield:
                case ItemType.Backpack: return 3;
                default: return 2;
            }
        }

	    public void SelectItem(Item item)
        {
            SelectedItem = item;
            SetAmount(1);
            ItemInfo.Initialize(SelectedItem, GetPrice(SelectedItem), Trader.Items.Contains(item));
            Refresh();
        }

        public void QuickAction(Item item)
        {
            SelectItem(item);

            if (Trader.Items.Contains(item))
            {
                Buy();
            }
            else
            {
                Sell();
            }
        }

        public void Buy()
        {
			if (!BuyButton.gameObject.activeSelf || !BuyButton.interactable || !CanBuy) return;

            var price = GetPrice(SelectedItem);

            if (GetCurrency(Bag, CurrencyId) < price)
            {
                Debug.LogWarning("You don't have enough gold!");
                AudioSource.PlayOneShot(NoMoney, SfxVolume);

                return;
            }

            OnBuy?.Invoke(SelectedItem);
            AddMoney(Bag, -price, CurrencyId);
			AddMoney(Trader, price, CurrencyId);
            MoveItem(SelectedItem, Trader, Bag, Amount, currencyId: CurrencyId);
            AudioSource.PlayOneShot(TradeSound, SfxVolume);
        }

        public void Sell()
        {
	        if (!SellButton.gameObject.activeSelf || !SellButton.interactable) return;

            var price = GetPrice(SelectedItem);

            if (GetCurrency(Trader, CurrencyId) < price)
            {
                Debug.LogWarning("Trader doesn't have enough gold!");
                AudioSource.PlayOneShot(NoMoney, SfxVolume);

                return;
            }

            OnSell?.Invoke(SelectedItem);
            AddMoney(Bag, price, CurrencyId);
            AddMoney(Trader, -price, CurrencyId);
            MoveItem(SelectedItem, Bag, Trader, Amount, currencyId: CurrencyId);
            AudioSource.PlayOneShot(TradeSound, SfxVolume);
        }

        public override void Refresh()
        {
            if (SelectedItem == null)
            {
                ItemInfo.Reset();
                BuyButton.SetActive(false);
                SellButton.SetActive(false);
            }
            else
            {
                if (Trader.Items.Contains(SelectedItem))
                {
                    InitBuy();
                }
                else if (Bag.Items.Contains(SelectedItem))
                {
                    InitSell();
                }
                else if (Trader.Items.Any(i => i.Hash == SelectedItem.Hash))
                {
                    InitBuy();
                }
                else if (Bag.Items.Any(i => i.Hash == SelectedItem.Hash))
                {
                    InitSell();
                }
            }

            OnRefresh?.Invoke(SelectedItem);
        }

        public void SetMinAmount()
        {
            SetAmount(1);
        }

        public void IncAmount(int value)
        {
            SetAmount(Amount + value);
        }

        public void SetMaxAmount()
        {
            SetAmount(SelectedItem.Count);
        }

        public void OnAmountChanged(string value)
        {
            if (value.IsEmpty()) return;

            SetAmount(int.Parse(value));
        }

        public void OnAmountEndEdit(string value)
        {
            if (value.IsEmpty())
            {
                SetAmount(1);
            }
        }

        private void SetAmount(int amount)
        {
            Amount = Mathf.Max(1, Mathf.Min(SelectedItem.Count, amount));
            AmountInput?.SetTextWithoutNotify(Amount.ToString());
            ItemInfo.UpdatePrice(SelectedItem, GetPrice(SelectedItem), Trader.Items.Contains(SelectedItem));
        }

        private void InitBuy()
        {
            BuyButton.SetActive(SelectedItem.Params.Type != ItemType.Currency && SelectedItem.Count > 0 && !SelectedItem.Params.Tags.Contains(ItemTag.NotForSale) && !SelectedItem.Params.Tags.Contains(ItemTag.Quest) && CanBuy);
            SellButton.SetActive(false);
            //BuyButton.interactable = GetCurrency(Bag, CurrencyId) >= SelectedItem.Params.Price;
        }

        private void InitSell()
        {
            BuyButton.SetActive(false);
            SellButton.SetActive(SelectedItem.Count > 0 && !SelectedItem.Params.Tags.Contains(ItemTag.NotForSale) && !SelectedItem.Params.Tags.Contains(ItemTag.Quest) && SelectedItem.Id != CurrencyId && CanSell);
            //SellButton.interactable = GetCurrency(Trader, CurrencyId) >= SelectedItem.Params.Price;
        }

        public static long GetCurrency(ItemContainer bag, string currencyId)
        {
            var currency = bag.Items.SingleOrDefault(i => i.Id == currencyId);

            return currency?.Count ?? 0;
        }

        private static void AddMoney(ItemContainer inventory, int value, string currencyId)
        {
            var currency = inventory.Items.SingleOrDefault(i => i.Id == currencyId);

            if (currency == null)
            {
                inventory.Items.Insert(0, new Item(currencyId, value));
            }
            else
            {
                currency.Count += value;

                if (currency.Count == 0)
                {
                    inventory.Items.Remove(currency);
                }
            }
        }
    }
}