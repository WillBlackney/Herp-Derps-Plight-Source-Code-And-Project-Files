using UnityEngine;
using System.Collections;

public class EndTurnButton : MonoBehaviour {

    public void OnClick()
    {
            TurnManager.Instance.EndTurn();
    }

}
