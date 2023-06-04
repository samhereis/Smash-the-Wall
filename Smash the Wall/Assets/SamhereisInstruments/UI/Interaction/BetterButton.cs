using Sound;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Interaction
{
    public class BetterButton : Button
    {
        [SerializeField] private SimpleSound _clickSoundResponce;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (IsActive() && IsInteractable())
            {
                SoundPlayer.instance?.TryPlay(_clickSoundResponce);
            }
        }
    }

# if UNITY_EDITOR
    [CustomEditor(typeof(BetterButton))]
    public class BetterButtonEditor : UnityEditor.UI.ButtonEditor
    {
        SerializedProperty _clickSoundResponce;

        protected override void OnEnable()
        {
            base.OnEnable();
            _clickSoundResponce = serializedObject.FindProperty("_clickSoundResponce");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_clickSoundResponce);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}