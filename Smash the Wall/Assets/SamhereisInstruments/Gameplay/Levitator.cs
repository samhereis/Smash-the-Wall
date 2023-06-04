using DG.Tweening;
using Helpers;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using LazyUpdators;

namespace Gameplay
{
    public sealed class Levitator : MonoBehaviour
    {
        [SerializeField] private LazyUpdator_SO _lazyUpdator;

        [SerializeField] private RotationData _rotation = new RotationData();
        [SerializeField] private LiftData _lift = new LiftData();

        [Header("Debug")]
        [SerializeField] private bool _isLifting = false;
        [SerializeField] private bool _isRotating = false;

        private bool _toRight = false;

        bool _up = false;

        private async void OnEnable()
        {
            await AsyncHelper.Delay(1000);

            _up = false;
            _lift.originalYPosition = transform.localPosition.y;

            _lazyUpdator?.AddToQueue(Rotate);

            _rotation.originalRotation = transform.localEulerAngles;

            _lazyUpdator?.AddToQueue(Lift);
        }

        private void OnDisable()
        {
            _lazyUpdator?.RemoveFromQueue(Rotate);
            _lazyUpdator?.RemoveFromQueue(Lift);
        }

        private async Task Rotate()
        {
            if (_isLifting == false)
            {
                await AsyncHelper.Delay();

                _isLifting = true;

                _rotation.randomRotationDuration = Random.Range(_rotation.rotationDuration.x, _rotation.rotationDuration.y);

                float x = Random.Range(_rotation.rotationRandomX.x, _rotation.rotationRandomX.y);
                float y = Random.Range(_rotation.rotationRandomY.x, _rotation.rotationRandomY.y);
                float z = Random.Range(_rotation.rotationRandomZ.x, _rotation.rotationRandomZ.y);

                _rotation.randomRot = new Vector3(x, y, z);

                if (_toRight)
                {
                    _toRight = false;
                    transform.DOLocalRotate(_rotation.originalRotation + _rotation.randomRot, _rotation.randomRotationDuration).SetEase(Ease.InOutBack).SetUpdate(true).OnComplete(() => _isLifting = false);
                }
                else
                {
                    _toRight = true;
                    transform.DOLocalRotate(_rotation.originalRotation + (_rotation.randomRot * -1), _rotation.randomRotationDuration).SetEase(Ease.InOutBack).SetUpdate(true).OnComplete(() => _isLifting = false);
                }
            }
        }

        private async Task Lift()
        {
            if (_isRotating == false)
            {
                await AsyncHelper.Delay();

                _isRotating = true;

                _lift.randomY = Random.Range(_lift.randomLiftValue.x, _lift.randomLiftValue.y);
                _lift.randomLiftDuration = Random.Range(_lift.liftDuration.x, _lift.liftDuration.y);

                if (_up)
                {
                    transform.DOLocalMoveY(_lift.originalYPosition + _lift.randomY, _lift.randomLiftDuration).SetEase(Ease.InOutBack).SetUpdate(true).OnComplete(() => _isRotating = false);
                    _up = false;
                }
                else
                {
                    transform.DOLocalMoveY(_lift.originalYPosition - _lift.randomY, _lift.randomLiftDuration).SetEase(Ease.InOutBack).SetUpdate(true).OnComplete(() => _isRotating = false);
                    _up = true;
                }
            }
        }

        [Serializable]
        internal class RotationData
        {
            [field: SerializeField] public Vector3 originalRotation;
            [field: SerializeField] public Vector2 rotationDuration = new Vector2(3, 10);

            [Space(5)]
            [field: SerializeField] public Vector2 rotationRandomX = new Vector2(1, 20);
            [field: SerializeField] public Vector2 rotationRandomY = new Vector2(1, 20);
            [field: SerializeField] public Vector2 rotationRandomZ = new Vector2(1, 20);

            [Header("Debug")]
            [field: SerializeField] public Vector3 randomRot;
            [field: SerializeField] public float randomRotationDuration = 0;
        }

        [Serializable]
        internal class LiftData
        {
            [Header("Lift Settings")]
            [field: SerializeField] public float originalYPosition;
            [field: SerializeField] public Vector2 liftDuration = new Vector2(4, 6);
            [field: SerializeField] public Vector2 randomLiftValue = new Vector2(0, 3);
            [field: SerializeField] public Vector2 _originalMinAndMaxY = new Vector2(20, 100);

            [Header("Debug")]
            [field: SerializeField] public float randomY;
            [field: SerializeField] public float randomLiftDuration = 0;
        }
    }
}