using DependencyInjection;
using Sirenix.OdinInspector;
using SO.DataHolders;
using Sounds;
using System;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MenuBase
    {
        public Action onPlayClicked;
        public Action onOpenSettingsClicked;
        public Action onQuitClicked;

        [Required]
        [FoldoutGroup("Components"), SerializeField, ChildGameObjectsOnly] private Image _buttonsInfoBlock;

        [Required]
        [FoldoutGroup("Buttons"), SerializeField, ChildGameObjectsOnly] private Button _playButton;

        [Required]
        [FoldoutGroup("Buttons"), SerializeField, ChildGameObjectsOnly] private Button _settingsButton;

        [Required]
        [FoldoutGroup("Buttons"), SerializeField, ChildGameObjectsOnly] private Button _quitButton;

        [FoldoutGroup("Dependencies"), SerializeField, ReadOnly] private SettingsMenu _settingsMenu;

        public void Initialize(SettingsMenu settingsMenu)
        {
            _settingsMenu = settingsMenu;
        }

        public override void Enable(float? duration = null)
        {
            base.Enable(duration);

            SubscribeToEvents();
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);

            UnsubscribeFromEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();

            _playButton.onClick.AddListener(StartGame);
            _settingsButton.onClick.AddListener(OpenSettings);
            _quitButton.onClick.AddListener(Quit);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _playButton.onClick.RemoveListener(StartGame);
            _settingsButton.onClick.RemoveListener(OpenSettings);
            _quitButton.onClick.RemoveListener(Quit);
        }

        private void StartGame()
        {
            onPlayClicked?.Invoke();
        }

        private void OpenSettings()
        {
            onOpenSettingsClicked?.Invoke();

            _settingsMenu?.Enable();
        }

        private void Quit()
        {
            onQuitClicked?.Invoke();
        }
    }
}