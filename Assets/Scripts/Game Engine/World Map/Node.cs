using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace MapSystem
{
    public class Node
    {
        public readonly Point point;
        public readonly List<Point> incoming = new List<Point>();
        public readonly List<Point> outgoing = new List<Point>();
        [JsonConverter(typeof(StringEnumConverter))]
        private EncounterType nodeType;
        private string blueprintName;        
        public Vector2 position;

        public EncounterType NodeType
        {
            get { return nodeType; }
            private set { nodeType = value; }
        }
        public string BlueprintName
        {
            get { return blueprintName; }
            private set { blueprintName = value; }
        }
        public Node(EncounterType nodeType, string blueprintName, Point point)
        {
            this.NodeType = nodeType;
            this.BlueprintName = blueprintName;
            this.point = point;
        }
        public void RerollType(EncounterType type, string blueprintName)
        {
            this.NodeType = type;
            this.BlueprintName = blueprintName;
        }

        public void AddIncoming(Point p)
        {
            if (incoming.Any(element => element.Equals(p)))
                return;

            incoming.Add(p);
        }

        public void AddOutgoing(Point p)
        {
            if (outgoing.Any(element => element.Equals(p)))
                return;

            outgoing.Add(p);
        }

        public void RemoveIncoming(Point p)
        {
            incoming.RemoveAll(element => element.Equals(p));
        }

        public void RemoveOutgoing(Point p)
        {
            outgoing.RemoveAll(element => element.Equals(p));
        }

        public bool HasNoConnections()
        {
            return incoming.Count == 0 && outgoing.Count == 0;
        }
    }
}