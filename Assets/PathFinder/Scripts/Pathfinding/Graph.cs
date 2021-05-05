using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Graph : MonoBehaviour
    {
        public INode[,] Nodes;
        public List<INode> Walls = new List<INode>();

        protected int[,] MapData;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        protected readonly Vector2[] AllDirections =
        {
        new Vector2(0f, 1f), new Vector2(0f, -1f),
        new Vector2(-1f, 0f), new Vector2(1f, 0f),
        new Vector2(-1f, 1f), new Vector2(1f, 1f),
        new Vector2(-1f, -1f), new Vector2(1f, -1f)
    };

        public void Init(int[,] mapData)
        {
            MapData = mapData;
            Width = mapData.GetLength(0);
            Height = mapData.GetLength(1);

            Nodes = new INode[Width, Height];
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    NodeType type = (NodeType)MapData[x, y];
                    INode newNode = new Node(new Vector2(x, y), type);
                    Nodes[x, y] = newNode;

                    newNode.Position = new Vector3(x, 0, y);

                    if (type == NodeType.Blocked)
                    {
                        Walls.Add(newNode);
                    }
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Nodes[x, y].NodeType != NodeType.Blocked)
                    {
                        Nodes[x, y].Neighbors = GetNeighbors(x, y);
                    }
                }
            }
        }

        public bool IsWithinBounds(int x, int y)
        {
            return (x >= 0 && x < Width && y >= 0 && y < Height);
        }

        public List<INode> GetNeighbors(int x, int y, INode[,] nodeArray, Vector2[] directions)
        {
            List<INode> neighborNodes = new List<INode>();

            foreach (Vector2 dir in directions)
            {
                int newX = (int)(x + dir.x);
                int newY = (int)(y + dir.y);

                if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null && nodeArray[newX, newY].NodeType != NodeType.Blocked)
                {
                    neighborNodes.Add(nodeArray[newX, newY]);
                }
            }

            return neighborNodes;
        }

        public List<INode> GetNeighbors(int x, int y)
        {
            return GetNeighbors(x, y, Nodes, AllDirections);
        }

        public float GetNodeDistance(INode start, INode target)
        {
            int dx = (int)Mathf.Abs(start.Position.x - target.Position.x);
            int dy = (int)Mathf.Abs(start.Position.y - target.Position.y);

            int min = Mathf.Min(dx, dy);
            int max = Mathf.Max(dx, dy);

            int diagonalSteps = min;
            int straightSteps = max - min;

            return 1.4f * diagonalSteps + straightSteps;
        }
    }
}
