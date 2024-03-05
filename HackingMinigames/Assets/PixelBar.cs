using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PixelBar : MonoBehaviour
{
    // Start is called before the first frame update
    private WindowMethods windowMethods;
    [SerializeField]public ResizePanel resizeButton;
    [SerializeField]public GameObject backButtonGj;
    [SerializeField]public DragPanel dragButtonGj;
    [SerializeField]public GameObject minimizeButtonGj;
    [SerializeField]public GameObject closeButtonGj;
    [SerializeField]public GameObject infoButtonGj;
    private float _startingOffsetMin;
    private float _startingOffsetMax;
    void Start()
    {
        windowMethods = GetComponentInParent<WindowMethods>();
        if (windowMethods == null)
        {
            Debug.LogError("Parent needs windowMethods");
        }

        var toggleButton = minimizeButtonGj.GetComponent<Button>();
        var closeButton = closeButtonGj.GetComponent<Button>();
        toggleButton.onClick.AddListener(() => windowMethods.MinimizeWindow());
        closeButton.onClick.AddListener(() => windowMethods.CloseWindow());
        
    _startingOffsetMax = dragButtonGj.GetComponent<RectTransform>().offsetMax.x;
    _startingOffsetMin = dragButtonGj.GetComponent<RectTransform>().offsetMin.x;
    }
    private void SetButtonsMinimized(bool isMinimized)
    {
 
        if (resizeButton != null)
        {
            resizeButton.gameObject.SetActive(!isMinimized);
        }
        if (infoButtonGj != null)
        {
            infoButtonGj.SetActive(!isMinimized);
        }
        if (backButtonGj != null)
        {
            backButtonGj.SetActive(!isMinimized);
        }

    }

    public void SetLowered(bool isLowered)
    {
        SetButtonsMinimized(isLowered);
        GetComponentInChildren<DragPanel>().ToggleVerticalDrag();
        var rectTransform = dragButtonGj.GetComponent<RectTransform>();
        if (isLowered)
        {
            rectTransform.offsetMin = new Vector2(10, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-120, rectTransform.offsetMax.y);
        }
        else
        {
            rectTransform.offsetMin = new Vector2(_startingOffsetMin, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(_startingOffsetMax, rectTransform.offsetMax.y);
        }


    }
    


}
