using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Authorization.Data.Input
{
    [Serializable]
    public class InputDataBase
    {
        [field: SerializeField] public bool isCorrect { get; protected set; }
        [SerializeField] protected Image frame;

        protected Color notCorrectColor = Color.yellow;
        protected Color correctColor = Color.white;

        protected virtual void IndicateCorrectOrNot()
        {
            if (isCorrect == true)
            {
                frame.DOColor(correctColor, 0.5f);
            }
            else
            {
                frame.DOColor(notCorrectColor, 0.5f);
            }
        }

        public virtual void Clear()
        {
            isCorrect = false;
            IndicateCorrectOrNot();
        }
    }
}