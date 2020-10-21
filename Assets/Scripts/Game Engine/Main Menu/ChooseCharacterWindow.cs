using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ChooseCharacterWindow : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI classNameText;

    [Header("Model References")]
    public UniversalCharacterModel myUCM;

    [Header("Properties")]
    private List<CharacterTemplateSO> selectableTemplates = new List<CharacterTemplateSO>();
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

            // populate templates list
            selectableTemplates = MainMenuController.Instance.selectableCharacterTemplates;

            // set starting view state
            SetMyTemplate(selectableTemplates[0]);
        }
    }
    #endregion

    // On Button Click Logic
    #region
    public void OnNextTemplateButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SetMyTemplate(GetNextTemplate());
    }
    public void OnPreviousTemplateButtonClicked()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SetMyTemplate(GetPreviousTemplate());
    }
    #endregion

    // View logic
    #region
    private void BuildMyViewsFromTemplate(CharacterTemplateSO template)
    {
        nameText.text = template.myName;
        classNameText.text = "The " + template.myClassName;
        CharacterModelController.BuildModelFromStringReferences(myUCM, template.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(template.serializedItemManager, myUCM);
    }
    #endregion

    // Get + Set Templates
    #region
    private void SetMyTemplate(CharacterTemplateSO template)
    {
        currentTemplateSelection = template;
        BuildMyViewsFromTemplate(template);
    }
    private CharacterTemplateSO GetNextTemplate()
    {
        CharacterTemplateSO templateReturned = null;

        int currentIndex = selectableTemplates.IndexOf(currentTemplateSelection);
        if(currentIndex == selectableTemplates.Count - 1)
        {
            templateReturned = selectableTemplates[0];
        }
        else
        {
            templateReturned = selectableTemplates[currentIndex + 1];
        }

        return templateReturned;
    }
    private CharacterTemplateSO GetPreviousTemplate()
    {
        CharacterTemplateSO templateReturned = null;

        int currentIndex = selectableTemplates.IndexOf(currentTemplateSelection);
        if (currentIndex == 0)
        {
            templateReturned = selectableTemplates[selectableTemplates.Count - 1];
        }
        else
        {
            templateReturned = selectableTemplates[currentIndex - 1];
        }

        return templateReturned;
    }
    #endregion


}
