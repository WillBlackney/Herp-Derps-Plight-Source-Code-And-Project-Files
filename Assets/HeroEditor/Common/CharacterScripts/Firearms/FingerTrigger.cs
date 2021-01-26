using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts.Firearms
{
    /// <summary>
    /// Forefinger animation when shooting.
    /// </summary>
    public class FingerTrigger : MonoBehaviour
    {
        public Vector3 From;
        public Vector3 To;

        private int _state;
        private float _time;

        public bool Pressed
        {
            set
            {
                _state = value ? 1 : 2;
                _time = Time.time;
            }
        }

        public void Update()
        {
            if (_state == 1)
            {
                var progress = (Time.time - _time) / 0.05f;

                if (progress < 0)
                {
                    transform.localPosition = From + (To - From) * progress;
                }
                else
                {
                    transform.localPosition = To;
                    _state = 0;
                }
            }
            else if (_state == 2)
            {
                var progress = 1 - (Time.time - _time) / 0.05f;

                if (progress < 0)
                {
                    transform.localPosition = From + (To - From) * progress;
                }
                else
                {
                    transform.localPosition = From;
                    _state = 0;
                }
            }
        }
    }
}