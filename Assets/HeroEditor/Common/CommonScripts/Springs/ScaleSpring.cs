using UnityEngine;

namespace Assets.HeroEditor.Common.CommonScripts.Springs
{
    /// <summary>
    /// Changes target scale like spring.
    /// </summary>
    public class ScaleSpring : SpringBase
    {
        public float From;
        public float To;
        public float Dumping;

        private Vector3 _scale;
        private float _amplitude = 1;

        public static void Begin(Component target, float from, float to, float speed, float dumping)
        {
            var component = target.GetComponent<ScaleSpring>() ?? target.gameObject.AddComponent<ScaleSpring>();

            component.From = from;
            component.To = to;
            component.Speed = speed;
            component.Dumping = dumping;
            component.enabled = true;
        }

        protected override void OnUpdate()
        {
            _amplitude = Mathf.Max(0, _amplitude - Dumping * UnityEngine.Time.deltaTime);

            transform.localScale = _scale * (From + (To - From) * Sin() * _amplitude);
     
            if (_amplitude <= 0)
            {
                enabled = false;
            }
        }

        public override void OnEnable()
        {
            _scale = transform.localScale;
            base.OnEnable();
            Reset();
        }

        public void OnDisable()
        {
            transform.localScale = _scale;
        }

        public void Reset()
        {
            _amplitude = 1;
        }
    }
}