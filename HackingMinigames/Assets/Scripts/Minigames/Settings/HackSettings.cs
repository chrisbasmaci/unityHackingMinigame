using System.Collections.Generic;

public class HackSettings : MgSettings
{
    private int defaultCardTotal;
    public int currentCardTotal;
    public int currentStreak;
    public Dictionary<int, int> BestStreak{ get; }
    public string GameMode => "|| GameMode [" + currentCardTotal + "] Cards ||";

    public HackSettings(){
        
        _defaultIntroTimer = 3;
        _defaultPuzzleTimer = 10;
        
        defaultCardTotal = 4;
        currentCardTotal = defaultCardTotal;

        BestStreak = new Dictionary<int, int>();

    }

    public override void ResetTemp()
    {
        currentStreak = 0;
    }

    public override string GetRecords()
    {
        return GetStreakRecord();
    }

    public override void UpdateRecords()
    {
        UpdateStreakRecord();
    }
    
    
    public string GetStreakRecord()
    {
        int record;
        string recordName = " Max Streak: ";
        if (BestStreak.TryGetValue(currentCardTotal, out record))
        {
            return GameMode+recordName + record;
        }
        return GameMode + recordName + "No Highscore";
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