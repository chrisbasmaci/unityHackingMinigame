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
    public bool isPaused =false;
    public MgPanel mgPanel;
    public MgSettings Settings;
    public PuzzleTimer _puzzleTimer;
    protected int currentStreak = 0;

    //initialization

    public IEnumerator Initialize(MgPanel panel)
    {
        mgPanel = panel;
        SetupPanels();
        _puzzleTimer = this.AddComponent<PuzzleTimer>();
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
        isPaused = false;
        ReadyUpGamePanel();
        yield return null;
        mgPanel.gameWindow.UUIpanel.gameObject.SetActive(true);
        mgPanel.gameWindow.BUIPanel.gameObject.SetActive(true);
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
    public abstract void ChildStartMinigame();
    public abstract void EndMinigame();
    public abstract void RetryMinigame();

    protected void SetupPanels()
    {
        Debug.Log("inside setup");
        mgPanel.gameWindow.highscoreBoardPanel = 
            Instantiate(Game.Instance.highscoreBoardPrefab, mgPanel.gameWindow.upperContainer.transform).
            GetComponent<HighscoreBoardPanel>();
            
        InitUpperUI();
        InitBottomUI();
    }
    protected abstract void InitBottomUI();
    protected abstract void InitUpperUI();
}

