using System;
using UnityEngine;
using UnityEngine.UI;

public class UntangleUsi : SettingsPanel
{
    private UntangleSettings Settings => GameWindow.MinigamePanel._miniGame.Settings as UntangleSettings;
    public SettingsSlider vertexSlider;
    public SettingsSlider timeSlider;
    // Start is called before the first frame update
    
    public override void InitSliders()
    {

        timeSlider =  Helpers.PrefabHandler.AddSliderPrefab(gameObject, "TimeSlider");
        vertexSlider =  Helpers.PrefabHandler.AddSliderPrefab(gameObject, "VertexSlider");
        
        vertexSlider.Initialize("Vertex", Settings.CurrentVertexTotal,5,30);
        timeSlider.Initialize("Time", Settings.CurrentPuzzleTimer,2,60);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        vertexSlider.slidingBar.onValueChanged.AddListener(VertexAmountSlider);
        timeSlider.slidingBar.onValueChanged.AddListener(TimeAmountSlider);
    }

    public void TimeAmountSlider(float value)
    {
        
        if (Settings != null) // Check if _settings is not null before using it.
        {
            Settings.CurrentPuzzleTimer = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TimeAmountSlider().");
        }
    }

    public void VertexAmountSlider(float value)
    {
        if (Settings!= null) // Check if _settings is not null before using it.
        {
            Settings.CurrentVertexTotal = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TileAmountSlider().");
        }
    }

}
