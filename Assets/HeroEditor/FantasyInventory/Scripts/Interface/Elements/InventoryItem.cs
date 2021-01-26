using System;
using System.Collections;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Represents inventory item.
    /// </summary>
    public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image Icon;
        public Image Background;
        public Text Count;
        public GameObject Modificator;
        public Item Item;
        public Toggle Toggle;
        public ItemContainer Container;

        private Action _scheduled;
        private float _clickTime;

        public void OnEnable()
        {
            if (_scheduled != null)
            {
                StartCoroutine(ExecuteScheduled());
            }
        }

        public void Start()
        {
            if (Icon != null)
            {
                var collection = IconCollection.Active ?? IconCollection.Instances.First().Value;

                Icon.sprite = collection.GetIcon(Item.Params.Path);
            }

            if (Toggle)
            {
                Toggle.group = GetComponentInParent<ToggleGroup>();
            }

            if (Modificator)
            {
                var mod = Item.Modifier != null && Item.Modifier.Id != ItemModifier.None;

                Modificator.SetActive(mod);

                if (mod)
                {
                    Modificator.GetComponentInChildren<Text>().text = Item.Modifier.Id.ToString().ToUpper()[0].ToString();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartCoroutine(OnPointerClickDelayed(eventData));
        }

        private IEnumerator OnPointerClickDelayed(PointerEventData eventData) // TODO: A workaround. We should wait for initializing other components.
        {
            yield return null;

            OnPointerClick(eventData.button);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Container.OnMouseEnter?.Invoke(Item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Container.OnMouseExit?.Invoke(Item);
        }

        public void OnPointerClick(PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left)
            {
                Container.OnLeftClick?.Invoke(Item);

                var delta = Mathf.Abs(Time.time - _clickTime);

                if (delta < 0.5f) // If double click.
                {
                    _clickTime = 0;
                    Container.OnDoubleClick?.Invoke(Item);
                }
                else
                {
                    _clickTime = Time.time;
                }
            }
            else if (button == PointerEventData.InputButton.Right)
            {
                Container.OnRightClick?.Invoke(Item);
            }
        }

        public void Select(bool selected)
        {
            if (Toggle == null) return;

            if (gameObject.activeInHierarchy || !selected)
            {
                Toggle.isOn = selected;
            }
            else
            {
                _scheduled = () => Toggle.isOn = true;
            }

            if (selected)
            {
                Container.OnLeftClick?.Invoke(Item);
            }
        }

        private IEnumerator ExecuteScheduled()
        {
            yield return null;

            _scheduled();
            _scheduled = null;
        }
    }
}