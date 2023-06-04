using Backend;
using DI;
using ECS.Systems.GameState;
using Helpers;
using Managers;
using PlayerInputHolder;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameplayMenu : CanvasWindowBase, IDIDependent
    {
        [SerializeField] private int _sceneIndex;

        [Header("DI")]
        [DI(InGameStrings.DIStrings.noAdsManager)] private NoAdsManager _noAdsManager;
        [DI(InGameStrings.DIStrings.inputHolder)][SerializeField] private Input_SO inputs;

        [Header("UI Components")]
        [SerializeField] private PauseMenu _pauseMenu;

        private Button _pauseButton;
        private Button _noAdsButton;

        private ProgressBar _whatNeedsToBeDestroyedProgressbar;
        private ProgressBar _whatNeedsToStayProgressbar;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override async void Start()
        {
            (this as IDIDependent).LoadDependencies();

            base.Start();

            Disable(0);

            await AsyncHelper.Delay(1000);

            Open();
        }

        private void Update()
        {
            if(_whatNeedsToBeDestroyedProgressbar != null && _whatNeedsToStayProgressbar != null)
            {
                _whatNeedsToBeDestroyedProgressbar.value = WinLoseChecker_System.releasedWhatNeedsToBeDestroysPercentage;
                _whatNeedsToStayProgressbar.value = WinLoseChecker_System.releasedWhatNeedsToStaysPercentage;
            }
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            inputs.Enable();
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            inputs.Disable();
        }

        protected override void FindAllUIElements()
        {
            base.FindAllUIElements();

            _pauseButton = baseSettings.root.Q<Button>("PauseButton");
            baseSettings.animatedVisualElements.SafeAdd(_pauseButton);

            _noAdsButton = baseSettings.root.Q<Button>("NoAdsButton");
            baseSettings.animatedVisualElements.SafeAdd(_noAdsButton);
            _noAdsManager.AddNoAdsButton(_noAdsButton);

            _whatNeedsToBeDestroyedProgressbar = baseSettings.root.Q<ProgressBar>("WNTBD_Progress");
            baseSettings.animatedVisualElements.SafeAdd(_whatNeedsToBeDestroyedProgressbar);

            _whatNeedsToStayProgressbar = baseSettings.root.Q<ProgressBar>("WNTS_Progress");
            baseSettings.animatedVisualElements.SafeAdd(_whatNeedsToStayProgressbar);
        }

        protected override void SubscribeToUIEvents()
        {
            base.SubscribeToUIEvents();

            _pauseButton.RegisterCallback<ClickEvent>(OpenPauseMenu);
        }

        protected override void UnSubscribeFromEvents()
        {
            base.UnSubscribeFromEvents();
        }

        private void OpenPauseMenu(ClickEvent evt)
        {
            _pauseMenu.Open();
        }
    }
}