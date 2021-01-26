using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts.Firearms
{
    /// <summary>
    /// Shell falling animation when shooting.
    /// </summary>
    public class Shell : MonoBehaviour
    {
        public SpriteRenderer Renderer;
        public Vector3 Speed;
        public float RotationSpeed;
        public float Lifetime;

        private float _startTime;

        public void Start()
        {
            Speed = new Vector3(Speed.x * Random.Range(0f, 1f), Speed.y * Random.Range(0.75f, 1.25f));
            RotationSpeed *= Random.Range(0.5f, 1.5f);
            _startTime = Time.time;
            Destroy(gameObject, Lifetime);
        }

        public void Update()
        {
            var color = Renderer.color;

            color.a = 1 - Mathf.Pow((Time.time - _startTime) / Lifetime, 3);
            Renderer.color = color;
            transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
            transform.localPosition += Speed * Time.deltaTime;
        }
    }
}