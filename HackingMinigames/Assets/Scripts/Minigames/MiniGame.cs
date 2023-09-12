using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Serialization;

public enum MinigameType {HACK =0, UNTANGLE}

public abstract class MiniGame : MonoBehaviour
{
    //this is set inside the game, cause im not sure how to  pass a variable as reference,
    //where da pointer at when I want it :(
    public MgSettings Settings;

    public bool isPaused = false;
    public MgPanel mgPanel;
    public PuzzleTimer _puzzleTimer;
    protected UIPanel BottomUI;
    protected UIPanel UpperUI;

    //initialization

    public IEnumerator Initialize(MgPanel panel)
    {
        mgPanel = panel;
        SetupPanels();

   
        Settings = AddSettings();
        _puzzleTimer = this.AddComponent<PuzzleTimer>();
        _puzzleTimer.Initialize(Settings);

        InitializeDerivative();
        yield return mgPanel.gameWindow.InitPanels();

    }

    protected abstract void InitializeDerivative();

    private void ReadyUpGamePanel()
    {
        var mgPanelRectTransform = mgPanel.gameObject.GetComponent<RectTransform>();
        mgPanel.panelBounds = GameCanvas.CalculateWsWithPadding(mgPanelRectTransform.rect, 0);

    }

    public IEnumerator StartMinigame()
    {
        Settings.ResetTemp();
        Debug.Log("Minigame Started");
        isPaused = false;
        ReadyUpGamePanel();
        yield return null;
        mgPanel.gameWindow.UUIpanel.gameObject.SetActive(true);
        mgPanel.gameWindow.BUIPanel.gameObject.SetActive(true);
        
        UpdateHighscoreBoard();
        UpperUI.ResetPanel();
        StartMinigameChild();
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
        BottomUI.ResetPanel();
        StopAllCoroutines();
        _puzzleTimer.reset_timer();
    }
    
    public virtual void RetryMinigame()
    {
        UpdateHighscoreBoard();
        StopAllCoroutines();
        Settings.ResetTemp();
        UpperUI.ResetPanel();
        BottomUI.ResetPanel();
        isPaused = false;
        _puzzleTimer.reset_timer();

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
        UpdateHighscoreBoard();
    }



    public virtual GameObject getUpperSettingPrefab()
    {
        Debug.Log("This minigame has no upper setting prefab set!");
        return null;
    }


    protected void SetupPanels()
    {
        BottomUI = InitBottomUI();
        UpperUI  = InitUpperUI();
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

