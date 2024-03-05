using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Serialization;


public abstract class MiniGame : MonoBehaviour
{
    public MgSettings Settings;

    public bool isPaused = false;
    public MgPanel mgPanel;
    public PuzzleTimer _puzzleTimer;
    protected UIPanel BottomUI;
    protected UIPanel UpperUI;

    //initialization
    public virtual void FixLayoutOrder(int order)
    {
        Debug.Log("FixLayoutOrder Not Handled");
    }
    public void Initialize(MgPanel panel,MgSettings startSetting)
    {
        mgPanel = panel;
        SetupPanels();

   
        Settings = startSetting;
        _puzzleTimer = this.AddComponent<PuzzleTimer>();
        _puzzleTimer.Initialize(Settings);

        InitializeDerivative();
        mgPanel.gameWindow.InitPanels();

    }

    public virtual void ResizeMinigame()
    {
        Debug.Log("Resize Not Handled");
    }

    protected abstract void InitializeDerivative();

    private void ReadyUpGamePanel()
    {
        var mgPanelRectTransform = mgPanel.gameObject.GetComponent<RectTransform>();
        // mgPanel.panelBounds = GameCanvas.CalculateWsWithPadding(mgPanel.panelBounds, 0);

    }

    public IEnumerator StartMinigame()
    {
        Settings.ResetTemp();
        Debug.Log("Minigame Started");
        isPaused = false;
        ReadyUpGamePanel();
        yield return waitingPage();
        mgPanel.gameWindow.UUIpanel?.gameObject.SetActive(true);
        mgPanel.gameWindow.BUIPanel?.gameObject.SetActive(true);
        _puzzleTimer.reset_timer(Settings);
        UpdateHighscoreBoard();
        UpperUI?.ResetPanel();
        StartMinigameChild();
    }
    private IEnumerator waitingPage()
    {
        mgPanel.gameWindow.Methods.loadingCurtain.gameObject.SetActive(true);
        mgPanel.gameWindow.Methods.curtain.color = Color.red;
        yield return new WaitForSeconds(1f);
        mgPanel.gameWindow.Methods.loadingCurtain.gameObject.SetActive(false);


    }
    public abstract void StartMinigameChild();
    public virtual void PauseMinigame()
    {
        isPaused = true;

        _puzzleTimer.PauseTimer();
    }

    public virtual void ResumeMinigame()
    {
        isPaused = false;
        _puzzleTimer.ResumeTimer();

    }

    public void UpdateHighscoreBoard()
    {
        var highscore = Settings.GetRecords();
        mgPanel.gameWindow.highscoreBoard.UpdateHighscore(highscore);
    }

    public virtual void EndMinigame()
    {
        ///Todo add bottom UI RESET, AND PUT IT HERE
        if (BottomUI) {
            BottomUI.ResetPanel();
        }
        StopAllCoroutines();
        _puzzleTimer.reset_timer(Settings);
    }
    
    public virtual void RetryMinigame()
    {
        UpdateHighscoreBoard();
        StopAllCoroutines();
        Settings.ResetTemp();
        UpperUI?.ResetPanel();
        BottomUI?.ResetPanel();
        isPaused = false;
        _puzzleTimer.reset_timer(Settings);

    }

    public void PuzzleSolved()
    {
        PuzzleSolvedChild();
    }
    public virtual void PuzzleSolvedChild()
    {
        throw new NotImplementedException();
    }

    public virtual void EndRound()
    {
        
        Settings.UpdateRecords();
        _puzzleTimer.reset_timer(Settings);
        UpdateHighscoreBoard();
    }



    public virtual GameObject InstantiateUpperSettings()
    {
        Debug.Log("This minigame has no upper setting prefab set!");
        return null;
    }


    protected void SetupPanels()
    {
        BottomUI = InitBottomUI();
        BottomUI?.gameObject.SetActive(false);
        UpperUI  = InitUpperUI();
        UpperUI?.gameObject.SetActive(false);
    }
    private UIPanel InitBottomUI()
    {
        return InitBottomUIChild();
    }

    private UIPanel InitUpperUI()
    {
        return InitUpperUIChild();
    }

    protected abstract UIPanel InitBottomUIChild();
    protected abstract UIPanel InitUpperUIChild();
    public virtual MgSettings AddSettings()
    {
        throw new NotImplementedException();
    }
}

