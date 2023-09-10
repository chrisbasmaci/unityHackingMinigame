using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public enum MinigameType {HACK =0, UNTANGLE}

public abstract class MiniGame : MonoBehaviour
{
    public bool isPaused = false;
    public MgPanel mgPanel;
    public MgSettings Settings;
    public PuzzleTimer _puzzleTimer;

    //initialization

    public IEnumerator Initialize(MgPanel panel)
    {
        mgPanel = panel;
        SetupPanels();
        _puzzleTimer = this.AddComponent<PuzzleTimer>();
        if (mgPanel.gameWindow.highscoreBoardPanel)
        {
            /// TODO URGENT THIS
            // mgPanel.gameWindow.highscoreBoard.ResetUI();
        }

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
        Debug.Log("Minigame Started");
        isPaused = false;
        ReadyUpGamePanel();
        yield return null;
        mgPanel.gameWindow.UUIpanel.gameObject.SetActive(true);
        mgPanel.gameWindow.BUIPanel.gameObject.SetActive(true);
        
        UpdateHighscoreBoard();
        ChildStartMinigame();
    }

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
    
    public abstract void ChildStartMinigame();
    public abstract void EndMinigame();
    public abstract void RetryMinigame();

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

    protected void SetupPanels()
    {
        Debug.Log("inside setup");

        InitUpperUI();
        InitBottomUI();
    }

    public virtual GameObject getUpperSettingPrefab()
    {
        Debug.Log("This minigame has no upper setting prefab set!");
        return null;
    }

    protected abstract void InitBottomUI();
    protected abstract void InitUpperUI();
}

