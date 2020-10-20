using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRPG
{
    public class PanelSettings : PanelBase
    {
        public SliderBar sliderBarSoundFx;
        public SliderBar sliderBarMusic;

        public GameObject[] imageSoundFx;
        public GameObject[] imageMusic;
        
        void Update()
        {
            if (sliderBarSoundFx.value <= 0)
            {
                imageSoundFx[0].SetActive(true);
                imageSoundFx[1].SetActive(false);
            }
            else
            {
                imageSoundFx[0].SetActive(false);
                imageSoundFx[1].SetActive(true);
            }
            
            
            if (sliderBarMusic.value <= 0)
            {
                imageMusic[0].SetActive(true);
                imageMusic[1].SetActive(false);
            }
            else
            {
                imageMusic[0].SetActive(false);
                imageMusic[1].SetActive(true);
            }
        }
    }
}
