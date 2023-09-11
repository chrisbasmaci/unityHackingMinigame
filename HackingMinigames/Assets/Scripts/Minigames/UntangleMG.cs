using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet;
using TriangleNet.Meshing;
using UnityEditor;
using Untangle;
using Edge = Untangle.Edge;
using Random = System.Random;
using Vertex = TriangleNet.Geometry.Vertex;

public class UntangleMG : MiniGame
{
    private UntangleSettings InternalSettings => Settings as UntangleSettings;
    
    private Polygon polygon;
    private List<Vertice> _vertices;
    private List<Edge> _edgeList;
    private float verticeScale = 1f;
    private BUIuntangle _bottomUI;
    private UUIuntangle _upperUI;
    List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap;
    public HashSet<Edge> TangledEdges;

    protected override void InitializeDerivative()
    {
        _puzzleTimer.InitializeLoadingBar(_bottomUI.loadingbarTimer);
    }
    public override MgSettings AddSettings()
    {
        return new UntangleSettings();
    }

    public override GameObject getUpperSettingPrefab()
    {
        var upperSettings = Resources.Load<GameObject>("Prefabs/Untangle/Settings/UntangleUSI");
        var _uiSettings = upperSettings.GetComponent<UntangleUsi>();
        return upperSettings;
    }

    protected override UIPanel InitBottomUIChild()
    {
        
        mgPanel.gameWindow.BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, mgPanel.gameWindow.bottomContainer.transform)
            .GetComponent<BUIuntangle>();
        _bottomUI = (BUIuntangle)mgPanel.gameWindow.BUIPanel;
        _bottomUI.InitializeLeftButton(showSolution);
        _bottomUI.InitializeRightButton(RetryMinigame);
        return _bottomUI;
    }    
    protected override UIPanel InitUpperUIChild()
    {
        mgPanel.gameWindow.UUIpanel = Instantiate(Game.Instance.upperUntanglePrefab, mgPanel.gameWindow.upperContainer.transform)
            .GetComponent<UUIuntangle>();
        _upperUI = (UUIuntangle)mgPanel.gameWindow.UUIpanel;
        return _upperUI;
    }

    public void CheckIfSolved()
    {
        Debug.Log("CHECK SOLVED");
        if (!_edgeList.Find(edge => edge._isTangled))
        {
            PuzzleSolved();
        }
    }
    public override void StartMinigameChild()
    {
        
        polygon = new Polygon(InternalSettings.CurrentVertexTotal);
        TangledEdges = new HashSet<Edge>();
        polygon = new Polygon(InternalSettings.CurrentVertexTotal);
        _edgeList = new List<Edge>();
        _vertices = new List<Vertice>();
        verticeConnectionMap = new List<(List<Vertice> connections, Vertice vertex)>();
        PauseMinigame();
        InstantiateVertices();
        InstantiateEdges();
        ResumeMinigame();
        _puzzleTimer.startPuzzleTimer();
        
    }

    public override void EndMinigame()
    {
        _puzzleTimer.reset_timer();
        StopAllCoroutines();
        polygon = new Polygon(InternalSettings.CurrentVertexTotal);
        _vertices.ForEach(vertice =>vertice.destr());
        _edgeList.ForEach(edge => Destroy(edge.gameObject));
    }

    public void UpdateMoves()
    {
        _upperUI.UpdateMoves(InternalSettings.currentMoves);
    }

    public void showSolution()
    {
        PauseMinigame();
        _vertices.ForEach(vertice => StartCoroutine(vertice.MoveToSolution()));

    }

  
    public override void RetryMinigame()
    {
        base.RetryMinigame();
        isPaused = false;
        _puzzleTimer.reset_timer();
        StopAllCoroutines();
        _vertices.ForEach(vertice =>vertice.destr());
        _edgeList.ForEach(edge => Destroy(edge.gameObject));
        StartMinigameChild();
    }

    public void IncrementMoveTotal()
    {
        InternalSettings.currentMoves++;
    }
    public override void PuzzleSolvedChild()
    {
        Debug.Log("Puzzle Solved with " +_puzzleTimer.puzzleTimeLeft + "seconds left"
        +"and in" +InternalSettings.currentMoves +"Moves");
        PauseMinigame();
        EndRound();
    }
    //===========
    private void InstantiateVertices()
    {
        int radius = (mgPanel.panelBounds.Height< mgPanel.panelBounds.Width)? (int)mgPanel.panelBounds.Height / 2: (int)mgPanel.panelBounds.Width / 2 ;
        PlaceVerticesCircleToLatin(InternalSettings.CurrentVertexTotal, radius);
    }


    private void InstantiateEdges()
    {
        Debug.Log("Poly count"+polygon.Count);
        var options = new ConstraintOptions() { Convex = true };
        var mesh = polygon.Triangulate(options);
        mesh.Edges.ToList().ForEach(edge =>
        {
            // Debug.Log("p0: "+edge.P0 +"P1:" +edge.P1);
            var leftVertex = _vertices.Find(vertex => vertex.verticeNo == edge.P0 + 1);
            var rightVertex = _vertices.Find(vertex => vertex.verticeNo == edge.P1 + 1);
            InstantiateEdge((leftVertex, rightVertex));
        });
    }
    
    
    private void PlaceVerticesCircleToLatin(int points, int radius)
    {
        var dots = Generate.GetLatinSpreadPoints(InternalSettings.CurrentVertexTotal, ref verticeScale, mgPanel.panelBounds);
        var startingDots = Generate.CirclePoints(radius, new Point(0,0), InternalSettings.CurrentVertexTotal);
        
        Debug.Log("points: " + points);
        for (int i = 0; i < points; i++)
        {
            var random = new Random();
            var circlePos = random.Next(startingDots.Count());
            InstantiateVertice(dots[i], startingDots[circlePos]);
            startingDots.RemoveAt(circlePos);
        }

    }

    public float CalculateVertexSize()
    {
        float panelResolution = mgPanel.panelBounds.Height * mgPanel.panelBounds.Width;
        float resolutionPerVertex = panelResolution / InternalSettings.CurrentVertexTotal;
        return (float)Math.Sqrt(resolutionPerVertex)/300 ;
    }

    private void InstantiateVertice(Vertex solvedVertex, Vertex unsolvedVertex)
    {
        var tmpObject = new GameObject("TMP");
        var vertice = tmpObject.AddComponent<Vertice>();


        var size = CalculateVertexSize();
        vertice.Initialize(mgPanel,ref polygon, solvedVertex, unsolvedVertex, verticeScale);
        vertice.gameObject.transform.localScale = new Vector2(size, size);
 
        
        vertice._rect.position =
            new Vector3(vertice._rect.position.x, vertice._rect.position.y, -2);
        
        tmpObject.name = "Vertice["+vertice.verticeNo+"]";
        Debug.Log("VX: "+tmpObject.name);
        _vertices.Add(vertice);
    }

    private void InstantiateEdge((Vertice leftVertice, Vertice rightVertice)verticePair)
    {
        var tmpObject = new GameObject("Edge[" + verticePair.leftVertice.verticeNo+"-" + verticePair.rightVertice.verticeNo +"]");
        var edge = tmpObject.AddComponent<Edge>();
        edge.Initialize(mgPanel, verticePair);
        
        verticePair.leftVertice.addEdge(edge, ref verticeConnectionMap);
        verticePair.rightVertice.addEdge(edge, ref verticeConnectionMap);

        _edgeList.Add(edge);
    }
    
}
