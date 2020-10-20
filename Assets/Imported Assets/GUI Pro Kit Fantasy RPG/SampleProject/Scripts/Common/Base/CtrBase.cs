using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRPG
{

    public class CtrBase : MonoBehaviour
    {
        private void Awake()
        {
            PlayManager.Instance.CurrentCtr = this;
        }
    }
}
