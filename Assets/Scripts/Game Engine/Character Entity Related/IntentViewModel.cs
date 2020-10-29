using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class IntentViewModel : MonoBehaviour
{
    [Header("General Component References")]
    public GameObject visualParent;
    [SerializeField] private Image intentImageHolder;
    [SerializeField] private Animator animator;
    public CanvasGroup myCg;
    public TextMeshProUGUI valueText;
    public GameObject sizingParent;

    public void PlayFloatAnimation()
    {
        animator.SetTrigger("Float");
    }
    public void SetIntentSprite(Sprite sprite)
    {
        intentImageHolder.sprite = sprite;
    }
}
