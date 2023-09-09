
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

    public MgSettings()
    {
        // Initialize current timers with default values
        _defaultIntroTimer = 3;
        _defaultPuzzleTimer = 10;

        CurrentPuzzleTimer = _defaultPuzzleTimer;
        CurrentIntroTimer = _defaultIntroTimer;
            BestTime = 0;
    }

}

public class UntangleSettings : MgSettings
{
    //defaults
    private int _defaultVertexTotal;
    public int CurrentVertexTotal;
    //currents
    //records
    public Dictionary<int, int> BestMoves;

    public UntangleSettings()
    {
        //defaults for this puzzle
        _defaultIntroTimer = 0;
        _defaultPuzzleTimer = 60;

        _defaultVertexTotal = 5;
        CurrentVertexTotal = _defaultVertexTotal;
        
        BestMoves = new Dictionary<int, int>();
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
    private int defaultCardTotal;
    public int currentCardTotal;
    public Dictionary<int, int> BestStreak;

    public HackSettings(){
        
        _defaultIntroTimer = 3;
        _defaultPuzzleTimer = 10;
        
        defaultCardTotal = 4;
        currentCardTotal = defaultCardTotal;

        BestStreak = new Dictionary<int, int>();

    }
    
    public void UpdateRecords(int streak)
    {
        if (BestStreak.ContainsKey(currentCardTotal)) {
            BestStreak[currentCardTotal] = 
                (BestStreak[currentCardTotal] < streak) ? streak : BestStreak[currentCardTotal];
        }else{
            BestStreak.Add(currentCardTotal, streak);
        }
    }
}
