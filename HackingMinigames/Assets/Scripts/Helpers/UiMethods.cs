using System;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers
{
    public class UiMethods : MonoBehaviour
    {
        public int nestedness;
        public GameObject parent =>GetParent();

        private LayoutElement layoutElement => parent.GetComponent<LayoutElement>();
        private RectTransform rectTransform => parent.GetComponent<RectTransform>();
        private float lastWidth;
        private float lastHeight;

        private float minHeight;
        private float minWidth;

        // private bool state;
        
        private void Start()
        {
            lastHeight = rectTransform.rect.height;
            lastWidth = rectTransform.rect.width;
            Debug.Log("SSSS" + parent.name);
            minHeight = layoutElement.minHeight;
            minWidth = layoutElement.minWidth;
            Debug.Log("SSSS" + "minHeightS: " + minHeight);
            Debug.Log("SSSS" + "minWidthS: " + minWidth);
            
        }
        public void ParentRefresh()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            Debug.Log("parent refreshed");
        }
        private GameObject GetParent()
        {
            Transform currentTransform = transform;
            for (int i = 0; i < nestedness && currentTransform.parent != null; i++)
            {
                currentTransform = currentTransform.parent;
            }
            Debug.Log("parent: " + currentTransform.gameObject.name);
            return currentTransform.gameObject;
        }
        private Canvas GetCanvas()
        {
            Transform currentTransform = transform;
            Canvas canvas;
            while (currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                Debug.Log(currentTransform.gameObject.name);
                canvas = currentTransform.gameObject.GetComponent<Canvas>();
                if (canvas)
                {
                    return canvas;
                }
            }
            Debug.LogWarning("no parent has a canvas bro!");
            return null;
        }
        
        public void SelfToggleActive () {
            gameObject.SetActive (!gameObject.activeSelf);
        }

        public void ParentDeactivate()
        {
            parent.SetActive(false);
        }


        public void ParentMoveToBottom()
        {
            var canvBottom = GetCanvas().GetComponent<RectTransform>().rect.yMin;
            RectTransform rectTransform = parent.GetComponent<RectTransform>();

            Debug.Log(canvBottom);

            rectTransform.localPosition =
                new Vector2(rectTransform.localPosition.x, canvBottom + rectTransform.rect.height / 2);
        }
        public void ParentMoveToMiddle()
        {
            RectTransform rectTransform = parent.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector2(0, 0);
        }
        public void ParentFitVerticalTight(Vector2? mins =null , float fitHeight = 0)
        {
            Debug.Log("SSSS" + parent.name);
            Debug.Log("SSSS" + "minHeightV1: " + minHeight);
            Debug.Log("SSSS" + "minWidthV1: " + minWidth);
            lastHeight = rectTransform.rect.height;
            lastWidth = rectTransform.rect.width;
            if (mins!=null)
            {
                minWidth = mins.Value.x;
                minHeight = mins.Value.y;
            }
      
            var sizeFitter = parent.GetComponent<ContentSizeFitter>();
            if (!sizeFitter)
            {
                sizeFitter = parent.AddComponent<ContentSizeFitter>();
            }
            parent.GetComponent<LayoutElement>().minWidth = 200;

            sizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            // sizeFitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            
            // rectTransform.sizeDelta = new Vector2(minWidth, lastHeight);
            if (fitHeight != 0)
            {
                parent.GetComponent<LayoutElement>().minHeight = fitHeight;
            }
            Debug.Log("minHeightV2: " + minHeight);
            Debug.Log("minWidthV2: " + minWidth);

        }
        public void ParentFitVerticalUnconstrained(float fitHeight)
        {
            Debug.Log("minHeightB: " + minHeight);
            Debug.Log("minWidthB: " + minWidth);
            var sizeFitter = parent.GetComponent<ContentSizeFitter>();
            if (!sizeFitter) {
                sizeFitter = parent.AddComponent<ContentSizeFitter>();
            }
            var layoutElement = parent.GetComponent<LayoutElement>();
            layoutElement.minHeight = fitHeight;
            parent.GetComponent<LayoutElement>().minWidth = minWidth;
            parent.GetComponent<LayoutElement>().minHeight = minHeight;
            Debug.Log("minHeight: " + minHeight);
            Debug.Log("minWidth: " + minWidth);
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            ParentRefresh();
            rectTransform.sizeDelta = new Vector2(lastWidth, lastHeight);
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
        }

        public void ToggleImageRaycast()
        {
            gameObject.GetComponent<Image>().raycastTarget = !gameObject.GetComponent<Image>().raycastTarget ;
        }
    
    }

}