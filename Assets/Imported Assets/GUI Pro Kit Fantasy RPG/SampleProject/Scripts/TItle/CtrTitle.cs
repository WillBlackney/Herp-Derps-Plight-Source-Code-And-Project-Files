using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace FantasyRPG
{

    public class CtrTitle : MonoBehaviour
    {
        public DownloadingBar downloadingBar;

        public CanvasGroup title;
        public Transform backGround;
        public PanelLogin panelLogin;
        public GameObject loading;

        public CanvasGroup buttonStart;

        private void Awake()
        {
            panelLogin.Hide();

            title.DOFade(0f, 0f);
            backGround.DOScale(1.05f, 0f);
            buttonStart.DOFade(0f, 0f);
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            //title animation
            title.DOFade(1f, 0.5f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.3f);

            //background animation
            backGround.transform.DOKill();
            backGround.transform.DOScale(1f, 1.5f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(0.5f);

            //data download animation
            yield return StartCoroutine(StartDownloadData());

            //show login panel
            panelLogin.Show(this);
        }


        IEnumerator StartDownloadData()
        {
            int maxSize = 64;
            int value = 0;

            downloadingBar.InIt(maxSize);
            downloadingBar.Show();

            while (value <= maxSize)
            {
                downloadingBar.SetBar(value);
                value += 1;
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1f);
            downloadingBar.Hide();
        }


        public void Login()
        {
            StartCoroutine(LoginCo());
        }

        IEnumerator LoginCo()
        {
            loading.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            loading.SetActive(false);

            title.transform.DOLocalMoveY(43f, 0.5f).SetEase(Ease.OutCubic).SetRelative(true);
            title.transform.DOScale(0.791f, 0.5f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.5f);

            buttonStart.gameObject.SetActive(true);
            buttonStart.DOFade(1f, 1.2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void Click_Start()
        {
            buttonStart.DOKill();
            buttonStart.DOFade(0f, 0f);
            buttonStart.DOFade(1f, 0.2f).SetEase(Ease.Linear).SetLoops(6, LoopType.Yoyo).OnComplete(() =>
            {
                PlayManager.Instance.LoadScene(Data.scene_home);
            });
        }
    }
}
