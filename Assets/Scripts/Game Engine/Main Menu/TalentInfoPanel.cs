using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TalentInfoPanel : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI talentNameText;
    [SerializeField] private Image talentImage;

    [Header("Pop Up Components")]
    [SerializeField] private TalentInfoPanelPopup popUpOne;
    [SerializeField] private TalentInfoPanelPopup popUpTwo;
    #endregion

    // how to trigger key word panels??

    // Initialization
    #region
    public void BuildFromTalentPairingModel(TalentPairingModel data)
    {
        // Hide pop up panels
        popUpOne.HideMe();
        popUpTwo.HideMe();

        // Build main panel views
        gameObject.SetActive(true);
        talentNameText.text = data.talentSchool.ToString() + " +" + data.talentLevel.ToString();
        talentImage.sprite = SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(data.talentSchool);

        // Build pop up views
        if(data.talentLevel > 1)
        {
            popUpOne.BuildMe(TextLogic.GetTalentPairingTierOneDescriptionText(data.talentSchool));
            popUpTwo.BuildMe(TextLogic.GetTalentPairingTierTwoDescriptionText(data.talentSchool));
        }
        else if (data.talentLevel == 1)
        {
            popUpOne.BuildMe(TextLogic.GetTalentPairingTierOneDescriptionText(data.talentSchool));
        }
    }
    #endregion

}


