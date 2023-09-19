using System;
using UnityEngine;
using UnityEngine.UI;

public class UntangleUsi : UIPanel
{
    public SettingsSlider vertexSlider;
    public SettingsSlider timeSlider;
    // Start is called before the first frame update

    
    public override void Initialize(GameWindow gameWindow)
    {
        GameWindow = gameWindow;
    }

    public void InitSliders(SettingsSlider time, SettingsSlider vertex)
    {
        vertexSlider = vertex;
        timeSlider = time;
        UntangleSettings settings = (UntangleSettings)GameWindow.MinigamePanel._miniGame.Settings;
        vertexSlider.Initialize("Vertex", settings.CurrentVertexTotal,5,30);
        timeSlider.Initialize("Time", settings.CurrentPuzzleTimer,2,60);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        vertexSlider.slidingBar.onValueChanged.AddListener(VertexAmountSlider);
        timeSlider.slidingBar.onValueChanged.AddListener(TimeAmountSlider);
    }

    public void TimeAmountSlider(float value)
    {
        
        var _settings = (UntangleSettings)GameWindow.MinigamePanel._miniGame.Settings;
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
        var _settings = (UntangleSettings)GameWindow.MinigamePanel._miniGame.Settings;
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
