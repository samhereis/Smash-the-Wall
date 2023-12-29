#if DoTweenInstalled
using DG.Tweening;
#endif

using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIAnimationElements
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIAnimationElement_Fade : UIAnimationElement_Base
    {
        [Required]
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnDestroy()
        {

#if DoTweenInstalled
            transform.DOKill();
#endif
        }

        public override void TurnOff(float? duration = null)
        {
#if DoTweenInstalled
            if (duration == null)
            {
                duration = _baseSettings.turnOffDuration;
            }

            if (duration.Value == 0)
            {
                _canvasGroup.alpha = 0;
            }
            else
            {
                _canvasGroup.DOKill();

                _canvasGroup.DOFade(0, duration.Value).SetEase(_baseSettings.ease);
            }
#endif
        }

        public override void TurnOn(float? duration = null)
        {
#if DoTweenInstalled
            if (duration == null)
            {
                duration = _baseSettings.turnOnDuration;
            }

            if (duration.Value == 0)
            {
                _canvasGroup.alpha = 1;
            }
            else
            {
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(1, duration.Value).SetEase(_baseSettings.ease);
            }
#endif
        }
    }
}