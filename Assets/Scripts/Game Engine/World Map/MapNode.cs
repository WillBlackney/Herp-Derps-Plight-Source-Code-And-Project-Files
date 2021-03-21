using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MapSystem
{
    public class MapNode : MonoBehaviour
    {
        public SpriteRenderer sr;
        public SpriteRenderer visitedCircle;
        public Image visitedCircleImage;
        public SpriteRenderer graphicGlow;
        public Canvas swirlCanvas;

        public Node Node { get; private set; }
        public NodeBlueprint Blueprint { get; private set; }

        private float initialScale;
        private const float HoverScaleFactor = 1.35f;
        private float mouseDownTime;

        private const float MaxClickDuration = 0.5f;

        public void SetUp(Node node, NodeBlueprint blueprint)
        {
            Node = node;
            Blueprint = blueprint;
            sr.sprite = blueprint.sprite;
            graphicGlow.sprite = blueprint.sprite;
            if (node.NodeType == EncounterType.BossEnemy) transform.localScale *= 1.5f;
            initialScale = sr.transform.localScale.x;
            visitedCircle.color = MapView.Instance.visitedColor;
            visitedCircle.gameObject.SetActive(false);
            SetState(NodeStates.Locked);
            AutoSetSortingOrder();
        }
        private void AutoSetSortingOrder()
        {
            // Set sorting order
            swirlCanvas.sortingOrder = MapView.Instance.BaseMapSortingLayer + 2;
            visitedCircle.sortingOrder = MapView.Instance.BaseMapSortingLayer + 3;
            sr.sortingOrder = MapView.Instance.BaseMapSortingLayer + 4;
            graphicGlow.sortingOrder = MapView.Instance.BaseMapSortingLayer + 3;
        }

        public void SetState(NodeStates state)
        {
            visitedCircle.gameObject.SetActive(false);
            switch (state)
            {
                case NodeStates.Locked:
                    sr.DOKill();
                    sr.color = MapView.Instance.lockedColor;
                    break;
                case NodeStates.Visited:
                    sr.DOKill();
                    sr.color = MapView.Instance.visitedColor;
                    visitedCircle.gameObject.SetActive(true);
                    break;
                case NodeStates.Attainable:
                    // start pulsating from visited to locked color:
                    sr.color = MapView.Instance.lockedColor;
                    sr.DOKill();
                    sr.DOColor(MapView.Instance.flashingColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void OnMouseEnter()
        {
            sr.transform.DOKill();
            sr.transform.DOScale(initialScale * HoverScaleFactor, 0.2f);
            graphicGlow.gameObject.SetActive(true);
            AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);
        }

        private void OnMouseExit()
        {
            graphicGlow.gameObject.SetActive(false);
            sr.transform.DOKill();
            sr.transform.DOScale(initialScale, 0.2f);
        }

        private void OnMouseDown()
        {
            mouseDownTime = Time.time;
        }

        private void OnMouseUp()
        {
            if (Time.time - mouseDownTime < MaxClickDuration)
            {
                // user clicked on this node:
                MapPlayerTracker.Instance.SelectNode(this);
            }
        }
        private void OnDisable()
        {
            graphicGlow.gameObject.SetActive(false);
        }

        public void ShowSwirlAnimation()
        {
            if (visitedCircleImage == null)
                return;

            const float fillDuration = 0.3f;
            visitedCircleImage.fillAmount = 0;

            DOTween.To(() => visitedCircleImage.fillAmount, x => visitedCircleImage.fillAmount = x, 1f, fillDuration);
        }
    }
}
