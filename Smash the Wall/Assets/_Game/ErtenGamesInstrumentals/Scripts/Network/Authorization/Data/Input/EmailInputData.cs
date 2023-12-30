using Helpers;
using System;
using System.Threading.Tasks;
using TMPro;
using UI;
using UnityEngine;

namespace Authorization.Data.Input
{
    [Serializable]
    public class EmailInputData : InputDataBase
    {
        [field: SerializeField] public TMP_InputField emailField { get; protected set; }

        public async void ValidateEmail_SignUp(string email = null)
        {
            if (string.IsNullOrEmpty(email)) email = emailField.text;

            isCorrect = IsCorrectEmail(email);
            if (isCorrect == true)
            {
                isCorrect = await DoesEmailExists(email) == true;
                if (isCorrect == false) MessageToUserMenu.instance.Log($"Email already taken");
            }
            IndicateCorrectOrNot();
        }

        public async void ValidateEmail_SignIn(string email = null)
        {
            if (string.IsNullOrEmpty(email)) email = emailField.text;

            isCorrect = IsCorrectEmail(email);
            if (isCorrect == true)
            {
                isCorrect = await DoesEmailExists(email) == false;
                if (isCorrect == false) MessageToUserMenu.instance.Log($"Email does not exists");
            }

            IndicateCorrectOrNot();
        }

        private bool IsCorrectEmail(string email)
        {
            bool correct = StringHelper.IsEmail(email);
            if (correct == false) MessageToUserMenu.instance.Log("Invalid email");

            return correct;
        }

        public static async Task<bool> DoesEmailExists(string email)
        {
            bool exists = false;

            if (ApplicationHelper.HasInternetConnection() == true)
            {
                await AsyncHelper.Skip();
            }

            return exists;
        }

        public override void Clear()
        {
            emailField.text = string.Empty;

            base.Clear();
        }
    }
}