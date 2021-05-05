using UnityEngine;
using System;

namespace com.arlinus.FiguresMaker2D
{
    public class LinesDrawer : MonoBehaviour
    {
        public event Action<LineRenderer> OnLineDrown;

        [SerializeField] private Camera _camera;
        [SerializeField] private LineRenderer _linePrefab;
        [SerializeField] private float _pointsMinOffset;

        private LineRenderer _currentLine;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartDrawing();
            if (Input.GetMouseButton(0))
                UpdateLine();
            if (Input.GetMouseButtonUp(0))
            {
                OnLineDrown?.Invoke(_currentLine);
            }
        }

        public void StartDrawing()
        {
            var position = _camera.ScreenToWorldPoint(Input.mousePosition);
            _currentLine = Instantiate(_linePrefab, transform);
            _currentLine.positionCount = 2;
            _currentLine.SetPosition(0, position);
            _currentLine.SetPosition(1, position);
        }

        public void UpdateLine()
        {
            var position = _camera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0f;
            var lastPosition = _currentLine.GetPosition(_currentLine.positionCount - 1);
            if (Vector2.Distance(position, lastPosition) < _pointsMinOffset)
                return;

            _currentLine.positionCount++;
            _currentLine.SetPosition(_currentLine.positionCount - 1, position);
        }
    }

}