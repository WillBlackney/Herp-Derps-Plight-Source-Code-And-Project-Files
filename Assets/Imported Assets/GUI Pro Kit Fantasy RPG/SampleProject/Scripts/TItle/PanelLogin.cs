using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FantasyRPG
{

    public class PanelLogin : MonoBehaviour
    {
        public Transform[] buttons;
        public Transform buttonGuest;

        public CtrTitle ctrTitle;

        public void Show(CtrTitle ctr)
        {
            ctrTitle = ctr;

            this.gameObject.SetActive(true);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].DOKill();
                buttons[i].DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetDelay(0.2f * i);
            }

            buttonGuest.DOKill();
            buttonGuest.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetDelay(0.5f);
        }

        public void Hide()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].DOKill();
                buttons[i].DOScale(0f, 0f);
            }

            buttonGuest.DOKill();
            buttonGuest.DOScale(0f, 0f);

            this.gameObject.SetActive(false);
        }

        public void Click_LoginGameCenter()
        {
            Hide();
            ctrTitle.Login();
        }

        public void CLick_LoginFacebook()
        {
            Hide();
            ctrTitle.Login();
        }

        public void Click_LoginGoogle()
        {
            Hide();
            ctrTitle.Login();
        }

        public void Click_LoginGuest()
        {
            Hide();
            ctrTitle.Login();
        }
    }
}
