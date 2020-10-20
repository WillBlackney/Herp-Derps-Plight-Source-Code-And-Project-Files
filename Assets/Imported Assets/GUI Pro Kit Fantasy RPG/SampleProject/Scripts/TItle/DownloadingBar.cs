using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRPG
{

   public class DownloadingBar : MonoBehaviour
   {
      private Slider sliderDownload;
      private CanvasGroup canvasGroup;
      public TextMeshProUGUI textDownlod;

      private int maxSize;


      public void InIt(int maxSize)
      {
         sliderDownload = GetComponent<Slider>();
         this.maxSize = maxSize;
         sliderDownload.maxValue = maxSize;

         canvasGroup = GetComponent<CanvasGroup>();
         canvasGroup.DOFade(0f, 0f);
      }

      public void Show()
      {
         gameObject.SetActive(true);
         canvasGroup.DOKill();
         canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutCubic);
      }

      public void Hide()
      {
         canvasGroup.DOKill();
         canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).OnComplete(() => { gameObject.SetActive(false); });
      }

      public void SetBar(int value)
      {
         textDownlod.text = string.Format("<color=#00FFFF>Downloading...</color>{0}MB/{1}MB",
            Utility.ChangeMoneyString(value), Utility.ChangeMoneyString(maxSize));
         sliderDownload.value = value;
      }
   }
}
