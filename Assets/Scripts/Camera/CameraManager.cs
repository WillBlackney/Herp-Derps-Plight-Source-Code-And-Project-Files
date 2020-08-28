using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;

    [Header("Properties")]
    [HideInInspector] public GameObject currentLookAtTarget;

    // Property Accessors
    #region
    public Camera MainCamera
    {
        get { return mainCamera; }
        private set { mainCamera = value; }
    }
    #endregion

}
