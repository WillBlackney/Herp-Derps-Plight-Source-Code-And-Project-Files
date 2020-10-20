using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FantasyRPG
{

    public class ButtonBase : Button, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private bool isDown = false;
        private Transform target;



        public enum ButtonAnimationtype
        {
            None,
            Elastic,
            Back,
        }

        public ButtonAnimationtype buttonAnimType = ButtonAnimationtype.Elastic;

        void Awake()
        {
            target = transform;
        }


        public void OnPointerDown(PointerEventData data)
        {

            isDown = true;
            target.transform.DOKill();
            target.transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutCubic).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isDown) return;
            isDown = false;

            target.transform.DOKill();

            switch (buttonAnimType)
            {
                case ButtonAnimationtype.Elastic:
                    target.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic).SetUpdate(true);
                    break;

                case ButtonAnimationtype.Back:
                    target.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
                    break;

                case ButtonAnimationtype.None:
                    target.transform.DOScale(1f, 0.25f).SetEase(Ease.OutCubic).SetUpdate(true);
                    break;
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (!isDown) return;
            isDown = false;

            target.transform.DOKill();

            switch (buttonAnimType)
            {
                case ButtonAnimationtype.Elastic:
                    target.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic).SetUpdate(true);
                    break;

                case ButtonAnimationtype.Back:
                    target.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
                    break;

                case ButtonAnimationtype.None:
                    target.transform.DOScale(1f, 0.25f).SetEase(Ease.OutCubic).SetUpdate(true);
                    break;
            }
        }
    }
}
