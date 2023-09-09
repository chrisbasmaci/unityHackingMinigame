
using System.Collections;
using JetBrains.Annotations;
using ui;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public static class ComponentHandler
{
    public static GameObject AddChildGameObject(GameObject parentObject, string childName, Vector3? pos = null)
    {
        GameObject gameObject = new GameObject(childName);
        Transform childTransform = gameObject.transform;
        childTransform.SetParent(parentObject.transform);

        if (pos != null) {
            childTransform.localPosition = pos.Value;
        }
        childTransform.localScale = Vector3.one;

        return gameObject;
    }
    
    
    public static Image AddImageComponent(GameObject gameObject, Sprite image = null, Color? color = null)
    {
        var imageComponent = gameObject.AddComponent<Image>();
        imageComponent.sprite = image;
        imageComponent.preserveAspect = true;

        if (color != null)
        {
            imageComponent.color = color.Value;
        }

        return imageComponent;
    }

    public static AspectRatioFitter AddAspectRatioFitter(GameObject gameObject, float width, float height)
    {
        var aspectRatioFitterComponent = gameObject.GetComponent<AspectRatioFitter>();
        if (!aspectRatioFitterComponent) {
            aspectRatioFitterComponent = gameObject.AddComponent<AspectRatioFitter>();
        }

        aspectRatioFitterComponent.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;

        aspectRatioFitterComponent.aspectRatio = width / height;

        return aspectRatioFitterComponent;
    }

    public static void SetAnchorToStretch(GameObject gameObject)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform = (rectTransform) ? rectTransform : gameObject.AddComponent<RectTransform>();



        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);

        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = Vector2.zero;
    }
    
    public static void AddVerticalLayoutGroup(GameObject gameObject)
    {
        VerticalLayoutGroup verticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
        if (verticalLayoutGroup == null)
        {
            verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
        

            // verticalLayoutGroup.spacing = 10f;

            // You can set other properties like padding, child alignment, etc.
            // verticalLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
            // verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
            // verticalLayoutGroup.childForceExpandHeight = false;
            // verticalLayoutGroup.childForceExpandWidth = false;
        }
        else
        {
            Debug.LogWarning("The GameObject already has a VerticalLayoutGroup component.");
        }
    }
    public static void AddLayoutElement(GameObject gameObject,
                                        float? flexibleWidth = null, float? flexibleHeight = null,  
                                        float? minWidth = null, float? minHeight = null,
                                        float? preferredWidth = null, float? preferredHeight = null )
    {
        LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();

        if (!layoutElement) {
            layoutElement = gameObject.AddComponent<LayoutElement>(); 
        }
        else {
            Debug.LogWarning("The GameObject already has a LayoutElement component.");
            
        }
        layoutElement.flexibleWidth = flexibleWidth ?? layoutElement.flexibleWidth;
        layoutElement.flexibleHeight = flexibleHeight ?? layoutElement.flexibleHeight;
        layoutElement.minWidth = minWidth ?? layoutElement.minWidth;
        layoutElement.minHeight = minHeight ?? layoutElement.minHeight;
        layoutElement.preferredWidth = preferredWidth ?? layoutElement.preferredWidth;
        layoutElement.preferredHeight = preferredHeight ?? layoutElement.preferredHeight;
    }
    public static AspectRatioFitter AddAspectRatioFitter(GameObject gameObject, AspectRatioFitter.AspectMode aspectMode, float aspectRatio = 1)
    {
        AspectRatioFitter aspectRatioFitter = gameObject.GetComponent<AspectRatioFitter>();

        if (aspectRatioFitter == null)
        {
            aspectRatioFitter = gameObject.AddComponent<AspectRatioFitter>();
        }

        aspectRatioFitter.aspectMode = aspectMode;
        aspectRatioFitter.aspectRatio = aspectRatio;

        return aspectRatioFitter;
    }

    public static void AddMaximisedGridLayout(GameObject gameObject, float ratio)
    {
        var layout = gameObject.AddComponent<GridLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        var maximiser = gameObject.AddComponent<GridLayoutMaximiser>();
        maximiser.cellRatio = ratio;
    }
    public static IEnumerator FillImage(
        GameObject targetGameObject, 
        float fillTime, int fillOrigin, Image.FillMethod fillMethod,
        float startFillAmount = 0f,float targetFillAmount = 1f)
    {
        Image targetImage = targetGameObject.GetComponent<Image>();
        targetImage.type = Image.Type.Filled;
        
        targetImage.fillMethod = fillMethod;
        targetImage.fillOrigin = fillOrigin;
        targetImage.fillAmount = startFillAmount;

        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            elapsedTime += Time.deltaTime;
            float fillPercentage = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / fillTime);
            targetImage.fillAmount = fillPercentage;
            yield return null;
        }

        // Ensure the fill is complete.
        targetImage.fillAmount = targetFillAmount;
    }

    public static Canvas AddCanvasAndRaycaster(GameObject gameObject, int order)
    {
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.sortingOrder = order;
        gameObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }
}
