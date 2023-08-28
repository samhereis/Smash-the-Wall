using DI;
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

        [Header("Components")]
        [SerializeField] private CanvasWindowBase _openOnStart;
        [SerializeField] private WinMenu _winMenu;
        [SerializeField] private LoseMenu _loseMenu;

        [Header("Settings")]
        [SerializeField] private float _openOnStartDelay = 1f;

        [Header("Debug")]
        [SerializeField] private List<CanvasWindowBase> _menus = new List<CanvasWindowBase>();

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
        }

        private void OnDestroy()
        {
            _onWin.RemoveListener(OnWin);
            _onLose.RemoveListener(OnLose);
        }

        private void OnWin()
        {
            int starsCount = _winMenu.CalculateStars();
            string levelName = _listOfAllPictures.GetCurrent().targetName;

            _listOfAllPictures.SetNextPicture();
            _listOfAllScenes.SetNextScene();

            GameSaveManager.IncreaseLevelIndex();

            _winMenu.Enable();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
               { "LevelMode", "DestroyBorder"},
               { "LevelName", levelName},
               { "Stars", starsCount},
            };

            EventsLogManager.LogEvent("LevelCompleted", parameters);
        }

        private void OnLose()
        {
            _loseMenu.Enable();
        }
    }
}