using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using TriangleNet;
using TriangleNet.Geometry;

public class Vertice : MonoBehaviour
{
    private Polygon _graph;
    private Vertex _solvedVertex;
    private Vertex _unsolvedVertex;
    private List<Edge> _edges;
    public int allowedEdges;
    public int weight;
    private bool _isMerged;
    private static int _verticeTotal;
    public int verticeNo;
    private Vector2 _difference = Vector2.zero;
    private BoxCollider2D _collider;
    private SpriteRenderer _renderer;
    private GameWindow _gameWindow;
    public RectTransform _rect;
    private float _verticeScale;

    public List<Vertice> connectedVertices;
    // Start is called before the first frame update
    void Start()
    {

    }

    public List<Edge> Edges()
    {
        return _edges;
    }

    public float GetEdgeThickness()
    {
        return _verticeScale/600;
    }
    public void Initialize(GameWindow gameWindow,ref Polygon polygon, Vertex solvedVertex, Vertex unsolvedVertex, float verticeScale)
    {
        _graph = polygon;
        allowedEdges = 4;
        connectedVertices = new List<Vertice>();
        verticeNo = ++_verticeTotal;
        _edges = new List<Edge>();
        _collider =gameObject.AddComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        _renderer = gameObject.AddComponent<SpriteRenderer>();
        _rect = gameObject.AddComponent<RectTransform>();
        _renderer.sprite = Game.Instance.shapeSheet[0];
        _gameWindow = gameWindow;
        transform.SetParent(_gameWindow.transform);
        _rect.transform.localPosition =
            new Vector3((float)unsolvedVertex.X, (float)unsolvedVertex.Y, -2);      
        _solvedVertex = solvedVertex;
        _unsolvedVertex = unsolvedVertex;
        _verticeScale = verticeScale;
        _rect.transform.localScale = new Vector3(verticeScale, verticeScale, 1);
        polygon.Add(_solvedVertex);
        
        Debug.Log("verticeNo Init:" +verticeNo );
        // _rect.transform.localScale = new Vector3(30, 30, 1);
        Debug.Log("vertice size: bounds "+ _renderer.sprite.bounds.size);



    }
    
    

    public bool IsEdgesFull()
    {
        Debug.Log(_edges.Count);
        return _edges.Count == allowedEdges;
    }

    public void addEdge(Edge edge, ref List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap)
    {
        var oldConnections = new List<Vertice>(connectedVertices);
        _edges.Add(edge);
        weight++;
        connectedVertices.Add(edge.OtherVertice(this));
        UpdateVertexConnectionMap(ref verticeConnectionMap, oldConnections, connectedVertices);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        // Debug.Log("pressed");
        _difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    
    private void OnMouseDrag()
    {
        var newPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _difference;
        ///TODO FIX windowsize struct
        var panelRectTransform = _gameWindow.GetComponent<RectTransform>();
        var leftBorderWorldPosition = panelRectTransform.TransformPoint(_gameWindow._windowSize.LeftBorder,0f,0f).x;
        var rightBorderWorldPosition = panelRectTransform.TransformPoint(_gameWindow._windowSize.RightBorder,0f,0f).x;
        var bottomBorderWorldPosition = panelRectTransform.TransformPoint(0f,_gameWindow._windowSize.BottomBorder,0f).y;
        var topBorderWorldPosition = panelRectTransform.TransformPoint(0f,_gameWindow._windowSize.TopBorder,0f).y;

        float minX = (leftBorderWorldPosition);
        float maxX = rightBorderWorldPosition;
        float minY = bottomBorderWorldPosition;
        float maxY = topBorderWorldPosition;

        // Clamp the new position within the bounds
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        // Debug.Log("minx: "+ minX +"_rect.position: "+_rect.localPosition.x+"leftBorderWorldPosition" +leftBorderWorldPosition);

        _rect.position = new Vector3(newPos.x,newPos.y, -2);
      
        stretchAllEdges();
    }

    private void stretchAllEdges()
    {
        if (_edges.Count != 0)
        {
            _edges.ForEach(edge => edge.stretchEdge());

        }
    }

    //for weight 2 vertices get the other edge
    public Vertice ConnectedTo(ref Edge edge)
    {
        Debug.Assert(_edges.Count ==2);
        var leftEdge = _edges[0];
        var rightEdge = _edges[1];

        if (leftEdge == edge)
        {
            edge = rightEdge;
            return rightEdge.OtherVertice(this);
        }else if (rightEdge == edge)
        {
            edge = leftEdge;
            return leftEdge.OtherVertice(this);
        }

        Debug.Assert(false);
        return null;
    }


    public bool IsMerged()
    {
        return _isMerged;
    }

    public void UpdateVertexConnectionMap
        (ref List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap, List<Vertice> oldConnections,List<Vertice> newConnections)
    {
        
        // if (verticeConnectionMap.ContainsKey(oldConnections)) {
        //     if (--verticeConnectionMap[oldConnections].== 0)
        //     {
        //         verticeConnectionMap.Remove(oldConnections);
        //     }
        // }
        
        // if (verticeConnectionMap.ContainsKey(newConnections)) {
        //     verticeConnectionMap[newConnections]++;
        // }
        // else {
        //     verticeConnectionMap.Add(newConnections, 1);
        // }
        Debug.Log("cc: "+oldConnections.Count);
        var pairToRemove = (oldConnections, this);
        verticeConnectionMap.Remove(pairToRemove);
        pairToRemove = (newConnections, this);
        verticeConnectionMap.Remove(pairToRemove);

        verticeConnectionMap.Add((newConnections, this));
    }

    public void OnDestroy()
    {

    }

    public void destr()
    {
        Debug.Log("verticeNO: destr" + _verticeTotal);
        _verticeTotal--;
        Destroy(gameObject);
    }
    
    public IEnumerator MoveCoroutine()
    {
        Vector3 startingPosition = _rect.transform.localPosition;
        float elapsedTime = 0f;
        var duration = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            _rect.transform.localPosition = Vector3.Lerp(startingPosition, new Vector3((float)_solvedVertex.X,(float)_solvedVertex.Y, startingPosition.z), t);
            stretchAllEdges();
            yield return null;
        }
    }
    
    
}
