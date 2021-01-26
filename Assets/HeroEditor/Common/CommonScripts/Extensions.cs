using System;
using System.Collections.Generic;
using System.Linq;
using HeroEditor.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.HeroEditor.Common.CommonScripts
{
    /// <summary>
    /// General purpose extension methods.
    /// </summary>
    public static class Extensions
    {
        public static void SetActive(this Component target, bool active)
        {
            target.gameObject.SetActive(active);
        }

        public static bool IsEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static T ToEnum<T>(this string value) where T : Enum
        {
            if (string.IsNullOrEmpty(value)) return (T) Enum.GetValues(typeof(T)).GetValue(0);

            return (T) Enum.Parse(typeof(T), value);
        }

        public static T Random<T>(this T[] source)
        {
            if (source == null || source.Length == 0) return default;

            return source[UnityEngine.Random.Range(0, source.Length)];
        }

        public static T Random<T>(this List<T> source)
        {
            if (source == null || source.Count == 0) return default;

            return source[UnityEngine.Random.Range(0, source.Count)];
        }

        public static T Random<T>(this List<T> source, int seed)
        {
            UnityEngine.Random.InitState(seed);

            return source[UnityEngine.Random.Range(0, source.Count)];
        }

        public static Sprite FindSprite(this List<SpriteGroupEntry> list, string path)
        {
            return list.SingleOrDefault(i => i.Path == path)?.Sprite;
        }

        public static List<Sprite> FindSprites(this List<SpriteGroupEntry> list, string path)
        {
            return list.SingleOrDefault(i => i.Path == path)?.Sprites;
        }
    }
}