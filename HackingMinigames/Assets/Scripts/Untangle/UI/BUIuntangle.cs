using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BUIuntangle :UIPanel
{
    [SerializeField]public  Button leftButton;
    [SerializeField]private Button rightButton;
    [SerializeField]public TMP_Text movesText;
    private GameObject _bottomPanel;
    [SerializeField]private GameObject _buttonsGOBJ;
    public override void Initialize(GameObject gameCanvas, float height)
    {
        _bottomPanel = gameCanvas;
        var rectTransform = _bottomPanel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, height);
        // var rectTransform2 = _buttonsGOBJ.GetComponent<RectTransform>();
        // rectTransform2.sizeDelta = new Vector2(newWidth, newHeight);

        // Debug.Log("BUTTON");
        // rightButton.onClick.AddListener(aaa);
    }

    
    public void InitializeLeftButton(UnityAction call)
    {
        leftButton.onClick.AddListener(call);
    }    
    
    public void InitializeRightButton(UnityAction call)
    {
        rightButton.onClick.AddListener(call);
    }

    private void aaa()
    {
        Debug.Log("AAAAA");

    }
}