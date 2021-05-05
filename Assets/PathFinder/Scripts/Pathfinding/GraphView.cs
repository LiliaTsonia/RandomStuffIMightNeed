using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(Graph))]
    public class GraphView : MonoBehaviour
    {
        [SerializeField] private NodeView _nodeviewPrefab;

        [SerializeField] private Material _baseMaterial;
        [SerializeField] private Material _wallMaterial;

        public NodeView[,] NodeViews;

        public void Init(Graph graph)
        {
            if (graph == null)
            {
                Debug.LogError("GRAPHVIEW No graph to initialize");
                return;
            }

            NodeViews = new NodeView[graph.Width, graph.Height];

            foreach (INode node in graph.Nodes)
            {
                NodeView nodeView = Instantiate(_nodeviewPrefab, Vector3.zero, Quaternion.identity);

                if (nodeView != null)
                {
                    nodeView.transform.SetParent(transform);
                    nodeView.Init(node);
                    if (node.NodeType == NodeType.Blocked)
                    {
                        nodeView.SetNodeMaterial(_wallMaterial);
                    }
                    else
                    {
                        nodeView.SetNodeMaterial(_baseMaterial);
                    }

                    NodeViews[(int)node.CoordIndexes.x, (int)node.CoordIndexes.y] = nodeView;
                }
            }
        }

        public void ColorNodes(List<INode> nodes, Material material)
        {
            foreach (INode node in nodes)
            {
                if (node != null)
                {
                    NodeView nodeView = NodeViews[(int)node.CoordIndexes.x, (int)node.CoordIndexes.y];

                    if (nodeView != null)
                    {
                        nodeView.SetNodeMaterial(material);
                    }
                }
            }
        }

        public void ShowNodeArrows(List<INode> nodes, Material material)
        {
            foreach (INode node in nodes)
            {
                ShowNodeArrows(node, material);
            }
        }

        private void ShowNodeArrows(INode node, Material material)
        {
            if (node != null)
            {
                NodeView nodeView = NodeViews[(int)node.CoordIndexes.x, (int)node.CoordIndexes.y];

                if (nodeView != null)
                {
                    nodeView.ShowArrow(material);
                }
            }
        }
    }
}
