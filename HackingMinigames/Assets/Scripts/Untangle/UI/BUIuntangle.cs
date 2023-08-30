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
    public override void Initialize(GameObject bottomPanel)
    {
        _bottomPanel = bottomPanel;
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