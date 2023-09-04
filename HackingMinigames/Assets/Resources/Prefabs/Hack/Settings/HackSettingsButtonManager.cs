using System;
using UnityEngine;
using UnityEngine.UI;

public class HackSettingsButtonManager : UIPanel
{
    [SerializeField] Slider tileSlider;
    [SerializeField] Slider timeSlider;
    private GameWindow _gameWindow;
    // Start is called before the first frame update

    public override void Initialize(GameWindow gameWindow)
    {
        Debug.Log("Initialized settings manager");
        _gameWindow = gameWindow;
        _gameWindow.MinigamePanel._miniGame.Settings = new HackSettings();
        
    }

    public void TimeAmountSlider()
    {
        var _settings = (HackSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        if (_settings != null) // Check if _settings is not null before using it.
        {
            _settings.CurrentPuzzleTimer = (int)timeSlider.value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TimeAmountSlider().");
        }
    }

    public void TileAmountSlider()
    {
        var _settings = (HackSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        if (_settings!= null) // Check if _settings is not null before using it.
        {
            _settings.currentCardTotal = (int)tileSlider.value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TileAmountSlider().");
        }
    }

}
