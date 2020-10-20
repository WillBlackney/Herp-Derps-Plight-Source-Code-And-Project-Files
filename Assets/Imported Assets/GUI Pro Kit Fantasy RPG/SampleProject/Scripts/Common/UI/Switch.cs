using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FantasyRPG
{

    public class Switch : MonoBehaviour, IPointerClickHandler
    {
        //bool value
        public bool isOn = false;
        
        //Background
        private Image imageBgOn;
        
        //Handle
        private Transform handle;
        private Image imageHandleOn;
        
        //Text
        private TextMeshProUGUI textSwitchOff;
        private TextMeshProUGUI textSwitchOn;


        private void Awake()
        {
            handle = transform.GetChild(2).transform;
            imageHandleOn = handle.transform.GetChild(1).GetComponent<Image>();
            textSwitchOff = transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            textSwitchOn = transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            imageBgOn = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        }

        public void Start()
        {
            SetSwitch(0f);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            isOn = !isOn;
            SetSwitch();
        }


        public  void SetSwitch(float time = 0.15f)
        {
            if (isOn)
            {
                SetOn(time);
            }
            else
            {
                SetOff(time);
            }
        }

        public void SetOn(float time)
        {
            imageBgOn.DOKill();
            imageBgOn.DOFade(1f, time).SetEase(Ease.OutCubic);

            imageHandleOn.DOKill();
            imageHandleOn.DOFade(1f, time).SetEase(Ease.OutCubic);
                
            handle.DOKill();
            handle.DOLocalMoveX(50f, time).SetEase(Ease.InOutCubic);

            textSwitchOff.DOFade(0f, time).SetEase(Ease.OutCubic);
            textSwitchOn.DOFade(1f, time).SetEase(Ease.OutCubic);
        }

        public void SetOff(float time)
        {
            imageBgOn.DOKill();
            imageBgOn.DOFade(0f, time).SetEase(Ease.OutCubic);

            imageHandleOn.DOKill();
            imageHandleOn.DOFade(0f, time).SetEase(Ease.OutCubic);
                
            handle.DOKill();
            handle.DOLocalMoveX(-50f, time).SetEase(Ease.InOutCubic);
                
            textSwitchOff.DOFade(1f, time).SetEase(Ease.OutCubic);
            textSwitchOn.DOFade(0f, time).SetEase(Ease.OutCubic);
        }


    }
}
