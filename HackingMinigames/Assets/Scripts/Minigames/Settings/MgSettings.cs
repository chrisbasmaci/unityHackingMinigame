
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
    public abstract string GetRecords();

    public virtual void ResetTemp() { }

    public MgSettings()
    {
        // Initialize current timers with default values
        _defaultIntroTimer = 3;
        _defaultPuzzleTimer = 10;

        CurrentPuzzleTimer = _defaultPuzzleTimer;
        CurrentIntroTimer = _defaultIntroTimer;
    }
    

}
