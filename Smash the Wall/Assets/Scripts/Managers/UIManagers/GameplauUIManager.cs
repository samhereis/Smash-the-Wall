using Configs;
using DG.Tweening;
using DI;
using ECS.Systems.GameState;
using Events;
using InGameStrings;
using SO.Lists;
using System.Collections;
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
        [DI(DIStrings.onWinEvent)][SerializeField] private EventWithNoParameters _onWin;
        [DI(DIStrings.onLoseEvent)][SerializeField] private EventWithNoParameters _onLose;

        [DI(DIStrings.listOfAllPictures)][SerializeField] private ListOfAllPictures _listOfAllPictures;
        [DI(DIStrings.listOfAllScenes)][SerializeField] private ListOfAllScenes _listOfAllScenes;

        [DI(DIStrings.gameSaveManager)][SerializeField] private GameSaveManager _gameSaveManager;
        [DI(DIStrings.gameConfigs)][SerializeField] private GameConfigs _gameConfigs;

        [Header("Components")]
        [SerializeField] private CanvasWindowBase _openOnStart;
        [SerializeField] private WinMenu _winMenu;
        [SerializeField] private LoseMenu _loseMenu;

        [Header("Settings")]
        [SerializeField] private float _openOnStartDelay = 1f;

        [Header("Debug")]
        [SerializeField] private List<CanvasWindowBase> _menus = new List<CanvasWindowBase>();

        private WaitForSecondsRealtime _waitForSecondsRealtime = new WaitForSecondsRealtime(1);

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

        private IEnumerator Start()
        {
            (this as IDIDependent).LoadDependencies();

            _onWin.AddListener(OnWin);
            _onLose.AddListener(OnLose);

            yield return new WaitForSecondsRealtime(_openOnStartDelay);

            _openOnStart?.Enable();

            if (_listOfAllPictures.GetCurrent().pictureMode == DataClasses.Enums.PictureMode.DestroyBorder)
            {
                yield return _waitForSecondsRealtime;

                _gameConfigs.globalReferences.borderMaterial.color = _listOfAllPictures.borderDefaultColor;
                _gameConfigs.globalReferences.borderMaterial.DOColor(_listOfAllPictures.GetCurrent().borderColor, _listOfAllPictures.borderMaterialAnimationDuration)
                    .SetLoops(-1).SetEase(Ease.Linear);

                while (WinLoseChecker_System.releasedWhatNeedsToBeDestroysPercentage < 10)
                {
                    yield return _waitForSecondsRealtime;
                }

                _gameConfigs.globalReferences.borderMaterial.DOKill();
                _gameConfigs.globalReferences.borderMaterial.DOColor(_listOfAllPictures.GetCurrent().borderColor, 1);
            }
        }

        private void OnDestroy()
        {
            _onWin.RemoveListener(OnWin);
            _onLose.RemoveListener(OnLose);

            _gameConfigs.globalReferences.borderMaterial.DOKill();
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