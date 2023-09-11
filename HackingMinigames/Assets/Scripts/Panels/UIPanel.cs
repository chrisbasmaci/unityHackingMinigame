
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIPanel :MonoBehaviour
{
    public abstract void Initialize(GameWindow gameWindow);

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
