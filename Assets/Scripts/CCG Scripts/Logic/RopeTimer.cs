using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RopeTimer : MonoBehaviour, IEventSystemHandler
{
    public GameObject RopeGameObject;
    public Slider RopeSlider;
	public float TimeForOneTurn;
    public float RopeBurnTime;
    public Text TimerText;

    private float timeTillZero;
    private bool counting = false;
    private bool ropeIsBurning;

    [SerializeField]
    public UnityEvent TimerExpired = new UnityEvent();

    void Awake()
    {
        if (RopeGameObject != null)
        {
            RopeSlider.minValue = 0;
            RopeSlider.maxValue = RopeBurnTime;
            RopeGameObject.SetActive(false);
        }
    }

    public void StartTimer()
	{
        timeTillZero = TimeForOneTurn;
		counting = true;
        ropeIsBurning = false;
        if (RopeGameObject!=null)
            RopeGameObject.SetActive(false);
	} 

	public void StopTimer()
	{
		counting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (counting) 
		{
			timeTillZero -= Time.deltaTime;
            if (TimerText!=null)
                TimerText.text = ToString();

            if (RopeGameObject != null)
            {
                // check for rope
                if (timeTillZero <= RopeBurnTime && !ropeIsBurning)
                {
                    ropeIsBurning = true;
                    RopeGameObject.SetActive(true);
                }
                // rope update
                if (ropeIsBurning)
                {
                    RopeSlider.value = timeTillZero;
                }
            }

            // check for TimeExpired
			if(timeTillZero<=0)
			{
				counting = false;
                //RopeGameObject.SetActive(false);
                TimerExpired.Invoke();
			}
		}
	
	}

	public override string ToString ()
	{
		int inSeconds = Mathf.RoundToInt (timeTillZero);
		string justSeconds = (inSeconds % 60).ToString ();
		if (justSeconds.Length == 1)
			justSeconds = "0" + justSeconds;
		string justMinutes = (inSeconds / 60).ToString ();
		if (justMinutes.Length == 1)
			justMinutes = "0" + justMinutes;

		return string.Format ("{0}:{1}", justMinutes, justSeconds);
	}
}
