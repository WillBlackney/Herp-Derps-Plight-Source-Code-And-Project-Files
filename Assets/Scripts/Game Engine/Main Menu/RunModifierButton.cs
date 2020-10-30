using UnityEngine;

public class RunModifierButton : MonoBehaviour
{
    public GameObject tickParent;
    public GameObject descriptionWindowParent;

    public void TickMe()
    {
        tickParent.SetActive(true);
    }
    public void CrossMe()
    {
        tickParent.SetActive(false);
    }
}
