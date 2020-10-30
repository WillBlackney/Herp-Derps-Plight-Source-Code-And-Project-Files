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
    [SerializeField] private TextMeshProUGUI infoPopupText;
    #endregion

    // Initialization
    #region
    public void BuildFromTalentPairingModel(TalentPairingModel data)
    {
        gameObject.SetActive(true);
        talentNameText.text = data.talentSchool.ToString() + " +" + data.talentLevel.ToString();
        talentImage.sprite = SpriteLibrary.Instance.GetTalentSchoolSpriteFromEnumData(data.talentSchool);
        infoPopupText.text = TextLogic.GetTalentPanelDescriptionText(data);
    }
    #endregion

}
