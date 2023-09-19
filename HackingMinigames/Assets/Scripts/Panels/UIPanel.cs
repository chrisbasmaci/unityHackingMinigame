
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIPanel :MonoBehaviour
{
    protected GameWindow GameWindow;

    public virtual void Initialize(GameWindow gameWindow)
    {
        GameWindow = gameWindow;
    }

    public virtual void ShowPanel()
    {
        gameObject.SetActive(true);
    }    
    public virtual void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public virtual void ResetPanel()
    {
        throw new NotImplementedException();
    }

}
