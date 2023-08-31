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

    public MgPanel mgPanel;
    public PuzzleTimer _puzzleTimer;
    protected int currentStreak = 0;


    //initialization

    public IEnumerator Initialize(MgPanel panel)
    {
        mgPanel = panel;
        SetupPanels();
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
        ReadyUpGamePanel();
        yield return null;
        mgPanel.gameWindow.UUIpanel.gameObject.SetActive(true);
        mgPanel.gameWindow.BUIPanel.gameObject.SetActive(true);
        ChildStartMinigame();
    }
    public abstract void ChildStartMinigame();
    public abstract void EndMinigame();
    public abstract void RetryMinigame();

    protected void SetupPanels()
    {
        Debug.Log("inside setup");

        InitUpperUI();
        InitBottomUI();
    }
    protected abstract void InitBottomUI();
    protected abstract void InitUpperUI();
}

