using Sirenix.OdinInspector;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class LoadingMenu : CanvasWindowBase
    {
        [Header("UI Elements")]
        [Required]
        [SerializeField] private Slider _progressSlider;

        public void SetProgress(float progress)
        {
            _progressSlider.value = progress;
        }
    }
}