using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownViewController : Singleton<TownViewController>
{
    // Properties + Component References
    #region
    [SerializeField] GameObject mainVisualParent;
    #endregion


    // View Logic
    #region
    public void ShowMainTownView()
    {
        mainVisualParent.SetActive(true);
    }
    public void HideMainTownView()
    {
        mainVisualParent.SetActive(false);
    }
    #endregion


}
