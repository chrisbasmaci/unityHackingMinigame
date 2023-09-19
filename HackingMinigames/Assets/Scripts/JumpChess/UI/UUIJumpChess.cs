using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UUIJumpChess :UIPanel
{
    [SerializeField]public  Button leftButton;
    [SerializeField]private Button rightButton;
    // [SerializeField]private TMP_Text streakText;
    private GameWindow _gameWindow;

    public override void Initialize(GameWindow gameWindow)
    {
        _gameWindow = gameWindow;
        Debug.Log("BUTTON");
        leftButton.onClick.AddListener(gameWindow.SettingsButton);
    }

    public override void ResetPanel()
    {
    }
    
    
}