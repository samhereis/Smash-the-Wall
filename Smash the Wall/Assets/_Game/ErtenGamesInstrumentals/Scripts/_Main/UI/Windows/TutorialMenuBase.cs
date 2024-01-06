using DependencyInjection;
using Helpers;
using System;
using UI.Canvases;
using UnityEngine;

namespace UI.Window.Tutorial
{
    public abstract class TutorialMenuBase : MenuBase, INeedDependencyInjection
    {
        [field: SerializeField] public TutorialBaseSettings tutorialBaseSettings { get; private set; } = new TutorialBaseSettings();

        [SerializeField] private float _openDelay = 1f;

        private void OnValidate()
        {
            _baseSettings.notifyOthers = false;
            tutorialBaseSettings.tutorialName = gameObject.name;
            this.TrySetDirty();
        }

        public override async void Enable(float? duration = null)
        {
            await AsyncHelper.DelayFloat(_openDelay);

            if (AreDependenciesCompleted() == true)
            {
                base.Enable(duration);
            }
        }

        public virtual void OnComplete()
        {
            if (AreDependenciesCompleted() == true)
            {
                PlayerPrefs.SetInt(tutorialBaseSettings.tutorialName, 1);
                Disable();
            }
        }

        public virtual bool IsCompleted()
        {
            return PlayerPrefs.GetInt(tutorialBaseSettings.tutorialName, 0) == 1;
        }

        public virtual bool AreDependenciesCompleted()
        {
            return IsCompleted() == false;
        }

        [Serializable]
        public class TutorialBaseSettings
        {
            [field: SerializeField] public string tutorialName { get; internal set; }
        }
    }
}