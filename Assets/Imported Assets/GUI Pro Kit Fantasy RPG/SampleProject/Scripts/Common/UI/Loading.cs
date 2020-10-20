using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FantasyRPG
{

    public class Loading : MonoBehaviour
    {
        public Transform[] loadingDot;
        private Coroutine _coroutine;
        private int _loadingCount = 0;

        public float dotAnimTime = 1f;
        public float dotAnimDistanceTime = 0.2f;

        public void Show()
        {
            for (int i = 0; i < loadingDot.Length; i++)
            {
                loadingDot[i].DOKill();
                loadingDot[i].DOScale(0.2f + 0.8f / loadingDot.Length * i, 0f).SetUpdate(true);
                loadingDot[i].transform.DOScale(0.2f, dotAnimTime / loadingDot.Length * i).SetEase(Ease.InSine)
                    .SetUpdate(true);
            }

            this.gameObject.SetActive(true);
            StartCoroutine(LoadingCo());
        }

        IEnumerator LoadingCo()
        {
            while (true)
            {
                loadingDot[_loadingCount].DOKill();
                loadingDot[_loadingCount].DOScale(1f, 0f);
                yield return new WaitForSeconds(dotAnimDistanceTime);

                loadingDot[_loadingCount].transform.DOScale(0.2f, dotAnimTime).SetEase(Ease.InSine);
                _loadingCount += 1;

                if (_loadingCount > loadingDot.Length - 1)
                {
                    _loadingCount = 0;
                }
            }
        }

        public void Hide()
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }

    }
}
