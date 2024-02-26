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
    private UiMethods UIMethods => gameObject.GetComponent<UiMethods>();
    private GameObject WindowParent => UIMethods.parent;
    public WindowMethods Methods => WindowParent.GetComponent<WindowMethods>();
    private int currentSortingLayer;

    public int CurrentSortingLayer
    {
        get { return currentSortingLayer; }
        set { currentSortingLayer = value; Debug.Log("gmbj name: "+gameObject.name); gameObject.GetComponentInParent<Canvas>().sortingOrder = value;}
    }
    [NonSerialized]public MinigameType currentMg;
    [NonSerialized] public GameCanvas gameCanvas;
    [SerializeField] public GameObject upperContainer;
    [SerializeField] public GameObject middleContainer;
    [SerializeField] public GameObject bottomContainer;
    [SerializeField] public GameObject highscoreBoardPanel;

    
    [NonSerialized]private LayoutElement _upperContainerLayout;
    [NonSerialized]private LayoutElement _gamePanelLayout;
    [NonSerialized]private LayoutElement _bottomContainerLayout;
    
    [NonSerialized]public UIPanel USPanel;
    [NonSerialized]public UIPanel UUIpanel;
    [NonSerialized]public UIPanel BUIPanel;
    [NonSerialized]private MinigamePanelFactory _minigamePanelFactory;
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
        _minigamePanelFactory = new MinigamePanelFactory(middleContainer, this);
        currentSortingLayer = currentLayer;
        currentMg = mgType;
        highscoreBoard = highscoreBoardPanel.GetComponent<HighscoreBoardPanel>();            
        _upperContainerLayout = upperContainer.GetComponent<LayoutElement>();
        _gamePanelLayout = middleContainer.GetComponent<LayoutElement>();
        _bottomContainerLayout = bottomContainer.GetComponent<LayoutElement>();

        // gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
        _upperContainerLayout.flexibleHeight = 200;
        _bottomContainerLayout.flexibleHeight = 2;
        GamePrep();

    }

    private void GamePrep()
    {
        Debug.Log(currentMg);
        MinigamePanel = _minigamePanelFactory.createMgPanel(currentMg);
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
        //TODO LAYOUT
        
        _upperContainerLayout.preferredHeight = (uuiLayoutElement)? uuiLayoutElement.preferredHeight+25 : 1;
        _gamePanelLayout.flexibleHeight = 10000;
        _bottomContainerLayout.preferredHeight = (buiLayoutElement)? buiLayoutElement.preferredHeight+25 : 1;
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

    public void SetMinimumSize(float minHeight, float minWidth)
    {
        
        LayoutElement lay = WindowParent.GetComponent<LayoutElement>();
        RectTransform rectt = UIMethods.parent.GetComponent<RectTransform>();
        lay.minWidth = minWidth;
        lay.minHeight = minHeight;
        if(rectt.rect.height < minHeight || rectt.rect.width < minWidth)
        {
            rectt.sizeDelta = new Vector2(minWidth, minHeight);
        }
        UIMethods.ParentRefresh();
        LayoutRebuilder.ForceRebuildLayoutImmediate(WindowParent.GetComponent<RectTransform>());
    }

    public IEnumerator SettingsCoroutine()
    {
        yield return ObjectHandler.ChangeSizeCoroutine(gameObject, MinigamePanel._miniGame.Settings.SettingDimensions, 0.3f);

    }
}
