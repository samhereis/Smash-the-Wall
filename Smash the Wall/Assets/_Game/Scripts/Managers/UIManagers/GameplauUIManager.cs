using Configs;
using DependencyInjection;
using DG.Tweening;
using ECS.Systems.GameState;
using Events;
using Helpers;
using Sirenix.OdinInspector;
using SO.Lists;
using System.Collections.Generic;
using System.Linq;
using UI;
using UI.Canvases;
using UnityEngine;

namespace Managers.UIManagers
{
    public class GameplauUIManager : MonoBehaviour, IDIDependent
    {
        [Header("DI")]
        [Inject][SerializeField] private EventWithNoParameters _onWin;
        [Inject][SerializeField] private EventWithNoParameters _onLose;

        [Inject][SerializeField] private ListOfAllPictures _listOfAllPictures;
        [Inject][SerializeField] private ListOfAllScenes _listOfAllScenes;

        [Inject][SerializeField] private GameSaveManager _gameSaveManager;
        [Inject][SerializeField] private GameConfigs _gameConfigs;

        [Header("Components")]
        [Required]
        [SerializeField] private CanvasWindowBase _openOnStart; 
        
        [Required]
        [SerializeField] private WinMenu _winMenu;

        [Required]
        [SerializeField] private LoseMenu _loseMenu;

        [Header("Settings")]
        [SerializeField] private float _openOnStartDelay = 1f;

        [Header("Debug")]
        [SerializeField] private List<CanvasWindowBase> _menus = new List<CanvasWindowBase>();

        private WaitForSecondsRealtime _waitForSecondsRealtime = new WaitForSecondsRealtime(1);

        private bool _isAnimationWallMaterial = false;
        private bool _shouldAnimatetWallMaterial => WinLoseChecker_System.releasedWhatNeedsToBeDestroysPercentage < 5;

        private void Awake()
        {
            _menus = GetComponentsInChildren<CanvasWindowBase>(true).ToList();
            _winMenu = GetComponentInChildren<WinMenu>(true);
            _loseMenu = GetComponentInChildren<LoseMenu>(true);

            foreach (CanvasWindowBase menu in _menus)
            {
                menu?.Initialize();
            }
        }

        private async void Start()
        {
            DependencyInjector.InjectDependencies(this);

            _onWin.AddListener(OnWin);
            _onLose.AddListener(OnLose);

            await AsyncHelper.DelayFloat(_openOnStartDelay);

            _openOnStart?.Enable();
        }

        private void OnDestroy()
        {
            _onWin.RemoveListener(OnWin);
            _onLose.RemoveListener(OnLose);
        }

        private void Update()
        {
            if (_shouldAnimatetWallMaterial == true && _isAnimationWallMaterial == false)
            {
                StartAnimatingWallMaterial();
            }
        }

        private async void StartAnimatingWallMaterial()
        {
            _gameConfigs.globalReferences.borderMaterial.DOKill();

            if (_listOfAllPictures.GetCurrent().pictureMode == DataClasses.Enums.PictureMode.DestroyBorder)
            {
                _isAnimationWallMaterial = true;

                _gameConfigs.globalReferences.borderMaterial.color = _listOfAllPictures.borderDefaultColor;
                _gameConfigs.globalReferences.borderMaterial.DOColor(_listOfAllPictures.GetCurrent().borderColor, _listOfAllPictures.borderMaterialAnimationDuration)
                    .SetLoops(-1).SetEase(Ease.Linear).SetUpdate(true);

                while (_shouldAnimatetWallMaterial)
                {
                    await AsyncHelper.DelayInt(1);
                }

                _isAnimationWallMaterial = false;

                _gameConfigs.globalReferences.borderMaterial.DOKill();
                _gameConfigs.globalReferences.borderMaterial.DOColor(_listOfAllPictures.GetCurrent().borderColor, 1).SetUpdate(true); ;
            }
        }

        private void OnWin()
        {
            int starsCount = _winMenu.CalculateStars();
            string levelName = _listOfAllPictures.GetCurrent().targetName;
            string modeName = _listOfAllPictures.GetCurrent().pictureMode.ToString();

            _listOfAllPictures.SetNextPicture();

            _gameSaveManager.IncreaseLevelIndex();

            _winMenu.Enable();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
               { "LevelMode", modeName},
               { "LevelName", levelName},
               { "Stars", starsCount},
            };

            EventsLogManager.LogEvent("LevelCompleted", parameters);
        }

        private void OnLose()
        {
            string levelName = _listOfAllPictures.GetCurrent().targetName;
            string modeName = _listOfAllPictures.GetCurrent().pictureMode.ToString();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
               { "LevelMode", modeName},
               { "LevelName", levelName},
            };

            EventsLogManager.LogEvent("LevelFailed", parameters);

            _loseMenu.Enable();
        }
    }
}