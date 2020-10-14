using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarHandle : MonoBehaviour
{
    [SerializeField] Scrollbar mySB;

    private void LateUpdate()
    {
        if (mySB)
        {
            mySB.size = 0.1f;
        }
    }
}
