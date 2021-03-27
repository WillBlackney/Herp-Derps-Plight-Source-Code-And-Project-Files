using UnityEngine;


namespace MapSystem
{
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public Sprite greyScaleSprite;
        public Sprite outlineSprite;
        public EncounterType nodeType;
        public Color color;
    }
}