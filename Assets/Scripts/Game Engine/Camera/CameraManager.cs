using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
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

    // Camera Shake Logic
    #region
    public void CreateSmallCameraShake()
    {
        CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1);
    }
    #endregion
}
