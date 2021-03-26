using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MapSystem
{
    public class MapGenerator: Singleton<MapGenerator>
    {
        // Properties + Components
        #region
        private MapConfig config;

        private readonly List<EncounterType> RandomNodes = new List<EncounterType>
        {EncounterType.BasicEnemy, EncounterType.EliteEnemy, EncounterType.BossEnemy, EncounterType.CampSite,
            EncounterType.RecruitCharacter, EncounterType.Shop};

        private List<float> layerDistances;
        private List<List<Point>> paths;
        // ALL nodes by layer:
        private readonly List<List<Node>> nodes = new List<List<Node>>();

        [Header("Map Encounter Properties")]
        public int maximumCampSites = 6;
        public int maximumShops = 6;
        public int maximumElites = 8;

        [Header("Misc Properties")]
        public int nodeFrequencyLimit = 3;

        private int spawnedCampSites = 0;
        private int spawnedShops = 0;
        private int spawnedElites = 0;

        private int timeTilCampSiteAllowed = 0;
        private int timeTilShopAllowed = 0;
        private int timeTilEliteAllowed = 0;

        private List<Node> previousLayerData = new List<Node>();

        #endregion

        // Getters + Accessors
        #region
        public int SpawnedCampSites
        {
            get { return spawnedCampSites; }
            private set { spawnedCampSites = value; }
        }
        public int SpawnedShops
        {
            get { return spawnedShops; }
            private set { spawnedShops = value; }
        }
        public int SpawnedElites
        {
            get { return spawnedElites; }
            private set { spawnedElites = value; }
        }
        public int TimeTilCampSiteAllowed
        {
            get { return timeTilCampSiteAllowed; }
            private set { timeTilCampSiteAllowed = value; }
        }
        public int TimeTilShopAllowed
        {
            get { return timeTilShopAllowed; }
            private set { timeTilShopAllowed = value; }
        }
        public int TimeTilEliteAllowed
        {
            get { return timeTilEliteAllowed; }
            private set { timeTilEliteAllowed = value; }
        }
        #endregion


        public void ResetGenerationSettings()
        {
            SpawnedCampSites = 0;
            SpawnedShops = 0;
            SpawnedElites = 0;

            TimeTilCampSiteAllowed = 0;
            TimeTilShopAllowed = 0;
            TimeTilEliteAllowed = 0;
        }

        public Map GetMap(MapConfig conf)
        {
            if (conf == null)
            {
                Debug.LogWarning("Config was null in MapGenerator.Generate()");
                return null;
            }

            ResetGenerationSettings();

            config = conf;
            nodes.Clear();

            GenerateLayerDistances();

            for (var i = 0; i < conf.layers.Length; i++)
                PlaceLayer(i);

            GeneratePaths();

            RandomizeNodePositions();

            SetUpConnections();

            RemoveCrossConnections();

            // select all the nodes with connections:
            var nodesList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            // pick a random name of the boss level for this map:
            // var bossNodeName = config.nodeBlueprints.Where(b => b.nodeType == EncounterType.BossEnemy).ToList().Random().name;
            var bossNodeName = "King Herp Derp Boss";
            return new Map(conf.name, bossNodeName, nodesList, new List<Point>());
        }

        private void GenerateLayerDistances()
        {
            layerDistances = new List<float>();
            foreach (var layer in config.layers)
                layerDistances.Add(layer.distanceFromPreviousLayer.GetValue());
        }

        private float GetDistanceToLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex > layerDistances.Count) return 0f;

            return layerDistances.Take(layerIndex + 1).Sum();
        }

        private void PlaceLayer(int layerIndex)
        {
            var layer = config.layers[layerIndex];
            var nodesOnThisLayer = new List<Node>();
            bool atleastOneRandom = false;

            // offset of this layer to make all the nodes centered:
            var offset = layer.nodesApartDistance * config.GridWidth / 2f;

            for (var i = 0; i < config.GridWidth; i++)
            {
                var nodeType = Random.Range(0f, 1f) < layer.randomizeNodes ? GetRandomNode(layer.possibleRandomNodeTypes) : layer.nodeType;
                if (nodeType != layer.nodeType)
                    atleastOneRandom = true;

                var blueprintName = config.nodeBlueprints.Where(b => b.nodeType == nodeType).ToList().Random().name;
                var node = new Node(nodeType, blueprintName, new Point(i, layerIndex))
                {
                    position = new Vector2(-offset + i * layer.nodesApartDistance, GetDistanceToLayer(layerIndex))
                };
                nodesOnThisLayer.Add(node);
            }

            if(!atleastOneRandom && layer.guaranteeAtleastOneRandom)
            {
                Debug.LogWarning("Didn't hit a random node, rerolling for a random node");

                // get a random node + type on the layer
                Node randomNode = nodesOnThisLayer[RandomGenerator.NumberBetween(0, nodesOnThisLayer.Count - 1)];
                EncounterType randomNodeType = GetRandomNode(layer.possibleRandomNodeTypes);

                // change the node to new random type
                var blueprintName = config.nodeBlueprints.Where(b => b.nodeType == randomNodeType).ToList().Random().name;
                randomNode.RerollType(randomNodeType, blueprintName);
            }

            nodes.Add(nodesOnThisLayer);

            // update previous layer data
            previousLayerData.Clear();
            previousLayerData.AddRange(nodesOnThisLayer);
        }
        private bool DoesLayerContainEncounterType(EncounterType type, List<Node> layerNodes)
        {
            bool bRet = false;
            foreach(Node node in layerNodes)
            {
                if(node.NodeType == type)
                {
                    bRet = true;
                    break;
                }

            }

            return bRet;
        }
        /*
        private static EncounterType CalculateNodeType()
        {

        }
        */

        // Handle Remove duplicate nodes function
        // Handle Reroll node type

        private void RandomizeNodePositions()
        {
            for (var index = 0; index < nodes.Count; index++)
            {
                var list = nodes[index];
                var layer = config.layers[index];
                var distToNextLayer = index + 1 >= layerDistances.Count
                    ? 0f
                    : layerDistances[index + 1];
                var distToPreviousLayer = layerDistances[index];

                foreach (var node in list)
                {
                    var xRnd = Random.Range(-1f, 1f);
                    var yRnd = Random.Range(-1f, 1f);

                    var x = xRnd * layer.nodesApartDistance / 2f;
                    var y = yRnd < 0 ? distToPreviousLayer * yRnd / 2f : distToNextLayer * yRnd / 2f;

                    node.position += new Vector2(x, y) * layer.randomizePosition;
                }
            }
        }

        private void SetUpConnections()
        {
            foreach (var path in paths)
            {
                for (var i = 0; i < path.Count; i++)
                {
                    var node = GetNode(path[i]);

                    if (i > 0)
                    {
                        // previous because the path is flipped
                        var nextNode = GetNode(path[i - 1]);
                        nextNode.AddIncoming(node.point);
                        node.AddOutgoing(nextNode.point);
                    }

                    if (i < path.Count - 1)
                    {
                        var previousNode = GetNode(path[i + 1]);
                        previousNode.AddOutgoing(node.point);
                        node.AddIncoming(previousNode.point);
                    }
                }
            }
        }

        private void RemoveCrossConnections()
        {
            for (var i = 0; i < config.GridWidth - 1; i++)
                for (var j = 0; j < config.layers.Length - 1; j++)
                {
                    var node = GetNode(new Point(i, j));
                    if (node == null || node.HasNoConnections()) continue;
                    var right = GetNode(new Point(i + 1, j));
                    if (right == null || right.HasNoConnections()) continue;
                    var top = GetNode(new Point(i, j + 1));
                    if (top == null || top.HasNoConnections()) continue;
                    var topRight = GetNode(new Point(i + 1, j + 1));
                    if (topRight == null || topRight.HasNoConnections()) continue;

                    // Debug.Log("Inspecting node for connections: " + node.point);
                    if (!node.outgoing.Any(element => element.Equals(topRight.point))) continue;
                    if (!right.outgoing.Any(element => element.Equals(top.point))) continue;

                    // Debug.Log("Found a cross node: " + node.point);

                    // we managed to find a cross node:
                    // 1) add direct connections:
                    node.AddOutgoing(top.point);
                    top.AddIncoming(node.point);

                    right.AddOutgoing(topRight.point);
                    topRight.AddIncoming(right.point);

                    var rnd = Random.Range(0f, 1f);
                    if (rnd < 0.2f)
                    {
                        // remove both cross connections:
                        // a) 
                        node.RemoveOutgoing(topRight.point);
                        topRight.RemoveIncoming(node.point);
                        // b) 
                        right.RemoveOutgoing(top.point);
                        top.RemoveIncoming(right.point);
                    }
                    else if (rnd < 0.6f)
                    {
                        // a) 
                        node.RemoveOutgoing(topRight.point);
                        topRight.RemoveIncoming(node.point);
                    }
                    else
                    {
                        // b) 
                        right.RemoveOutgoing(top.point);
                        top.RemoveIncoming(right.point);
                    }
                }
        }

        private Node GetNode(Point p)
        {
            if (p.y >= nodes.Count) return null;
            if (p.x >= nodes[p.y].Count) return null;

            return nodes[p.y][p.x];
        }

        private Point GetFinalNode()
        {
            var y = config.layers.Length - 1;
            if (config.GridWidth % 2 == 1)
                return new Point(config.GridWidth / 2, y);

            return Random.Range(0, 2) == 0
                ? new Point(config.GridWidth / 2, y)
                : new Point(config.GridWidth / 2 - 1, y);
        }

        private void GeneratePaths()
        {
            var finalNode = GetFinalNode();
            paths = new List<List<Point>>();
            var numOfStartingNodes = config.numOfStartingNodes.GetValue();
            var numOfPreBossNodes = config.numOfPreBossNodes.GetValue();

            var candidateXs = new List<int>();
            for (var i = 0; i < config.GridWidth; i++)
                candidateXs.Add(i);

            candidateXs.Shuffle();
            var preBossXs = candidateXs.Take(numOfPreBossNodes);
            var preBossPoints = (from x in preBossXs select new Point(x, finalNode.y - 1)).ToList();
            var attempts = 0;

            // start by generating paths from each of the preBossPoints to the 1st layer:
            foreach (var point in preBossPoints)
            {
                var path = Path(point, 0, config.GridWidth);
                path.Insert(0, finalNode);
                paths.Add(path);
                attempts++;
            }

            while (!PathsLeadToAtLeastNDifferentPoints(paths, numOfStartingNodes) && attempts < 100)
            {
                var randomPreBossPoint = preBossPoints[UnityEngine.Random.Range(0, preBossPoints.Count)];
                var path = Path(randomPreBossPoint, 0, config.GridWidth);
                path.Insert(0, finalNode);
                paths.Add(path);
                attempts++;
            }

            Debug.Log("Attempts to generate paths: " + attempts);
        }

        private bool PathsLeadToAtLeastNDifferentPoints(IEnumerable<List<Point>> paths, int n)
        {
            return (from path in paths select path[path.Count - 1].x).Distinct().Count() >= n;
        }

        private List<Point> Path(Point from, int toY, int width, bool firstStepUnconstrained = false)
        {
            if (from.y == toY)
            {
                Debug.LogError("Points are on same layers, return");
                return null;
            }

            // making one y step in this direction with each move
            var direction = from.y > toY ? -1 : 1;

            var path = new List<Point> { from };
            while (path[path.Count - 1].y != toY)
            {
                var lastPoint = path[path.Count - 1];
                var candidateXs = new List<int>();
                if (firstStepUnconstrained && lastPoint.Equals(from))
                {
                    for (var i = 0; i < width; i++)
                        candidateXs.Add(i);
                }
                else
                {
                    // forward
                    candidateXs.Add(lastPoint.x);
                    // left
                    if (lastPoint.x - 1 >= 0) candidateXs.Add(lastPoint.x - 1);
                    // right
                    if (lastPoint.x + 1 < width) candidateXs.Add(lastPoint.x + 1);
                }

                var nextPoint = new Point(candidateXs[Random.Range(0, candidateXs.Count)], lastPoint.y + direction);
                path.Add(nextPoint);
            }

            return path;
        }

        private EncounterType GetRandomNode()
        {
            if (RandomNodes.Count == 1)
            {
                return RandomNodes[0];
            }
            else
            {
                return RandomNodes[RandomGenerator.NumberBetween(0, RandomNodes.Count - 1)];
            }
        }
        private EncounterType GetRandomNode(EncounterType[] possibleRandomNodes)
        {
            EncounterType nodeReturned = EncounterType.BasicEnemy;

            // Filter out possible choices
            List<EncounterType> filteredChoices = new List<EncounterType>();

            if(possibleRandomNodes.Contains(EncounterType.CampSite) && 
                (SpawnedCampSites <= maximumCampSites) &&
                TimeTilCampSiteAllowed <= 0 &&
                !DoesLayerContainEncounterType(EncounterType.CampSite, previousLayerData))
            {
                filteredChoices.Add(EncounterType.CampSite);
            }
            if (possibleRandomNodes.Contains(EncounterType.EliteEnemy) &&
               (SpawnedElites <= maximumElites) &&
                TimeTilEliteAllowed <= 0 &&
                !DoesLayerContainEncounterType(EncounterType.EliteEnemy, previousLayerData))
            {
                filteredChoices.Add(EncounterType.EliteEnemy);
            }
            if (possibleRandomNodes.Contains(EncounterType.Shop) &&
              (SpawnedShops <= maximumShops) &&
                TimeTilShopAllowed <= 0 &&
                !DoesLayerContainEncounterType(EncounterType.Shop, previousLayerData))
            {
                filteredChoices.Add(EncounterType.Shop);
            }

            // Randomly pick an encounter type
            if (filteredChoices.Count == 0)
            {
                nodeReturned = EncounterType.BasicEnemy;
            }
            else if (filteredChoices.Count == 1)
            {
                nodeReturned = filteredChoices[0];
            }
            else
            {
                nodeReturned = filteredChoices[RandomGenerator.NumberBetween(0, filteredChoices.Count - 1)];
            }

            if (nodeReturned == EncounterType.BasicEnemy)
            {
                TimeTilShopAllowed--;
                TimeTilCampSiteAllowed--;
                TimeTilEliteAllowed--;
            }
            if (nodeReturned == EncounterType.EliteEnemy)
            {
                SpawnedElites++;
                TimeTilEliteAllowed = nodeFrequencyLimit;
                TimeTilShopAllowed--;
                TimeTilCampSiteAllowed--; 

            }
            else if (nodeReturned == EncounterType.Shop)
            {
                SpawnedShops++;
                TimeTilShopAllowed = nodeFrequencyLimit;
                TimeTilEliteAllowed--;
                TimeTilCampSiteAllowed--;
            }
            else if (nodeReturned == EncounterType.CampSite)
            {
                SpawnedCampSites++;
                TimeTilCampSiteAllowed = nodeFrequencyLimit;
                TimeTilShopAllowed--;
                TimeTilEliteAllowed--;
            }

            return nodeReturned;
           
        }


    }

}