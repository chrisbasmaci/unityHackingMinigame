using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

public enum MinigameType {HACK =0, UNTANGLE}
public abstract class MiniGame : MonoBehaviour
{

    protected WindowSize _hackWindowDimensions;
    public GameWindow _gameWindow;
    public PuzzleTimer _puzzleTimer;
    protected int currentStreak = 0;
    protected MinigameType _minigameType;

    //initialization
    public abstract void Initialize(WindowSize hackWindowDimensions, GameWindow window);
    public abstract void StartMinigame();
    public abstract void EndMinigame();
}

