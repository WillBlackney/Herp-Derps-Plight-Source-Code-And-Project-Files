using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRPG
{
    public class PanelRanking : PanelBase
    {
        public ScrollRect[] scrollRect;
        public TextMeshProUGUI[] textTap;
        public Transform focus;

        public Transform[] menuTransform;
        
        public Color colorDefault;
        public Color colorFocus;

        
        public void Click_Tap(int num)
        {
            for (int i = 0; i < scrollRect.Length; i++)
            {
                if (i == num)
                {
                    scrollRect[i].gameObject.SetActive(true);
                    textTap[i].color = colorFocus;
                }
                else
                {
                    scrollRect[i].gameObject.SetActive(false);
                    textTap[i].color = colorDefault;
                }
            }
         
            focus.transform.DOMoveY(menuTransform[num].transform.position.y, 0.2f).SetEase(Ease.InOutCubic);

        }
    }
}
