using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingArrow : Singleton<TargettingArrow>
{

#pragma warning disable 649
    [SerializeField]
    private GameObject bodyPrefab;
    [SerializeField]
    private GameObject headPrefab;

#pragma warning restore 649

    private const int NumPartsTargetingArrow = 17;
    private readonly List<GameObject> arrow = new List<GameObject>(NumPartsTargetingArrow);
    private Camera mainCamera;
    private bool isArrowEnabled;
    private CardViewModel cvm;

    private void Start()
    {
        for (var i = 0; i < NumPartsTargetingArrow - 1; i++)
        {
            var body = Instantiate(bodyPrefab, gameObject.transform);
            arrow.Add(body);
        }

        var head = Instantiate(headPrefab, gameObject.transform);
        arrow.Add(head);

        foreach (var part in arrow)
            part.SetActive(false);

        mainCamera = CameraManager.Instance.MainCamera;
    }

    public void EnableArrow(CardViewModel cardVM)
    {
        cvm = cardVM;
        isArrowEnabled = true;
        foreach (var part in arrow)
            part.SetActive(true);
    }
    public void DisableArrow()
    {
        cvm = null;
        isArrowEnabled = false;
        foreach (var part in arrow)
            part.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!isArrowEnabled || !cvm)
            return;

        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var mouseX = mousePos.x;
        var mouseY = mousePos.y;

        // const float centerX = 0.0f;
        //const float centerY = -4.0f;
        float centerX = cvm.mainParent.position.x;
        float centerY = cvm.mainParent.position.y;

        var controlAx = centerX - (mouseX - centerX) * 0.3f;
        var controlAy = centerY + (mouseY - centerY) * 0.8f;
        var controlBx = centerX + (mouseX - centerX) * 0.1f;
        var controlBy = centerY + (mouseY - centerY) * 1.4f;

        for (var i = 0; i < arrow.Count; i++)
        {
            var part = arrow[i];

            var t = (i + 1) * 1.0f / arrow.Count;
            var tt = t * t;
            var ttt = tt * t;
            var u = 1.0f - t;
            var uu = u * u;
            var uuu = uu * u;

            var arrowX = uuu * centerX +
                         3 * uu * t * controlAx +
                         3 * u * tt * controlBx +
                         ttt * mouseX;
            var arrowY = uuu * centerY +
                         3 * uu * t * controlAy +
                         3 * u * tt * controlBy +
                         ttt * mouseY;

            arrow[i].transform.position = new Vector3(arrowX, arrowY, 0.0f);

            float lenX;
            float lenY;
            if (i > 0)
            {
                lenX = arrow[i].transform.position.x - arrow[i - 1].transform.position.x;
                lenY = arrow[i].transform.position.y - arrow[i - 1].transform.position.y;
            }
            else
            {
                lenX = arrow[i + 1].transform.position.x - arrow[i].transform.position.x;
                lenY = arrow[i + 1].transform.position.y - arrow[i].transform.position.y;
            }

            part.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(lenX, lenY) * Mathf.Rad2Deg);

            part.transform.localScale = new Vector3(
                1.0f - 0.03f * (arrow.Count - 1 - i),
                1.0f - 0.03f * (arrow.Count - 1 - i),
                0);
        }
    }


}
