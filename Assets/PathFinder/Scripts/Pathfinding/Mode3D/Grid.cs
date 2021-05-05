using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Mode3D
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private LayerMask _unwalkableMask;
        [SerializeField] private Vector2 _gridWorldSize;
        [SerializeField] private float _nodeRadius;

        private Node[,] _grid;

        private float _nodeDiameter;
        int _gridSizeX, _gridSizeY;

        void Start()
        {
            _nodeDiameter = _nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);

            CreateGrid();
        }

        public Node NodeFromWorldPosition(Vector3 worldPosition)
        {
            var percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
            var percentY = (worldPosition.z + _gridWorldSize.y / 2) / _gridWorldSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

            return _grid[x, y];
        }
        private void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.y / 2;

            for(int x = 0; x < _gridSizeX; x++)
            {
                for(int y = 0; y < _gridSizeY; y++)
                {
                    var worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
                    NodeType nodeType = !Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableMask) ? NodeType.Open : NodeType.Closed;
                    _grid[x, y] = new Node(nodeType, worldPoint);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));

            if(_grid != null)
            {
                foreach(var node in _grid)
                {
                    Gizmos.color = node.NodeType == NodeType.Closed ? Color.red : Color.white;
                    Gizmos.DrawCube(node.Position, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }
    }
}
