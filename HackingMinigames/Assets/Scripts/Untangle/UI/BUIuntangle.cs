using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BUIuntangle :UIPanel
{
    [SerializeField]private  Button _leftButton;
    [SerializeField]private Button _rightButton;
    [SerializeField]public TMP_Text movesText;
    private GameObject _bottomPanel;
    [SerializeField]private GameObject _buttonsGOBJ;
    public override void Initialize(GameObject bottomPanel, float height)
    {
        _bottomPanel = bottomPanel;
        var rectTransform = _bottomPanel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, height);
        // var rectTransform2 = _buttonsGOBJ.GetComponent<RectTransform>();
        // rectTransform2.sizeDelta = new Vector2(newWidth, newHeight);

        // Debug.Log("BUTTON");
        // rightButton.onClick.AddListener(aaa);
    }

    
    public void InitializeLeftButton(UnityAction call)
    {
        _leftButton.onClick.AddListener(call);
    }    
    
    public void InitializeRightButton(UnityAction call)
    {
        _rightButton.onClick.AddListener(call);
    }
    
}