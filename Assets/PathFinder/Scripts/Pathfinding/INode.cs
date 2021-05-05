using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode : IComparable<INode>
{
    NodeType NodeType { get; set; }

    Vector2 CoordIndexes { get; set; }

    Vector3 Position { get; set; }

    List<INode> Neighbors { get; set; }

    INode ParentNode { get; set; }

    float DistanceTravelled { get; set; }

    float Priority { get; set; }

    int CompareTo(INode other);
}
