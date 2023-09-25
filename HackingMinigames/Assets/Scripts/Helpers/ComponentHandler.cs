
using System.Collections;
using JetBrains.Annotations;
using ui;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public static class ComponentHandler
{
    private static GameObject _settingSlider =Resources.Load<GameObject>("UI_Prefabs/SliderPrefab") ;
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
    public static SpriteRenderer AddSpriteComponent(GameObject gameObject, Sprite image = null, Color? color = null)
    {
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = image;

        if (color != null)
        {
            spriteRenderer.color = color.Value;
        }

        return spriteRenderer;
    }
    public static SortingGroup AddSortingGroup(GameObject obj, string sortingLayerName, int sortingOrder = 0)
    {
        
        SortingGroup sortingGroup = obj.AddComponent<SortingGroup>();
        sortingGroup.sortingLayerName = sortingLayerName;
        sortingGroup.sortingOrder = sortingOrder;
        return sortingGroup;
    }

    public static void AddCanvasWithOverrideSorting(GameObject obj, string sortingLayerName, int sortingOrder = 0)
    {
        Canvas canvas = obj.GetComponent<Canvas>();
        if (!canvas)
        { 
            canvas = obj.AddComponent<Canvas>();
        }
        canvas.overrideSorting = true;
        canvas.sortingLayerName = sortingLayerName;
        canvas.sortingOrder = sortingOrder;

        if (obj.GetComponent<GraphicRaycaster>() == null)
        {
            GraphicRaycaster raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = false;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        }
        canvas.overrideSorting = true;
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
    
    public static VerticalLayoutGroup AddVerticalLayoutGroup(GameObject gameObject, RectOffset padding)
    {
        VerticalLayoutGroup verticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
        if (!verticalLayoutGroup) {
            verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            
        }else {
            Debug.LogWarning("The GameObject already has a VerticalLayoutGroup component.");
        }
        
        // verticalLayoutGroup.spacing = 10f;

        // You can set other properties like padding, child alignment, etc.
        verticalLayoutGroup.padding = padding;
        verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        // verticalLayoutGroup.childForceExpandHeight = false;
        // verticalLayoutGroup.childForceExpandWidth = false;
        return verticalLayoutGroup;
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

    public static void AddMaximisedGridLayout(GameObject gameObject, float ratio = 1)
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

    public static void AddFlowLayout(GameObject gameObject)
    {
        var currentLayoutGroup = gameObject.GetComponent<LayoutGroup>();
        if (currentLayoutGroup) {
            Object.DestroyImmediate(currentLayoutGroup);
        }

        var flowLayout = gameObject.AddComponent<FlowLayoutGroup>();
        flowLayout.childAlignment = TextAnchor.MiddleCenter;
        flowLayout.ChildForceExpandWidth = true;
    }
}
