using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using JumpChess;
using TriangleNet.Geometry;
using TriangleNet;
using TriangleNet.Meshing;
using UnityEditor;
using UnityEngine.UI;

public class JumpChessMG : MiniGame
{
    private UntangleSettings InternalSettings => Settings as UntangleSettings;
    private BUIJumpChess JumpChessBottomUI => BottomUI as BUIJumpChess;
    private UUIJumpChess JumpChessUpperUI => UpperUI as UUIJumpChess;

    public ChessPiece[,] Board;
    public int gridDimension =8;
    
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
        SetupBoard();

    }

    private void SetupBoard()
    {
        var boardbg = ComponentHandler.AddChildGameObject(gameObject, "Board");
        ComponentHandler.SetAnchorToStretch(boardbg);
        ComponentHandler.AddAspectRatioFitter(boardbg, AspectRatioFitter.AspectMode.HeightControlsWidth);
        ComponentHandler.AddImageComponent(boardbg, null, Color.black);
        Board = PrefabHandler.CreateGrid<ChessPiece>(boardbg, gridDimension);
        for (int row = 0; row < Board.GetLength(0); row++)
        {
            for (int col = 0; col < Board.GetLength(1); col++)
            {
                ChessPiece chessPiece = Board[row, col];
                chessPiece.Initialize(this, (row, col));
            }
        }


        DorRtoSolution();
    }

    private void DorRtoSolution()
    {
        var startingTile = Board[0, 0];

        while (startingTile.IsDirectionAvailabe(Direction.Bottom) || startingTile.IsDirectionAvailabe(Direction.Right))
        {
            Direction direction = UnityEngine.Random.Range(0, 2) == 0 ? Direction.Bottom : Direction.Right;
            direction = startingTile.IsDirectionAvailabe(direction) ? direction : 
                (direction == Direction.Bottom ? Direction.Right : Direction.Bottom);

            int jumpRange = (direction == Direction.Bottom) ? 
                gridDimension - startingTile.pos.row : gridDimension - startingTile.pos.col;

            int jumpAmount = UnityEngine.Random.Range(1, jumpRange);

            startingTile = (direction == Direction.Bottom) 
                ? Board[startingTile.pos.row + jumpAmount, startingTile.pos.col]
                : Board[startingTile.pos.row, startingTile.pos.col + jumpAmount];
            Debug.Log("The tile is = " + startingTile.pos);

        }
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
