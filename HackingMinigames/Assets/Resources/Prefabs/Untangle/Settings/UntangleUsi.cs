using System;
using UnityEngine;
using UnityEngine.UI;

public class UntangleUsi : UIPanel
{
    [SerializeField] Slider vertexSlider;
    [SerializeField] Slider timeSlider;
    private GameWindow _gameWindow;
    // Start is called before the first frame update

    
    public override void Initialize(GameWindow gameWindow)
    {
        Debug.Log("Initialized settings manager");
        _gameWindow = gameWindow;
        _gameWindow.MinigamePanel._miniGame.Settings = new UntangleSettings();
        vertexSlider.onValueChanged.AddListener(VertexAmountSlider);
        timeSlider.onValueChanged.AddListener(TimeAmountSlider);
    }

    public override void ShowPanel()
    {
        var mg = (UntangleSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        vertexSlider.value = mg.CurrentVertexTotal;
        timeSlider.value = mg.CurrentPuzzleTimer;
        base.ShowPanel();
    }

    public void TimeAmountSlider(float value)
    {
        var _settings = (UntangleSettings)_gameWindow.MinigamePanel._miniGame.Settings;
        if (_settings != null) // Check if _settings is not null before using it.
        {
            _settings.CurrentPuzzleTimer = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TimeAmountSlider().");
        }
    }

    public void VertexAmountSlider(float value)
    {
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
