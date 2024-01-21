#if DoTweenInstalled
using DG.Tweening;
#endif

using DependencyInjection;
using Helpers;
using Sirenix.OdinInspector;
using Sounds;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SO;

namespace UI.Interaction
{
    public class BetterButton : Button, INeedDependencyInjection, ISelfValidator
    {
        [SerializeField] private Sound_String_SO _clickSound;

#if DoTweenInstalled
        [FoldoutGroup("Settings"), SerializeField] private float _onOverScale = 0.75f;
#endif

        [FoldoutGroup("Settings"), SerializeField] private float _animationDuration = 0.25f;
        [FoldoutGroup("Settings"), SerializeField] private bool _animate = true;
        [FoldoutGroup("Settings"), SerializeField] private bool _playSound = true;
        [FoldoutGroup("Settings"), SerializeField] private bool _vibrate = true;
        [FoldoutGroup("Settings"), SerializeField] private bool _requireAudio = true;

        private static VibrationHelper _vibrationHelper_;

        private VibrationHelper vibrationHelper
        {
            get
            {
                if (_vibrationHelper_ == null) { _vibrationHelper_ = DependencyContext.diBox.Get<VibrationHelper>(); }
                return _vibrationHelper_;
            }
        }

        private bool _hasDownAnimationEnded = false;

        public void Validate(SelfValidationResult result)
        {
            if (enabled == false)
            {
                enabled = true;
            }

            if (_requireAudio == true)
            {
                if (_clickSound == null)
                {
                    result.AddError("Audio is not set").WithFix<Sound_String_SO>((soundString) =>
                    {
                        _clickSound = soundString;
                    });
                }
            }
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

            if (_animate == true)
            {
                AnimateScale();
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            bool canClick = IsActive() && IsInteractable();

            base.OnPointerClick(eventData);

            if (canClick)
            {
                if (_playSound) { SoundPlayer.instance?.TryPlay(_clickSound); }

                if (_vibrate) { vibrationHelper?.LightVibration(); }
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (_animate == true)
            {
                ResetScale();
            }
        }

        private void AnimateScale()
        {
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
        SerializedProperty _clickSound;
        SerializedProperty _onOverScale;
        SerializedProperty _animationDuration;
        SerializedProperty _animate;
        SerializedProperty _playSound;
        SerializedProperty _vibrate;
        SerializedProperty _requireAudio;

        protected override void OnEnable()
        {
            base.OnEnable();

            _clickSound = serializedObject.FindProperty(nameof(_clickSound));
            _onOverScale = serializedObject.FindProperty(nameof(_onOverScale));
            _animationDuration = serializedObject.FindProperty(nameof(_animationDuration));
            _animate = serializedObject.FindProperty(nameof(_animate));
            _playSound = serializedObject.FindProperty(nameof(_playSound));
            _vibrate = serializedObject.FindProperty(nameof(_vibrate));
            _requireAudio = serializedObject.FindProperty(nameof(_requireAudio));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            serializedObject.Update();

            if (_clickSound != null) EditorGUILayout.PropertyField(_clickSound);
            if (_onOverScale != null) EditorGUILayout.PropertyField(_onOverScale);
            if (_animationDuration != null) EditorGUILayout.PropertyField(_animationDuration);
            if (_animate != null) EditorGUILayout.PropertyField(_animate);
            if (_playSound != null) EditorGUILayout.PropertyField(_playSound);
            if (_vibrate != null) EditorGUILayout.PropertyField(_vibrate);
            if (_requireAudio != null) EditorGUILayout.PropertyField(_requireAudio);

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
#endif
}