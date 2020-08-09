using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardViewModel : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("General Properties")]
    public Card card;

    [Header("General Components")]
    public CardPreviewViewModel myPreviewCard;

    [Header("Text References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI energyText;

    [Header("Image References")]
    public Image graphicImage;
    public Image talentSchoolImage;
    public GameObject talentSchoolParent;

    [Header("Card Type Parent References")]
    public GameObject mAttackParent;
    public GameObject rAttackParent;
    public GameObject skillParent;
    public GameObject powerParent;
    #endregion

    public Defender owner()
    {
        return card.owner;
    }

    // Set View Elements Logic
    #region
    public void SetNameText(string name)
    {
        nameText.text = name;
        if(myPreviewCard != null)
        {
            myPreviewCard.nameText.text = name;
        }
    }
    public void SetDescriptionText(string description)
    {
        descriptionText.text = description;
        if (myPreviewCard != null)
        {
            myPreviewCard.descriptionText.text = description;
        }
    }
    public void SetEnergyText(string energyCost)
    {
        energyText.text = energyCost;
        if (myPreviewCard != null)
        {
            myPreviewCard.energyText.text = energyCost;
        }
    }
    public void SetGraphicImage(Sprite sprite)
    {
        graphicImage.sprite = sprite;
        if (myPreviewCard != null)
        {
            myPreviewCard.graphicImage.sprite = sprite;
        }
    }
    public void SetTalentSchoolImage(Sprite sprite)
    {
        if (sprite)
        {
            talentSchoolParent.SetActive(true);
            talentSchoolImage.sprite = sprite;
            if (myPreviewCard != null)
            {
                myPreviewCard.talentSchoolParent.SetActive(true);
                myPreviewCard.talentSchoolImage.sprite = sprite;
            }
        }
       
    }
    public void SetCardTypeImage(CardType cardType)
    {
        if(cardType == CardType.MeleeAttack)
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

        // do for card preview also
        if(myPreviewCard != null)
        {
            myPreviewCard.SetCardTypeImage(cardType);
        }
    }
    #endregion

}
