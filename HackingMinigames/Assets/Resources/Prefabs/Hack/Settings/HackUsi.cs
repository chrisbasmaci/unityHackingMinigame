using System;
using UnityEngine;
using UnityEngine.UI;

public class HackUsi : UIPanel
{
    [SerializeField] Slider tileSlider;
    [SerializeField] Slider timeSlider;
    private GameWindow _gameWindow;
    // Start is called before the first frame update

    public override void Initialize(GameWindow gameWindow)
    {
        Debug.Log("Initialized settings manager");
        _gameWindow = gameWindow;
        tileSlider.onValueChanged.AddListener(TileAmountSlider);
        timeSlider.onValueChanged.AddListener(TimeAmountSlider);

        
    }

    public override void ShowPanel()
    {
        var mg = (HackSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        tileSlider.value = mg.currentCardTotal;
        timeSlider.value = mg.CurrentPuzzleTimer;
        base.ShowPanel();
    }

    public void TimeAmountSlider(float value)
    {
        var _settings = (HackSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        if (_settings != null) // Check if _settings is not null before using it.
        {
            _settings.CurrentPuzzleTimer = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TimeAmountSlider().");
        }
    }

    public void TileAmountSlider(float value)
    {
        var _settings = (HackSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        if (_settings!= null) // Check if _settings is not null before using it.
        {
            _settings.currentCardTotal = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TileAmountSlider().");
        }
    }

}
