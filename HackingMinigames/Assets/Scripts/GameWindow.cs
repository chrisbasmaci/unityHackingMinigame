using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{
    [NonSerialized]public MinigameType currentMg;
    [NonSerialized] private GameCanvas gameCanvas;
    [SerializeField] public GameObject upperContainer;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] public GameObject bottomContainer;
    [SerializeField] public GameObject highscoreBoardPanel;

    
    [NonSerialized]private LayoutElement _upperContainerLayout;
    [NonSerialized]private LayoutElement _gamePanelLayout;
    [NonSerialized]private LayoutElement _bottomContainerLayout;
    
    [NonSerialized]public UIPanel UUIpanel;
    [NonSerialized]public UIPanel BUIPanel;
    [NonSerialized]public MgPanel MinigamePanel;
    [NonSerialized]public HighscoreBoardPanel highscoreBoard;
    
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject navigationPanel;
    [SerializeField] private GameObject hackPanelGobj;

    
    

    public void Initialize(MinigameType mgType)
    {
        gameCanvas = GetComponentInParent<GameCanvas>();
        highscoreBoard = highscoreBoardPanel.GetComponent<HighscoreBoardPanel>();
        MinigamePanel = _gamePanel.GetComponent<MgPanel>();
        currentMg = mgType;

        _upperContainerLayout = upperContainer.GetComponent<LayoutElement>();
        _gamePanelLayout = _gamePanel.GetComponent<LayoutElement>();
        _bottomContainerLayout = bottomContainer.GetComponent<LayoutElement>();

        _upperContainerLayout.flexibleHeight = 100;
        _bottomContainerLayout.flexibleHeight = 1;
        
        StartCoroutine(MinigamePanel.AddMinigameScript());
        ShowSettings();
    }

    public void ShowSettings()
    {
        Debug.Log("showing settings");
        hackPanelGobj.SetActive(false);
        highscoreBoardPanel.SetActive(false);


        UUIpanel.gameObject.SetActive(false);
        BUIPanel.gameObject.SetActive(false);
        if (MinigamePanel.currentSettingsPrefab)
        {
            if (!settingsPanel)
            {
                settingsPanel = Instantiate(MinigamePanel.currentSettingsPrefab, upperContainer.transform);
                settingsPanel.GetComponent<UIPanel>().Initialize(this);
            }
            settingsPanel.SetActive(true);
        }
        navigationPanel.SetActive(true);
    }
    public void ShowGame()
    {
        if (settingsPanel) {
            settingsPanel.SetActive(false);
        }
        highscoreBoardPanel.SetActive(true);
        
        navigationPanel.SetActive(false);
        hackPanelGobj.SetActive(true);
    }
    public IEnumerator InitPanels()
    {

        UUIpanel.Initialize(this);
        BUIPanel.Initialize(this);
        var uuiLayoutElement = UUIpanel.GetComponent<LayoutElement>();
        var buiLayoutElement = BUIPanel.GetComponent<LayoutElement>();
        
        
        _upperContainerLayout.preferredHeight = uuiLayoutElement.preferredHeight;
        _gamePanelLayout.flexibleHeight = 10000;
        _bottomContainerLayout.preferredHeight = buiLayoutElement.preferredHeight;
        yield return null;
    }
    

    // Update is called once per frame
    public void GameStartButton()
    {
        StartCoroutine(StartMinigame());
    }
    public IEnumerator StartMinigame()
    {
        ShowGame();
        yield return gameCanvas.ChangePaddingWithAnimation();
        Debug.Log("aboutta start minigame");
        MinigamePanel.StartMinigame();
        yield return null;
    }
    //Buttons

    public void MainMenuButton(){
        Debug.Log("Normal Hack Button Pressed");
        Game.Instance.gameCanvas.SetActive(false);
        Game.Instance.selectionCanvas.SetActive(true);
        Destroy(gameObject);

    }

    public void SettingsButton()
    {
        MinigamePanel?._miniGame.EndMinigame();
        ShowSettings();
        StartCoroutine(SettingsCoroutine());
    }

    public IEnumerator SettingsCoroutine()
    {
        yield return gameCanvas.ChangePaddingWithAnimation();

    }
}
