using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TargetIndicator
{
    [DefaultExecutionOrder(-1)]
    public class OffScreenIndicator : MonoBehaviour
    {
        public static Action<Target, bool> targetStateChanged;

        [SerializeField] private Camera _mainCamera_;

        [Range(0.5f, 0.9f)]
        [SerializeField] private float _screenBoundOffset = 0.9f;

        [FoldoutGroup("Debug"), SerializeField] private List<Target> _targets = new List<Target>();
        [FoldoutGroup("Debug"), SerializeField] private Vector3 _screenCentre;
        [FoldoutGroup("Debug"), SerializeField] private Vector3 _screenBounds;

        private Camera _mainCamera
        {
            get
            {
                if (_mainCamera_ == null)
                {
                    _mainCamera_ = FindFirstObjectByType<Camera>(FindObjectsInactive.Include);
                }

                return _mainCamera_;
            }
        }

        private void Awake()
        {
            _screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
            _screenBounds = _screenCentre * _screenBoundOffset;
            targetStateChanged += HandleTargetStateChanged;
        }

        private void OnDestroy()
        {
            targetStateChanged -= HandleTargetStateChanged;
        }

        private void Update()
        {
            DrawIndicators();
        }

        private void DrawIndicators()
        {
            if (_mainCamera == null) { return; }

            foreach (Target target in _targets)
            {
                Vector3 screenPosition = OffScreenIndicatorCore.GetScreenPosition(_mainCamera, target.transform.position);
                bool isTargetVisible = OffScreenIndicatorCore.IsTargetVisible(screenPosition);
                float distanceFromCamera = target.NeedDistanceText ? target.GetDistanceFromCamera(_mainCamera.transform.position) : float.MinValue;
                Indicator indicator = null;

                if (target.NeedBoxIndicator && isTargetVisible)
                {
                    screenPosition.z = 0;
                    indicator = GetIndicator(ref target.indicator, IndicatorType.BOX);
                }
                else if (target.NeedArrowIndicator && !isTargetVisible)
                {
                    float angle = float.MinValue;
                    OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, _screenCentre, _screenBounds);
                    indicator = GetIndicator(ref target.indicator, IndicatorType.ARROW);
                    indicator.SetRotation(Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));
                }
                if (indicator)
                {
                    indicator.SetImageColor(target.TargetColor);
                    indicator.SetDistanceText(distanceFromCamera);
                    indicator.transform.position = screenPosition;
                }
            }
        }

        private void HandleTargetStateChanged(Target target, bool active)
        {
            if (active)
            {
                _targets.Add(target);
            }
            else
            {
                target.indicator?.Activate(false);
                target.indicator = null;
                _targets.Remove(target);
            }
        }

        private Indicator GetIndicator(ref Indicator indicator, IndicatorType type)
        {
            if (indicator != null)
            {
                if (indicator.Type != type)
                {
                    indicator.Activate(false);
                    indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
                    indicator.Activate(true);
                }
            }
            else
            {
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
                indicator.Activate(true);
            }
            return indicator;
        }
    }
}