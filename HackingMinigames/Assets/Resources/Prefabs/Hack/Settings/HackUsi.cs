using System;
using UnityEngine;
using UnityEngine.UI;

public class HackUsi : UIPanel
{
    public HackSettings Settings => GameWindow.MinigamePanel._miniGame.Settings as HackSettings;

    public SettingsSlider tileSlider;
    public SettingsSlider timeSlider;
    // Start is called before the first frame update
    
    public void InitSliders(SettingsSlider time, SettingsSlider vertex)
    {
        tileSlider = vertex;
        timeSlider = time;
        tileSlider.Initialize("Card", Settings.currentCardTotal,2,9);
        timeSlider.Initialize("Time", Settings.CurrentPuzzleTimer,2,60);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        tileSlider.slidingBar.onValueChanged.AddListener(TileAmountSlider);
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

    public void TileAmountSlider(float value)
    {
        if (Settings!= null) // Check if _settings is not null before using it.
        {
            Settings.currentCardTotal = (int)value;
        }
        else
        {
            Debug.LogError("_settings is null. Ensure Initialize() is called before using TileAmountSlider().");
        }
    }

}
