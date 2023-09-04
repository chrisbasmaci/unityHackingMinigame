using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UUIhack :UIPanel
{
    [SerializeField]public  Button leftButton;
    [SerializeField]private Button rightButton;
    [SerializeField]public TMP_Text streakText;
    private GameWindow _gameWindow;

    public override void Initialize(GameWindow gameWindow)
    {
        _gameWindow = gameWindow;
        Debug.Log("BUTTON");
        leftButton.onClick.AddListener(gameWindow.SettingsButton);
        rightButton.onClick.AddListener(aaa);
    }

    private void aaa()
    {
        Debug.Log("AAAAA");

    }
}