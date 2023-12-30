using Authorization.Data.Input;
using Helpers;
using System;
using System.Threading.Tasks;
using TMPro;
using UI;
using UI.Canvases;
using UnityEngine;
using UnityEngine.UI;

namespace Authorization.UI
{
    public sealed class SignUp : MenuBase
    {
        [Header("UI Elements")]
        [SerializeField] private EmailInputData _email;
        [SerializeField] private NicknameInputData _nickName;
        [SerializeField] private PasswordInputData _password;
        [SerializeField] private Slider _ageSlider;
        [SerializeField] private TMP_InputField _age;
        [SerializeField] private Toggle _male;
        [SerializeField] private Toggle _female;
        [SerializeField] private Button _signUp;

        [Header("Components")]
        [SerializeField] private Authorization _authorization;
        [SerializeField] private bool _correctAge;

        protected override void Awake()
        {
            _signUp.interactable = false;
            base.Awake();
        }

        private void OnEnable()
        {
            if (ApplicationHelper.HasInternetConnection() == false) MessageToUserMenu.instance.Log("You are offline");
            SubscribeFromEvents();
        }

        private void OnDisable()
        {
            UnSubscribeFromEvents();
        }

        private void SubscribeFromEvents()
        {
            UnSubscribeFromEvents();

            _signUp.onClick.AddListener(TrySignUp);

            _email.emailField.onEndEdit.AddListener(_email.ValidateEmail_SignUp);
            _nickName.nickNameField.onEndEdit.AddListener(_nickName.ValidateNickName);

            _password.passwordField.onEndEdit.AddListener(_password.ValidatePassword);
            _password.passwordConfirmationField.onEndEdit.AddListener(_password.ConfirmPassword);

            _age.onValueChanged.AddListener(OnAgeInputFieldValueChanged);
            _ageSlider.onValueChanged.AddListener(OnAgeSliderValueChanged);

            _male.onValueChanged.AddListener(OnMaleToggleChanged);
            _female.onValueChanged.AddListener(OnFemaleToggleChanged);
        }

        private void UnSubscribeFromEvents()
        {
            _signUp.onClick.RemoveListener(TrySignUp);

            _email.emailField.onEndEdit.RemoveListener(_email.ValidateEmail_SignUp);
            _nickName.nickNameField.onEndEdit.RemoveListener(_nickName.ValidateNickName);

            _password.passwordField.onEndEdit.RemoveListener(_password.ValidatePassword);
            _password.passwordConfirmationField.onEndEdit.RemoveListener(_password.ConfirmPassword);

            _age.onValueChanged.RemoveListener(OnAgeInputFieldValueChanged);
            _ageSlider.onValueChanged.RemoveListener(OnAgeSliderValueChanged);

            _male.onValueChanged.RemoveListener(OnMaleToggleChanged);
            _female.onValueChanged.RemoveListener(OnFemaleToggleChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void Update()
        {
            if (_email.isCorrect && _password.isConfirmed && _correctAge && _password.isConfirmed && _nickName.isCorrect)
            {
                _signUp.interactable = true;
            }
            else
            {
                _signUp.interactable = false;
            }

            _correctAge = string.IsNullOrEmpty(_age.text) == false;
        }

        public void TrySignUp()
        {
            _email.ValidateEmail_SignUp();
            _nickName.ValidateNickName();
            _password.ValidatePassword();
            _password.ConfirmPassword();

            if ((_email.isCorrect && _password.isConfirmed && _correctAge && _password.isConfirmed && _nickName.isCorrect) == false) return;

            _email.emailField.text = _email.emailField.text.Replace(" ", String.Empty);
            _password.passwordField.text = _password.passwordField.text.Replace(" ", String.Empty);
            _age.text = _age.text.Replace(" ", String.Empty);

            if (ApplicationHelper.HasInternetConnection() == false)
            {
                MessageToUserMenu.instance.Log("You are offline");
                return;
            }

            _signUp.onClick.RemoveListener(TrySignUp);




            //string sex = "";

            //if (_male.isOn == true && _female.isOn == false) sex = "male";
            //else if (_female.isOn == true && _male.isOn == false) sex = "female";
        }

        private void OnAgeSliderValueChanged(float value)
        {
            _age.text = value.ToString();
        }

        private void OnAgeInputFieldValueChanged(string value)
        {
            float age = _ageSlider.value;

            try
            {
                age = Convert.ToSingle(value);
            }
            finally
            {
                _ageSlider.value = age;
            }
        }

        private void OnMaleToggleChanged(bool isOn)
        {
            if (isOn) _female.isOn = false;
        }

        private void OnFemaleToggleChanged(bool isOn)
        {
            if (isOn) _male.isOn = false;
        }
    }
}