using DI;
using InGameStrings;
using SO.DataHolders;
using Sound;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Canvases;
using UnityEngine;

namespace Managers.UIManagers
{
    public class MainMenuUIManager : MonoBehaviour, IDIDependent
    {
        [Header("DI")]
        [DI(DIStrings.backgrounMusicPlayer)][SerializeField] private BackgroundMusicPlayer _backgroundMusicPlayer;

        [Header("Components")]
        [SerializeField] private CanvasWindowBase _openOnStart;
        [SerializeField] private SoundsPack_DataHolder _mainMenuSoundsPack;

        [Header("Settings")]
        [SerializeField] private float _openOnStartDelay = 1f;

        [Header("Debug")]
        [SerializeField] private List<CanvasWindowBase> _menus = new List<CanvasWindowBase>();

        private void Awake()
        {
            _menus = GetComponentsInChildren<CanvasWindowBase>(true).ToList();
        }

        private IEnumerator Start()
        {
            _backgroundMusicPlayer?.StopMusic();

            yield return new WaitUntil(() => BindDIScene.isGLoballyInjected == true);

            foreach (CanvasWindowBase menu in _menus)
            {
                menu?.Initialize();
            }

            (this as IDIDependent).LoadDependencies();

            yield return new WaitForSecondsRealtime(_openOnStartDelay);

            _openOnStart?.Enable();

            yield return new WaitForSecondsRealtime(1f);
            _backgroundMusicPlayer?.PlayMusic(_mainMenuSoundsPack);
        }
    }
}