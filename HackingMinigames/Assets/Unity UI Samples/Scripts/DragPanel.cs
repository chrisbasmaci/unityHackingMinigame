using CoreScripts;
using Helpers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private RectTransform panelRectTransform;
    private RectTransform parentRectTransform;

    [SerializeField]public WindowMethods windowMethods;
    public bool lockHorizontalDrag = true;
    public bool lockVerticalDrag = false;
    public int nestedness;

    
    void Awake()
    {
        panelRectTransform = GetParent().transform as RectTransform;
        parentRectTransform = panelRectTransform.parent as RectTransform;
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
    public void OnPointerDown(PointerEventData data)
    {
        originalPanelLocalPosition = panelRectTransform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
        if (windowMethods == null)
        {
            Debug.Log("parent: " + GetParent().name);
            windowMethods = GetParent().GetComponent<WindowMethods>();
        }
        if (windowMethods != null)
        {
            windowMethods.FixLayer();
        }else
        {
            Debug.LogError("WindowMethods is null");
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (panelRectTransform == null || parentRectTransform == null)
            return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;

            Vector3 newPanelPosition = originalPanelLocalPosition + offsetToOriginal;


            if (lockHorizontalDrag)
            {
                newPanelPosition.x = originalPanelLocalPosition.x;
            }
            if (lockVerticalDrag)
            {
                newPanelPosition.y = originalPanelLocalPosition.y;
            }
            
            panelRectTransform.localPosition = newPanelPosition;
            ClampToWindow();
        }
    }

    public void ToggleHorizontalDrag()
    {
        lockHorizontalDrag = !lockHorizontalDrag;
    }    
    
    public void ToggleVerticalDrag()
    {
        lockVerticalDrag = !lockVerticalDrag;
    }

    void ClampToWindow()
    {
        Vector3 pos = panelRectTransform.localPosition;

        Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
        Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;

        if (!lockHorizontalDrag)
        {
            pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        }
        if (!lockVerticalDrag)
        {
            pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);
        }

        panelRectTransform.localPosition = pos;
    }
}
