using DG.Tweening;
using System;
using UnityEngine;

namespace UI.UIAnimationElements
{
    public class UIAnimationElement_Scale : UIAnimationElement_Base
    {
        private void OnDestroy()
        {
            transform?.DOKill();
        }

        public override void TurnOff(float? duration = null)
        {
            if (duration == null)
            {
                duration = _baseSettings.turnOffDuration;
            }

            if (duration.Value == 0)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                transform?.DOKill();
                transform?.DOScale(0, duration.Value).SetEase(_baseSettings.ease); ;
            }
        }

        public override void TurnOn(float? duration = null)
        {
            if (duration == null)
            {
                duration = _baseSettings.turnOnDuration;
            }

            if (duration.Value == 0)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform?.DOKill();
                transform?.DOScale(1, duration.Value).SetEase(_baseSettings.ease);
            }
        }
    }
}