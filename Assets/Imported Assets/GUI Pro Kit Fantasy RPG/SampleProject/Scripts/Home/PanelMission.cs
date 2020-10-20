using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRPG
{
    public class PanelMission : PanelBase
    {
        public ScrollRect[] scrollRect;
        public TextMeshProUGUI[] textTap;
        public Transform focus;

        public Transform[] menuTransform;
        
        public Color colorDefault;
        public Color colorFocus;

        public Sprite[] spriteDefault;
        public Sprite[] spriteActive;

        public Image[] iconTap;

        public void Click_Tap(int num)
        {
            for (int i = 0; i < iconTap.Length; i++)
            {
                iconTap[i].sprite = spriteDefault[i];
            }

            iconTap[num].sprite = spriteActive[num];
            
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
