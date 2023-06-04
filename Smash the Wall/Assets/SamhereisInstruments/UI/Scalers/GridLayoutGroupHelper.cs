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

            float horrizontal = NumberHelper.GetNumberFromPercentage100(_parent.rect.size.x, _horrizontalOffset);
            float vertical = NumberHelper.GetNumberFromPercentage100(_parent.rect.size.y, _verticalOffset);

            _gridLayout.cellSize = new Vector2(horrizontal, vertical);
        }
    }
}