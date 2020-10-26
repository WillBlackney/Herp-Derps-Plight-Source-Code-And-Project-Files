using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCharacterWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private TextMeshProUGUI currentIndexText;
    [SerializeField] private TextMeshProUGUI maxIndexText;
    [SerializeField] private Image bgImage;

    [Header("Model References")]
    public UniversalCharacterModel myUCM;

    [Header("Color Properties")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color mouseOverColor;    

    [Header("Properties")]
    public CharacterTemplateSO currentTemplateSelection;
    private bool hasRunInitialSetup = false;
    #endregion

    // Set + Initialization
    #region
    private void OnEnable()
    {
        if(hasRunInitialSetup == false)
        {
            hasRunInitialSetup = true;
            maxIndexText.text = MainMenuController.Instance.selectableCharacterTemplates.Count.ToString();
        }
    }
    #endregion

    // On Button Click Logic
    #region
        /*
    public void OnNextTemplateButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        MainMenuController.Instance.GetAndSetNextAvailableTemplate(this);
    }
    public void OnPreviousTemplateButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        MainMenuController.Instance.GetAndSetPreviousAvailableTemplate(this);
    }
    */
    #endregion

    // View logic
    #region
    private void BuildMyViewsFromTemplate(CharacterTemplateSO template)
    {
        nameText.text = template.myName;
        classNameText.text = "The " + template.myClassName;
        CharacterModelController.BuildModelFromStringReferences(myUCM, template.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(template.serializedItemManager, myUCM);
        currentIndexText.text = (MainMenuController.Instance.selectableCharacterTemplates.IndexOf(currentTemplateSelection) + 1).ToString();
    }
    #endregion

    // Get + Set Templates
    #region
    public void SetMyTemplate(CharacterTemplateSO template)
    {
        currentTemplateSelection = template;
        BuildMyViewsFromTemplate(template);
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
        bgImage.color = normalColor;
        MainMenuController.Instance.BuildWindowFromCharacterTemplateData(currentTemplateSelection);
    }
    #endregion
}
