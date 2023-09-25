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
    // [SerializeField]GameCanvas gameCanvas;
    [SerializeField]public GameWindow gameWindow;
    public Rect _panelRect => gameObject.GetComponent<RectTransform>().rect;
    [NonSerialized]public WindowSize panelBounds;
    [NonSerialized]public MiniGame _miniGame;
    [NonSerialized]private MinigameType _minigameType;
    // [NonSerialized]public GameObject currentSettingsPrefab;





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
    public void AddMinigameScript()
    {
        switch (gameWindow.currentMg)
        {
            case MinigameType.EXAMPLE:
                _miniGame = gameObject.AddComponent<ExampleMG>();
                break;       
            case MinigameType.HACK:
                _miniGame = gameObject.AddComponent<HackingMG>();
                break;
            case MinigameType.UNTANGLE:
                _miniGame = gameObject.AddComponent<UntangleMG>();
                break;            
            case MinigameType.JumpChess:
                _miniGame = gameObject.AddComponent<JumpChessMG>();
                break;
            default:
                _miniGame = gameObject.AddComponent<ExampleMG>();
                break;
        }

        var set =_miniGame.AddSettings();
        ComponentHandler.AddCanvasWithOverrideSorting(gameObject, "GameWindow", gameWindow.currentSortingLayer +1);

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
