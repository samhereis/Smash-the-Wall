using DG.Tweening;
using Helpers;
using Sound;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Interaction
{
    public class BetterButton : Button
    {
        [SerializeField] private SimpleSound _clickSoundResponce;

        [Header("Settings")]
        [SerializeField] private float _onOverScale = 0.75f;
        [SerializeField] private float _animationDuration = 0.25f;
        [SerializeField] private bool _playSound = true;
        [SerializeField] private bool _vibrate = true;

        private bool _hasDownAnimationEnded = false;

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

            transform.DOKill();
            transform?.DOScale(_onOverScale, _animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                _hasDownAnimationEnded = true;
            });
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            bool canClick = IsActive() && IsInteractable();

            base.OnPointerClick(eventData);

            if (canClick)
            {
                if (_playSound) { SoundPlayer.instance.TryPlay(_clickSoundResponce); }

                if (_vibrate) { VibrationHelper.LightVibration(); }
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            StartCoroutine(ResetScale());
        }

        private IEnumerator ResetScale()
        {
            if (_hasDownAnimationEnded) { yield return new WaitForSeconds(_animationDuration); }

            transform.DOKill();
            transform?.DOScale(1f, _animationDuration).SetEase(Ease.OutBack);
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