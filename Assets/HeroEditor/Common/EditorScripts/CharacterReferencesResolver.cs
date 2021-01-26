using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.ExampleScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor.Common.EditorScripts
{
    /// <summary>
    /// A helper used in character editor scenes.
    /// </summary>
    public class CharacterReferencesResolver : MonoBehaviour
    {
        public CharacterEditor CharacterEditor;
        public AnimationManager AnimationManager;
        public AttackingExample AttackingExample;
        public BowExample BowExample;

        public Slider WidthSlider;
        public Slider HeightSlider;
        public Button WidthReset;
        public Button HeightReset;

        public void OnValidate()
        {
            var character = FindObjectOfType<Character>();

            CharacterEditor.Character = character;
            AnimationManager.Character = character;
            AttackingExample.Character = character;
            BowExample.Character = character;
        }

        public void Awake()
        {
            var sculptor = CharacterEditor.Character.GetComponent<CharacterBodySculptor>();

            sculptor.WidthSlider = WidthSlider;
            sculptor.HeightSlider = HeightSlider;

            WidthSlider.onValueChanged.RemoveAllListeners();
            WidthSlider.onValueChanged.AddListener(sculptor.OnWidthChanged);

            HeightSlider.onValueChanged.RemoveAllListeners();
            HeightSlider.onValueChanged.AddListener(sculptor.OnHeightChanged);

            WidthReset.onClick.RemoveAllListeners();
            WidthReset.onClick.AddListener(sculptor.ResetWidth);

            HeightReset.onClick.RemoveAllListeners();
            HeightReset.onClick.AddListener(sculptor.ResetHeight);
        }
    }
}