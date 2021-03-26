using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MapSystem
{
    public class MapNode : MonoBehaviour
    {
        [Header("Components")]
        public SpriteRenderer encounterSprite;
        public SpriteRenderer encounterSpriteShadow;
        public SpriteRenderer boxBgSprite;
        public Transform scalingParent;
        public SpriteRenderer boxGlowOutline;
        public SpriteRenderer boxShadowSprite;

        [Header("Encounter Colors")]
        public Color boxBgNormalColor;
        public Color boxBgHighlightColor;

        [Header("DEPRECATED")]
        public Canvas swirlCanvas;
        public SpriteRenderer visitedCircle;
        public Image visitedCircleImage;

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
            encounterSprite.sprite = blueprint.sprite;
            //encounterSprite.color = blueprint.color;
            encounterSpriteShadow.sprite = blueprint.sprite;
            //graphicGlow.sprite = blueprint.sprite;
            if (node.NodeType == EncounterType.BossEnemy) transform.localScale *= 1.5f;
            initialScale = encounterSprite.transform.localScale.x;
            //visitedCircle.color = MapView.Instance.visitedColor;
            //visitedCircle.gameObject.SetActive(false);
            SetState(NodeStates.Locked);
            AutoSetSortingOrder();
        }
        private void AutoSetSortingOrder()
        {
            boxGlowOutline.sortingOrder = MapView.Instance.BaseMapSortingLayer + 1;
            boxShadowSprite.sortingOrder = MapView.Instance.BaseMapSortingLayer + 2;
            boxBgSprite.sortingOrder = MapView.Instance.BaseMapSortingLayer + 3;
            encounterSpriteShadow.sortingOrder = MapView.Instance.BaseMapSortingLayer + 4;
            encounterSprite.sortingOrder = MapView.Instance.BaseMapSortingLayer + 5;
        }

        public void SetState(NodeStates state)
        {
           // visitedCircle.gameObject.SetActive(false);
            switch (state)
            {
                case NodeStates.Locked:
                   // encounterSprite.DOKill();
                    //encounterSprite.color = MapView.Instance.lockedColor;
                    break;
                case NodeStates.Visited:
                   // encounterSprite.DOKill();
                   // encounterSprite.color = MapView.Instance.visitedColor;
                    //visitedCircle.gameObject.SetActive(true);
                    break;
                case NodeStates.Attainable:
                    // start pulsating from visited to locked color:
                   // encounterSprite.color = MapView.Instance.lockedColor;
                    scalingParent.DOKill();
                    scalingParent.DOScale(1.3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    //encounterSprite.DOColor(MapView.Instance.flashingColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void OnMouseEnter()
        {
            //encounterSprite.transform.DOKill();
            boxBgSprite.color = boxBgHighlightColor;
            boxGlowOutline.gameObject.SetActive(true);
            AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);
        }

        private void OnMouseExit()
        {
            //graphicGlow.gameObject.SetActive(false);
            boxBgSprite.color = boxBgNormalColor;
            boxGlowOutline.gameObject.SetActive(false);
           // encounterSprite.transform.DOKill();
            //encounterSprite.transform.DOScale(initialScale, 0.2f);
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
