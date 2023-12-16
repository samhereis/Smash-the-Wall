using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers
{
    public class UIHelper
    {
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _raycastResults;

        public static bool IsPointOverUI()
        {
            return IsPointOverUI(UnityEngine.InputSystem.Mouse.current.position.value);
        }

        public static bool IsPointOverUI(Vector2 mousePosition)
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = mousePosition };
            _raycastResults = new List<RaycastResult>();

            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _raycastResults);

            return _raycastResults.Count > 0;
        }

        public static Vector2 GetWorlPositonOfCanvasElement(RectTransform rectTransform, Camera camera)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, rectTransform.position, camera, out var worldPoint);

            return worldPoint;
        }
    }
}