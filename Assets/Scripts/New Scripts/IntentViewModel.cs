using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class IntentViewModel : MonoBehaviour
{
    public GameObject visualParent;
    public CanvasGroup myCg;
    public TextMeshProUGUI valueText;
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
    public IEnumerator FadeInViewCoroutine()
    {
        visualParent.SetActive(true);
        myCg.alpha = 0;

        while (myCg.alpha < 1)
        {
            myCg.alpha += 1 * Time.deltaTime;
            yield return null;
        }
    }
}
