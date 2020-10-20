using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRPG
{

    public class PanelLoading : MonoBehaviour
    {
        public Slider slider;
        public TextMeshProUGUI textSlider;

        public CanvasGroup canvasGroup;

        
        private void Awake()
        {
            transform.SetParent(PlayManager.Instance.transform);
            slider.maxValue = 100;
        }
        
        public void Show()
        {
            SetSlider(0);
            canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutCubic);
            
            DOTween.To(() => slider.value, x => slider.value = x, 100, 2f).SetEase(Ease.InOutSine);
        }

        private void Update()
        {
            SetSlider((int)slider.value);
        }

        public void Hide()
        {
            SetSlider(0);
            canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
        }


        void SetSlider(int value)
        {
            print("SetValue");
            textSlider.text = String.Format("Loading... {0}%", value);
            slider.value = value;
        }
    }
}
