using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffEffect : MonoBehaviour
{
    [Header("Component References")]    
    public Animator myAnim;
    public Canvas myCanvas;
    public void InitializeSetup(Vector3 location)
    {
        Debug.Log("SETUP");
        transform.position = new Vector2(location.x, location.y);       

    }
    public void InitializeSetup(Vector3 location, int sortingOrder)
    {
        Debug.Log("SETUP");
        transform.position = new Vector2(location.x, location.y);
        myCanvas.sortingOrder = sortingOrder;

    }

    public void DestroyThis()
    {
        Debug.Log("DESTROY");
        Destroy(gameObject);
    }
}
