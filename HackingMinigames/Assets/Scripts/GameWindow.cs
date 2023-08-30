using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{
    [SerializeField]private GameCanvas _gameCanvas;
    
    [SerializeField] public GameObject upperContainer;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] public GameObject bottomContainer;

    
    [NonSerialized]private LayoutElement _upperContainerLayout;
    [NonSerialized]private LayoutElement _gamePanelLayout;
    [NonSerialized]private LayoutElement _bottomContainerLayout;
    
    [NonSerialized]public UIPanel UUIpanel;
    [NonSerialized]public UIPanel BUIPanel;
    [NonSerialized]public MgPanel MinigamePanel;
    
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject navigationPanel;
    [SerializeField] private GameObject hackPanelGobj;


    private RectTransform _mgPanelRectTransform;
    [NonSerialized] public WindowSize gameWindowSize;


    private void Start()
    {
        MinigamePanel = _gamePanel.GetComponent<MgPanel>();
        _mgPanelRectTransform = MinigamePanel.GetComponent<RectTransform>();
        gameWindowSize = _gameCanvas.GetGameWindowSize();
        Game.Instance.CurrentGameWindow = this;

        _upperContainerLayout = upperContainer.GetComponent<LayoutElement>();
        _gamePanelLayout = _gamePanel.GetComponent<LayoutElement>();
        _bottomContainerLayout = bottomContainer.GetComponent<LayoutElement>();

        _upperContainerLayout.flexibleHeight = 100;
        _bottomContainerLayout.flexibleHeight = 1;
        ShowSettings();

    }

    public void ShowSettings()
    {
        Debug.Log("showing settings");
        hackPanelGobj.SetActive(false);
        if (Game.Instance.currentSettingsPrefab)
        {
            settingsPanel = Instantiate(Game.Instance.currentSettingsPrefab, upperContainer.transform);
            settingsPanel.SetActive(true);
        }
        navigationPanel.SetActive(true);
    }
    public void ShowGame()
    {
        if (settingsPanel)
        {
            settingsPanel.SetActive(false);
        }
        navigationPanel.SetActive(false);
        hackPanelGobj.SetActive(true);
    }
    public IEnumerator InitPanels(int topWeight,int mgPanelWeight, int bottomWeight)
    {

        UUIpanel.Initialize(upperContainer);
        BUIPanel.Initialize(bottomContainer);
        _mgPanelRectTransform = MinigamePanel.GetComponent<RectTransform>();
        
        
        _upperContainerLayout.flexibleHeight = topWeight;
        _gamePanelLayout.flexibleHeight = mgPanelWeight;
        _bottomContainerLayout.flexibleHeight = bottomWeight;
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
                UUIpanel = Instantiate(Game.Instance.upperHackPrefab, upperContainer.transform)
                    .GetComponent<UUIhack>();
                BUIPanel = Instantiate(Game.Instance.bottomHackPrefab, bottomContainer.transform)
                    .GetComponent<BUIhack>();
                yield return InitPanels(1, 10, 1);
                panelBounds = _gameCanvas.CalculateWsWithPadding(_mgPanelRectTransform.rect, 0);
                break;
            case MinigameType.UNTANGLE:
                Debug.Log("Untangle Started");
                minigameType = MinigameType.UNTANGLE;
                miniGame = MinigamePanel.gameObject.AddComponent<UntangleMG>();
                UUIpanel = Instantiate(Game.Instance.upperUntanglePrefab, upperContainer.transform, false)
                    .GetComponent<UUIuntangle>();
                BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, bottomContainer.transform, false)
                    .GetComponent<BUIuntangle>();
                yield return InitPanels(1, 10, 1);
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
