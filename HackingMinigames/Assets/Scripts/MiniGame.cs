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
    public GameWindow _gameWindow;
    public PuzzleTimer _puzzleTimer;
    protected int currentStreak = 0;
    protected MinigameType _minigameType;
    //Prefabs
    public GameObject _upperUIPrefab;
    //Panels

    //initialization

    public void Initialize(WindowSize hackWindowDimensions, GameWindow window)
    {
        _gameWindow = window;
        _hackWindowDimensions = hackWindowDimensions;


        InitializeDerivative(hackWindowDimensions);
    }
    protected abstract void InitializeDerivative(WindowSize hackWindowDimensions);
    
    public abstract void StartMinigame();
    public abstract void EndMinigame();
    public abstract void RetryMinigame();
}

