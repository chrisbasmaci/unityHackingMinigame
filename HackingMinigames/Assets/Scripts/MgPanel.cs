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
    [NonSerialized] private RectTransform _panelRect;
    [NonSerialized]public WindowSize panelBounds;
    [NonSerialized]public MiniGame _miniGame;
    [NonSerialized]private MinigameType _minigameType;
    // [NonSerialized]public UIPanel UUIpanel;
    // [NonSerialized]public UIPanel BUIPanel;
    private float _panelHeight = 200f;



    public void Initialize( WindowSize bounds)
    {
        panelBounds = bounds;
        _miniGame = GetComponent<MiniGame>();
    }
    public void Retry()
    {
        _miniGame.RetryMinigame();
    }
    public void stopGameCoroutines()
    {
        ///TODO INSTANT HERE
        Destroy(gameWindow.UUIpanel.gameObject);
        Destroy(gameWindow.BUIPanel.gameObject);
        ///TODO END HERE
        Debug.Log("tyoe"+ _miniGame._minigameType);
        _miniGame.EndMinigame();
        StopAllCoroutines();
    }
 
    
}
