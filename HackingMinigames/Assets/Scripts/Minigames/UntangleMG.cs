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
    private BUIuntangle UntangleBottomUI => BottomUI as BUIuntangle;
    private UUIuntangle UntangleUpperUI => UpperUI as UUIuntangle;
    private Polygon polygon;
    private List<Vertice> _vertices;
    private List<Edge> _edgeList;
    private float verticeScale = 1f;

    List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap;
    public HashSet<Edge> TangledEdges;

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

    public override GameObject InstantiateUpperSettings()
    {
        var newPanel = ComponentHandler.AddChildGameObject(mgPanel.gameWindow.upperContainer, "UpperSetting").
            AddComponent<UntangleUsi>();

        ComponentHandler.AddFlowLayout(newPanel.gameObject);
        newPanel.Initialize(mgPanel.gameWindow);
        newPanel.InitSliders( 
            Helpers.PrefabHandler.AddSliderPrefab(newPanel.gameObject, "TimeSlider"),
            Helpers.PrefabHandler.AddSliderPrefab(newPanel.gameObject, "VertexSlider")
        );

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
        mgPanel.gameWindow.UUIpanel = Instantiate(Game.Instance.upperUntanglePrefab, mgPanel.gameWindow.upperContainer.transform)
            .GetComponent<UUIuntangle>();
        var upperUI = (UUIuntangle)mgPanel.gameWindow.UUIpanel;
        return upperUI;
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
        Debug.Log("actual puzzle timer"+Settings.CurrentPuzzleTimer);
        
        polygon = new Polygon(InternalSettings.CurrentVertexTotal);
        TangledEdges = new HashSet<Edge>();
        polygon = new Polygon(InternalSettings.CurrentVertexTotal);
        _edgeList = new List<Edge>();
        _vertices = new List<Vertice>();
        verticeConnectionMap = new List<(List<Vertice> connections, Vertice vertex)>();
        // PauseMinigame();
        InstantiateVertices();
        InstantiateEdges();
        // ResumeMinigame();
        _puzzleTimer.startPuzzleTimer();
        
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        _vertices.ForEach(vertice =>vertice.destr());
        _edgeList.ForEach(edge => Destroy(edge.gameObject));
    }

    public void UpdateMoves()
    {
        UntangleUpperUI.UpdateMoves(InternalSettings.currentMoves);
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
