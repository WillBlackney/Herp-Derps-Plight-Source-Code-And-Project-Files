using System.Collections;
using System.Collections.Generic;
using FantasyRPG;
using UnityEngine;

public class PanelStage : PanelBase
{
    public Navicontrol navicontrol;
    
    public void Click_Prev()
    {
        navicontrol.Prev();
    }

    public void Click_Next()
    {
        navicontrol.Next();
    }

    public void Click_Stage()
    {
        PlayManager.Instance.LoadScene(Data.scene_play);
    }
}
