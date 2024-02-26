using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public static class ObjectHandler
    {
        public static IEnumerator MoveCoroutine(GameObject gameObject, Vector2 destination, float boundsWidth, float boundsHeight, float duration, UnityAction call = null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();

            Vector3 startingPosition = rectTransform.transform.localPosition;

            // Clamp destination within bounds
            Vector2 clampedDestination = new Vector2(
                Mathf.Clamp(destination.x, -boundsWidth / 2, boundsWidth / 2),
                Mathf.Clamp(destination.y, -boundsHeight / 2, boundsHeight / 2)
            );
            
            yield return TimerCoroutine(duration, t => 
            {
                // Use 't' directly as it is already normalized to [0, 1] over the duration
                rectTransform.anchoredPosition = Vector2.Lerp(startingPosition, clampedDestination, t);
            }, call);

        }
        public static void  ClampPositionLocal(RectTransform objectRectTransform, Rect boundsRect)
        {
            Vector2 newPosition = objectRectTransform.anchoredPosition;

            newPosition.x = Mathf.Clamp(newPosition.x, boundsRect.xMin, boundsRect.xMax);
            newPosition.y = Mathf.Clamp(newPosition.y, boundsRect.yMin, boundsRect.yMax);

            objectRectTransform.anchoredPosition = newPosition;
        }

        public static IEnumerator ChangeSizeCoroutine(GameObject gameObject, (float targetWidth, float targetHeight)dimensions, float duration, UnityAction call = null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
    
            Vector2 startingSize = rectTransform.sizeDelta;
            Vector2 targetSize = new Vector2(dimensions.targetWidth, dimensions.targetHeight);
            

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                rectTransform.sizeDelta = Vector2.Lerp(startingSize, targetSize, t);
                if (call != null) call.Invoke();
                yield return null;
            }
        }
        public static void SetSize(GameObject gameObject, (float targetWidth, float targetHeight)dimensions)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(dimensions.targetWidth, dimensions.targetHeight);
        }

        
        public static IEnumerator TimerCoroutine(float duration, System.Action<float> perTickCallback, UnityAction call = null)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsed / duration);

                // Call the per-tick callback with the normalized time
                perTickCallback?.Invoke(normalizedTime);

                // Optionally call 'call' on each tick if needed
                call?.Invoke();

                yield return null;
            }
        }



    }
}