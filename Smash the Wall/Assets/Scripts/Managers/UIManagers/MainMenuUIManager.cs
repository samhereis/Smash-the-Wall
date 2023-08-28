using Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Canvases;
using UnityEngine;

namespace Managers.UIManagers
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private CanvasWindowBase _openOnStart;

        [Header("Settings")]
        [SerializeField] private float _openOnStartDelay = 1f;

        [Header("Debug")]
        [SerializeField] private List<CanvasWindowBase> _menus = new List<CanvasWindowBase>();

        private void Awake()
        {
            _menus = GetComponentsInChildren<CanvasWindowBase>().ToList();

            foreach (CanvasWindowBase menu in _menus)
            {
                menu?.Initialize();
            }
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(_openOnStartDelay);

            _openOnStart?.Enable();
        }
    }
}