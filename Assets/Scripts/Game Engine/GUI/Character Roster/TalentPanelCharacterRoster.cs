using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TalentPanelCharacterRoster : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Components
    #region
    [Header("Properties")]
    [SerializeField] TalentSchool talentScool;
    [SerializeField] Color textHighlightColour;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI tierText;
    [SerializeField] TextMeshProUGUI talentNameText;
    [SerializeField] GameObject plusButtonParent;
    [SerializeField] Image talentSchoolImage;
    #endregion

    // Getters
    #region
    public TalentSchool TalentSchool
    {
        get { return talentScool; }
    }
    public GameObject PlusButtonParent
    {
        get { return plusButtonParent; }
        private set { PlusButtonParent = value; }
    }
    #endregion

    // Set Views
    #region
    public void SetTierText(string text)
    {
        tierText.text = text;

        // Set text colouring automatically
        if(text == "0")
        {
            tierText.color = Color.white;
        }
        else
        {
            tierText.color = textHighlightColour;
        }
    }
    public void SetTalentNameText(string text)
    {
        talentNameText.text = text;
    }
    public void SetPlusButtonActiveState(bool onOrOff)
    {
        plusButtonParent.SetActive(onOrOff);
    }
    public void SetMyImage(Sprite sprite)
    {
        talentSchoolImage.sprite = sprite;
    }
    #endregion

    // Input Listeners + Logic
    #region
    public void OnPlusButtonClicked()
    {
        Debug.LogWarning("OnPlusButtonClicked");
        CharacterRosterViewController.Instance.OnTalentPanelPlusButtonClicked(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        CharacterRosterViewController.Instance.OnTalentPanelMouseEnter(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CharacterRosterViewController.Instance.OnTalentPanelMouseExit(this);
    }
    #endregion
}
