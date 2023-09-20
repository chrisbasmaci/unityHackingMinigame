using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TriangleNet.Geometry;
using TriangleNet;
using TriangleNet.Meshing;
using UnityEditor;

public class JumpChessMG : MiniGame
{
    private UntangleSettings InternalSettings => Settings as UntangleSettings;
    private BUIJumpChess JumpChessBottomUI => BottomUI as BUIJumpChess;
    private UUIJumpChess JumpChessUpperUI => UpperUI as UUIJumpChess;
    private string _usiPrefabLocation = "not set";
    private string _buiPrefabLocation = "not set";
    private string _uuiPrefabLocation = "not set";

    protected override void InitializeDerivative()
    {
        // _puzzleTimer.InitializeLoadingBar(UntangleBottomUI.loadingbarTimer);
        
    }
    public override MgSettings AddSettings()
    {
        return new JumpChessSettings();
    }

    public override GameObject InstantiateUpperSettings()
    {
        var newPanel = ComponentHandler.AddChildGameObject(mgPanel.gameWindow.upperContainer, "UpperSetting").
            AddComponent<JumpChessUsi>();

        ComponentHandler.AddFlowLayout(newPanel.gameObject);
        newPanel.Initialize(mgPanel.gameWindow);

        return newPanel.gameObject;    
    }

    protected override UIPanel InitBottomUIChild()
    {
        var bui = Resources.Load<GameObject>(_buiPrefabLocation);

        if (!bui) {
            return null;
        }
        mgPanel.gameWindow.BUIPanel = Instantiate(bui, mgPanel.gameWindow.bottomContainer.transform)
            .GetComponent<BUIJumpChess>();
        var bottomUI = (BUIuntangle)mgPanel.gameWindow.BUIPanel;
        return bottomUI;
    }    
    protected override UIPanel InitUpperUIChild()
    {
        var uui = Resources.Load<GameObject>(_uuiPrefabLocation);

        ComponentHandler.AddFlowLayout(mgPanel.gameWindow.upperContainer);

        return null;
    }


    public override void StartMinigameChild()
    {
        _puzzleTimer.startPuzzleTimer();
        PrefabHandler.CreateGrid(gameObject, 3);

    }

    public override void EndMinigame()
    {
        base.EndMinigame();
    }
    
    
    public override void RetryMinigame()
    {
        base.RetryMinigame();
        StartMinigameChild();
    }
    
    public override void PuzzleSolvedChild()
    {
        Debug.Log("Puzzle Solved");
        PauseMinigame();
        EndRound();
    }
    


}
