#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Helpers
{
    [DisallowMultipleComponent]
    public class UIWindowEditorHelper : MonoBehaviour
    {
        [SerializeField] private bool _rename = false;

        [ContextMenu("Auto Configure")]
        public void Hide()
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.zero;
        }

        private void OnValidate()
        {
            if (gameObject.name.StartsWith("Window") == false & _rename) gameObject.name = "Window    " + gameObject.name + "    Window";
        }

        [ContextMenu("Change")]
        public void Show()
        {
            foreach (var window in transform.parent.GetComponentsInChildren<UIWindowEditorHelper>()) window.Hide();

            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
        }
    }

    [CustomEditor(typeof(UIWindowEditorHelper))]
    public class UIWindowEditorHelperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UIWindowEditorHelper editorHelper = (UIWindowEditorHelper)target;
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(editorHelper.Hide))) editorHelper.Hide();
            if (GUILayout.Button(nameof(editorHelper.Show))) editorHelper.Show();

            GUILayout.EndHorizontal();
        }
    }
}
#endif