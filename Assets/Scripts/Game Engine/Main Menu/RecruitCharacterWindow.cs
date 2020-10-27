using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecruitCharacterWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image glowOutline;

    [Header("Model References")]
    public UniversalCharacterModel myUCM;

    [Header("Color Properties")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color mouseOverColor;

    [Header("Properties")]
    [HideInInspector] public CharacterData myTemplateData;
    #endregion

    // Logic
    #region
    public void BuildMyViewsFromTemplate(CharacterData template)
    {
        myTemplateData = template;
        nameText.text = template.myName;
        classNameText.text = "The " + template.myClassName;
        CharacterModelController.BuildModelFromStringReferences(myUCM, template.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(template.itemManager, myUCM);
    }
    public void DisableGlow()
    {
        glowOutline.gameObject.SetActive(false);
    }
    public void EnableGlow()
    {
        glowOutline.gameObject.SetActive(true);
    }
    #endregion

    // Input Listeners
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        bgImage.color = mouseOverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bgImage.color = normalColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        RecruitCharacterController.Instance.OnRecruitCharacterWindowClicked(this);     

    }
    public void OnRecruitCharacterWindowViewInfoButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        RecruitCharacterController.Instance.OnRecruitCharacterWindowViewInfoButtonClicked(myTemplateData);
    }
    #endregion

}
