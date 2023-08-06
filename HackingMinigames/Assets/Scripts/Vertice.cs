using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vertice : MonoBehaviour
{
    [SerializeField] private List<Edge> _edges;

    private Vector2 _difference = Vector2.zero;
    private BoxCollider2D _collider;
    private SpriteRenderer _renderer;
    private GameWindow _gameWindow;
    private RectTransform _rect;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize(GameWindow gameWindow)
    {
        _edges = new List<Edge>();
        _collider =gameObject.AddComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        _renderer = gameObject.AddComponent<SpriteRenderer>();
        _rect = gameObject.AddComponent<RectTransform>();
        _renderer.sprite = Game.Instance.shapeSheet[0];
        _gameWindow = gameWindow;
        transform.SetParent(_gameWindow.transform);
    }

    public void addEdge(Edge edge)
    {
        _edges.Add(edge);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("pressed");
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
        Debug.Log("minx: "+ minX +"_rect.position: "+_rect.localPosition.x+"leftBorderWorldPosition" +leftBorderWorldPosition);

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

}
