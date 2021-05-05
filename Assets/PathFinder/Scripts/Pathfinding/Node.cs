using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pathfinding
{
    public enum NodeType
    {
        Open = 0,
        Blocked = 1
    }

    public class Node : INode
    {
        public NodeType NodeType { get; set; }

        public Vector2 CoordIndexes { get; set; }
        public Vector3 Position { get; set; }

        public List<INode> Neighbors { get; set; }
        public INode ParentNode { get; set; }

        public float DistanceTravelled { get; set; }
        public float Priority { get; set; }

        public Node(Vector2 indexes, NodeType nodeType)
        {
            CoordIndexes = indexes;
            NodeType = nodeType;

            Neighbors = new List<INode>();
            ParentNode = null;
            DistanceTravelled = Mathf.Infinity;
        }

        public void Reset()
        {
            ParentNode = null;
        }

        public int CompareTo(INode other)
        {
            if (Priority < other.Priority)
            {
                return -1;
            }
            else if (Priority > other.Priority)
            {
                return 1;
            }

            return 0;
        }
    }
}
