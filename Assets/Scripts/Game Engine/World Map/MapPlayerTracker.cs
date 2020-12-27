using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace MapSystem
{
    public class MapPlayerTracker : Singleton<MapPlayerTracker>
    {
        // Components + Properties
        #region
        [Header("Component References")]
        public MapManager mapManager;
        public MapView view;

        [Header("Properties")]
        public float enterNodeDelay = 1f;  
        private bool locked = true;
        #endregion

        // Getters + Accessors
        #region
        public bool Locked 
        {
            get { return locked; }
            private set { locked = value; } 
        }
        #endregion

        public void LockMap()
        {
            Locked = true;
        }
        public void UnlockMap()
        {
            Locked = false;
        }
        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            // Locked = lockAfterSelecting;
            LockMap();

            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            // mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => HandleEnterNode(mapNode));
        }

        private static void HandleEnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node: " + mapNode.Node.BlueprintName + " of type: " + mapNode.Node.NodeType);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false

            EventSequenceController.Instance.HandleLoadNextEncounter(mapNode);

            // TO DO: journey manager 'next encounter' logic is replaced and goes here.
            // e.g. if player clicks a basic enemy node, we need to calculate which enemy wave to load
            // next, save it to persistency, then load the combat event.

            /*
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                    break;
                case NodeType.EliteEnemy:
                    break;
                case NodeType.CampSite:
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Shop:
                    break;
                case NodeType.Boss:
                    break;
                case NodeType.Mystery:
                    break;
                case NodeType.Recruit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            */
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}