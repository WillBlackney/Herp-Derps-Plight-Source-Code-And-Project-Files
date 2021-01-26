using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Represents hero (player) equipment. Based on equipment slots.
    /// </summary>
    public class Equipment : ItemContainer
    {
        public Action OnRefresh;

        /// <summary>
        /// Defines what kinds of items can be equipped.
        /// </summary>
        public List<ItemSlot> Slots;

        /// <summary>
        /// Equipped items will be instantiated in front of equipment slots.
        /// </summary>
        public InventoryItem ItemPrefab;

	    /// <summary>
	    /// Character preview.
	    /// </summary>
		public Character Preview;

        public Transform Scheme;
        public int BagSize;

        public readonly List<InventoryItem> InventoryItems = new List<InventoryItem>(); 

        public void OnValidate()
        {
            if (Application.isPlaying) return;

            Slots = GetComponentsInChildren<ItemSlot>(true).ToList();

            //if (Character == null)
            //{
            //    Character = FindObjectOfType<Character>();
            //}
        }

	    public void Start()
	    {
		    //Character.Animator.SetBool("Ready", false);
		}

        public void SetBagSize(int size)
        {
            BagSize = size;

            var supplies = GetComponentsInChildren<ItemSlot>(true).Where(i => i.Types.Contains(ItemType.Supply)).ToList();

            for (var i = 0; i < supplies.Count; i++)
            {
                supplies[i].Locked = i >= size;
            }
        }

        public bool SelectAny()
        {
            if (InventoryItems.Count > 0)
            {
                InventoryItems[0].Select(true);

                return true;
            }

            return false;
        }

        public override void Refresh(Item selected)
        {
            var items = Slots.Select(FindItem).Where(i => i != null).ToList();

            Reset();

            foreach (var slot in Slots)
            {
                var item = FindItem(slot);

                slot.gameObject.SetActive(item == null);

                if (item == null) continue;

                var inventoryItem = Instantiate(ItemPrefab, slot.transform.parent);

                inventoryItem.Container = this;
                inventoryItem.Item = item;
                inventoryItem.Count.text = null;
                inventoryItem.transform.position = slot.transform.position;
                inventoryItem.transform.SetSiblingIndex(slot.transform.GetSiblingIndex());
                
                if (SelectOnRefresh) inventoryItem.Select(item == selected);

                InventoryItems.Add(inventoryItem);
            }

            if (Preview)
            {
                CharacterInventorySetup.Setup(Preview, items);
                Preview.Initialize();
            }

            OnRefresh?.Invoke();
        }

        private void Reset()
        {
            foreach (var inventoryItem in InventoryItems)
            {
                Destroy(inventoryItem.gameObject);
            }

            InventoryItems.Clear();
        }

        private Item FindItem(ItemSlot slot)
        {
            if (slot.Types.Contains(ItemType.Shield))
            {
                var copy = Items.SingleOrDefault(i => i.Params.Type == ItemType.Weapon && (i.IsTwoHanded || i.IsFirearm));

                if (copy != null)
                {
                    return copy;
                }
            }

            var index = Slots.Where(i => i.Types.SequenceEqual(slot.Types)).ToList().IndexOf(slot);
            var items = Items.Where(slot.Supports).ToList();

            return index < items.Count ? items[index] : null;
        }
    }
}