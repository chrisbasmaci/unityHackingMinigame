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

    


    private void Start()
    {
        MinigamePanel = _gamePanel.GetComponent<MgPanel>();
        
        Game.Instance.CurrentGameWindow = this;

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
        
        UUIpanel.gameObject.SetActive(false);
        BUIPanel.gameObject.SetActive(false);
        if (Game.Instance.currentSettingsPrefab)
        {
            if (!settingsPanel)
            {
                settingsPanel = Instantiate(Game.Instance.currentSettingsPrefab, upperContainer.transform);
            }
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
    public IEnumerator InitPanels()
    {

        UUIpanel.Initialize(upperContainer);
        BUIPanel.Initialize(bottomContainer);
        var uuiLayoutElement = UUIpanel.GetComponent<LayoutElement>();
        var buiLayoutElement = BUIPanel.GetComponent<LayoutElement>();
        
        
        _upperContainerLayout.preferredHeight = uuiLayoutElement.preferredHeight;
        _gamePanelLayout.flexibleHeight = 10000;
        _bottomContainerLayout.preferredHeight = buiLayoutElement.preferredHeight;
        yield return null;
    }
    

    // Update is called once per frame
    public IEnumerator StartMinigame()
    {
        MinigamePanel.StartMinigame();
        yield return null;

        // miniGame.StartMinigame();
    }
}
