using System;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers
{
    public class UiMethods : MonoBehaviour
    {
        public int nestedness;
        private GameObject parent;
        private bool state;

        private void Start()
        {
            parent = GetParent();
        }
        public void ParentRefresh()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
            Debug.Log("parent refreshed");
        }
        private GameObject GetParent()
        {
            Transform currentTransform = transform;
            for (int i = 0; i < nestedness && currentTransform.parent != null; i++)
            {
                currentTransform = currentTransform.parent;
            }
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

        public void ParentToBottomToggle()
        {
            if (state)
            {
                state = false;
                ParentMoveToMiddle();
            }else {
                state = true;
                ParentMoveToBottom();
            }
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
        public void ParentFitVerticalTightToggle()
        {
            var sizeFitter = parent.GetComponent<ContentSizeFitter>();
            if (!sizeFitter)
            {
                sizeFitter = parent.AddComponent<ContentSizeFitter>();
            }

            sizeFitter.verticalFit = (sizeFitter.verticalFit == ContentSizeFitter.FitMode.Unconstrained)
                ? ContentSizeFitter.FitMode.MinSize
                : ContentSizeFitter.FitMode.PreferredSize;
            
            ParentRefresh();
            sizeFitter.verticalFit = (sizeFitter.verticalFit == ContentSizeFitter.FitMode.PreferredSize)
                ? ContentSizeFitter.FitMode.Unconstrained 
                : sizeFitter.verticalFit;
        }

        public void ToggleImageRaycast()
        {
            gameObject.GetComponent<Image>().raycastTarget = !gameObject.GetComponent<Image>().raycastTarget ;
        }
    
    }

}