using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class IntentViewModel : MonoBehaviour
{
    [Header("General Component References")]
    [SerializeField] private GameObject visualParent;
    [SerializeField] private Image intentImageHolder;
    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup myCg;
    public TextMeshProUGUI valueText;


    public void FadeInView()
    {
        StartCoroutine(FadeInViewCoroutine());
    }
    private IEnumerator FadeInViewCoroutine()
    {
        visualParent.SetActive(true);
        PlayFloatAnimation();
        myCg.alpha = 0;

        while (myCg.alpha < 1)
        {
            myCg.alpha += 1 * Time.deltaTime;
            yield return null;
        }
    }

    private void PlayFloatAnimation()
    {
        animator.SetTrigger("Float");
    }
    public void SetIntentSprite(Sprite sprite)
    {
        intentImageHolder.sprite = sprite;
    }
}
