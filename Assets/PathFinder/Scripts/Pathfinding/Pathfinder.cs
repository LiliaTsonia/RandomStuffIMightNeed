using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    public enum SearchMode
    {
        BreadthFirst = 0,
        Dijkstra = 1,
        AStar = 2
    }

    public class Pathfinder : MonoBehaviour
    {
        [SerializeField] private Material _startMaterial;
        [SerializeField] private Material _targetMaterial;
        [SerializeField] private Material _frontierMaterial;
        [SerializeField] private Material _exploredMaterial;
        [SerializeField] private Material _pathMaterial;
        [SerializeField] private Material _baseArrowMaterial;
        [SerializeField] private Material _highlightArrowMaterial;

        [SerializeField] private bool _showIterations = true;
        [SerializeField] private bool _showColors = true;
        [SerializeField] private bool _showArrows = true;
        [SerializeField] private bool _exitOnGoal = true;

        [SerializeField] private SearchMode _searchMode;

        private INode _startNode;
        private INode _targetNode;

        private Graph _graph;
        private GraphView _graphView;

        private PriorityQueue<INode> _frontierNodes;
        private List<INode> _exploredNodes;
        private List<INode> _pathNodes;

        private int _iterations;

        public bool IsComplete { get; private set; }

        public bool Init(Graph graph, GraphView graphView, INode startNode, INode targetNode)
        {
            if (startNode.NodeType == NodeType.Blocked || targetNode.NodeType == NodeType.Blocked)
            {
                Debug.LogWarning("PATHFINDER Init error : start and target node must be unblocked");
                return false;
            }

            _graph = graph;
            _graphView = graphView;
            _startNode = startNode;
            _targetNode = targetNode;

            SetAndShowNodes(graphView, startNode, targetNode);

            _frontierNodes = new PriorityQueue<INode>();
            _frontierNodes.Enqueue(_startNode);

            _exploredNodes = new List<INode>();
            _pathNodes = new List<INode>();

            Array.Clear(_graph.Nodes, 0, _graph.Nodes.Length);

            _iterations = 0;
            IsComplete = false;
            _startNode.DistanceTravelled = 0;

            return true;
        }

        public IEnumerator SearchRoutine(float timeStep = 0.1f)
        {
            yield return null;

            while (!IsComplete)
            {
                if (_frontierNodes.Count > 0)
                {
                    INode currentNode = _frontierNodes.Dequeue();
                    _iterations++;

                    if (!_exploredNodes.Contains(currentNode))
                    {
                        _exploredNodes.Add(currentNode);
                    }

                    switch (_searchMode)
                    {
                        case SearchMode.Dijkstra:
                            ExpandFrontierDijkstra(currentNode);
                            break;
                        case SearchMode.BreadthFirst:
                            ExpandFrontier(currentNode);
                            break;
                        case SearchMode.AStar:
                            ExpandFrontierAStar(currentNode);
                            break;
                    }

                    if (_frontierNodes.Contains(_targetNode))
                    {
                        _pathNodes = GetPathNodes(_targetNode);

                        if (_exitOnGoal)
                        {
                            IsComplete = true;
                        }
                    }

                    if (_showIterations)
                    {
                        ShowDiagnostics();
                        yield return new WaitForSeconds(timeStep);
                    }
                }
                else
                {
                    IsComplete = true;
                }
            }

            ShowDiagnostics();
        }

        private void ShowDiagnostics()
        {
            if (_showColors)
            {
                SetAndShowNodes();
            }

            if (_graphView != null && _showArrows)
            {
                _graphView.ShowNodeArrows(_frontierNodes.ToList(), _baseArrowMaterial);

                if (_frontierNodes.Contains(_targetNode))
                {
                    _graphView.ShowNodeArrows(_pathNodes, _highlightArrowMaterial);
                }
            }
        }

        private void ExpandFrontier(INode node)
        {
            if (node != null)
            {
                for (int i = 0; i < node.Neighbors.Count; i++)
                {
                    if (!_exploredNodes.Contains(node.Neighbors[i]) && !_frontierNodes.Contains(node.Neighbors[i]))
                    {
                        node.Neighbors[i].ParentNode = node;
                        _frontierNodes.Enqueue(node.Neighbors[i]);
                    }
                }
            }
        }

        private void ExpandFrontierDijkstra(INode node)
        {
            if (node != null)
            {
                for (int i = 0; i < node.Neighbors.Count; i++)
                {
                    if (!_exploredNodes.Contains(node.Neighbors[i]))
                    {
                        float distanceToNeighbour = _graph.GetNodeDistance(node, node.Neighbors[i]);
                        float newDistanceTravelled = distanceToNeighbour + node.DistanceTravelled;

                        if (float.IsPositiveInfinity(node.Neighbors[i].DistanceTravelled) || newDistanceTravelled < node.Neighbors[i].DistanceTravelled)
                        {
                            node.Neighbors[i].ParentNode = node;
                            node.Neighbors[i].DistanceTravelled = newDistanceTravelled;
                        }

                        if (!_frontierNodes.Contains(node.Neighbors[i]))
                        {
                            node.Neighbors[i].Priority = node.Neighbors[i].DistanceTravelled;
                            _frontierNodes.Enqueue(node.Neighbors[i]);
                        }
                    }
                }
            }
        }

        private void ExpandFrontierAStar(INode node)
        {
            if (node != null)
            {
                for (int i = 0; i < node.Neighbors.Count; i++)
                {
                    if (!_exploredNodes.Contains(node.Neighbors[i]))
                    {
                        float distanceToNeighbour = _graph.GetNodeDistance(node, node.Neighbors[i]);
                        float newDistanceTravelled = distanceToNeighbour + node.DistanceTravelled;

                        if (float.IsPositiveInfinity(node.Neighbors[i].DistanceTravelled) || newDistanceTravelled < node.Neighbors[i].DistanceTravelled)
                        {
                            node.Neighbors[i].ParentNode = node;
                            node.Neighbors[i].DistanceTravelled = newDistanceTravelled;
                        }

                        if (!_frontierNodes.Contains(node.Neighbors[i]) && _graph != null)
                        {
                            float distanceToGoal = _graph.GetNodeDistance(node.Neighbors[i], _targetNode);
                            node.Neighbors[i].Priority = node.Neighbors[i].DistanceTravelled + distanceToGoal;
                            _frontierNodes.Enqueue(node.Neighbors[i]);
                        }
                    }
                }
            }
        }

        private void SetAndShowNodes()
        {
            SetAndShowNodes(_graphView, _startNode, _targetNode);
        }

        private void SetAndShowNodes(GraphView graphView, INode startNode, INode targetNode)
        {
            if (_frontierNodes != null)
            {
                _graphView.ColorNodes(_frontierNodes.ToList(), _frontierMaterial);
            }

            if (_exploredNodes != null)
            {
                _graphView.ColorNodes(_exploredNodes, _exploredMaterial);
            }

            if (_pathNodes != null && _pathNodes.Count > 0)
            {
                _graphView.ColorNodes(_pathNodes, _pathMaterial);
            }

            NodeView startNodeView = graphView.NodeViews[(int)startNode.CoordIndexes.x, (int)startNode.CoordIndexes.y];

            if (startNodeView != null)
            {
                startNodeView.SetNodeMaterial(_startMaterial);
            }

            NodeView targetNodeView = graphView.NodeViews[(int)targetNode.CoordIndexes.x, (int)targetNode.CoordIndexes.y];

            if (targetNodeView != null)
            {
                targetNodeView.SetNodeMaterial(_targetMaterial);
            }
        }

        private List<INode> GetPathNodes(INode endNode)
        {
            List<INode> path = new List<INode>();

            if (endNode == null)
            {
                return path;
            }

            path.Add(endNode);

            INode currentNode = endNode.ParentNode;

            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                currentNode = currentNode.ParentNode;
            }

            return path;
        }
    }
}
