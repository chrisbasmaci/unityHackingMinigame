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
    [SerializeField]GameCanvas gameCanvas;
    [NonSerialized] private Rect _panelRect;
    [NonSerialized]public WindowSize panelBounds;
    [NonSerialized]public MiniGame _miniGame;
    [NonSerialized]private MinigameType _minigameType;
    [NonSerialized]public TMP_Text streakText;
    [NonSerialized]public Image loadingbarTimer;
    [NonSerialized]public UIPanel UUIpanel;
    [NonSerialized]public UIPanel BUIPanel;



    public void Initialize(GameCanvas canvas)
    {
        gameCanvas = canvas;
        _panelRect = gameObject.GetComponent<RectTransform>().rect;
    }
    public void StartMinigame(MinigameType minigameType)
    {
        switch (minigameType)
        { 
            case MinigameType.HACK:
                minigameType = MinigameType.HACK;
                _miniGame = this.AddComponent<HackingMG>();
                UUIpanel = Instantiate(Game.Instance.upperHackPrefab, gameCanvas.upperGUI.transform).GetComponent<UUIuntangle>();
                BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, gameCanvas.bottomGUI.transform).GetComponent<BUIhack>();
                panelBounds = gameCanvas.CalculateWsWithPadding(_panelRect, 0);
                UUIpanel.Initialize();
                BUIPanel.Initialize();
                break;
            case MinigameType.UNTANGLE:
                Debug.Log("Untangle Started");
                minigameType = MinigameType.UNTANGLE;
                _miniGame = this.AddComponent<UntangleMG>();
                UUIpanel = Instantiate(Game.Instance.upperUntanglePrefab, gameCanvas.upperGUI.transform).GetComponent<UUIuntangle>();
                BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, gameCanvas.bottomGUI.transform).GetComponent<BUIuntangle>();
                panelBounds = gameCanvas.CalculateWsWithPadding(_panelRect, 0.05f);
                UUIpanel.Initialize();
                BUIPanel.Initialize();
                break;
            default:
                panelBounds = gameCanvas.CalculateWsWithPadding(_panelRect, 0);
                Debug.Log("NOT IMPLEMENTED");
                break;
        }
        _miniGame.Initialize(panelBounds, this);
        // gameCanvas.InitPanels();
        _miniGame.StartMinigame();
        //questionInputField.Select();

    }

    public void Retry()
    {
        _miniGame.RetryMinigame();
    }
    public void stopGameCoroutines()
    {
        _miniGame.EndMinigame();
        StopAllCoroutines();
    }

}
