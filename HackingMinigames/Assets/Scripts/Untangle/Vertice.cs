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
using Helpers;
namespace Untangle
{
    public class Vertice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {

        private MgPanel _mgPanel;
        private Vector3 currentPos;
        private Vertex _solvedVertex;
        private Vertex _unsolvedVertex;
        public List<Vertice> connectedVertices;
        private List<Edge> _edges;
        public int weight;
        public int verticeNo;
        private Vector2 _difference = Vector2.zero;
        private float _verticeScale;

        private Image _vertexImage;
        private Collider2D _collider;
        public RectTransform _rect;
        public Rect BoundsRect => _mgPanel.gameObject.GetComponent<RectTransform>().rect;

        
        public List<Edge> Edges()
        {
            return _edges;
        }

        public float GetEdgeThickness()
        {
            return _verticeScale / 5;
        }

        public void Initialize(MgPanel mgPanel, ref Polygon polygon, Vertex solvedVertex, Vertex unsolvedVertex,
            float verticeScale,int  verticeNumber)
        {
            EventSystem.current.pixelDragThreshold = 0;

            _rect = gameObject.AddComponent<RectTransform>();
            ComponentHandler.SetAnchorToStretch(gameObject);
            _rect.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            _verticeScale = _rect.transform.localScale.x;
            connectedVertices = new List<Vertice>();
            verticeNo = verticeNumber;
            _edges = new List<Edge>();

            SetupTriggers();

            _vertexImage = ComponentHandler.AddImageComponent(gameObject, Game.Instance.shapeSheet[0]);
            _mgPanel = mgPanel;
            _rect.transform.localPosition =
                new Vector3((float)unsolvedVertex.X, (float)unsolvedVertex.Y, -2);
            _solvedVertex = solvedVertex;
            _unsolvedVertex = unsolvedVertex;
            _rect = gameObject.GetComponent<RectTransform>();

            polygon.Add(_solvedVertex);
            
        }

        private void SetupTriggers()
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            // EventHandler.AddTrigger(eventTrigger, EventTriggerType.PointerClick, OnBeginDrag);
            // EventHandler.AddTrigger(eventTrigger, EventTriggerType.Drag, OnMouseDrag);
            // EventHandler.AddTrigger(eventTrigger, EventTriggerType.PointerUp, OnMouseUp);

        }

        public void addEdge(Edge edge, ref List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap)
        {
            var oldConnections = new List<Vertice>(connectedVertices);
            _edges.Add(edge);
            weight++;
            connectedVertices.Add(edge.OtherVertice(this));
            UpdateVertexConnectionMap(ref verticeConnectionMap, oldConnections, connectedVertices);
        }

        Vector2 offset = Vector2.zero;

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("onbegin");
            offset = _rect.anchoredPosition - eventData.position / _rect.GetComponentInParent<Canvas>().scaleFactor;
        }

        void Awake()
        {
            Debug.Log("onbegin");
            if (_rect != null)
            {
                Debug.Log("xyz");
                ObjectHandler.ClampPositionLocal(_rect, BoundsRect);
            }

        }

        public void OnDrag(PointerEventData eventData)
        {
            float scaleFactor = _rect.GetComponentInParent<Canvas>().scaleFactor;
            Vector2 newPosition = eventData.position / scaleFactor + offset;

            _rect.anchoredPosition = newPosition;
            ObjectHandler.ClampPositionLocal(_rect, BoundsRect);
            stretchAllEdges();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_mgPanel._miniGame.isPaused)
            {
                return;
            }

            var mg = _mgPanel?._miniGame as UntangleMG;
            mg.IncrementMoveTotal();
            mg.UpdateMoves();
            Debug.Log("Gonna check solve");
            mg?.CheckIfSolved();
        }
        // private (float minX, float maxX, float minY, float maxY) GetPanelBounds()
        // {
        //     float minX = _mgPanel.panelBounds.LeftBorder;
        //     float maxX = _mgPanel.panelBounds.RightBorder;
        //     float minY = _mgPanel.panelBounds.BottomBorder;
        //     float maxY = _mgPanel.panelBounds.TopBorder;
        //     return (minX, maxX, minY, maxY);
        // }
        
        public void stretchAllEdges()
        {
            if (_edges.Count != 0)
            {
                _edges.ForEach(edge => edge.stretchEdge());

            }
        }

        //for weight 2 vertices get the other edge
        public Vertice ConnectedTo(ref Edge edge)
        {
            Debug.Assert(_edges.Count == 2);
            var leftEdge = _edges[0];
            var rightEdge = _edges[1];

            if (leftEdge == edge)
            {
                edge = rightEdge;
                return rightEdge.OtherVertice(this);
            }
            else if (rightEdge == edge)
            {
                edge = leftEdge;
                return leftEdge.OtherVertice(this);
            }

            Debug.Assert(false);
            return null;
        }


        public void UpdateVertexConnectionMap
        (ref List<(List<Vertice> connections, Vertice vertex)> verticeConnectionMap, List<Vertice> oldConnections,
            List<Vertice> newConnections)
        {

            Debug.Log("cc: " + oldConnections.Count);
            var pairToRemove = (oldConnections, this);
            verticeConnectionMap.Remove(pairToRemove);
            pairToRemove = (newConnections, this);
            verticeConnectionMap.Remove(pairToRemove);

            verticeConnectionMap.Add((newConnections, this));
        }

        public void destr()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }



        public IEnumerator MoveToSolution()
        {
            // MoveSprite(new Vector2((float)_solvedVertex.X, (float)_solvedVertex.Y), false);
            var test = _mgPanel.gameWindow.middleContainer.GetComponent<RectTransform>().rect;
            Debug.Log("test width: "+test.width);
            Debug.Log("test height: "+test.height);
            yield return null;
            yield return ObjectHandler.MoveCoroutine
                (gameObject, new Vector2((float)_solvedVertex.X, 
                    (float)_solvedVertex.Y),
                    test.width, 
                    test.height, 
                    0.8f,stretchAllEdges);
            //TODO MAYBE MOVE FOR CLEARER SOLUTION NOT URGENT
            _edges.ForEach(edge => edge.SetLineColor(Color.green));
        }

    }
}