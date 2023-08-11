using DG.Tweening;
using UnityEngine;

namespace Displayers
{
    public class TrajectoryDisplayer : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private LineRenderer _lineRenderer;

        [Header("Settings")]
        [SerializeField] private float _scaleUpDuration = 0.5f;

        [Header("Debug")]
        [SerializeField] private bool _isScaledUp = false;
        [SerializeField] private bool _isActive = false;
        [SerializeField] private Transform _followedObject;

        private void Awake()
        {
            if (_lineRenderer == null) _lineRenderer = GetComponentInChildren<LineRenderer>(true);

            HideTrajectory();
        }

        private void OnDestroy()
        {
            _lineRenderer.transform.DOKill();
        }

        private void Update()
        {
            if (_followedObject == null)
            {
                _lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });

                return;
            }

            if (_isActive == true)
            {
                DisplayTrajectory();
            }
            else
            {
                HideTrajectory();
            }

            Vector3 endPosition = _followedObject.position + _followedObject.forward * transform.localScale.z;
            _lineRenderer.SetPositions(new Vector3[] { _followedObject.position, endPosition });
        }

        public void Enable(Transform from)
        {
            _followedObject = from;

            _isActive = true;
        }

        public void Disable()
        {
            _isActive = false;
        }

        public void DisplayTrajectory()
        {
            if (_isScaledUp == false)
            {
                transform.DOKill();
                transform.DOScale(25f, _scaleUpDuration);

                _isScaledUp = true;
            }
        }

        public void HideTrajectory()
        {
            if (_isScaledUp == true)
            {
                transform.DOKill();
                transform.DOScale(0, _scaleUpDuration);

                _isScaledUp = false;
            }
        }
    }
}