using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UUIhack :UIPanel
{
    [SerializeField]public  Button leftButton;
    [SerializeField]private Button rightButton;
    [SerializeField]public TMP_Text streakText;

    public override void Initialize(GameObject gameCanvas, float height)
    {
        Debug.Log("BUTTON");
        // leftButton.onClick = new Button.ButtonClickedEvent();
        leftButton.onClick.AddListener(ButtonManager.Instance.backToSettings);
        rightButton.onClick.AddListener(aaa);
    }

    private void aaa()
    {
        Debug.Log("AAAAA");

    }
}