using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPreviewViewModel : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI energyText;

    [Header("Image References")]
    public Image graphicImage;
    public Image talentSchoolImage;
    public GameObject talentSchoolParent;
    public Canvas canvas;

    [Header("Card Type Parent References")]
    public GameObject mAttackParent;
    public GameObject rAttackParent;
    public GameObject skillParent;
    public GameObject powerParent;

    private void OnEnable()
    {
        SetUpPreviewCanvas();
    }
    private void SetUpPreviewCanvas()
    {
        canvas.overrideSorting = true;
        canvas.sortingOrder = 20;
    }
    public void SetCardTypeImage(CardType cardType)
    {
        if (cardType == CardType.MeleeAttack)
        {
            mAttackParent.SetActive(true);
        }
        else if (cardType == CardType.RangedAttack)
        {
            rAttackParent.SetActive(true);
        }
        else if (cardType == CardType.Skill)
        {
            skillParent.SetActive(true);
        }
        else if (cardType == CardType.Power)
        {
            powerParent.SetActive(true);
        }
    }
}
