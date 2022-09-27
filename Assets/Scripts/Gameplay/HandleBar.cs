using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;
using UnityEngine.EventSystems;

namespace Gameplay.Main
{
    public class HandleBar : MonoBehaviour, IHandleBar, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [field: SerializeField] public float CurrentSpeed { get; private set; }
        [SerializeField] private RectTransform rectTransform;

        private Vector2 anchorPosition;
        private float horizontal;
        private float lastAnchorX;
        private Coroutine coroutine;
        private float pointerClickDistance;


        public void Move(float direction)
        {
            transform.Translate(Vector3.right * direction * CurrentSpeed * Time.deltaTime);
        }

        public void SetSpeed(float newSpeed)
        {
            CurrentSpeed = newSpeed;
        }

        public float GetHorizontalForce()
        {
            return horizontal;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            lastAnchorX = rectTransform.anchoredPosition.x;
            pointerClickDistance = rectTransform.anchoredPosition.x - ToScreenHorizontal(eventData.position.x);
        }

        public void OnDrag(PointerEventData eventData)
        {
            anchorPosition = rectTransform.anchoredPosition;
            anchorPosition.x = ToScreenHorizontal(eventData.position.x) + pointerClickDistance;
            rectTransform.anchoredPosition = anchorPosition;
            horizontal = Mathf.Sign(anchorPosition.x - lastAnchorX);
            lastAnchorX = anchorPosition.x;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            else
            {
                coroutine = StartCoroutine(DelayToResetHorizontal(2));
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            horizontal = 0;
        }

        private float ToScreenHorizontal(float value)
        {
            return value / ScreenEdgeController.Instance.CanvasScaleFactor - ScreenEdgeController.Instance.CanvasResolution.x / 2f;
        }
        private IEnumerator DelayToResetHorizontal(int frame)
        {
            for (int i = 0; i < frame; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            horizontal = 0;
        }
    }
}