using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

//======================================================//
//Class of general helper functions                     //
//======================================================//

namespace Utility
{
    public static class HelperFunctions
    {
        private static Camera mainCamera;
        //======================================================//
        private static PointerEventData currentPointerPosition;
        private static List<RaycastResult> results;
        //======================================================//

        public static Camera MainCamera
        {
            get
            {
                if(mainCamera == null)
                {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }

        public static bool IsPointerOverUI()
        {
            currentPointerPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(currentPointerPosition, results);

            return results.Count > 0;
        }

        public static void DeleteChildren(this Transform transform)
        {
            foreach(Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static Vector2 GetCanvasElementWorldPosition(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, MainCamera, out var result);

            return result;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return worldPos;
        }

        public static float GetAngleFromVector(Vector3 dir)
        {
            dir = dir.normalized;
            float r = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if(r < 0)
            {
                r += 360;
            }

            return r;
        }
    } 
}