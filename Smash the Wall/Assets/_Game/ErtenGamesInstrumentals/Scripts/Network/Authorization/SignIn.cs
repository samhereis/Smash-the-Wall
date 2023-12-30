using Authorization.Data.Input;
using Authorization.Models;
using Helpers;
using System;
using System.Threading.Tasks;
using UI;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace Authorization.UI
{
    public sealed class SignIn : MenuBase
    {
        [Header("UI Elements")]
        [SerializeField] private EmailInputData _email;
        [SerializeField] private PasswordInputData _password;
        [SerializeField] private Button _signIn;

        [Header("Components")]
        [SerializeField] private Authorization _authorization;
        [SerializeField] private ForgotPassword _forgotPassord;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            SubscribeToInputEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromInputEvents();
        }

        private void SubscribeToInputEvents()
        {
            UnsubscribeFromInputEvents();

            _signIn.onClick.AddListener(TrySignIn);

            _email.emailField.onEndEdit.AddListener(_email.ValidateEmail_SignIn);
            _password.passwordField.onEndEdit.AddListener(_password.ValidatePassword);
        }

        private void UnsubscribeFromInputEvents()
        {
            _signIn.onClick.RemoveListener(TrySignIn);

            _email.emailField.onEndEdit.RemoveListener(_email.ValidateEmail_SignIn);
            _password.passwordField.onEndEdit.RemoveListener(_password.ValidatePassword);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void OnOffline()
        {
            MessageToUserMenu.instance.Log("You are offline");
        }

        public override void Enable(float? duration = null)
        {
            _forgotPassord.Close();
            base.Enable(duration);
        }

        public void TrySignIn()
        {
            if (ApplicationHelper.HasInternetConnection() == false)
            {
                OnOffline();
                return;
            }

            _email.emailField.text = _email.emailField.text.Replace(" ", String.Empty);
            _password.passwordField.text = _password.passwordField.text.Replace(" ", String.Empty);

            SignInUserModel user = new SignInUserModel(_email.emailField.text, _password.passwordField.text);
        }

        private async Task OnSignIn()
        {
            await AsyncHelper.Skip();
        }
    }
}