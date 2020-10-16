using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeyWordLayoutController : Singleton<KeyWordLayoutController>
{
    // Propeties + Component References
    #region
    [Header("Transform + Parent Component")]
    [SerializeField] private GameObject visualParent;
    [SerializeField] private RectTransform mainGridRect;
    [SerializeField] private RectTransform[] mainColumnRects;

    [Header("Keyword Panel Components")]
    [SerializeField] private KeyWordPanel[] allKeyWordPanels;

    [Header("Canvas Components")]
    [SerializeField] private CanvasGroup mainCg;
    #endregion

    // Build Keyword Panels
    #region
    public void BuildAllViewsFromKeyWordModels(List<KeyWordModel> keyWords)
    {
        // Enable + reset main view
        ResetAllKeyWordPanels();
        EnableMainView();      
        FadeInMainView();

        // build each panel
        for (int i = 0; i < keyWords.Count; i++)
        {
            // Setup
            KeyWordPanel panel = allKeyWordPanels[i];
            KeyWordModel data = keyWords[i];

            // Disable panel image icon
            panel.panelImageParent.SetActive(false);

            // Enable panel parent
            panel.gameObject.SetActive(true);

            // Is this a passive?
            if(data.kewWordType == KeyWordType.Passive)
            {
                BuildKeywordPanelFromPassiveData
                    (panel, PassiveController.Instance.GetPassiveIconDataByName(TextLogic.SplitByCapitals(data.passiveType.ToString())));
            }

            // Else build normally
            else
            {
                BuildKeywordPanelFromModel(panel, data);
            }
            
            // Rebuild content on panel
            panel.RebuildLayout();
        }

        // Rebuild entire GUI
        RebuildEntireLayout();
    }
    public void BuildAllViewsFromPassiveString(string passiveName)
    {
        // Enable + Reset main view
        ResetAllKeyWordPanels();
        EnableMainView();
        FadeInMainView();

        // Get passive data
        PassiveIconData data = PassiveController.Instance.GetPassiveIconDataByName(passiveName);

        // Get and enable first panel
        KeyWordPanel panel = allKeyWordPanels[0];
        panel.gameObject.SetActive(true);

        // Build panel views from data
        BuildKeywordPanelFromPassiveData(panel, data);

        // Rebuild the panel, then rebuild the entire GUI
        panel.RebuildLayout();
        RebuildEntireLayout();
    }
    private void BuildKeywordPanelFromModel(KeyWordPanel panel, KeyWordModel model)
    {
        // Find data
        KeyWordData data = GetMatchingKeyWordData(model);

        // Set text values
        panel.nameText.text = GetKeyWordNameString(data);
        panel.descriptionText.text = GetKeyWordDescriptionString(data);
    }
    private void BuildKeywordPanelFromPassiveData(KeyWordPanel panel, PassiveIconData data)
    {
        // Set texts
        panel.nameText.text = data.passiveName;
        panel.descriptionText.text = TextLogic.ConvertCustomStringListToString(data.passiveDescription);

        // Enable image component if it has sprite data
        panel.panelImageParent.SetActive(true);
        panel.panelImage.sprite = data.passiveSprite;
    }
    #endregion

    // Get data
    #region
    private KeyWordData GetMatchingKeyWordData(KeyWordModel model)
    {
        KeyWordData dataReturned = null;

        foreach (KeyWordData data in KeywordLibrary.Instance.allKeyWordData)
        {
            if (model.kewWordType == data.kewWordType)
            {
                // check weapon req first
                if (model.kewWordType == KeyWordType.WeaponRequirement &&
                    model.weaponRequirement == data.weaponRequirement)
                {
                    dataReturned = data;
                    break;
                }
                else
                {
                    dataReturned = data;
                    break;
                }
            }
        }

        if (dataReturned == null)
        {
            Debug.LogWarning("GetMatchingKeyWordData() could not find a matching key word data object for '" +
                model.ToString() + "' key word modelm, returning null!...");
        }

        return dataReturned;
    }
    private string GetKeyWordDescriptionString(KeyWordData data)
    {
        string stringReturned = "empty";

        if (data.kewWordType == KeyWordType.Passive)
        {
            // do passive stuff
            //stringReturned = data.passiveType.ToString();
        }
        else
        {
            stringReturned = data.keyWordDescription;
        }

        return stringReturned;
    }
    private string GetKeyWordNameString(KeyWordData data)
    {
        string stringReturned = "empty";

        if(data.kewWordType == KeyWordType.Passive)
        {
            // do passive stuff
            stringReturned = data.passiveType.ToString();
        }
        else if (data.kewWordType == KeyWordType.WeaponRequirement)
        {
            stringReturned = TextLogic.SplitByCapitals(data.weaponRequirement.ToString());
        }
        else
        {
            stringReturned = data.kewWordType.ToString();
        }

        return stringReturned;
    }
    #endregion

    // View Logic
    #region
    private void ResetAllKeyWordPanels()
    {
        foreach (KeyWordPanel panel in allKeyWordPanels)
        {
            panel.gameObject.SetActive(false);
        }
    }
    private void RebuildEntireLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(mainGridRect);

        for (int i = 0; i < mainColumnRects.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(mainColumnRects[i]);
        }
    }
    private void EnableMainView()
    {
        visualParent.SetActive(true);
    }
    private void DisableMainView()
    {
        visualParent.SetActive(false);
    }
    private void FadeInMainView()
    {
        Sequence s = DOTween.Sequence();
        s.Append(mainCg.DOFade(1f, 0.25f));
    }
    public void FadeOutMainView()
    {
        DisableMainView();
        mainCg.alpha = 0f;

    }
    #endregion


}
