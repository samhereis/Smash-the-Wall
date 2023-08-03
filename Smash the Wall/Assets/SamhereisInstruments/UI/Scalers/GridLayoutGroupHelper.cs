using Helpers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Helpers
{
    [ExecuteAlways]
    public sealed class GridLayoutGroupHelper : MonoBehaviour
    {
        [SerializeField] private RectTransform _parent;
        private Action _updateAction;

        [Header("Componenets")]
        [SerializeField] private GridLayoutGroup _gridLayout;

        [Header("Settings")]
        [SerializeField] private float _horrizontalOffset = 0;
        [SerializeField] private float _verticalOffset = 0;

        private void OnEnable()
        {
            SetGridLayoutSizes();
        }

        private void Update()
        {
            SetGridLayoutSizes();
        }

        private void SetGridLayoutSizes()
        {
            if (_parent == null) return;

            float horrizontal = NumberHelper.GetNumberFromPercentage(_parent.rect.size.x, _horrizontalOffset);
            float vertical = NumberHelper.GetNumberFromPercentage(_parent.rect.size.y, _verticalOffset);

            if (horrizontal == 0)
            {
                horrizontal = _gridLayout.cellSize.x;
            }

            if (vertical == 0)
            {
                vertical = _gridLayout.cellSize.y;
            }

            _gridLayout.cellSize = new Vector2(horrizontal, vertical);
        }
    }
}