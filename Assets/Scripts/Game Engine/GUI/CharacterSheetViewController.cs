using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheetViewController : Singleton<CharacterSheetViewController>
{
    // Properties + Components
    #region
    [SerializeField] private GameObject mainVisualParent;
    #endregion

    // Getters + Accessors
    #region
    public GameObject MainVisualParent
    {
        get { return mainVisualParent; }
    }
    #endregion

    // Show Views
    #region
    public void ShowMainView()
    {
        MainVisualParent.SetActive(true);
    }
    public void HideMainView()
    {
        MainVisualParent.SetActive(false);
    }
    public void BuildViewsFromCharacterData(CharacterData data)
    {
        ShowMainView();
    }

    #endregion


    // Input + Click Logic
    #region
    public void OnCloseWindowButtonClicked()
    {
        HideMainView();
    }
    #endregion
}
