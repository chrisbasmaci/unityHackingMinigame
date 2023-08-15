using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BUIuntangle :UIPanel
{
    [SerializeField]public  Button leftButton;
    [SerializeField]private Button rightButton;
    [SerializeField]public TMP_Text movesText;

    public override void Initialize()
    {

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