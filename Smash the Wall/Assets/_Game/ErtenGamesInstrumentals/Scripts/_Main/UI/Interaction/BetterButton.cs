#if DoTweenInstalled
using DG.Tweening;
#endif

using DependencyInjection;
using Helpers;
using Sirenix.OdinInspector;
using Sound;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Interaction
{
    public class BetterButton : Button, IDIDependent, ISelfValidator
    {
        [ShowInInspector] private SimpleSound _clickSoundResponce = new SimpleSound();

        [FoldoutGroup("Settings"), ShowInInspector] private float _onOverScale = 0.75f;
        [FoldoutGroup("Settings"), ShowInInspector] private float _animationDuration = 0.25f;
        [FoldoutGroup("Settings"), ShowInInspector] private bool _playSound = true;
        [FoldoutGroup("Settings"), ShowInInspector] private bool _vibrate = true;

        [Inject] private VibrationHelper _vibrationHelper;

        private bool _hasDownAnimationEnded = false;

        public void Validate(SelfValidationResult result)
        {
            if (_clickSoundResponce == null) { _clickSoundResponce = new SimpleSound(); }
        }

        protected override void Awake()
        {
            DependencyInjector.InjectDependencies(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            transform.localScale = Vector3.one;
            StopAllCoroutines();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            _hasDownAnimationEnded = false;

#if DoTweenInstalled
            transform.DOKill();
            
#endif

#if DoTweenInstalled
            transform?.DOScale(_onOverScale, _animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                _hasDownAnimationEnded = true;
            });
#endif
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            bool canClick = IsActive() && IsInteractable();

            base.OnPointerClick(eventData);

            if (canClick)
            {
                if (_playSound) { SoundPlayer.instance?.TryPlay(_clickSoundResponce); }

                if (_vibrate) { _vibrationHelper?.LightVibration(); }
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            ResetScale();
        }

        private async void ResetScale()
        {
            if (_hasDownAnimationEnded) { await AsyncHelper.DelayFloat(_animationDuration); }

#if DoTweenInstalled
            transform.DOKill();
            
#endif

#if DoTweenInstalled
            transform?.DOScale(1f, _animationDuration).SetEase(Ease.OutBack);
#endif
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BetterButton))]
    public class BetterButtonEditor : UnityEditor.UI.ButtonEditor
    {
        SerializedProperty _clickSoundResponce;
        SerializedProperty _onOverScale;
        SerializedProperty _animationDuration;
        SerializedProperty _playSound;
        SerializedProperty _vibrate;

        protected override void OnEnable()
        {
            base.OnEnable();
            _clickSoundResponce = serializedObject.FindProperty("_clickSoundResponce");
            _onOverScale = serializedObject.FindProperty("_onOverScale");
            _animationDuration = serializedObject.FindProperty("_animationDuration");
            _playSound = serializedObject.FindProperty("_playSound");
            _vibrate = serializedObject.FindProperty("_vibrate");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_clickSoundResponce);
            EditorGUILayout.PropertyField(_onOverScale);
            EditorGUILayout.PropertyField(_animationDuration);
            EditorGUILayout.PropertyField(_playSound);
            EditorGUILayout.PropertyField(_vibrate);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}