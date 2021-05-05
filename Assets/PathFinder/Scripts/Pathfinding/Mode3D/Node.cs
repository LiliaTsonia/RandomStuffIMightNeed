using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Mode3D
{
    public enum NodeType
    {
        Open,
        Closed
    }

    public class Node
    {
        public NodeType NodeType { get; private set; }
        public Vector3 Position { get; private set; }

        public int GCost;
        public int HCost;

        public Node ParentNode;

        public int FCost => GCost + HCost;

        public Node(NodeType nodeType, Vector3 position)
        {
            NodeType = nodeType;
            Position = position;
        }
    }

}
