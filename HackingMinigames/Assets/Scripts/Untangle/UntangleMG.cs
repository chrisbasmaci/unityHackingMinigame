using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System;
using TriangleNet.Geometry;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using TriangleNet;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Geometry;
using Random = System.Random;
using System;
using TMPro;
using Vertex = TriangleNet.Geometry.Vertex;

public class UntangleMG : MiniGame
{
    private Polygon polygon;
    
    private List<Vertice> _vertices;
    private List<Edge> _edgeList;
    private int _verticeTotal =10;
    private float verticeScale = 60;
    List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap;

    protected override void InitializeDerivative(WindowSize hackWindowDimensions)
    {

        ///TODO merge main minigame inits

        _upperUIPrefab = Game.Instance.upperHackPrefab;
        var tmp = (BUIuntangle)mgPanel.BUIPanel;
        tmp.InitializeLeftButton(showSolution);
        tmp.InitializeRightButton(RetryMinigame);
    }
    

    public override void StartMinigame()
    {
        polygon = new Polygon(_verticeTotal);
        _edgeList = new List<Edge>();
        _vertices = new List<Vertice>();
        verticeConnectionMap = new List<(List<Vertice> connections, Vertice vertex)>();
        polygon = new Polygon(_verticeTotal);

        InstantiateVertices();
        InstantiateEdges();
    }

    public override void EndMinigame()
    {
        StopAllCoroutines();
        polygon = new Polygon(_verticeTotal);
        _vertices.ForEach(vertice =>vertice.destr());
        _edgeList.ForEach(edge => Destroy(edge.gameObject));
        

    }

    public void showSolution()
    {
        _vertices.ForEach(vertice => StartCoroutine(vertice.MoveCoroutine()));

    }

    public override void RetryMinigame()
    {
        EndMinigame();
        StartMinigame();
    }
    //===========
    private void InstantiateVertices()
    {
        PlaceVerticesCircleToLatin(_verticeTotal, (int)_hackWindowDimensions.Height / 2);
    }


    private void InstantiateEdges()
    {
        Debug.Log("Poly count"+polygon.Count);
        var options = new ConstraintOptions() { Convex = true };
        var mesh = polygon.Triangulate(options);
        mesh.Edges.ToList().ForEach(edge =>
        {
            Debug.Log("p0: "+edge.P0 +"P1:" +edge.P1);
            var leftVertex = _vertices.Find(vertex => vertex.verticeNo == edge.P0 + 1);
            var rightVertex = _vertices.Find(vertex => vertex.verticeNo == edge.P1 + 1);
            InstantiateEdge((leftVertex, rightVertex));
        });
    }
    
    
    // private void PlaceVerticesCircular(int points, int radius)
    // {
    //     (float xCoord, float yCoord) centerVertice = (0, 0);
    //
    //     // Calculate the angle between each vertex in degrees
    //     double slice = 2 * Math.PI / points;
    //
    //
    //     for (int i = 0; i < points; i++)
    //     {
    //         
    //         double angleInRadians = slice * i;
    //
    //         float xCoord = (float)(centerVertice.xCoord + radius * Math.Cos(angleInRadians));
    //         float yCoord = (float)(centerVertice.yCoord + radius * Math.Sin(angleInRadians));
    //         InstantiateVertice((xCoord, yCoord));
    //     }
    //
    // }    
    private void PlaceVerticesCircleToLatin(int points, int radius)
    {
        (float xCoord, float yCoord) centerVertice = (0, 0);
        
        var dots = Generate.GetLatinSpreadPoints(_verticeTotal, ref verticeScale, _hackWindowDimensions);
        var startingDots = Generate.CirclePoints(radius, new Point(0,0), _verticeTotal);
        
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
