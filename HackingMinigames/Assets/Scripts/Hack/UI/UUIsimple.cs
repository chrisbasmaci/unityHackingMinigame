using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UUIsimple :UIPanel
{
    [SerializeField]private TMP_Text streakText;
    private GameWindow _gameWindow;
    string displayText = "None";

    public override void Initialize(GameWindow gameWindow)
    {
        _gameWindow = gameWindow;
    }
    public void SetDisplayText(string text)
    {
        displayText = text;
    }  

    public override void ResetPanel()
    {
        resetStreak(displayText);
    }

    public void updateDisplay(int currentStreak, string displayText)
    {
        streakText.text = displayText+": " + currentStreak;
    }

    public void resetStreak(string displayText)
    {
        streakText.text = displayText+": -";
    }
}