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
    [FormerlySerializedAs("_gameWindow")] public MgPanel mgPanel;
    public PuzzleTimer _puzzleTimer;
    protected int currentStreak = 0;
    protected MinigameType _minigameType;
    //Prefabs
    public GameObject _upperUIPrefab;
    //Panels

    //initialization

    public void Initialize(WindowSize hackWindowDimensions, MgPanel window)
    {
        mgPanel = window;
        _hackWindowDimensions = hackWindowDimensions;


        InitializeDerivative(hackWindowDimensions);
    }
    protected abstract void InitializeDerivative(WindowSize hackWindowDimensions);
    
    public abstract void StartMinigame();
    public abstract void EndMinigame();
    public abstract void RetryMinigame();
}

