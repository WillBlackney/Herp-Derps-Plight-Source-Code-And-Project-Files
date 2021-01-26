using System.Collections.Generic;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor.FantasyInventory.Scripts
{
    public class ElementComponents : MonoBehaviour
    {
        public Color ColorPhysic;
        public Color ColorMagic;
        public Color ColorFire;
        public Color ColorIce;
        public Color ColorLightning;
        public Color ColorLight;
        public Color ColorDarkness;
        public Color ColorPoison;
        public Color ColorBleeding;
        public Color ColorVampiric;

        public static ElementComponents Instance;

        public void Awake()
        {
            Instance = this;
        }

        public string Display<T>(Dictionary<ElementId, T> dict)
        {
            var parts = new List<string>();

            foreach (var element in dict.Keys)
            {
                parts.Add($"<color=#{ColorUtility.ToHtmlStringRGB(GetElementColor(element))}>{dict[element]}</color>");
            }

            return string.Join(" / ", parts);
        }

        //public string GetColor(ModificatorType modType)
        //{
        //    return ColorUtility.ToHtmlStringRGB(GetElementColor(Modificator.GetElement(modType)));
        //}

        public Color GetElementColor(ElementId element)
        {
            switch (element)
            {
                case ElementId.Physic: return ColorPhysic;
                case ElementId.Magic: return ColorMagic;
                case ElementId.Fire: return ColorFire;
                case ElementId.Ice: return ColorIce;
                case ElementId.Lightning: return ColorLightning;
                case ElementId.Light: return ColorLight;
                case ElementId.Darkness: return ColorDarkness;
                default: return Color.white;
            }
        }
    }
}