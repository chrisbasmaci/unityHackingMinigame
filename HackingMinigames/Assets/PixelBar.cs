using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PixelBar : MonoBehaviour
{
    // Start is called before the first frame update
    private WindowMethods windowMethods;
    [SerializeField]public GameObject resizeButtonGj;
    [SerializeField]public GameObject dragButtonGj;
    [SerializeField]public GameObject toggleButtonGj;
    [SerializeField]public GameObject closeButtonGj;
    void Start()
    {
        windowMethods = GetComponentInParent<WindowMethods>();
        if (windowMethods == null)
        {
            Debug.LogError("Parent needs windowMethods");
        }

        var toggleButton = toggleButtonGj.GetComponent<Button>();
        var closeButton = closeButtonGj.GetComponent<Button>();
        toggleButton.onClick.AddListener(() => windowMethods.MinimizeWindow());
        closeButton.onClick.AddListener(() => windowMethods.CloseWindow());
    }
    


}
