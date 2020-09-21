using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class CameraManager : Singleton<CameraManager>
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;

    [Header("Small Shake Properties")]
    public float sMagnitude;
    public float sRoughness;
    public float sDuration;

    [Header("Medium Shake Properties")]
    public float mMagnitude;
    public float mRoughness;
    public float mDuration;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateSmallCameraShake();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CreateMediumCameraShake();
        }
    }   

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
    public void CreateCameraShake(CameraShakeType shakeType)
    {
        if(shakeType == CameraShakeType.Small)
        {
            CreateSmallCameraShake();
        }
        else if (shakeType == CameraShakeType.Medium)
        {
            CreateMediumCameraShake();
        }
    }
    public void CreateSmallCameraShake()
    {
        Debug.Log("Shaking camera...");
        CameraShaker.Instance.ShakeOnce(sMagnitude, sRoughness, 0.1f, sDuration);
    }
    public void CreateMediumCameraShake()
    {
        Debug.Log("Shaking camera...");
        CameraShaker.Instance.ShakeOnce(mMagnitude, mRoughness, 0.1f, mDuration);
    }
    #endregion
}
