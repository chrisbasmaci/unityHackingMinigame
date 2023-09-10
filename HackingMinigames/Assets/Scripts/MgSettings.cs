
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

    public abstract void UpdateRecords();



    public MgSettings()
    {
        // Initialize current timers with default values
        _defaultIntroTimer = 3;
        _defaultPuzzleTimer = 10;

        CurrentPuzzleTimer = _defaultPuzzleTimer;
        CurrentIntroTimer = _defaultIntroTimer;
    }
    

}

public class UntangleSettings : MgSettings
{
    //defaults
    private int _defaultVertexTotal;
    private int _currentVertexTotal;
    public int currentMoves;
    public string GameMode => "GameMode [" + _currentVertexTotal + "] Vertices";
    
    public int CurrentVertexTotal
    {
        get => _currentVertexTotal;
        set => _currentVertexTotal = value;
    }
    //currents
    //records

    public Dictionary<int, int> BestMoves { get; }

    public UntangleSettings()
    {
        //defaults for this puzzle
        _defaultIntroTimer = 0;
        _defaultPuzzleTimer = 60;

        _defaultVertexTotal = 5;
        _currentVertexTotal = _defaultVertexTotal;
        
        BestMoves = new Dictionary<int, int>();
    }

    public override void UpdateRecords()
    {
        UpdateMoveRecord();
    }

    public void UpdateMoveRecord()
    {
        if (BestMoves.ContainsKey(_currentVertexTotal)) {
            BestMoves[_currentVertexTotal] = 
                (BestMoves[_currentVertexTotal] > currentMoves) ? currentMoves : BestMoves[_currentVertexTotal];
        }else{
            BestMoves.Add(_currentVertexTotal, currentMoves);
        }
    }

    public (string mode, int? record) GetMoveRecord()
    {
        int? record = null; // Use int? (nullable int) to allow for null values
        if (BestMoves.TryGetValue(_currentVertexTotal, out int tempRecord)) {
            record = tempRecord; 
        }
        return (GameMode, record);


    }
}
public class HackSettings : MgSettings
{
    private int defaultCardTotal;
    public int currentCardTotal;
    public int currentStreak;
    public Dictionary<int, int> BestStreak;

    public HackSettings(){
        
        _defaultIntroTimer = 3;
        _defaultPuzzleTimer = 10;
        
        defaultCardTotal = 4;
        currentCardTotal = defaultCardTotal;

        BestStreak = new Dictionary<int, int>();

    }

    public override void UpdateRecords()
    {
        UpdateStreakRecord();
    }
    
    public void UpdateStreakRecord()
    {
        if (BestStreak.ContainsKey(currentCardTotal)) {
            BestStreak[currentCardTotal] = 
                (BestStreak[currentCardTotal] < currentStreak) ? currentStreak : BestStreak[currentCardTotal];
        }else{
            BestStreak.Add(currentCardTotal, currentStreak);
        }
    }
}
