using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class NodeView : MonoBehaviour
    {
        [SerializeField] private GameObject _tile;
        [SerializeField] private Transform _arrow;

        [Range(0, 0.5f)]
        [SerializeField] private float _borderSize = 0.15f;

        private INode _node;

        public void Init(INode node)
        {
            if (_tile != null)
            {
                name = "Node (" + node.CoordIndexes + ")";
                transform.position = node.Position;
                _tile.transform.localScale = new Vector3(1f - _borderSize, 1f, 1f - _borderSize);
                _node = node;
                EnableObject(_arrow.gameObject, false);
            }
        }

        public void SetNodeMaterial(Material material)
        {
            SetNodeMaterial(material, _tile);
        }

        public void ShowArrow(Material material)
        {
            if (_node != null && _arrow != null && _node.ParentNode != null)
            {
                EnableObject(_arrow.gameObject, true);
                Vector3 dirToPrevious = (_node.ParentNode.Position - _node.Position).normalized;
                _arrow.rotation = Quaternion.LookRotation(dirToPrevious);

                Renderer arrowRenderer = _arrow.GetComponent<Renderer>();
                if (arrowRenderer != null)
                {
                    arrowRenderer.material = material;
                }
            }
        }

        private void SetNodeMaterial(Material material, GameObject go)
        {
            if (go != null)
            {
                Renderer goRenderer = go.GetComponent<Renderer>();
                goRenderer.material = material;
            }
        }

        private void EnableObject(GameObject gameObject, bool state)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(state);
            }
        }
    }
}
