using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMaster : MonoBehaviour
{
    public static StaticMaster Instance;

    [Header("Table Element References")]
    public PlayerArea playerArea;

    private void Awake()
    {
        Instance = this;
    }
}
