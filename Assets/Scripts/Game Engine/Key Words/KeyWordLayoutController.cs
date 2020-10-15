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

    // Build Views
    #region
    public void BuildAllViewsFromKeyWordModels(List<KeyWordModel> keyWords)
    {
        // Enable main view
        EnableMainView();
        FadeInMainView();

        // build each panel
        for (int i = 0; i < keyWords.Count; i++)
        {
            KeyWordPanel panel = allKeyWordPanels[i];
            KeyWordModel data = keyWords[i];

            panel.gameObject.SetActive(true);
            BuildKeywordPanelFromModel(panel, data);
            panel.RebuildLayout();
        }

        RebuildEntireLayout();

    }  
    private void BuildKeywordPanelFromModel(KeyWordPanel panel, KeyWordModel model)
    {
        KeyWordData data = GetMatchingKeyWordData(model);

        panel.nameText.text = GetKeyWordNameString(data);
        panel.descriptionText.text = GetKeyWordDescriptionString(data);
    }
    private void RebuildEntireLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(mainGridRect);

        for (int i = 0; i < mainColumnRects.Length; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(mainColumnRects[i]);
        }
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
            stringReturned = data.weaponRequirement.ToString();
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
        //mainCg.alpha = 0f;

        Sequence s = DOTween.Sequence();
        s.Append(mainCg.DOFade(1f, 0.25f));
    }
    public void FadeOutMainView()
    {
        Debug.LogWarning("FadeOutMainView() called...");

        //Sequence s = DOTween.Sequence();
        //s.Append(mainCg.DOFade(0f, 0.25f));
        // s.OnComplete(() => DisableMainView());

        DisableMainView();
        mainCg.alpha = 0f;

    }
    #endregion


}
