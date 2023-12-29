#if DoTweenInstalled
using DG.Tweening;
#endif

using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.UIAnimationElements
{
    public class UIAnimationElement_Position : UIAnimationElement_Base
    {
        [Required]
        [SerializeField] private RectTransform _holder;
        [SerializeField] private Vector3 _onOffPosition;

        private void OnDestroy()
        {
#if DoTweenInstalled
            _holder.DOKill();
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
                _holder.position = _onOffPosition;
            }
            else
            {
                _holder.DOKill();

                _holder.DOLocalMove(_onOffPosition, duration.Value).SetEase(_baseSettings.ease);
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
                _holder.position = Vector3.zero;
            }
            else
            {
                _holder.DOKill();

                _holder.DOLocalMove(Vector3.zero, duration.Value).SetEase(_baseSettings.ease);
            }
#endif
        }
    }
}