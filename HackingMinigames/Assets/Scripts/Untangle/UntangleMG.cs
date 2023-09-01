using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet;
using TriangleNet.Meshing;
using Random = System.Random;
using Vertex = TriangleNet.Geometry.Vertex;

public class UntangleMG : MiniGame
{
    private UntangleSettings _internalSettings;
    private Polygon polygon;
    public int moveTotal = 0;
    private List<Vertice> _vertices;
    private List<Edge> _edgeList;
    private int _verticeTotal;
    private float verticeScale = 60;
    private BUIuntangle _bottomUI;
    private UUIuntangle _upperUI;
    List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap;
    public HashSet<Edge> TangledEdges;

    protected override void InitializeDerivative()
    {
        //TODO GET THE NUMBERS from the sliders
        if (Settings == null) {
            _internalSettings = new UntangleSettings(0,60);
            Settings = _internalSettings;
        }
        Debug.Log("Best Time: "+_internalSettings.BestTime);
        Debug.Log("vertot: "+_verticeTotal);
        TangledEdges = new HashSet<Edge>();

        _verticeTotal = 5;
        polygon = new Polygon(_verticeTotal);
        _edgeList = new List<Edge>();
        _vertices = new List<Vertice>();
        verticeConnectionMap = new List<(List<Vertice> connections, Vertice vertex)>();
        polygon = new Polygon(_verticeTotal);
        _puzzleTimer.Initialize(ref _bottomUI.loadingbarTimer, _internalSettings);

    }

    protected override void InitBottomUI()
    {
        
        mgPanel.gameWindow.BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, mgPanel.gameWindow.bottomContainer.transform)
            .GetComponent<BUIuntangle>();
        _bottomUI = (BUIuntangle)mgPanel.gameWindow.BUIPanel;
        _bottomUI.InitializeLeftButton(showSolution);
        _bottomUI.InitializeRightButton(RetryMinigame);
    }    
    protected override void InitUpperUI()
    {
        mgPanel.gameWindow.UUIpanel = Instantiate(Game.Instance.upperUntanglePrefab, mgPanel.gameWindow.upperContainer.transform)
            .GetComponent<UUIuntangle>();
        _upperUI = (UUIuntangle)mgPanel.gameWindow.UUIpanel;
        _upperUI.UpdateMoves(10);
    }

    public void CheckIfSolved()
    {
        Debug.Log("CHECK SOLVED");
        if (!_edgeList.Find(edge => edge._isTangled))
        {
            PuzzleSolved();
        }
    }
    public override void ChildStartMinigame()
    {
        _upperUI.ResetUI();
        moveTotal = 0;
        TangledEdges = new HashSet<Edge>();
        polygon = new Polygon(_verticeTotal);
        _edgeList = new List<Edge>();
        _vertices = new List<Vertice>();
        verticeConnectionMap = new List<(List<Vertice> connections, Vertice vertex)>();
        polygon = new Polygon(_verticeTotal);

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
        polygon = new Polygon(_verticeTotal);
        _vertices.ForEach(vertice =>vertice.destr());
        _edgeList.ForEach(edge => Destroy(edge.gameObject));
    }

    public void UpdateMoves(int movesCount)
    {
        _upperUI.UpdateMoves(movesCount);
    }

    public void showSolution()
    {
        PauseMinigame();
        _vertices.ForEach(vertice => StartCoroutine(vertice.MoveCoroutine()));

    }

    public override void RetryMinigame()
    {
        isPaused = false;
        _puzzleTimer.reset_timer();
        StopAllCoroutines();
        _vertices.ForEach(vertice =>vertice.destr());
        _edgeList.ForEach(edge => Destroy(edge.gameObject));
        ChildStartMinigame();
    }

    public void PuzzleSolved()
    {
        Debug.Log("Puzzle Solved with " +_puzzleTimer.puzzleTimeLeft + "seconds left"
        +"and in" +moveTotal +"Moves");
        PauseMinigame();
        _internalSettings.UpdateRecords(_puzzleTimer.puzzleTimeLeft,_verticeTotal, moveTotal);
    }
    //===========
    private void InstantiateVertices()
    {
        int radius = (mgPanel.panelBounds.Height< mgPanel.panelBounds.Width)? (int)mgPanel.panelBounds.Height / 2: (int)mgPanel.panelBounds.Width / 2 ;
        PlaceVerticesCircleToLatin(_verticeTotal, radius);
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
        var dots = Generate.GetLatinSpreadPoints(_verticeTotal, ref verticeScale, mgPanel.panelBounds);
        var startingDots = Generate.CirclePoints(radius, new Point(0,0), _verticeTotal);
        
        Debug.Log("points: " + points);
        for (int i = 0; i < points; i++)
        {
            var random = new Random();
            var circlePos = random.Next(startingDots.Count());
            InstantiateVertice(dots[i], startingDots[circlePos]);
            startingDots.RemoveAt(circlePos);
        }

    }

    private void InstantiateVertice(Vertex solvedVertex, Vertex unsolvedVertex)
    {
        var tmpObject = new GameObject("TMP");
        var vertice = tmpObject.AddComponent<Vertice>();

        
        vertice.Initialize(mgPanel,ref polygon, solvedVertex, unsolvedVertex, verticeScale);
 
        
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
