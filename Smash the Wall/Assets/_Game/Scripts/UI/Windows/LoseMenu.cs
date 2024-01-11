using DependencyInjection;
using Interfaces;
using Sirenix.OdinInspector;
using SO;
using Sounds;
using System;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoseMenu : MenuBase, IInitializable, INeedDependencyInjection
    {
        public Action onMainMenuClicked;
        public Action onReplayClicked;

        [Header("Effects")]

        [Required]
        [SerializeField] private Sound_String_SO _loseAudio;

        [Header("Components")]

        [Required]
        [SerializeField] private Button _restartButton;

        [Required]
        [SerializeField] private Button _goToMainMenuButton;

        [Required]
        [SerializeField] private Image _loseInfoBlock;

        [Required]
        [SerializeField] private Image _loseButtonsBlock;

        [Inject] private SoundPlayer _soundPlayer;

        public void Initialize()
        {
            DependencyContext.InjectDependencies(this);
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();

            _soundPlayer.TryPlay(_loseAudio);
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _restartButton.onClick.AddListener(RestartGame);
            _goToMainMenuButton.onClick.AddListener(GotoMainMenu);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _restartButton.onClick.RemoveListener(RestartGame);
            _goToMainMenuButton.onClick.RemoveListener(GotoMainMenu);
        }

        private void RestartGame()
        {
            onReplayClicked?.Invoke();
        }

        private void GotoMainMenu()
        {
            onMainMenuClicked?.Invoke();
        }
    }
}