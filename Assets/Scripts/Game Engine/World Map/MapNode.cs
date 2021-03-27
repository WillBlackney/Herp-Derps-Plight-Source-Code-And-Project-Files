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
        public SpriteRenderer[] encounterGlowSprites;
        public SpriteRenderer encounterSpriteShadow;
        public SpriteRenderer boxBgSprite;
        public Transform scalingParent;
        public SpriteRenderer boxGlowOutline;
        public SpriteRenderer boxShadowSprite;
        public GameObject redXParent;
        public SpriteRenderer[] redXSprites;

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
            foreach(SpriteRenderer sr in encounterGlowSprites)
                sr.sprite = blueprint.outlineSprite;
            encounterSpriteShadow.sprite = blueprint.sprite;
            if (node.NodeType == EncounterType.BossEnemy) transform.localScale *= 1.5f;
            initialScale = encounterSprite.transform.localScale.x;
            SetState(NodeStates.Locked);
            AutoSetSortingOrder();
        }
        private void AutoSetSortingOrder()
        {
            boxGlowOutline.sortingOrder = MapView.Instance.BaseMapSortingLayer + 1;
            for(int i = 0; i < encounterGlowSprites.Length; i++)
            {
                encounterGlowSprites[i].sortingOrder = MapView.Instance.BaseMapSortingLayer + 1 + i;
            }
                
            encounterSprite.sortingOrder = MapView.Instance.BaseMapSortingLayer + 10;
            for (int i = 0; i < redXSprites.Length; i++)
            {
                redXSprites[i].sortingOrder = MapView.Instance.BaseMapSortingLayer + 11 + i;
            }
        }

        public void SetState(NodeStates state)
        {
            if(state == NodeStates.Attainable && !MapPlayerTracker.Instance.Locked)
            {
                scalingParent.DOKill();
                scalingParent.DOScale(1.3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else if(state == NodeStates.Visited)
            {
                scalingParent.DOKill();
                encounterSprite.sprite = Blueprint.greyScaleSprite;
                redXParent.SetActive(true);
                // set red x image
            }
            /*
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
                case NodeStates.Attainable && !MapPlayerTracker.Instance.Locked:
                    // start pulsating from visited to locked color:
                   // encounterSprite.color = MapView.Instance.lockedColor;
                    scalingParent.DOKill();
                    scalingParent.DOScale(1.3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    //encounterSprite.DOColor(MapView.Instance.flashingColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }*/
        }

        private void OnMouseEnter()
        {
            //encounterSprite.transform.DOKill();
            //boxBgSprite.color = boxBgHighlightColor;
            //boxGlowOutline.gameObject.SetActive(true);
            float alphaMod = 0.05f;
            float currentAlphaBonus = 0f;

            foreach (SpriteRenderer sr in encounterGlowSprites)
            {
                sr.DOKill();
                sr.DOFade(0, 0);
                sr.DOFade(0.1f + currentAlphaBonus, 0.2f);
                currentAlphaBonus += alphaMod;
            }

               
            AudioManager.Instance.PlaySoundPooled(Sound.GUI_Button_Mouse_Over);
        }

        private void OnMouseExit()
        {
            foreach (SpriteRenderer sr in encounterGlowSprites)
            {
                sr.DOKill();
                sr.DOFade(0f, 0f);
            }

            //graphicGlow.gameObject.SetActive(false);
            //  boxBgSprite.color = boxBgNormalColor;
            //boxGlowOutline.gameObject.SetActive(false);
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
