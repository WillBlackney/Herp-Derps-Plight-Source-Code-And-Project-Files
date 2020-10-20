using System.Collections;
using System.Collections.Generic;
using FantasyRPG;
using UnityEngine;

public class PanelBase : MonoBehaviour
{

    public virtual void Open()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        this.gameObject.SetActive(false);
        (PlayManager.Instance.CurrentCtr as CtrHome).SetHome();
    }
}
