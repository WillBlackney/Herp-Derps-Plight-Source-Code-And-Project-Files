using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FantasyRPG;
using UnityEngine;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    public Image imageFade;

    private void Awake()
    {
        transform.SetParent(PlayManager.Instance.transform);
    }

    public void FadeIn(float time = 0.5f)
    {
        imageFade.DOFade(1f, time).SetEase(Ease.OutCubic);
    }

    public void FadeOut(float time = 0.5f)
    {
        imageFade.DOFade(1f, time).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
