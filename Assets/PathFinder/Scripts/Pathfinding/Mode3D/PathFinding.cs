using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Mode3D
{
    public class PathFinding : MonoBehaviour
    {
        [SerializeField] private Grid _grid;

        private void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Node startNode = _grid.NodeFromWorldPosition(startPos);
            Node targetNode = _grid.NodeFromWorldPosition(targetPos);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);
        }
    }
}
