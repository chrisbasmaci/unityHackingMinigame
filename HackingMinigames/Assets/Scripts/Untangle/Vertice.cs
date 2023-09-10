using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using TriangleNet;
using TriangleNet.Geometry;
using UnityEngine.EventSystems;

public class Vertice : MonoBehaviour
{
    
    private MgPanel _mgPanel;

    private Vertex _solvedVertex;
    private Vertex _unsolvedVertex;
    public List<Vertice> connectedVertices;
    private List<Edge> _edges;
    public int weight;
    private static int _verticeTotal;
    public int verticeNo;
    private Vector2 _difference = Vector2.zero;
    private float _verticeScale;

    private Image _vertexImage;
    private BoxCollider2D _collider;
    public RectTransform _rect;
    

    public List<Edge> Edges()
    {
        return _edges;
    }

    public float GetEdgeThickness()
    {
        return _verticeScale/10;
    }
    public void Initialize(MgPanel mgPanel,ref Polygon polygon, Vertex solvedVertex, Vertex unsolvedVertex, float verticeScale)
    {
        _rect = gameObject.AddComponent<RectTransform>();
        connectedVertices = new List<Vertice>();
        Debug.Log("vertissceNO: added" + _verticeTotal);
        verticeNo = ++_verticeTotal;
        _edges = new List<Edge>();

        SetupTriggers();
        
        _vertexImage = ComponentHandler.AddImageComponent(gameObject, Game.Instance.shapeSheet[0]);
        _mgPanel = mgPanel;
        transform.SetParent(_mgPanel.transform);
        _rect.transform.localPosition =
            new Vector3((float)unsolvedVertex.X, (float)unsolvedVertex.Y, -2);      
        _solvedVertex = solvedVertex;
        _unsolvedVertex = unsolvedVertex;
        _verticeScale = verticeScale;
        _rect.transform.localScale = new Vector3(verticeScale, verticeScale, 1);
        _rect = gameObject.GetComponent<RectTransform>();

        polygon.Add(_solvedVertex);
        
        Debug.Log("verticeNo Init:" +verticeNo );
        Debug.Log("verticePos:" + _rect.transform.localPosition );
        // _rect.transform.localScale = new Vector3(30, 30, 1);



    }

    private void SetupTriggers()
    {
        var eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventHandler.AddTrigger(eventTrigger, EventTriggerType.PointerClick, OnMouseDown);      
        EventHandler.AddTrigger(eventTrigger, EventTriggerType.Drag, OnMouseDrag);      
        EventHandler.AddTrigger(eventTrigger, EventTriggerType.PointerUp, OnMouseUp);      
        
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
    private void OnMouseDown()
    {
        if (_mgPanel._miniGame.isPaused)
        {
            return;
        }
        // Debug.Log("pressed");
        _difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    
    private void OnMouseDrag()
    {
        if (_mgPanel._miniGame.isPaused)
        {
            return;
        }

        var newPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _difference;
        ///TODO FIX windowsize struct
        var panelRectTransform = _mgPanel.GetComponent<RectTransform>();
        var leftBorderWorldPosition = panelRectTransform.TransformPoint(_mgPanel.panelBounds.LeftBorder,0f,0f).x;
        var rightBorderWorldPosition = panelRectTransform.TransformPoint(_mgPanel.panelBounds.RightBorder,0f,0f).x;
        var bottomBorderWorldPosition = panelRectTransform.TransformPoint(0f,_mgPanel.panelBounds.BottomBorder,0f).y;
        var topBorderWorldPosition = panelRectTransform.TransformPoint(0f,_mgPanel.panelBounds.TopBorder,0f).y;

        float minX = (leftBorderWorldPosition);
        float maxX = rightBorderWorldPosition;
        float minY = bottomBorderWorldPosition;
        float maxY = topBorderWorldPosition;
        // Debug.Log("panelRectTransform: "+panelRectTransform.rect.width);

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        _rect.position = new Vector3(newPos.x,newPos.y, -2);
        // Debug.Log(_rect.position);
      
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
    

    public void UpdateVertexConnectionMap
        (ref List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap, List<Vertice> oldConnections,List<Vertice> newConnections)
    {
        
        Debug.Log("cc: "+oldConnections.Count);
        var pairToRemove = (oldConnections, this);
        verticeConnectionMap.Remove(pairToRemove);
        pairToRemove = (newConnections, this);
        verticeConnectionMap.Remove(pairToRemove);

        verticeConnectionMap.Add((newConnections, this));
    }

    public void destr()
    {
        StopAllCoroutines();
        Debug.Log("vertissceNO: destr" + _verticeTotal);
        _verticeTotal--;

        Destroy(gameObject);
        
    }
    private void OnMouseUp()
    {
        if (_mgPanel._miniGame.isPaused)
        {
            return;
        }
        var mg = _mgPanel?._miniGame as UntangleMG;
        mg.IncrementMoveTotal();
        mg.UpdateMoves();
        mg?.CheckIfSolved();
    
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
        //TODO MAYBE MOVE FOR CLEARER SOLUTION NOT URGENT
        _edges.ForEach(edge => edge.SetLineColor(Color.green));
    }
    
}
