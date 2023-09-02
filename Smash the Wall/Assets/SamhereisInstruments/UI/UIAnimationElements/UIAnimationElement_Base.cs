using DG.Tweening;
using System;
using UnityEngine;

namespace UI.UIAnimationElements
{
    public class UIAnimationElement_Base : MonoBehaviour
    {
        [SerializeField] protected BaseSettings _baseSettings = new BaseSettings();

        public virtual void TurnOff(float? duration = null)
        {

        }

        public virtual void TurnOn(float? duration = null)
        {

        }

        [Serializable]
        protected class BaseSettings
        {
            public Ease ease = Ease.OutBack;
            public float turnOffDuration = 0.2f;
            public float turnOnDuration = 0.5f;
        }
    }
}