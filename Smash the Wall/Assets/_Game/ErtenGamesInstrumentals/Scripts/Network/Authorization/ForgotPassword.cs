using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Authorization.UI
{
    public class ForgotPassword : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_InputField _emailField;
        [SerializeField] private Button _sendButton;
        [SerializeField] private CanvasGroup _thisCanvasGroup;
        [SerializeField] private CanvasGroup _sendCanvasGroup;
        [SerializeField] private CanvasGroup _emailSentCanvasGroup;

        private void Awake()
        {
            Close();
        }

        public void SendNewPassword()
        {
            if (StringHelper.IsEmail(_emailField.text))
            {

            }
        }

        public void Show()
        {
#if DoTweenInstalled
            _thisCanvasGroup.FadeUp(0);

            _emailSentCanvasGroup.FadeDown(0);
            _sendCanvasGroup.FadeUp(0.5f);
#endif
        }

        public void Close()
        {
#if DoTweenInstalled
            _thisCanvasGroup.FadeDown(0.25f);

            _sendCanvasGroup.FadeDown(0.25f);
            _emailSentCanvasGroup.FadeDown(0.25f);
#endif
        }
    }
}