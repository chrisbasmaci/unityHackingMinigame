using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWindow : MonoBehaviour
{

    [SerializeField]private GameCanvas _gameCanvas;
    [SerializeField]private GameObject _topPanel;
    [SerializeField]private GameObject _gamePanel;
    [SerializeField]private GameObject _bottomPanel;
    
    [NonSerialized]public UIPanel UUIpanel;
    [NonSerialized]public UIPanel BUIPanel;
    [NonSerialized]public MgPanel MinigamePanel;

    private RectTransform _mgPanelRectTransform;
    [NonSerialized] public WindowSize gameWindowSize;


    private void Start()
    {
        MinigamePanel = _gamePanel.GetComponent<MgPanel>();
        _mgPanelRectTransform = MinigamePanel.GetComponent<RectTransform>();
        gameWindowSize = _gameCanvas.GetGameWindowSize();

    }

    public IEnumerator InitPanels(float topHeight,float mgPanelHeight, float bottomHeight)
    {
        _mgPanelRectTransform = MinigamePanel.GetComponent<RectTransform>();
        // UUIpanel = Instantiate(Game.Instance.upperHackPrefab, _gameCanvas.upperGUI.transform).GetComponent<UUIhack>();
        // BUIPanel = Instantiate(Game.Instance.bottomHackPrefab, _gameCanvas.bottomGUI.transform).GetComponent<BUIhack>();
        _mgPanelRectTransform.sizeDelta = new Vector2(_gameCanvas.gameWindowSize.Width, mgPanelHeight);

        UUIpanel.Initialize(_topPanel,topHeight);
        BUIPanel.Initialize(_bottomPanel, bottomHeight);
        _mgPanelRectTransform = MinigamePanel.GetComponent<RectTransform>();

        yield return null;
    }
    

    // Update is called once per frame
    public IEnumerator StartMinigame(MinigameType minigameType)
    {
        MinigamePanel = _gamePanel.GetComponent<MgPanel>();
        _mgPanelRectTransform = MinigamePanel.gameObject.GetComponent<RectTransform>();
        MiniGame miniGame;
        WindowSize panelBounds;
        switch (minigameType)
        {
            case MinigameType.HACK:
                minigameType = MinigameType.HACK;
                miniGame = MinigamePanel.gameObject.AddComponent<HackingMG>();
                UUIpanel = Instantiate(Game.Instance.upperHackPrefab, _gameCanvas.upperGUI.transform)
                    .GetComponent<UUIhack>();
                BUIPanel = Instantiate(Game.Instance.bottomHackPrefab, _gameCanvas.bottomGUI.transform)
                    .GetComponent<BUIhack>();
                yield return InitPanels(100f, 340f, 100f);
                panelBounds = _gameCanvas.CalculateWsWithPadding(_mgPanelRectTransform.rect, 0);
                break;
            case MinigameType.UNTANGLE:
                Debug.Log("Untangle Started");
                minigameType = MinigameType.UNTANGLE;
                miniGame = MinigamePanel.gameObject.AddComponent<UntangleMG>();
                UUIpanel = Instantiate(Game.Instance.upperUntanglePrefab, _gameCanvas.upperGUI.transform, false)
                    .GetComponent<UUIuntangle>();
                BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, _gameCanvas.bottomGUI.transform, false)
                    .GetComponent<BUIuntangle>();
                yield return InitPanels(100f, 300f, 100f);
                panelBounds = _gameCanvas.CalculateWsWithPadding(_mgPanelRectTransform.rect, 0f);
                break;
            default:
                miniGame = MinigamePanel.gameObject.AddComponent<UntangleMG>();
                panelBounds = _gameCanvas.CalculateWsWithPadding(_mgPanelRectTransform.rect, 0);
                Debug.Log("NOT IMPLEMENTED");
                break;
        }
        MinigamePanel.Initialize(panelBounds);
        miniGame.Initialize(panelBounds, MinigamePanel);
        miniGame.StartMinigame();
    }
}
