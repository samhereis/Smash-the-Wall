using DataClasses;
using DG.Tweening;
using Identifiers;
using IdentityCards;
using Helpers;
using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewModel3DForPrevieData : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ObjectRotator_UserInput _objectRotator;
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private IdentifierBase _podiumPrefab;
        [SerializeField] private Transform _podium;
        [SerializeField] private Transform _podiumMeshParent;

        [Header("Button")]
        [SerializeField] private Button _putOnButton;
        [SerializeField] private Button _selectedButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _price;

        [Header("Settings")]
        [SerializeField] private bool _canRotate = true;
        [SerializeField] private float _aimationDuration = 1f;

        [SerializeField] private float _aimationDistance = 200;

        [Header("Debug")]
        [SerializeField] private APlayerIdentityCard _currentPlayerCard;
        [SerializeField] private List<IdentifierBase> _currentModels = new List<IdentifierBase>();
        [SerializeField] private List<Transform> _currentPodiums = new List<Transform>();
        [SerializeField] private Vector2 _lastPosition;

        private void OnEnable()
        {
            Clear(0);
            ClearButtons(0);
            _playerName.text = string.Empty;

            _objectRotator.onBeginRotate += OnBeginRotate;
            _objectRotator.onRotate += OnRotate;
        }

        private void OnDisable()
        {
            Clear(0);

            _objectRotator.onBeginRotate -= OnBeginRotate;
            _objectRotator.onRotate -= OnRotate;

            ClearButtons(0);
            _playerName.text = string.Empty;

            foreach (var podium in FindObjectsOfType<IdentifierBase>(true))
            {
                Destroy(podium.gameObject);
            }
        }

        public void ViewNext()
        {
            
        }

        public void ViewPrevious()
        {
            
        }

        public void View(APlayerIdentityCard aPlayer, int direction = 0)
        {
            _currentPlayerCard = aPlayer;

            ManageButtons(aPlayer);

            try
            {
                Clear(_aimationDuration, direction);

                var currentModel = Instantiate(_podiumPrefab, _podium);
                currentModel.transform.localScale = new Vector3(1, 1, 1);
                currentModel.transform.localPosition = new Vector3((_aimationDistance * direction) / 100, 0, 0);

                var currentPodiumMesh = Instantiate<IdentifierBase>(_podiumPrefab).transform;
                currentPodiumMesh.parent = _podiumMeshParent;
                currentPodiumMesh.localScale = new Vector3(1, 1, 1);
                currentPodiumMesh.localPosition = new Vector3(-_aimationDistance * direction, 0, 0);
                currentPodiumMesh.localEulerAngles = new Vector3(0, 0, 0);

                currentModel.transform.DOLocalMoveX(0, _aimationDuration);
                currentPodiumMesh.DOLocalMoveX(0, _aimationDuration);

                _currentPodiums.Add(currentPodiumMesh);
                _currentModels.Add(currentModel);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        private void ManageButtons(APlayerIdentityCard aPlayer)
        {

        }

        private void ClearButtons(float duration = 0.25f)
        {
            _putOnButton.transform.DOScale(0, duration);
            _selectedButton.transform.DOScale(0, duration);
            _buyButton.transform.DOScale(0, duration);
        }

        private void Clear(float duration = 0, int direction = 0)
        {

        }

        public void SelectCurrentPlayer()
        {
            MessageToUser.instance?.Log("Character selected");
            try
            {
                _putOnButton.transform.DOScale(0, 0.25f);
                _selectedButton.transform.DOScale(1, 0.25f);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        public void OnBeginRotate(PointerEventData eventData)
        {
            _lastPosition = eventData.position;
        }

        public void OnRotate(PointerEventData eventData)
        {
            try
            {
                if (_canRotate)
                {
                    var dir = _lastPosition - eventData.position;

                    foreach (IdentifierBase player in _currentModels) player.transform.Rotate(0, dir.x, 0);

                    _lastPosition = eventData.position;
                }
            }
            finally
            {

            }
        }
    }
}