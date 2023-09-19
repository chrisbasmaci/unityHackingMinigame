
using System;
using UnityEngine;

public class SettingsPanel : UIPanel
{
    public override void Initialize(GameWindow gameWindow)
    {
        base.Initialize(gameWindow);
        InitSliders();
    }

    public virtual void InitSliders() {
        Debug.LogWarning("No sliders for this game");
    }
}
