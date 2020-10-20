using System.Collections.Generic;
using UnityEngine;

namespace FantasyRPG
{

    public class Utility
    {
        /// <summary>
        /// 캔바스 기준 좌표를 월드좌표로 변환해서 가져오기
        /// </summary>
        public static Vector2 GetScreenToWorldPosition(Canvas canvas, Vector3 position)
        {
            Vector3 screenPoint = new Vector3(position.x, position.y, 100.0f);
            return canvas.worldCamera.ScreenToWorldPoint(screenPoint);
        }

        /// <summary>
        /// 랜덤 리스트 섞기
        /// </summary>
        public static List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();
            System.Random r = new System.Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count);
                randomList.Add(inputList[randomIndex]);
                inputList.RemoveAt(randomIndex);
            }

            return randomList;
        }

        /// <summary>
        /// hex값으로 컬러 값 가져오기
        /// </summary>
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
            byte a = 255; //assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new Color32(r, g, b, a);
        }

        public static string ColorToHex(Color color)
        {
            Color32 c = color;
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", c.r, c.g, c.b, c.a);
        }

        /// <summary>
        /// int > 1,000,000 변환 string
        /// </summary>
        public static string ChangeMoneyString(int myScore)
        {
            return string.Format("{0:n0}", myScore);
        }


        /// <summary>
        /// 이름으로 sprite찾아서 가져오기
        /// </summary>
        public static Sprite GetItemSprite(Sprite[] sprites, string name)
        {
            foreach (Sprite sprite in sprites)
            {
                if (sprite.name == name)
                {
                    return sprite;
                }
            }

            return null;
        }
    }
}
