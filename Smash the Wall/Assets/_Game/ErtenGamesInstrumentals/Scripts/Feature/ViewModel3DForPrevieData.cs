#if DoTweenInstalled
using DG.Tweening;
#endif

using Identifiers;
using Sirenix.OdinInspector;
using SO.DataHolders;
using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.UI.Menu.Canvas
{
    public class ViewModel3DForPrevieData : MonoBehaviour
    {
        [FoldoutGroup("Components"), SerializeField] private ObjectRotator_UserInput _objectRotator;
        [FoldoutGroup("Components"), SerializeField] private TextMeshProUGUI _playerName;
        [FoldoutGroup("Components"), SerializeField] private IdentifierBase _podiumPrefab;
        [FoldoutGroup("Components"), SerializeField] private Transform _podium;
        [FoldoutGroup("Components"), SerializeField] private Transform _podiumMeshParent;

        [FoldoutGroup("Button"), SerializeField] private Button _putOnButton;
        [FoldoutGroup("Button"), SerializeField] private Button _selectedButton;
        [FoldoutGroup("Button"), SerializeField] private Button _buyButton;
        [FoldoutGroup("Button"), SerializeField] private TextMeshProUGUI _price;

        [FoldoutGroup("Settings"), SerializeField] private bool _canRotate = true;
        [FoldoutGroup("Settings"), SerializeField] private float _aimationDuration = 1f;
        [FoldoutGroup("Settings"), SerializeField] private float _aimationDistance = 200;

        [FoldoutGroup("Debug"), SerializeField] private DataHolder_Base<IdentifierBase> _currentObjectCard;
        [FoldoutGroup("Debug"), SerializeField] private List<IdentifierBase> _currentModels = new List<IdentifierBase>();
        [FoldoutGroup("Debug"), SerializeField] private List<Transform> _currentPodiums = new List<Transform>();
        [FoldoutGroup("Debug"), SerializeField] private Vector2 _lastPosition;

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

            foreach (var podium in FindObjectsByType<IdentifierBase>(FindObjectsInactive.Include, FindObjectsSortMode.None))
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

        public void View(DataHolder_Base<IdentifierBase> objectToView, int direction = 0)
        {
            _currentObjectCard = objectToView;

            ManageButtons(objectToView);

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

#if DoTweenInstalled
                currentModel.transform.DOLocalMoveX(0, _aimationDuration);
                currentPodiumMesh.DOLocalMoveX(0, _aimationDuration);
#endif

                _currentPodiums.Add(currentPodiumMesh);
                _currentModels.Add(currentModel);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        private void ManageButtons(DataHolder_Base<IdentifierBase> objectToView)
        {

        }

        private void ClearButtons(float duration = 0.25f)
        {
#if DoTweenInstalled
            _putOnButton.transform.DOScale(0, duration);
            _selectedButton.transform.DOScale(0, duration);
            _buyButton.transform.DOScale(0, duration);
#endif
        }

        private void Clear(float duration = 0, int direction = 0)
        {

        }

        public void SelectCurrentPlayer()
        {
            MessageToUserMenu.instance?.Log("Character selected");
            try
            {
#if DoTweenInstalled
                _putOnButton.transform.DOScale(0, 0.25f);
                _selectedButton.transform.DOScale(1, 0.25f);
#endif
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