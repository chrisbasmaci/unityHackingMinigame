using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Helpers;
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
    private BUIuntangle UntangleBottomUI => BottomUI as BUIuntangle;
    private UUIsimple UntangleUpperUI => UpperUI as UUIsimple;
    private GameObject _verticeGroup;
    private GameObject _edgeGroup;
    private Polygon polygon;
    private List<Vertice> _vertices;
    private List<Edge> _edgeList;
    private float verticeScale = 1f;

    List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap;
    public HashSet<Edge> TangledEdges;

    public override void FixLayoutOrder(int order)
    {
        if (_edgeList != null)
        {
            foreach (var edge in _edgeList)
            {
                edge.setLayoutOrder(order-1);
            } 
        }

    }

    protected override void InitializeDerivative()
    {
        _puzzleTimer.InitializeLoadingBar(UntangleBottomUI.loadingbarTimer);
    }
    public override MgSettings AddSettings()
    {
        Debug.Log("added settings");
        Settings = new UntangleSettings();
        return Settings;
    }
    public override void ResizeMinigame()
    {
        _vertices.ForEach(vertex =>
        {
            ObjectHandler.ClampPositionLocal(vertex._rect, mgPanel._panelRect);
            vertex.stretchAllEdges();
        });
    }
    public override GameObject InstantiateUpperSettings()
    {
        var newPanel = ComponentHandler.AddChildGameObject(mgPanel.gameWindow.upperContainer, "UpperSetting").
            AddComponent<UntangleUsi>();

        ComponentHandler.AddFlowLayout(newPanel.gameObject);
        newPanel.Initialize(mgPanel.gameWindow);
        
        return newPanel.gameObject;
    }
    protected override UIPanel InitBottomUIChild()
    {
        mgPanel.gameWindow.BUIPanel = Instantiate(Game.Instance.bottomUntanglePrefab, mgPanel.gameWindow.bottomContainer.transform)
            .GetComponent<BUIuntangle>();
        var bottomUI = (BUIuntangle)mgPanel.gameWindow.BUIPanel;
        bottomUI.InitializeLeftButton(showSolution);
        bottomUI.InitializeRightButton(RetryMinigame);
        return bottomUI;
    }    
    protected override UIPanel InitUpperUIChild()
    {
        mgPanel.gameWindow.UUIpanel = Instantiate(Game.Instance.simpleUpperUiPrefab, mgPanel.gameWindow.upperContainer.transform)
            .GetComponent<UUIsimple>();
        var upperUI = (UUIsimple)mgPanel.gameWindow.UUIpanel;
        upperUI.SetDisplayText("Moves");
        return upperUI;
    }
    public void CheckIfSolved()
    {
        Debug.Log(_edgeList.Count(edge => edge._isTangled));
        Debug.Log("CHECK SOLVED");
        if (!_edgeList.Find(edge => edge._isTangled))
        {
            PuzzleSolved();
        }
    }
    public override void StartMinigameChild()
    {
        //TODO THE minimum also effect the settings page, need to separate the two
        mgPanel.gameWindow.SetMinimumSize(800,800);

        Debug.Log("actual puzzle timer"+Settings.CurrentPuzzleTimer);
        
        polygon = new Polygon(InternalSettings.CurrentVertexTotal);
        TangledEdges = new HashSet<Edge>();
        polygon = new Polygon(InternalSettings.CurrentVertexTotal);
        _edgeList = new List<Edge>();
        _vertices = new List<Vertice>();
        verticeConnectionMap = new List<(List<Vertice> connections, Vertice vertex)>();
        _verticeGroup = ComponentHandler.AddChildGameObject(mgPanel.gameObject, "Vertices");
        ComponentHandler.SetAnchorToStretch(_verticeGroup);
        _edgeGroup = ComponentHandler.AddChildGameObject(mgPanel.gameObject, "Edges");
        ComponentHandler.SetAnchorToStretch(_edgeGroup);

        // PauseMinigame();
        InstantiateVertices();
        InstantiateEdges();
        // ResumeMinigame();
        _puzzleTimer.startPuzzleTimer();
        
    }
    public override void EndMinigame()
    {
        base.EndMinigame();
        _vertices?.ForEach(vertice =>vertice.destr());
        _edgeList?.ForEach(edge => Destroy(edge.gameObject));
        _vertices?.Clear();
        _edgeList?.Clear();
    }
    public void UpdateMoves()
    {
        UntangleUpperUI.updateDisplay(InternalSettings.currentMoves, "Moves");
    }
    public void showSolution()
    {
        PauseMinigame();
        _vertices.ForEach(vertice => StartCoroutine(vertice.MoveToSolution()));

    }
    public override void RetryMinigame()
    {
        base.RetryMinigame();
        _vertices.ForEach(vertice =>vertice.destr());
        _edgeList.ForEach(edge => Destroy(edge.gameObject));
        // ResizeMinigame();
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
        int radius = Mathf.Min((int)mgPanel._panelRect.height, (int)mgPanel._panelRect.width) / 2;
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
            var leftVertex = _vertices.Find(vertex => vertex.verticeNo == edge.P0);
            var rightVertex = _vertices.Find(vertex => vertex.verticeNo == edge.P1);
            if (!leftVertex) {
                Debug.Log("left vertex not found" + edge.P0);
            }            
            if (!rightVertex) {
                Debug.Log("right vertex not found" + edge.P1);
            }
            InstantiateEdge((leftVertex, rightVertex));
        });
    }
    private void PlaceVerticesCircleToLatin(int points, int radius)
    {
        var dots = Generate.GetLatinSpreadPoints(InternalSettings.CurrentVertexTotal, ref verticeScale,  
            GameCanvas.CalculateWsWithPadding(mgPanel._panelRect, 0f));
        var startingDots = Generate.CirclePoints(radius, new Point(0,0), InternalSettings.CurrentVertexTotal);
        
        Debug.Log("points: " + points);
        for (int i = 0; i < points; i++)
        {
            var random = new Random();
            var circlePos = random.Next(startingDots.Count());
            InstantiateVertice(dots[i], startingDots[circlePos], i);
            startingDots.RemoveAt(circlePos);
        }

    }
    public float CalculateVertexSize()
    {
        float panelResolution = mgPanel._panelRect.height * mgPanel._panelRect.width;
        float resolutionPerVertex = panelResolution / InternalSettings.CurrentVertexTotal;
        return (float)Math.Sqrt(resolutionPerVertex)/300 ;
    }
    private void InstantiateVertice(Vertex solvedVertex, Vertex unsolvedVertex, int verticeNo)
    {
        var tmpObject = ComponentHandler.AddChildGameObject(_verticeGroup, "Tmp");
        var vertice = tmpObject.AddComponent<Vertice>();


        vertice.Initialize(mgPanel,ref polygon, solvedVertex, unsolvedVertex, verticeScale,verticeNo);
        vertice._rect.position =
            new Vector3(vertice._rect.position.x, vertice._rect.position.y, 0);
        
        tmpObject.name = "Vertice["+vertice.verticeNo+"]";
        _vertices.Add(vertice);
    }

    private void InstantiateEdge((Vertice leftVertice, Vertice rightVertice)verticePair)
    {
        var edgeObject = ComponentHandler.AddChildGameObject(_edgeGroup,
            "Edge[" + verticePair.leftVertice.verticeNo + "-" + verticePair.rightVertice.verticeNo + "]");
        var edge = edgeObject.AddComponent<Edge>();
        edge.Initialize(mgPanel, verticePair);
        
        verticePair.leftVertice.addEdge(edge, ref verticeConnectionMap);
        verticePair.rightVertice.addEdge(edge, ref verticeConnectionMap);

        _edgeList.Add(edge);
    }
    
}
