using Authorization.Data.Input;
using Helpers;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UI;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace Authorization.UI
{
    public sealed class SignIn : CanvasWindowBase
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
            MessageToUser.instance.Log("You are offline");
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
            await AsyncHelper.Delay();
        }

        [Serializable]
        internal class SignInUserModel
        {
            [JsonProperty] public string email { get; set; }
            [JsonProperty] public string password { get; set; }

            public SignInUserModel(string email, string password)
            {
                this.email = email;
                this.password = password;
            }
        }
    }
}