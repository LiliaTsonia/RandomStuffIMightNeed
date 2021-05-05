using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindController : MonoBehaviour
    {
        [SerializeField] private MapData _mapData;
        [SerializeField] private Graph _graph;
        [SerializeField] private Pathfinder _pathFinder;

        [SerializeField] private Vector2 _startNode;
        [SerializeField] private Vector2 _targetNode;

        [SerializeField] private float _timeStep = 0.1f;

        private void Start()
        {
            if (_mapData != null && _graph != null)
            {
                int[,] mapInstance = _mapData.CreateMap();
                _graph.Init(mapInstance);

                GraphView graphView = _graph.GetComponent<GraphView>();

                if (graphView != null)
                {
                    graphView.Init(_graph);
                }

                if (_graph.IsWithinBounds((int)_startNode.x, (int)_startNode.y) && _graph.IsWithinBounds((int)_targetNode.x, (int)_targetNode.y)
                    && _pathFinder != null)
                {
                    INode startNode = _graph.Nodes[(int)_startNode.x, (int)_startNode.y];
                    INode targetNode = _graph.Nodes[(int)_targetNode.x, (int)_targetNode.y];

                    bool initialized = _pathFinder.Init(_graph, graphView, startNode, targetNode);
                    if (initialized)
                    {
                        StartCoroutine(_pathFinder.SearchRoutine(_timeStep));
                    }
                }
            }
        }
    }
}
