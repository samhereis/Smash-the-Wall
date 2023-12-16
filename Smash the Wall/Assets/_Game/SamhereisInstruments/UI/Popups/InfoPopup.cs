using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class InfoPopup : PopupBase
    {
        [SerializeField] private Button[] _closeButtons;

        protected override void Awake()
        {
            base.Awake();

            Disable(0);

            foreach (var button in _closeButtons)
            {
                button.onClick.AddListener(Close);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var button in _closeButtons)
            {
                button.onClick.RemoveListener(Close);
            }
        }
    }
}