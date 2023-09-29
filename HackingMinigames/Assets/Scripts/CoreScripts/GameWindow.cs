using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Helpers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{
    private GameObject WindowParent => gameObject.GetComponent<UiMethods>().parent;
    public WindowMethods Methods => WindowParent.GetComponent<WindowMethods>();
    public int currentSortingLayer;
    [NonSerialized]public MinigameType currentMg;
    [NonSerialized] private GameCanvas gameCanvas;
    [SerializeField] public GameObject upperContainer;
    [SerializeField] private GameObject middleContainer;
    [SerializeField] public GameObject bottomContainer;
    [SerializeField] public GameObject highscoreBoardPanel;

    
    [NonSerialized]private LayoutElement _upperContainerLayout;
    [NonSerialized]private LayoutElement _gamePanelLayout;
    [NonSerialized]private LayoutElement _bottomContainerLayout;
    
    [NonSerialized]public UIPanel USPanel;
    [NonSerialized]public UIPanel UUIpanel;
    [NonSerialized]public UIPanel BUIPanel;
    [SerializeField]public MgPanel MinigamePanel;
    [NonSerialized]public HighscoreBoardPanel highscoreBoard;
    
    [FormerlySerializedAs("navigationPanelContainer")] [SerializeField] private GameObject navigationPanelGOBJ;
    private (int height, int width) _dimensions;


    private void Start()
    {
        gameCanvas = GetComponentInParent<GameCanvas>();
    }

    public void Initialize(MinigameType mgType,int currentLayer)
    {
        currentSortingLayer = currentLayer;
        currentMg = mgType;
        Debug.Log("Adding sorting");

        highscoreBoard = highscoreBoardPanel.GetComponent<HighscoreBoardPanel>();            
        _upperContainerLayout = upperContainer.GetComponent<LayoutElement>();
        _gamePanelLayout = middleContainer.GetComponent<LayoutElement>();
        _bottomContainerLayout = bottomContainer.GetComponent<LayoutElement>();

        // gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
        _upperContainerLayout.flexibleHeight = 100;
        _bottomContainerLayout.flexibleHeight = 2;
        GamePrep();

    }

    private void GamePrep()
    {
        MinigamePanel.AddMinigameScript();
        Debug.Log("added script");
        ObjectHandler.SetSize(gameObject, MinigamePanel._miniGame.Settings.SettingDimensions);
        ShowSettings();
    }

    public void ShowSettings()
    {
        Debug.Log("showing settings");
        middleContainer.SetActive(false);
        highscoreBoard.HidePanel();
        
        UUIpanel?.HidePanel();
        BUIPanel?.HidePanel();

        if (!USPanel)
        {
            var currentSettings = MinigamePanel._miniGame.InstantiateUpperSettings();
            USPanel = currentSettings.GetComponent<UIPanel>();
        }
        USPanel.ShowPanel();
        
        navigationPanelGOBJ.SetActive(true);
    }
    public void ShowGame()
    {
        if (USPanel) {
            USPanel.HidePanel();
        }

        if (highscoreBoard)
        {
            highscoreBoard.ShowPanel();
        }
        
        navigationPanelGOBJ.SetActive(false);
        middleContainer.SetActive(true);
    }
    public void InitPanels()
    {

        UUIpanel?.Initialize(this);
        BUIPanel?.Initialize(this);
        var uuiLayoutElement = UUIpanel?.GetComponent<LayoutElement>();
        var buiLayoutElement = BUIPanel?.GetComponent<LayoutElement>();
        
        
        _upperContainerLayout.preferredHeight = (uuiLayoutElement)? uuiLayoutElement.preferredHeight : 1;
        _gamePanelLayout.flexibleHeight = 10000;
        _bottomContainerLayout.preferredHeight = (buiLayoutElement)? buiLayoutElement.preferredHeight : 1;
    }
    

    // Update is called once per frame
    public void GameStartButton()
    {
        StartCoroutine(StartMinigame());
    }
    public IEnumerator StartMinigame()
    {
        ShowGame();
        yield return ObjectHandler.ChangeSizeCoroutine(gameObject, (MinigamePanel._miniGame.Settings.GameDimensions), 0.3f);
        Debug.Log("aboutta start minigame");
        MinigamePanel.StartMinigame();
        yield return null;
    }
    //Buttons


    public void SettingsButton()
    {
        MinigamePanel?._miniGame.EndMinigame();
        ShowSettings();
        StartCoroutine(SettingsCoroutine());
    }

    public void SetMinimumSize(Vector2 minSize)
    {
        var resizer=  WindowParent.GetComponentInChildren<ResizePanel>();
        resizer.minSize = minSize;
    }

    public IEnumerator SettingsCoroutine()
    {
        yield return ObjectHandler.ChangeSizeCoroutine(gameObject, MinigamePanel._miniGame.Settings.SettingDimensions, 0.3f);

    }
}