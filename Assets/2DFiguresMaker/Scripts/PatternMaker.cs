using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.arlinus.FiguresMaker2D
{
    public enum FigureType
    {
        Polygon,
        Rectangle,
        Star
    }

    public class PatternMaker : MonoBehaviour
    {
        [SerializeField] FigureType _figureType;

        [SerializeField] private GameObject _figure;
        [SerializeField] private string _figureName = "Figure";
        [SerializeField] private int _vertexNumber;
        [SerializeField] private Vector2 _lineSize = new Vector2(0.1f, 0.1f);
        [SerializeField] private float _radius = 1f;

        private LineRenderer _lineRenderer;

        [ContextMenu("Make figure")]
        public void MakePolygon()
        {
            DrawPolygon(_vertexNumber, _radius, Vector3.forward, _lineSize.x, _lineSize.y, _figureType);
        }

#if UNITY_EDITOR
        [ContextMenu("Save figure")]
        public void SavePrefabAsset()
        {
            string localPath = "Assets/Resources/" + _figureName + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAsset(_figure, localPath);
        }
#endif

        private void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth, FigureType figureType = FigureType.Polygon)
        {
            _lineRenderer = _figure.GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;

            _lineRenderer.startWidth = startWidth;
            _lineRenderer.endWidth = endWidth;
            _lineRenderer.loop = true;
            var angle = 2 * Mathf.PI / vertexNumber;
            _lineRenderer.positionCount = figureType == FigureType.Star ? vertexNumber * 2 : vertexNumber;

            var positionsOuter = new List<Vector3>();
            var positionsInner = new List<Vector3>();

            for (int i = 0; i < vertexNumber; i++)
            {
                var rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                     new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                     new Vector4(0, 0, 1, 0),
                                     new Vector4(0, 0, 0, 1));

                var initialRelativePosition = new Vector3(0, radius, 0);
                var position = centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition);

                if (figureType == FigureType.Star)
                {
                    positionsOuter.Add(position);
                    positionsInner.Add(centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition / 2) * -1f);
                }
                else
                {
                    _lineRenderer.SetPosition(i, position);
                }
            }

            if (figureType == FigureType.Star)
            {
                DrawStar(positionsOuter, positionsInner);
            }

        }

        private void DrawStar(List<Vector3> outerList, List<Vector3> innerList)
        {
            var first = innerList.GetRange(0, 3);
            innerList.RemoveRange(0, 3);
            innerList.AddRange(first);

            int outIndex = 0, inIndex = 0;

            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                if (i % 2 == 0)
                {
                    _lineRenderer.SetPosition(i, outerList[outIndex]);
                    outIndex++;
                }
                else
                {
                    _lineRenderer.SetPosition(i, innerList[inIndex]);
                    inIndex++;
                }
            }
        }
    }
}
