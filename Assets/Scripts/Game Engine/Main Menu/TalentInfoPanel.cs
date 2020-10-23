using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TalentInfoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region
    [Header("General Components")]
    [SerializeField] private TextMeshProUGUI talentNameText;
    [SerializeField] private Image talentImage;

    [Header("Info Popup Components")]
    [SerializeField] private TextMeshProUGUI infoPopupText;
    [SerializeField] private GameObject infoPopupParent;
    [SerializeField] private CanvasGroup infoPopupCG;
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

    // Input Listeners
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPopupCG.alpha = 0;
        infoPopupParent.SetActive(true);
        infoPopupCG.DOFade(1f, 0.25f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        infoPopupParent.SetActive(false);
    }
    #endregion
}
