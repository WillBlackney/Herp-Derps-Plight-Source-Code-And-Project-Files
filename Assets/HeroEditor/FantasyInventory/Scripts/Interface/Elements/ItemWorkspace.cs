using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using HeroEditor.Common;
using UnityEngine;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Abstract item workspace. It can be shop or player inventory. Items can be managed here (selected, moved and so on).
    /// </summary>
    public abstract class ItemWorkspace : MonoBehaviour
    {
        public ItemInfo ItemInfo;

        public static float SfxVolume = 1;

        public Item SelectedItem { get; protected set; }
        
        public abstract void Refresh();

        protected void Reset()
        {
            SelectedItem = null;
            ItemInfo.Reset();
        }

        protected void MoveItem(Item item, ItemContainer from, ItemContainer to, int amount = 1, string currencyId = null)
        {
            MoveItemSilent(item, from, to, amount);
            
            var moved = to.Items.Last(i => i.Hash == item.Hash);

            if (from.Expanded)
            {
                SelectedItem = from.Items.LastOrDefault(i => i.Hash == item.Hash) ?? moved;
            }
            else
            {
                if (item.Count == 0)
                {
                    SelectedItem = currencyId == null ? moved : from.Items.Single(i => i.Id == currencyId);
                }
            }

            Refresh();
            from.Refresh(SelectedItem);
            to.Refresh(SelectedItem);
        }

        public void MoveItemSilent(Item item, ItemContainer from, ItemContainer to, int amount = 1)
        {
            if (item.Count <= 0) throw new ArgumentException("item.Count <= 0");
            if (amount <= 0) throw new ArgumentException("amount <= 0");
            if (item.Count < amount) throw new ArgumentException("item.Count < amount");

            if (to.Expanded)
            {
                to.Items.Add(new Item(item.Id, item.Modifier, amount));
            }
            else
            {
                var target = to.Items.SingleOrDefault(i => i.Hash == item.Hash);

                if (target == null)
                {
                    to.Items.Add(new Item(item.Id, item.Modifier, amount));
                }
                else
                {
                    target.Count += amount;
                }
            }

            var moved = to.Items.Last(i => i.Hash == item.Hash);

            if (from.Expanded)
            {
                from.Items.Remove(item);
            }
            else
            {
                item.Count -= amount;

                if (item.Count == 0)
                {
                    from.Items.Remove(item);
                }
            }
        }
        protected void GenerateRandomItemsJustForExample(SpriteCollection spriteCollection, ItemCollection itemCollection)
        {
            itemCollection.GeneratedItems = new List<ItemParams> { new ItemParams { Id = "Gold", Type = ItemType.Currency, Path = "Equipment/Supplies/Basic/Currency/Coin" } };

            for (var i = 0; i < 5; i++)
            {
                var helmet = new ItemParams
                {
                    Id = spriteCollection.Helmet[i].Name,
                    Type = ItemType.Helmet,
                    Price = i,
                    Properties = new List<Property> { new Property(PropertyId.Resistance, i) },
                    Path = spriteCollection.Helmet[i].Path
                };

                var armor = new ItemParams
                {
                    Id = spriteCollection.Armor[i].Name,
                    Type = ItemType.Armor,
                    Price = i,
                    Properties = new List<Property> { new Property(PropertyId.Resistance, i) },
                    Path = spriteCollection.Armor[i].Path
                };

                var weapon = new ItemParams
                {
                    Id = spriteCollection.MeleeWeapon1H[i].Name,
                    Type = ItemType.Weapon,
                    Class = ItemClass.Sword,
                    Price = i,
                    Properties = new List<Property> { new Property(PropertyId.Damage, i) },
                    Path = spriteCollection.MeleeWeapon1H[i].Path
                };

                var bow = new ItemParams
                {
                    Id = spriteCollection.Bow[i].Name,
                    Type = ItemType.Weapon,
                    Class = ItemClass.Bow,
                    Price = i,
                    Properties = new List<Property> { new Property(PropertyId.Damage, i) },
                    Tags = new List<ItemTag> { ItemTag.TwoHanded },
                    Path = spriteCollection.Bow[i].Path
                };

                var shield = new ItemParams
                {
                    Id = spriteCollection.Shield[i].Name,
                    Type = ItemType.Shield,
                    Price = i,
                    Properties = new List<Property> { new Property(PropertyId.Blocking, i) },
                    Path = spriteCollection.Shield[i].Path
                };

                itemCollection.GeneratedItems.Add(helmet);
                itemCollection.GeneratedItems.Add(armor);
                itemCollection.GeneratedItems.Add(weapon);
                itemCollection.GeneratedItems.Add(bow);
                itemCollection.GeneratedItems.Add(shield);
            }

            itemCollection.InitializeDict();
        }
    }
}