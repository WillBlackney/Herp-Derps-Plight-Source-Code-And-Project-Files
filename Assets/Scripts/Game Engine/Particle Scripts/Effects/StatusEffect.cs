using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class StatusEffect : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI statusText;
    public CanvasGroup myCg;

    public void InitializeSetup(string statusName, Color textColor)
    {
        statusText.text = statusName;
        statusText.color = textColor;
        PlayAnimation();
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
    public void PlayAnimation()
    {
        myCg.alpha = 0;
        myCg.DOFade(1, 0.5f);
        transform.DOLocalMoveY(1, 1.5f);
        transform.DOScale(new Vector2(1.25f, 1.25f), 1);

        Sequence s1 = DOTween.Sequence();
        s1.Append(transform.DOScale(new Vector2(1.25f, 1.25f), 1));
        s1.OnComplete(() => 
        {
            myCg.DOFade(0, 0.5f).OnComplete(() => DestroyThis());
        });
       
    }
}
