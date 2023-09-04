using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BUIuntangle :UIPanel
{
    [SerializeField]private  Button _leftButton;
    [SerializeField]private Button _rightButton;
    [SerializeField]public Image loadingbarTimer;
    public override void Initialize(GameWindow gameWindow)
    {
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