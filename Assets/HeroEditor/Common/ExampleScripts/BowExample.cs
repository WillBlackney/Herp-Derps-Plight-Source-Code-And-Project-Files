using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

namespace Assets.HeroEditor.Common.ExampleScripts
{
    /// <summary>
    /// Bow shooting behaviour (charge/release bow, create arrow). It's just an example!
    /// </summary>
    public class BowExample : MonoBehaviour
    {
        public Character Character;
        public AnimationClip ClipCharge;
	    public Transform FireTransform;
	    public GameObject ArrowPrefab;
        public bool CreateArrows;

        /// <summary>
        /// Should be set outside (by input manager or AI).
        /// </summary>
        [HideInInspector] public bool ChargeButtonDown;
        [HideInInspector] public bool ChargeButtonUp;

        private float _chargeTime;

        public void Update()
        {
            if (ChargeButtonDown)
            {
                _chargeTime = Time.time;
                Character.Animator.SetInteger("Charge", 1);
            }

            if (ChargeButtonUp)
            {
                var charged = Time.time - _chargeTime > ClipCharge.length;

                Character.Animator.SetInteger("Charge", charged ? 2 : 3);

                if (charged && CreateArrows)
                {
	                CreateArrow();
                }
            }
        }

		private void CreateArrow()
		{
			var arrow = Instantiate(ArrowPrefab, FireTransform);
			var sr = arrow.GetComponent<SpriteRenderer>();
			var rb = arrow.GetComponent<Rigidbody>();
			const float speed = 18.75f; // TODO: Change this!
			
			arrow.transform.localPosition = Vector3.zero;
			arrow.transform.localRotation = Quaternion.identity;
			arrow.transform.SetParent(null);
			sr.sprite = Character.Bow.Single(j => j.name == "Arrow");
			rb.velocity = speed * FireTransform.right * Mathf.Sign(Character.transform.lossyScale.x) * Random.Range(0.85f, 1.15f);

			var characterCollider = Character.GetComponent<Collider>();

			if (characterCollider != null)
			{
				Physics.IgnoreCollision(arrow.GetComponent<Collider>(), characterCollider);
			}

			arrow.gameObject.layer = 31; // TODO: Create layer in your project and disable collision for it (in physics settings)
			Physics.IgnoreLayerCollision(31, 31, true); // Disable collision with other projectiles.
		}
	}
}