using DG.Tweening;
using UnityEngine;
namespace MapSystem
{
    public class ScrollNonUI : MonoBehaviour
    {
        public float tweenBackDuration = 0.3f;
        public Ease tweenBackEase;
        public bool freezeX;
        public FloatMinMax xConstraints = new FloatMinMax();
        public bool freezeY;
        public FloatMinMax yConstraints = new FloatMinMax();
        private Vector2 offset;
        // distance from the center of this Game Object to the point where we clicked to start dragging 
        private Vector3 pointerDisplacement;
        private float zDisplacement;
        private bool dragging;
        private Camera mainCamera;

        private float mouseWheelScrollAmount = 3f;

        private void Awake()
        {
            mainCamera = CameraManager.Instance.MainCamera;
            zDisplacement = -mainCamera.transform.position.z + transform.position.z;
        }

        public void OnMouseDown()
        {
            pointerDisplacement = -transform.position + MouseInWorldCoords();
            transform.DOKill();
            dragging = true;
        }

        public void OnMouseUp()
        {
            dragging = false;
            TweenBack();
        }

        private void Update()
        {
            DragWithMouse();
            DragWithMouseWheel();
        }
        private void DragWithMouse()
        {
            if (!dragging) return;

            var mousePos = MouseInWorldCoords();
            //Debug.Log(mousePos);
            transform.position = new Vector3(
                freezeX ? transform.position.x : mousePos.x - pointerDisplacement.x,
                freezeY ? transform.position.y : mousePos.y - pointerDisplacement.y,
                transform.position.z);
        }
        private void DragWithMouseWheel()
        {
            bool doMove = false;
            float newY = 0f;

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                doMove = true;
                newY = transform.localPosition.y + mouseWheelScrollAmount;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                doMove = true;
                newY = transform.localPosition.y - mouseWheelScrollAmount;
            }

            if (doMove)
            {
                Debug.Log("My local y pos = " + transform.localPosition.y.ToString());

                if (transform.localPosition.y >= yConstraints.min && transform.localPosition.y <= yConstraints.max)
                {
                    // nothing
                }
                else
                {
                    newY = transform.localPosition.y < yConstraints.min ? yConstraints.min : yConstraints.max;
                }  


                transform.DOKill();
                transform.DOLocalMoveY(newY, 0.1f).SetEase(Ease.Linear);

                TweenBack();
            }



            /*
            bool doMove = false;
            float newY = 0f;

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                doMove = true;
                newY = transform.position.y + mouseWheelScrollAmount;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                doMove = true;
                newY = transform.position.y - mouseWheelScrollAmount;
            }

            if (doMove)
            {
                Debug.Log("My y pos = " + transform.position.y.ToString());
                if (newY < -35f)
                {
                    newY = -35f;
                }
                  
                else if (newY > 1f)
                {
                    newY = 1;
                }                   

                transform.DOKill();
                transform.DOMove(new Vector3(transform.position.x, newY, 0.1f), transform.position.z).SetEase(Ease.Linear);

                TweenBack();
            }
            */
        }

        // returns mouse position in World coordinates for our GameObject to follow. 
        private Vector3 MouseInWorldCoords()
        {
            var screenMousePos = Input.mousePosition;
            //Debug.Log(screenMousePos);
            screenMousePos.z = zDisplacement;
            return mainCamera.ScreenToWorldPoint(screenMousePos);
        }

        private void TweenBack()
        {
            if (freezeY)
            {
                if (transform.localPosition.x >= xConstraints.min && transform.localPosition.x <= xConstraints.max)
                    return;

                var targetX = transform.localPosition.x < xConstraints.min ? xConstraints.min : xConstraints.max;
                transform.DOKill();
                transform.DOLocalMoveX(targetX, tweenBackDuration).SetEase(tweenBackEase);
            }
            else if (freezeX)
            {
                if (transform.localPosition.y >= yConstraints.min && transform.localPosition.y <= yConstraints.max)
                    return;

                var targetY = transform.localPosition.y < yConstraints.min ? yConstraints.min : yConstraints.max;
                transform.DOKill();
                transform.DOLocalMoveY(targetY, tweenBackDuration).SetEase(tweenBackEase);
            }
        }
    }
}
