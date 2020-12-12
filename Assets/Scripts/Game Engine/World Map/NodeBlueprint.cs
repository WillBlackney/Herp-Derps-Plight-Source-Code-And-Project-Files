using UnityEngine;

namespace MapSystem
{
    public enum NodeType
    {
        MinorEnemy = 0,
        EliteEnemy = 1,
        CampSite = 2,
        Treasure = 3,
        Shop = 4,
        Boss = 5,
        Mystery = 6,
        Recruit = 7,
    }
}

namespace MapSystem
{
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public NodeType nodeType;
    }
}