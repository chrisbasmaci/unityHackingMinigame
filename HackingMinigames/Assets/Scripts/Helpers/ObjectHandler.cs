using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public static class ObjectHandler
    {
        public static IEnumerator MoveCoroutine(GameObject gameObject, Vector2 destination, float duration, UnityAction call = null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (!rectTransform) {
                rectTransform = gameObject.AddComponent<RectTransform>();
            }
            
            Vector3 startingPosition = rectTransform.transform.localPosition;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                rectTransform.transform.localPosition = Vector3.Lerp(startingPosition,
                    new Vector3(destination.x, destination.y, startingPosition.z), t);
                
                if (call != null) {
                    call();
                }
                yield return null;

            }
        }
    }
}