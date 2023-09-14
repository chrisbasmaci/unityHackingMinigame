using System;
using UnityEngine;
using UnityEngine.UI;

public class UntangleUsi : UIPanel
{
    public SettingsSlider vertexSlider;
    public SettingsSlider timeSlider;
    private GameWindow _gameWindow;
    // Start is called before the first frame update

    
    public override void Initialize(GameWindow gameWindow)
    {
        Debug.Log("Initialized settings manager");
        _gameWindow = gameWindow;
        _gameWindow.MinigamePanel._miniGame.Settings = new UntangleSettings();

    }

    public void InitSliders(SettingsSlider time, SettingsSlider vertex)
    {
        vertexSlider = vertex;
        timeSlider = time;
    }

    public override void ShowPanel()
    {
        var mg = (UntangleSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        vertexSlider.Initialize("Vertex", mg.CurrentVertexTotal,5,30);
        timeSlider.Initialize("Time", mg.CurrentPuzzleTimer,2,60);
        base.ShowPanel();
        vertexSlider.slidingBar.onValueChanged.AddListener(VertexAmountSlider);
        timeSlider.slidingBar.onValueChanged.AddListener(TimeAmountSlider);
    }

    public void TimeAmountSlider(float value)
    {
        
        var _settings = (UntangleSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        if (_settings != null) // Check if _settings is not null before using it.
        {
            Debug.Log("Vallahi set" + value);
            _settings.CurrentPuzzleTimer = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TimeAmountSlider().");
        }
    }

    public void VertexAmountSlider(float value)
    {
        Debug.Log("Vertex Slider");
        var _settings = (UntangleSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        if (_settings!= null) // Check if _settings is not null before using it.
        {
            _settings.CurrentVertexTotal = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TileAmountSlider().");
        }
    }

}
