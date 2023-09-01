
using System.Collections.Generic;

public abstract class MgSettings
{
    //defaults
    protected int _defaultIntroTimer;
    protected int _defaultPuzzleTimer;
    
    // Getters for default timer values
    public int DefaultIntroTimer => _defaultIntroTimer;
    public int DefaultPuzzleTimer => _defaultPuzzleTimer;

    //currents
    public int CurrentIntroTimer;
    public int CurrentPuzzleTimer;
    
    //records
    public float BestTime;

    public MgSettings(int introTimer, int puzzleTimer)
    {
        // Initialize current timers with default values
        CurrentIntroTimer = introTimer;
        CurrentPuzzleTimer = puzzleTimer;
        BestTime = 0;
    }

}

public class UntangleSettings : MgSettings
{
    //defaults
    //currents
    //records
    public Dictionary<int, int> BestMoves;

    public UntangleSettings(int introTimer, int puzzleTimer)
        : base(introTimer, puzzleTimer)
    {
        //defaults for this puzzle
        _defaultIntroTimer = 0;
        _defaultPuzzleTimer = 60;
    }

    public void UpdateRecords(float currentTime, int verticeTotal, int currentMoves)
    {
        if (BestMoves.ContainsKey(verticeTotal)) {
            BestMoves[verticeTotal] = 
                (BestMoves[verticeTotal] > currentMoves) ? currentMoves : BestMoves[verticeTotal];
        }else{
            BestMoves.Add(verticeTotal, currentMoves);
        }
    }
}
public class HackSettings : MgSettings
{
    public HackSettings(int introTimer, int puzzleTimer)
        : base(introTimer, puzzleTimer)
    {
        _defaultIntroTimer = 3;
        _defaultPuzzleTimer = 10;
    }
    
    public void UpdateRecords()
    {
    }
}
