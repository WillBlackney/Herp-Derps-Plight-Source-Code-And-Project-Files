using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConditions : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Debug Properties")]
    public bool enableDebugLog;
    #endregion

    // Singleton Pattern 
    #region
    public static GlobalConditions Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Initialization + Start
    #region
    public void OnAwake()
    {
        Debugger.SetLoggingState(enableDebugLog);
    }
    #endregion

}
