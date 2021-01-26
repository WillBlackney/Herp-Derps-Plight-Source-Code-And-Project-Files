using UnityEngine;

namespace Assets.HeroEditor.Common.CommonScripts.Springs
{
    /// <summary>
    /// A base class for spring actions.
    /// </summary>
    public abstract class SpringBase : MonoBehaviour
    {
        public float Speed;
        public float Period;
        public bool RandomPeriod;
	    public bool SaveState;

        protected float Time;

	    public void Start()
	    {
		    if (RandomPeriod)
		    {
			    Period = Random.Range(0, 360);
		    }
		}

        public virtual void OnEnable()
        {
	        if (!SaveState)
	        {
		        Time = 0;
			}
        }

        public void Update()
        {
            OnUpdate();
            Time += UnityEngine.Time.deltaTime;
        }

        protected virtual void OnUpdate()
        {
        }

        protected float Sin()
        {
            return (Mathf.Sin(Speed * Time + Period * Mathf.Deg2Rad) + 1) / 2;
        }
    }
}