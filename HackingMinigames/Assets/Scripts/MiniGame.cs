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

    protected WindowSize _hackWindowDimensions;
    public MgPanel mgPanel;
    public PuzzleTimer _puzzleTimer;
    protected int currentStreak = 0;
    public MinigameType _minigameType;
    //Prefabs
    public GameObject _upperUIPrefab;
    //Panels

    //initialization

    public void Initialize(WindowSize hackWindowDimensions, MgPanel panel)
    {
        mgPanel = panel;
        _hackWindowDimensions = hackWindowDimensions;


        InitializeDerivative(hackWindowDimensions);
        InitBottomUI();
    }
    protected abstract void InitializeDerivative(WindowSize hackWindowDimensions);
    
    public abstract void StartMinigame();
    public abstract void EndMinigame();
    public abstract void RetryMinigame();
    protected abstract void SetupPanels();
    protected abstract void InitBottomUI();
}

