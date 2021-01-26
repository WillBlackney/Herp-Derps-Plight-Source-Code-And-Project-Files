using System.Collections.Generic;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Represents equipment slot. Inventory items can be placed here.
    /// </summary>
    public class ItemSlot : MonoBehaviour
    {
        public Image Icon;
        public Image Background;
        public Sprite ActiveSprite;
        public Sprite LockedSprite;
        public List<ItemType> Types;
        public List<ItemClass> Classes;

        public bool Locked
        {
            get => Icon.sprite == LockedSprite;
            set
            {
                Icon.sprite = value ? LockedSprite : ActiveSprite;
                Background.color = value ? new Color32(150, 150, 150, 255) : new Color32(255, 255, 255, 255);
            }
        }

        public bool Supports(Item item)
        {
            return Types.Contains(item.Params.Type) && (Classes.Count == 0 || Classes.Contains(item.Params.Class)) && !Locked;
        }

        //public void OnValidate()
        //{
        //    if (gameObject.activeSelf)
        //    {
        //        Types = new List<ItemType> { name == "Slot" ? ItemType.Undefined : name.ToEnum<ItemType>() };
        //    }
        //}
    }
}