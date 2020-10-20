using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Navicontrol : MonoBehaviour
{
   public int page;
   
   public Transform[] naviTransform;
   public Transform navi;
   
   public void Prev()
   {
      if (page <= 0) return;
      page--;
      SetNavi(page);
   }

   public void Next()
   {
      if (page >= naviTransform.Length - 1) return;
      page++;
      SetNavi(page);
   }

   public void SetNavi(int page)
   {
      this.page = page;
      float time = 0.2f;
      navi.transform.DOKill();
      navi.transform.DOScale(new Vector3(3f, 0.5f, 1f), time/2).SetEase(Ease.OutCubic);
      navi.transform.DOScale(new Vector3(1f, 1f, 1f), time/2).SetEase(Ease.OutCubic).SetDelay(time/2);
      
      navi.transform.DOMove(naviTransform[page].position, time).SetEase(Ease.InOutCubic);
   }
   
   void Update()
   {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         SetNavi(0);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         SetNavi(1);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha3))
      {
         SetNavi(2);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha4))
      {
         SetNavi(3);
      }
   }
}
