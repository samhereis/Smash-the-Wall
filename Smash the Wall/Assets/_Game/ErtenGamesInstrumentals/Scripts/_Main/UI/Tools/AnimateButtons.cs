#if DoTweenInstalled
using DG.Tweening;
#endif

using Helpers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Interaction
{
    [DisallowMultipleComponent]
    public class AnimateButtons : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [FoldoutGroup("Scale Settings"), ShowInInspector] private float _onOverScale = 1.1f;
        [FoldoutGroup("Scale Settings"), ShowInInspector] private float _normaleScale = 1;

        [FoldoutGroup("Timing"), ShowInInspector] private float _animationDuration = 0.5f;
        [FoldoutGroup("Timing"), ShowInInspector] private float _delayBetweenAnimations = 0.1f;

        [FoldoutGroup("Events"), ShowInInspector] private AnimateButtonsEvents _events = new AnimateButtonsEvents();

        [Button]
        public async void OnPointerEnter(PointerEventData eventData)
        {
            await AsyncHelper.DelayFloat(_delayBetweenAnimations);

#if DoTweenInstalled
            transform.DOScale(_onOverScale, _animationDuration).SetEase(Ease.InOutBack);
#endif

            _events._onHover?.Invoke();
        }

        [Button]
        public void OnPointerClick(PointerEventData eventData)
        {
            _events._onClick?.Invoke();
        }

        [Button]
        public async void OnPointerExit(PointerEventData eventData)
        {
            await AsyncHelper.DelayFloat(_delayBetweenAnimations);

#if DoTweenInstalled
            transform.DOScale(_normaleScale, _animationDuration).SetEase(Ease.InOutBack);
#endif

            _events._onExit?.Invoke();
        }
    }

    [System.Serializable]
    internal class AnimateButtonsEvents
    {
        [SerializeField] internal UnityEvent _onHover = new UnityEvent();
        [SerializeField] internal UnityEvent _onClick = new UnityEvent();
        [SerializeField] internal UnityEvent _onExit = new UnityEvent();
    }
}