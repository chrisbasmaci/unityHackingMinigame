using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MgPanel : MonoBehaviour
{
    public Rect _panelRect => gameObject.GetComponent<RectTransform>().rect;
    public GameWindow gameWindow;
    [NonSerialized]public MiniGame _miniGame;
    [NonSerialized]private MinigameType _minigameType;
    // [NonSerialized]public GameObject currentSettingsPrefab;

    private void Start()
    {
        if (_panelRect == null)
        {
            Debug.LogError("Panel Bounds not found");
        }
        Debug.Log("height: " + _panelRect.height);
        Debug.Log("width: " + _panelRect.width);
    }

    public void Initialize(GameWindow window, MinigameType mgType)
    {
        gameWindow = window;
        AddMinigameScript(mgType);
    }
    public void FixLayoutOrder(int order)
    {
        gameObject.GetComponent<Canvas>().sortingOrder = order;
        _miniGame.FixLayoutOrder(order);
        
    }

    public void stopGameCoroutines()
    {
        // Destroy(gameWindow.UUIpanel.gameObject);
        // Destroy(gameWindow.BUIPanel.gameObject);
        
        _miniGame.EndMinigame();
        StopAllCoroutines();
    }
    /// <instruction>
    /// EXAMPLE INSTRUCTION (3)
    /// Add it to the switch
    /// </instruction>
    public void AddMinigameScript(MinigameType mgType)
    {
        Debug.Log("Adding minigame script0");
        Type type = Type.GetType(mgType.ToString());
        Debug.Log("Adding minigame script1");
        if (type != null) {
            _miniGame = (MiniGame)gameObject.AddComponent(type);
        }else {
            Debug.LogWarning("Type not found");
            _miniGame = gameObject.AddComponent<ExampleMG>();
        }
        Debug.Log("Added minigame script");
        
        var set =_miniGame.AddSettings();  

        _miniGame.Initialize(this,set);
    }
    public void StartMinigame()
    {
        StartCoroutine(_miniGame.StartMinigame());
    }

    public void WindowResizeEvent(Component sender, object data)
    {

        Debug.Log("signal rect");

        _miniGame?.ResizeMinigame();
    
    }

 
    
}
