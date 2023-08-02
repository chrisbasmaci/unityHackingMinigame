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
    
    // Start is called before the first frame update
    void Start()
    {
        _collider =gameObject.AddComponent<BoxCollider2D>();
        _renderer = gameObject.AddComponent<SpriteRenderer>();
        _renderer.sprite = Game.Instance.shapeSheet[0];
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        _difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    
    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _difference;
        stretchAllEdges();
    }

    private void stretchAllEdges()
    {
        _edges.ForEach(edge => edge.stretchEdge());
    }

}
