using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFXManager : MonoBehaviour
{
    [Header("Component + Prefab References")]
    public GameObject impactPrefabOne;
    public Sprite[] randomImpactSprites;

    public static ImpactVFXManager Instance;
    private void Awake()
    {
        Instance = this;
    }

}
