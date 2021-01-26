using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Represents item when it was selected. Displays icon, name, price and properties.
    /// </summary>
    public class ItemInfo : MonoBehaviour
    {
        public GameObject Selection;
        public GameObject Buttons;
        public Text Name;
        public Text Labels;
        public Text Values;
        public Text Price;
        public Image Icon;

        public Item Item { get; protected set; }

        protected static readonly List<PropertyId> Sorting = new List<PropertyId>
        {
            PropertyId.Damage,
            PropertyId.StaminaMax,
            PropertyId.Blocking,
            PropertyId.Resistance
        };

        public void OnEnable()
        {
            if (Item == null)
            {
                Reset();
            }
        }

        public void Reset()
        {
            Selection.SetActive(false);
            Buttons.SetActive(false);
            Name.text = Labels.text = Values.text = null;

            if (Price) Price.text = null;
        }

        public virtual void Initialize(Item item, int price, bool trader = false)
        {
            Item = item;

            if (item == null)
            {
                Reset();
                return;
            }

            Selection.SetActive(true);
            Buttons.SetActive(true);

            Name.text = item.Params.GetLocalizedName(Application.systemLanguage.ToString());
            Icon.transform.parent.gameObject.SetActive(true);
            Icon.sprite = IconCollection.Instances["FantasyHeroes"].GetIcon(item.Params.Path);
            UpdatePrice(item, price, trader);

            var main = new List<object> { item.Params.Type };

            if (item.Params.Class != ItemClass.Unknown) main.Add(item.Params.Class);

            foreach (var t in item.Params.Tags)
            {
                main.Add(t);
            }

            var dict = new Dictionary<string, object> { { "ItemInfo.Type", string.Join(" / ", main) } };

            if (item.Params.Level >= 0) dict.Add("ItemInfo.Level", item.Params.Level);

            if (item.Modifier != null)
            {
                dict.Add("ItemInfo.Modifier", $"{item.Modifier.Id} [{item.Modifier.Level}]");
            }

            var props = item.Params.Properties.ToList().OrderBy(i => { var index = Sorting.IndexOf(i.Id); return index == -1 ? 999 : index; }).ToList();

            foreach (var p in props)
            {
                switch (p.Id)
                {
                    case PropertyId.Damage:
                        dict.Add($"ItemInfo.{p.Id}", $"{p.Min}-{p.Max}");
                        break;
                    case PropertyId.CriticalChance:
                    case PropertyId.CriticalDamage:
                        dict.Add($"ItemInfo.{p.Id}", $"+{p.Value}%");
                        break;
                    case PropertyId.ChargeTimings:
                        dict.Add($"ItemInfo.{p.Id}", $"{p.Value.Split(',').Length}");
                        break;
                    default:
                        dict.Add($"ItemInfo.{p.Id}", $"{p.Value}");
                        break;
                }
            }

            dict.Add("ItemInfo.Weight", $"{item.Params.Weight / 10f:0.##} kg");

            if (Price && item.Params.Type != ItemType.Currency)
            {
                dict.Add("ItemInfo.Price", $"{item.Params.Price} gold");
            }

            Labels.text = string.Join("\n", dict.Keys);
            Values.text = string.Join("\n", dict.Values);
        }

        public virtual void UpdatePrice(Item item, int price, bool trader)
        {
            if (!Price) return;

            if (item.Params.Type == ItemType.Currency)
            {
                Price.text = null;
            }
            else
            {
                Price.text = trader ? $"Buy price: {price}G" : $"Sell price: {price}G";
            }
        }
    }
}