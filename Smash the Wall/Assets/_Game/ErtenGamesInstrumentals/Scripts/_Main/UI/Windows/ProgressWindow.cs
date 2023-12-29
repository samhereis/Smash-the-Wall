using Helpers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvases
{
    public sealed class ProgressWindow : CanvasWindowBase
    {
        public static ProgressWindow instance;

        [Required]
        [SerializeField] private Slider _progressSlider;

        protected override void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Disable(0);
            base.Awake();
        }

        public override async void Enable(float? duration = null)
        {
            onAWindowOpen?.Invoke(this);

            await AsyncHelper.DelayFloat(0.5f);

            base.Enable(duration);
        }

        public override void Disable(float? duration = null)
        {
            base.Disable(duration);
        }

        private void OnEnable()
        {
            SetProgress(0);
        }

        private void OnDisable()
        {
            SetProgress(0);
        }

        public void SetProgress(float value)
        {
            _progressSlider.value = value;
        }
    }
}