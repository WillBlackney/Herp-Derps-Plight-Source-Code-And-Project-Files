using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CtrPanel : MonoBehaviour
{
    private int page = 0;
    private List<GameObject> panels = new List<GameObject>();
    private TextMeshProUGUI textTitle;
    public Transform panelTransform;
    private bool isReady = false;
    
    private void Start()
    {
        textTitle = transform.GetComponentInChildren<TextMeshProUGUI>();
        
        foreach (Transform t in panelTransform)
        {
            panels.Add(t.gameObject);
        }

        isReady = true;
        
        SetTitle();
    }

    void Update()
    {
        if (panels.Count <= 0 || !isReady) return;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Click_Prev();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Click_Next();
        }
    }
    
    //이전
    public void Click_Prev()
    {
        if (page <= 0 || !isReady) return;

        panels[page].SetActive(false);
        page--;
        panels[page].SetActive(true);
        textTitle.text = panels[page].name;
        SetTitle();
    }

    //다음
    public void Click_Next()
    {
        if (page >= panels.Count - 1) return;

        panels[page].SetActive(false);
        page++;
        panels[page].SetActive(true);
        SetTitle();
    }

    //하단 제목 설정
    private void SetTitle()
    {
        textTitle.text = panels[page].name;
    }
}
