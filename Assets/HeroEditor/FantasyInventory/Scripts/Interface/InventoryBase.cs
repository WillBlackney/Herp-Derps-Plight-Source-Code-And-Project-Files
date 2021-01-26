using System;
using System.Collections.Generic;
using System.Linq;
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
    /// High-level inventory interface.
    /// </summary>
    public class InventoryBase : ItemWorkspace
    {
		public Equipment Equipment;
        public ScrollInventory Bag;
        public ScrollInventory Materials;
        public Button EquipButton;
        public Button RemoveButton;
        public Button CraftButton;
        public Button LearnButton;
        public Button UseButton;
        public AudioClip EquipSound;
        public AudioClip CraftSound;
        public AudioClip UseSound;
        public AudioSource AudioSource;
        public bool InitializeExample;

        public Func<Item, bool> CanEquip = i => true; // Your game can override this.
        public Action<Item> OnEquip;

        public void Start()
        {
            Bag.OnLeftClick = Equipment.OnLeftClick = SelectItem;
            Bag.OnRightClick = Equipment.OnRightClick = QuickAction;

            if (InitializeExample)
            {
                var spriteCollection = SpriteCollection.Instances.ContainsKey("FantasyHeroes") ? SpriteCollection.Instances["FantasyHeroes"] : SpriteCollection.Instances.First().Value;

                IconCollection.Active = IconCollection.Instances[spriteCollection.Id]; // This is very important step.

                GenerateRandomItemsJustForExample(spriteCollection, ItemCollection.Instance);

                var inventory = ItemCollection.Instance.Dict.Select(i => new Item(i.Key)).ToList();
                var equipped = new List<Item>();

                Bag.Initialize(ref inventory);
                Equipment.Initialize(ref equipped);

                if (!Equipment.SelectAny() && !Bag.SelectAny())
                {
                    ItemInfo.Reset();
                }
            }
        }

        public void Initialize(ref List<Item> playerItems, ref  List<Item> equippedItems, int bagSize, Action onRefresh)
        {
            Bag.Initialize(ref playerItems);
            Equipment.SetBagSize(bagSize);
            Equipment.Initialize(ref equippedItems);
            Equipment.OnRefresh = onRefresh;

            if (!Equipment.SelectAny() && !Bag.SelectAny())
            {
                ItemInfo.Reset();
            }
        }

        public void SelectItem(Item item)
        {
            SelectedItem = item;
            ItemInfo.Initialize(SelectedItem, SelectedItem.Params.Price);
            Refresh();
        }

        private void QuickAction(Item item)
        {
            SelectItem(item);

            if (Equipment.Items.Contains(item))
            {
                Remove();
            }
            else if (CanEquipSelectedItem())
            {
                Equip();
            }
        }

        public void Equip()
        {
            if (!CanEquip(SelectedItem)) return;

            var equipped = SelectedItem.IsFirearm
                ? Equipment.Items.Where(i => i.IsFirearm).ToList()
                : Equipment.Items.Where(i => i.Params.Type == SelectedItem.Params.Type && !i.IsFirearm).ToList();

            if (equipped.Any())
            {
                AutoRemove(equipped, Equipment.Slots.Count(i => i.Supports(SelectedItem)));
            }

            if (SelectedItem.IsTwoHanded) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            if (SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());

            if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());
            if (SelectedItem.IsTwoHanded || SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsFirearm).ToList());

            MoveItem(SelectedItem, Bag, Equipment);
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
            OnEquip?.Invoke(SelectedItem);
        }

        public void Remove()
        {
            MoveItem(SelectedItem, Equipment, Bag);
            SelectItem(SelectedItem);
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
        }

        public void Craft()
        {
            var materials = MaterialList;

            if (CanCraft(materials))
            {
                materials.ForEach(i => Bag.Items.Single(j => j.Hash == i.Hash).Count -= i.Count);
                Bag.Items.RemoveAll(i => i.Count == 0);

                var itemId = SelectedItem.Params.FindProperty(PropertyId.Craft).Value;
                var existed = Bag.Items.SingleOrDefault(i => i.Id == itemId && i.Modifier == null);

                if (existed == null)
                {
                    Bag.Items.Add(new Item(itemId));
                }
                else
                {
                    existed.Count++;
                }

                Bag.Refresh(SelectedItem);
                CraftButton.interactable = CanCraft(materials);
                AudioSource.PlayOneShot(CraftSound, SfxVolume);
            }
            else
            {
                Debug.Log("No materials.");
            }
        }

        public void Learn()
        {
            // Implement your logic here!
        }

        public void Use()
        {
            var sound = SelectedItem.Params.Type == ItemType.Coupon ? EquipSound : UseSound;

            if (SelectedItem.Count == 1)
            {
                Bag.Items.Remove(SelectedItem);
                SelectedItem = Bag.Items.FirstOrDefault();

                if (SelectedItem == null)
                {
                    Bag.Refresh(null);
                    SelectedItem = Equipment.Items.FirstOrDefault();

                    if (SelectedItem != null)
                    {
                        Equipment.Refresh(SelectedItem);
                    }
                }
                else
                {
                    Bag.Refresh(SelectedItem);
                }
            }
            else
            {
                SelectedItem.Count--;
                Bag.Refresh(SelectedItem);
            }

            Equipment.OnRefresh?.Invoke();
            AudioSource.PlayOneShot(sound, SfxVolume);
        }

        public override void Refresh()
        {
            if (SelectedItem == null)
            {
                ItemInfo.Reset();
                EquipButton.SetActive(false);
                RemoveButton.SetActive(false);
            }
            else
            {
                var equipped = Equipment.Items.Contains(SelectedItem);

                EquipButton.SetActive(!equipped && CanEquipSelectedItem());
                RemoveButton.SetActive(equipped);
                UseButton.SetActive(CanUse());
            }

            var receipt = SelectedItem != null && SelectedItem.Params.Type == ItemType.Recipe;

            if (CraftButton != null) CraftButton.SetActive(false);
            if (LearnButton != null) LearnButton.SetActive(false);

            if (receipt)
            {
                if (LearnButton == null)
                {
                    var materialSelected = !Bag.Items.Contains(SelectedItem) && !Equipment.Items.Contains(SelectedItem);

                    CraftButton.SetActive(true);
                    Materials.SetActive(materialSelected);
                    Equipment.Scheme.SetActive(!materialSelected);

                    var materials = MaterialList;

                    Materials.Initialize(ref materials);
                }
                else
                {
                    LearnButton.SetActive(true);
                }
            }
        }

        private List<Item> MaterialList => SelectedItem.Params.FindProperty(PropertyId.Materials).Value.Split(',').Select(i => i.Split(':')).Select(i => new Item(i[0], int.Parse(i[1]))).ToList();

        private bool CanEquipSelectedItem()
        {
            return Bag.Items.Contains(SelectedItem) && Equipment.Slots.Any(i => i.Supports(SelectedItem)) && SelectedItem.Params.Class != ItemClass.Booster;
        }

        private bool CanUse()
        {
            return SelectedItem.Params.Class == ItemClass.Booster || SelectedItem.Params.Type == ItemType.Coupon;
        }

        private bool CanCraft(List<Item> materials)
        {
            return materials.All(i => Bag.Items.Any(j => j.Hash == i.Hash && j.Count >= i.Count));
        }

        /// <summary>
        /// Automatically removes items if target slot is busy.
        /// </summary>
        private void AutoRemove(List<Item> items, int max = 1)
        {
            long sum = 0;

            foreach (var p in items)
            {
                sum += p.Count;
            }

            if (sum == max)
            {
                MoveItemSilent(items.LastOrDefault(i => i.Id != SelectedItem.Id) ?? items.Last(), Equipment, Bag);
            }
        }
    }
}