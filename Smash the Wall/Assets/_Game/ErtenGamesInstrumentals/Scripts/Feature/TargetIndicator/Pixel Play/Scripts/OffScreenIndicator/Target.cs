using Sirenix.OdinInspector;
using UnityEngine;

namespace TargetIndicator
{
    [DefaultExecutionOrder(0)]
    public class Target : MonoBehaviour
    {
        [FoldoutGroup("Settings"), SerializeField] private Color _targetColor = Color.red;
        [FoldoutGroup("Settings"), SerializeField] private bool _needBoxIndicator = true;
        [FoldoutGroup("Settings"), SerializeField] private bool _needArrowIndicator = true;
        [FoldoutGroup("Settings"), SerializeField] private bool _needDistanceText = true;

        [SerializeField, ReadOnly] public Indicator indicator;

        public Color TargetColor
        {
            get
            {
                return _targetColor;
            }
        }

        public bool NeedBoxIndicator
        {
            get
            {
                return _needBoxIndicator;
            }
        }

        public bool NeedArrowIndicator
        {
            get
            {
                return _needArrowIndicator;
            }
        }

        public bool NeedDistanceText
        {
            get
            {
                return _needDistanceText;
            }
        }

        private void OnEnable()
        {
            if (OffScreenIndicator.targetStateChanged != null)
            {
                OffScreenIndicator.targetStateChanged.Invoke(this, true);
            }
        }

        private void OnDisable()
        {
            if (OffScreenIndicator.targetStateChanged != null)
            {
                OffScreenIndicator.targetStateChanged.Invoke(this, false);
            }
        }

        public float GetDistanceFromCamera(Vector3 cameraPosition)
        {
            float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
            return distanceFromCamera;
        }
    }
}