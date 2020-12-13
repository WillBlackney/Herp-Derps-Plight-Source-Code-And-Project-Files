using UnityEngine;


namespace MapSystem
{
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public EncounterType nodeType;
    }
}