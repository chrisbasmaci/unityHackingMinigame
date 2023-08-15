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
    private List<Vertice> _freeVertices;
    private List<Vertice> _boundVertices;
    private List<Edge> _edgeList;
    private int _verticeTotal =10;
    public float verticeScale = 60;
    private int _edgeMax;
    private List<(Vertice leftVertice, Vertice rightVertice)> _subEdgeList;
    private HashSet<Vertice> _subVertices;
    List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap;
    //

    protected override void InitializeDerivative(WindowSize hackWindowDimensions)
    {

        ///TODO merge main minigame inits
        _edgeMax = (_verticeTotal * 2) - 4;

        
        
        _upperUIPrefab = Game.Instance.upperHackPrefab;
        var tmp = (BUIuntangle)mgPanel.BUIPanel;
        tmp.InitializeLeftButton(showSolution);
        tmp.InitializeRightButton(RetryMinigame);
        

    }



    public override void StartMinigame()
    {
        polygon = new Polygon(_verticeTotal);
        _subVertices = new HashSet<Vertice>();
        _vertices = new List<Vertice>();
        _freeVertices = new List<Vertice>();
        _boundVertices = new List<Vertice>();
        _edgeList = new List<Edge>();
        _subEdgeList = new List<(Vertice leftVertice, Vertice rightVertice)>();
        _vertices = new List<Vertice>();
        _freeVertices = new List<Vertice>();
        _boundVertices = new List<Vertice>();
        _edgeList = new List<Edge>();
        _subEdgeList = new List<(Vertice leftVertice, Vertice rightVertice)>();
        verticeConnectionMap = new List<(List<Vertice> connections, Vertice vertex)>();
        _subVertices = new HashSet<Vertice>();

        _edgeMax = (_verticeTotal * 2) - 4;
        Debug.Log("Poly count"+polygon.Count);
        polygon = new Polygon(_verticeTotal);
        Debug.Log("Poly count"+polygon.Count);

        
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
            _vertices.ForEach(vert => Debug.Log("vertno: "+vert.verticeNo));
            InstantiateEdge(_vertices.Find(vertice => vertice.verticeNo == edge.P0+1), _vertices.Find(vertice => vertice.verticeNo == edge.P1+1));
        });
    }
    
    

    public void DelauneyConnectVertices()
    {

    }

    public void AddRemainingEdges()
    {
        var freeVertices = new List<Vertice>(_freeVertices);
        Debug.Log("vertice count: "+freeVertices.Count);
        while (_edgeList.Count < _edgeMax)
        {
            (Vertice, Vertice) verticePair;
            do {
                var firstVertice = freeVertices[UnityEngine.Random.Range(0, freeVertices.Count)];
                freeVertices.Remove(firstVertice);
                var secondVertice = freeVertices[UnityEngine.Random.Range(0, freeVertices.Count)];
                verticePair = (firstVertice.verticeNo > secondVertice.verticeNo) 
                    ? (secondVertice, firstVertice)
                    : (firstVertice, secondVertice);
            } while (verticePair.Item1.IsEdgesFull() || verticePair.Item2.IsEdgesFull()||_edgeList.Find(edge => edge._verticePair == verticePair));
            freeVertices = new List<Vertice>(_freeVertices);
            InstantiateEdge(verticePair.Item1, verticePair.Item2);
        }
        
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
        _freeVertices.Add(vertice);
        _vertices.Add(vertice);
    }

    private void InstantiateEdge(Vertice leftVertice, Vertice rightVertice)
    {
        var tmpObject = new GameObject("Edge[" + leftVertice.verticeNo+"-" + rightVertice.verticeNo +"]");
        var edge = tmpObject.AddComponent<Edge>();
        // if (leftVertice.IsEdgesFull())
        // {
        //     _freeVertices.Remove(leftVertice);
        // }
        // if (rightVertice.IsEdgesFull())
        // {
        //     _freeVertices.Remove(rightVertice);
        // }
        edge.Initialize(mgPanel, leftVertice, rightVertice);

        leftVertice.addEdge(edge, ref verticeConnectionMap);
        rightVertice.addEdge(edge, ref verticeConnectionMap);

        _edgeList.Add(edge);
    }

    private void TangleVertices()
    {
        while(_freeVertices.Count >= 2 )
        {
            retry:
            int lVerticeIndex = UnityEngine.Random.Range(0, _freeVertices.Count);

            // Get a random index for the second vertice (excluding the first index)
            int rVerticeIndex;
            do {
                rVerticeIndex = UnityEngine.Random.Range(0, _freeVertices.Count);
            } while (rVerticeIndex == lVerticeIndex);
            
            Vertice lVertice, rVertice;
            
            if (lVerticeIndex > rVerticeIndex) {
                lVertice = _freeVertices[rVerticeIndex];
                rVertice = _freeVertices[lVerticeIndex];
            }
            else {
                 lVertice = _freeVertices[lVerticeIndex];
                 rVertice = _freeVertices[rVerticeIndex];
            }

            var verticePair = (lVertice, rVertice);
            if (_edgeList.Find(edge => edge._verticePair == verticePair))
            {
                // Debug
                goto retry;
            }
            InstantiateEdge(lVertice, rVertice);
            
            if (lVertice.IsEdgesFull()) {
                _freeVertices.Remove(lVertice);
                _boundVertices.Add(lVertice);
            }            
            if (rVertice.IsEdgesFull()) {
                _freeVertices.Remove(rVertice);
                _boundVertices.Add(rVertice);
            }
        }
    }   
    private void RandomConnectAllVertices()
    {
        var freeVertices = new List<Vertice>(_freeVertices);
        Vertice startVertice = freeVertices[UnityEngine.Random.Range(0, freeVertices.Count)];
        Vertice currentVertice = startVertice;
        freeVertices.Remove(currentVertice);
        
        while (freeVertices.Count >0)
        {
            Vertice nextVertice = nextVertice = freeVertices[UnityEngine.Random.Range(0, freeVertices.Count)];
            freeVertices.Remove(nextVertice);
            var verticePair = (currentVertice.verticeNo > nextVertice.verticeNo) 
                ? (nextVertice, currentVertice) 
                : (currentVertice, nextVertice);
            InstantiateEdge(verticePair.Item1, verticePair.Item2);

            currentVertice = nextVertice;
        }
        var vPair = (currentVertice.verticeNo > startVertice.verticeNo) 
            ? (startVertice, currentVertice) 
            : (currentVertice, startVertice);
        InstantiateEdge(vPair.Item1, vPair.Item2);

    }

    private void MakeGraphHomeomorphic()
    {

        //remove weight 2 vertices to check for subgraphs
        _edgeList.ForEach(edge =>
        {
            if (!edge.IsMerged())
            {
                Debug.Log("edge not merged" + edge.name);
                var subEdge = edge.SubEdge();
                _subEdgeList.Add(subEdge);
                _subVertices.Add(subEdge.leftVertice);
                _subVertices.Add(subEdge.rightVertice);
            }
            else
            {
                Debug.Log("edge is merged" + edge.name);
            }
            
        });
        int subVerticeTotal = _subVertices.Count;

        _edgeMax = (subVerticeTotal * 2) - 4;
        if (_subEdgeList.Count > _edgeMax)
        {
            Debug.Log("subVerticeTotal"+subVerticeTotal+"edge total: "+_subEdgeList.Count +"edge Max" +_edgeMax);
            Debug.Log("Unsolvable");
        }
    }

    public void RemoveK3SubGraphs()
    {
        verticeConnectionMap.Where(pair => pair.vertex.weight == 2).ToList().ForEach(pair =>
        {

            var leftConnection = pair.connections[0];
            var rightConnection = pair.connections[1];
            
            leftConnection.connectedVertices.Add(rightConnection);
            rightConnection.connectedVertices.Add(leftConnection);
            verticeConnectionMap.Remove(pair);
        });
        foreach (var pair in verticeConnectionMap)
        {
            Debug.Log("vertex: " + pair.vertex);
    
            foreach (var connection in pair.connections)
            {
                Debug.Log("connection: " + connection.name);
            }
        }

    }
}
