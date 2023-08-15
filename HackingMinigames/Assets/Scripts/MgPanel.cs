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
    [NonSerialized]public WindowSize _windowSize;
    [NonSerialized]public MiniGame _miniGame;
    [NonSerialized]private MinigameType _minigameType;
    [NonSerialized]public TMP_Text streakText;
    [NonSerialized]public Image loadingbarTimer;
    [NonSerialized]public UIPanel UUIpanel;
    [NonSerialized]public UIPanel BUIPanel;


    // Start is called before the first frame update
    void Start()
    {
        _windowSize = gameCanvas.HackWindowSize;
    }

    public void Initialize(GameCanvas canvas, WindowSize windowSize)
    {
        gameCanvas = canvas;
        _windowSize = windowSize;
        //questionInputField.Select();
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
                UUIpanel.Initialize();
                BUIPanel.Initialize();
                break;
            case MinigameType.UNTANGLE:
                Debug.Log("Untangle Started");
                minigameType = MinigameType.UNTANGLE;
                _miniGame = this.AddComponent<UntangleMG>();
                UUIpanel = Instantiate(Game.Instance.upperUntanglePrefab, gameCanvas.upperGUI.transform).GetComponent<UUIuntangle>();
                BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, gameCanvas.bottomGUI.transform).GetComponent<BUIuntangle>();
                UUIpanel.Initialize();
                BUIPanel.Initialize();
                break;
            default:
                Debug.Log("NOT IMPLEMENTED");
                break;
        }
        _windowSize = gameCanvas.SetupWindow2(0f);
        _miniGame.Initialize(_windowSize, this);
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
