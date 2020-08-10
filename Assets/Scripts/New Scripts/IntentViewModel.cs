using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class IntentViewModel : MonoBehaviour
{
    [Header("General Component References")]
    [SerializeField] private GameObject visualParent;
    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup myCg;
    public TextMeshProUGUI valueText;

    [Header("Intent Image References")]
    public Image attackTargetImage;
    public Image defendImage;
    public Image mysteryImage;
    public Image debuffImage;
    public Image buffImage;
    public Image attackAndBuffImage;
    public Image attackAndDefendImage;
    public Image defendAndBuff;

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
}
