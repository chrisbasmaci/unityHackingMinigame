using System.Collections.Generic;

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

    public override string GetRecords()
    {
        return GetMoveRecord();
    }

    public string GetMoveRecord()
    {
        int record;
        string recordName = " Minimum Moves: ";
        if (BestMoves.TryGetValue(_currentVertexTotal, out record))
        {
            return GameMode+recordName + record;
        }
        return GameMode + recordName + "No Highscore";
    }
}