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
    public class BetterButton : Button, INeedDependencyInjection, ISelfValidator
    {
        [SerializeField] private SimpleSound _clickSoundResponce = new SimpleSound();

#if DoTweenInstalled
        [FoldoutGroup("Settings"), SerializeField] private float _onOverScale = 0.75f;
#endif

        [FoldoutGroup("Settings"), SerializeField] private float _animationDuration = 0.25f;
        [FoldoutGroup("Settings"), SerializeField] private bool _playSound = true;
        [FoldoutGroup("Settings"), SerializeField] private bool _vibrate = true;

        [Inject] private VibrationHelper _vibrationHelper;

        private bool _hasDownAnimationEnded = false;

        public void Validate(SelfValidationResult result)
        {
            if (_clickSoundResponce == null) { _clickSoundResponce = new SimpleSound(); }
            if (enabled == false)
            {
                enabled = true;
            }
        }

        protected override void Awake()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) { return; }
#endif

            DependencyContext.InjectDependencies(this);
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
            if (_clickSoundResponce != null) EditorGUILayout.PropertyField(_clickSoundResponce);
            if (_onOverScale != null) EditorGUILayout.PropertyField(_onOverScale);
            if (_animationDuration != null) EditorGUILayout.PropertyField(_animationDuration);
            if (_playSound != null) EditorGUILayout.PropertyField(_playSound);
            if (_vibrate != null) EditorGUILayout.PropertyField(_vibrate);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}