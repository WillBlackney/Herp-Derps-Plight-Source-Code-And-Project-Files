using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarHandle : MonoBehaviour
{
    [SerializeField] float handleSize = 0.1f;
    [SerializeField] Scrollbar mySB;

    private void LateUpdate()
    {
        if (mySB)
        {
            mySB.size = handleSize;
        }
    }
}
