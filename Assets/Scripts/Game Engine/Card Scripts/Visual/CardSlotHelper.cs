using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSlotHelper : MonoBehaviour
{
    [SerializeField] Transform t;
    public void UpdateAngles(int myIndex, float middleIndex)
    {
        t.DOKill();
        UpdateRotation(myIndex, middleIndex);
        UpdateYDrop(myIndex, middleIndex);
    }
    private void UpdateRotation(int myIndex, float middleIndex)
    {
        float myDif = myIndex - middleIndex;

        // Rotate left or right
        if (myIndex < middleIndex || myIndex > middleIndex)
            t.DORotate(new Vector3(0, 0, 2f * myDif), 0.2f);

        // Rotate as the centre card
        else
            t.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
    }
    private void UpdateYDrop(int myIndex, float middleIndex)
    {
        float slotStartY = 0f;
        float yStep = 0.08f;
        float myDif = Mathf.Abs(myIndex - middleIndex);
        t.DOLocalMoveY(slotStartY - (yStep * myDif), 0.2f);
    }
    public void ResetAngles()
    {
        t.DOKill();
        t.DORotate(new Vector3(0, 0, 0), 0f);
        t.transform.localPosition = Vector3.zero;
    }
}
