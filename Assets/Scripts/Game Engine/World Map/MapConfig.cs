using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    [CreateAssetMenu]
    public class MapConfig : ScriptableObject
    {
        public List<NodeBlueprint> nodeBlueprints;
        public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

        public IntMinMax numOfPreBossNodes;
        public IntMinMax numOfStartingNodes;

        public MapLayer[] layers;
    }
}