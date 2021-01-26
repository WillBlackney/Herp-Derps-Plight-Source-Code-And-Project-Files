using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Scrollable item container that can display item list. Automatic vertical scrolling.
    /// </summary>
    public class ScrollInventory : ItemContainer
    {
        public ScrollRect ScrollRect;
        public GridLayoutGroup Grid;
        public InventoryItem ItemPrefab;
	    public InventoryItem ItemNoSpritesPrefab;
		public GameObject CellPrefab;
		public bool AddEmptyCells = true;
        public bool Extend;
        public bool HideCountLabels;
        public Func<Item, bool> GenericFilter;

        public static string IconCollectionId = "FantasyHeroes";

        private readonly List<ItemType> _sortByItemType = new List<ItemType>
        {
            ItemType.Currency,
            ItemType.Supply,
            ItemType.Weapon,
            ItemType.Armor,
            ItemType.Helmet,
            ItemType.Shield,
            ItemType.Backpack,
            ItemType.Jewelry,
            ItemType.Loot,
            ItemType.Recipe,
            ItemType.Material
		};
        private Dictionary<Item, InventoryItem> _inventoryItems = new Dictionary<Item, InventoryItem>(); // Reusing instances to reduce Instantiate() calls.
	    private List<GameObject> _emptyCells = new List<GameObject>();
	    private bool _initialized;
        private int _hash;

        public void Initialize(ref List<Item> items, Item selected, bool reset = false)
        {
            base.Initialize(ref items, selected);

			if (reset) _hash = 0;
		}

        public void Initialize(ref List<Item> items, bool reset = false)
        {
            base.Initialize(ref items);
            ResetNormalizedPosition();

            if (reset) _hash = 0;
        }

        public void SelectItem(Item item)
        {
            if (_inventoryItems.ContainsKey(item))
            {
                _inventoryItems[item].Select(true);
            }
        }

        public bool SelectAny()
        {
            if (_inventoryItems.Count > 0)
            {
                _inventoryItems.First().Value.Select(true);
                
                return true;
            }

            return false;
        }

		public void SetTypeFilter(string input)
        {
            var type = input.ToEnum<ItemType>();

			SetTypeFilter(new List<ItemType> { type });
        }

		public void SetTypeFilter(List<ItemType> types)
        {
            GenericFilter = item => types.Contains(item.Params.Type);
			Refresh(null, force: true);
        }

        public void UnsetFilter()
        {
            GenericFilter = null;
            Refresh(null, force: true);
        }

		public override void Refresh(Item selected)
        {
            Refresh(selected, force: false);
        }

        public void Refresh(Item selected, bool force)
		{
            if (Items == null) return;

            var inventoryItems = new Dictionary<Item, InventoryItem>();
	        var emptyCells = new List<GameObject>();
			var items = Items.OrderBy(i => _sortByItemType.Contains(i.Params.Type) ? _sortByItemType.IndexOf(i.Params.Type) : 0).ToList();
            var groups = items.GroupBy(i => i.Params.Type);

            items = new List<Item>();

            foreach (var group in groups)
            {
                items.AddRange(group.OrderBy(i => i.Params.Price));
            }

            if (GenericFilter != null)
            {
                items.RemoveAll(i => !GenericFilter(i));
			}

            if (!force && _initialized && _hash == JsonConvert.SerializeObject(items).GetHashCode())
            {
                var dict = new Dictionary<Item, InventoryItem>();

                for (var i = 0; i < items.Count; i++)
                {
                    var inventoryItem = _inventoryItems.ElementAt(i).Value;

                    inventoryItem.Item = items[i];
					dict.Add(items[i], inventoryItem);
				}

                _inventoryItems = dict;

                return;
            }

            foreach (var item in items)
            {
                InventoryItem inventoryItem;
                
                if (_inventoryItems.ContainsKey(item))
	            {
                    inventoryItem = _inventoryItems[item];
		            inventoryItem.transform.SetAsLastSibling();
					inventoryItems.Add(item, inventoryItem);
		            _inventoryItems.Remove(item);
				}
	            else
	            {
                    inventoryItem = Instantiate(item.Params.Path.IsEmpty() ? ItemNoSpritesPrefab : ItemPrefab, Grid.transform);
                    inventoryItem.Container = this;
                    inventoryItem.Item = item;
                    inventoryItems.Add(item, inventoryItem);
                }

                inventoryItem.Count.text = item.Count.ToString();
                inventoryItem.Count.enabled = !HideCountLabels;

                if (SelectOnRefresh) inventoryItem.Select(item == selected);
			}

			if (AddEmptyCells)
	        {
		        var columns = 0;
		        var rows = 0;

		        switch (Grid.constraint)
		        {
			        case GridLayoutGroup.Constraint.FixedColumnCount:
			        {
				        var height = Mathf.FloorToInt((ScrollRect.GetComponent<RectTransform>().rect.height + Grid.spacing.y) / (Grid.cellSize.y + Grid.spacing.y));

				        columns = Grid.constraintCount;
				        rows = Mathf.Max(height, Mathf.FloorToInt((float) items.Count / columns));

                        if (Extend) rows++;

						break;
			        }
			        case GridLayoutGroup.Constraint.FixedRowCount:
			        {
				        var width = Mathf.FloorToInt((ScrollRect.GetComponent<RectTransform>().rect.width + Grid.spacing.x) / (Grid.cellSize.x + Grid.spacing.x));

				        rows = Grid.constraintCount;
				        columns = Mathf.Max(width, Mathf.FloorToInt((float) items.Count / rows));

                        if (Extend) columns++;

						break;
			        }
		        }

		        for (var i = items.Count; i < columns * rows; i++)
		        {
			        var existing = _emptyCells.LastOrDefault();

			        if (existing != null)
			        {
				        existing.transform.SetAsLastSibling();
				        emptyCells.Add(existing);
				        _emptyCells.Remove(existing);
			        }
			        else
			        {
				        emptyCells.Add(Instantiate(CellPrefab, Grid.transform));
			        }
		        }
	        }

	        foreach (var instance in _inventoryItems.Values)
	        {
		        DestroyImmediate(instance.gameObject);
	        }

	        foreach (var instance in _emptyCells)
	        {
                DestroyImmediate(instance);
	        }

	        _inventoryItems = inventoryItems;
	        _emptyCells = emptyCells;
			_initialized = true;
		    _hash = JsonConvert.SerializeObject(items).GetHashCode();
        }

        public InventoryItem GetItemById(string id)
        {
            return _inventoryItems.Values.SingleOrDefault(i => i.Item.Id == id);
        }

        public void ResetNormalizedPosition()
        {
            if (ScrollRect.horizontal) ScrollRect.horizontalNormalizedPosition = 0;
            if (ScrollRect.vertical) ScrollRect.verticalNormalizedPosition = 1;
        }
    }
}