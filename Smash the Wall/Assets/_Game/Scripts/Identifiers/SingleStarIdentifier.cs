using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public sealed class SingleStarIdentifier : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _starImage;

        [Header("Settings")]
        [SerializeField] private Color _activeStarColor = Color.yellow;

        private void OnDestroy()
        {
            _starImage.DOKill();
            _starImage.transform.DOKill();
        }

        public void Activate()
        {
            _starImage.DOColor(_activeStarColor, 0.5f);
        }

        public void Deactivate()
        {
            _starImage.DOColor(Color.white, 0);
        }
    }
}