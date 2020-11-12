using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class BlackScreenController : Singleton<BlackScreenController>
{
    [Header("Component References")]
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private GameObject visualParent;

    [Header("Properties")]
    private bool fadeInProgess;
    public bool FadeInProgress
    {
        get { return fadeInProgess; }
        private set { fadeInProgess = value; }
    }

    public void FadeOutAndBackIn(float outDuration, float middlePause, float inDuration)
    {
        StartCoroutine(FadeOutAndBackInCoroutine(outDuration, middlePause, inDuration));
    }
    private IEnumerator FadeOutAndBackInCoroutine(float outDuration, float middlePause, float inDuration)
    {
        // Fade out and wait
        visualParent.SetActive(true);
        FadeInProgress = true;
        FadeOutScreen(outDuration);
        yield return new WaitForSeconds(outDuration);

        // Middle pause
        yield return new WaitForSeconds(middlePause);

        // Fade back in
        FadeInScreen(inDuration);
        yield return new WaitForSeconds(inDuration);
        FadeInProgress = false;
        visualParent.SetActive(false);
    }
    public void FadeOutAndBackIn(float outDuration, float middlePause, float inDuration, Action onPauseReachedCallBack)
    {
        StartCoroutine(FadeOutAndBackInCoroutine(outDuration, middlePause, inDuration, onPauseReachedCallBack));
    }
    private IEnumerator FadeOutAndBackInCoroutine(float outDuration, float middlePause, float inDuration, Action onPauseReachedCallBack)
    {
        // Fade out and wait
        DisableClickThrough();
        FadeInProgress = true;
        FadeOutScreen(outDuration);
        yield return new WaitForSeconds(outDuration);

        // Middle pause + run callback
        if(onPauseReachedCallBack != null)
        {
            onPauseReachedCallBack.Invoke();
        }
        yield return new WaitForSeconds(middlePause);

        // Fade back in
        FadeInScreen(inDuration);
        yield return new WaitForSeconds(inDuration);
        FadeInProgress = false;
        EnableClickThrough();
    }
    public void FadeOutScreen(float duration)
    {
        fadeInProgess = true;
        cg.alpha = 0;
        DisableClickThrough();
        Sequence s = DOTween.Sequence();
        s.Append(cg.DOFade(1, duration));

        s.OnComplete(() => 
        { 
            EnableClickThrough(); 
            fadeInProgess = false; 
        });
    }
    public void FadeInScreen(float duration)
    {
        // Reset alpha / set transparent
        fadeInProgess = true;
        cg.alpha = 1;
        DisableClickThrough();
        Sequence s = DOTween.Sequence();
        s.Append(cg.DOFade(0, duration));

        s.OnComplete(() =>
        {
            EnableClickThrough();
            fadeInProgess = false;
        });
    }
    public void DoInstantFadeOut()
    {
        cg.alpha = 1;
    }
    public void EnableClickThrough()
    {
        cg.blocksRaycasts = false;
    }
    public void DisableClickThrough()
    {
        cg.blocksRaycasts = true;
    }

}
