using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRPG
{
    public class SliderBar : MonoBehaviour
    {
        private Slider slider;
        private TextMeshProUGUI textValue;
        public float value;
        
        private void Awake()
        {
            slider = GetComponent<Slider>();
            textValue = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            
            slider.onValueChanged.AddListener((value) =>
            {
                ValueChangeed(value);
            });
        }


        private void Start()
        {
            SetValue(value);
        }

        public void SetValue(float value)
        {
            slider.value = value;
        }

        private void ValueChangeed(float value)
        {
            int v = (int) Mathf.Ceil(value);
            textValue.text = string.Format("{0}", v);
            
            if (value <= 0)
            {
                textValue.color = Utility.HexToColor("76617D");
            }
            else
            {
                textValue.color = Utility.HexToColor("FAB037");
            }

            this.value = v;
        }
        
        
    }
}
