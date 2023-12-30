#if DoTweenInstalled
using DG.Tweening;
#endif

using Helpers;
using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Authorization.Data.Input
{
    [Serializable]
    internal class PasswordInputData : InputDataBase
    {
        [field: SerializeField] public bool isConfirmed { get; protected set; }
        [field: SerializeField] public TMP_InputField passwordField { get; protected set; }
        [field: SerializeField] public TMP_InputField passwordConfirmationField { get; protected set; }
        [SerializeField] private Image _passwordFrame;
        [SerializeField] private Image _passwordConfirmationFrame;

        public void ValidatePassword(string password = null)
        {
            if (string.IsNullOrEmpty(password)) password = passwordField.text;

            if (StringHelper.IsPassword(password) == false)
            {
                isCorrect = false;
                MessageToUserMenu.instance.Log("Password must include only digits and Latin characters");
            }
            else if (password.Length < 8)
            {
                isCorrect = false;
                MessageToUserMenu.instance.Log("Password must be at least 8 characters long");
            }
            else
            {
                isCorrect = true;
            }

            IndicateCorrectOrNot();
        }

        public void ConfirmPassword(string password = null)
        {
            if (string.IsNullOrEmpty(password)) password = passwordConfirmationField.text;

            isConfirmed = password == passwordField.text;

            if (isConfirmed == false) MessageToUserMenu.instance.Log("Please confirm password");

#if DoTweenInstalled
            if (isConfirmed == true) _passwordConfirmationFrame.DOColor(correctColor, 0.5f); else _passwordConfirmationFrame.DOColor(notCorrectColor, 0.5f);
#endif
        }

        public override void Clear()
        {
            passwordField.text = string.Empty;
            passwordConfirmationField.text = string.Empty;

            isConfirmed = false;

#if DoTweenInstalled
            _passwordConfirmationFrame.DOColor(correctColor, 0.5f);
#endif

            base.Clear();
        }
    }
}