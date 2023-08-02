using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Edge : MonoBehaviour
{
    [SerializeField] private (Vertice leftVertice, Vertice rightVertice) _verticePair;
    private LineRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<BoxCollider2D>();
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        // var lVert = GameObject.Find("leftV").GetComponent<Vertice>();
        // var rVert = GameObject.Find("rightV").GetComponent<Vertice>();
        // _verticePair = (lVert, rVert);
    }

    public void Initialize(Vertice leftVertice, Vertice rightVertice)
    {
        _verticePair = (leftVertice, rightVertice);
    }

    // Update is called once per frame
    public void stretchEdge()
    {
        Vector3 startPoint = _verticePair.leftVertice.transform.position;
        Vector3 endPoint = _verticePair.rightVertice.transform.position;

        _lineRenderer.SetPosition(0, startPoint);
        _lineRenderer.SetPosition(1, endPoint);
    }
}
