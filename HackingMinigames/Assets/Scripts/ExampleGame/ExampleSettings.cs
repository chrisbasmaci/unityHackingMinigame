using System.Collections.Generic;
using UnityEngine;

public class ExampleSettings : MgSettings
{
    //defaults
    private int _defaultVertexTotal;
    public string GameMode => "|| GameMode [" + "MODE" + "]"+ "Type" + "||";
    


    public ExampleSettings()
    {
        //defaults for this puzzle
        _defaultIntroTimer = 0;
        _defaultPuzzleTimer = 60;

        _defaultVertexTotal = 5;
    }
    // public override void ResetTemp()
    // {
    // }
    public override void UpdateRecords()
    {
    }
    public override string GetRecords()
    {
        return "NOT IMPLEMENTED";
    }


}