using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Untangle
{
    public class Edge : MonoBehaviour
    {
        public (Vertice leftVertice, Vertice rightVertice) _verticePair;
        public LineRenderer _lineRenderer;
        private PolygonCollider2D _collider;
        private int _tangleCount;
        private MgPanel _mgPanel;
        public bool _isTangled;
        private bool _gettingStretched;

        public void setLayoutOrder(int order)
        {
            _lineRenderer.sortingOrder = order;
        }
        // Start is called before the first frame update
        public void Initialize(MgPanel mgPanel, (Vertice leftVertice, Vertice rightVertice) verticePair)
        {
            _mgPanel = mgPanel;
            _verticePair = verticePair;

            _collider = gameObject.AddComponent<PolygonCollider2D>();
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;

            _lineRenderer.startWidth = verticePair.leftVertice.GetEdgeThickness();
            _lineRenderer.endWidth = verticePair.rightVertice.GetEdgeThickness();
            _lineRenderer.material = new Material(Shader.Find("UI/Default"));
            _lineRenderer.sortingLayerName = "GameWindow";
            _lineRenderer.sortingOrder = mgPanel.gameWindow.CurrentSortingLayer;
            SetLineColor(Color.green);


            var rigidbdy = gameObject.AddComponent<Rigidbody2D>();
            rigidbdy.bodyType = RigidbodyType2D.Dynamic;
            rigidbdy.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

            rigidbdy.constraints = RigidbodyConstraints2D.FreezeAll;
            _collider.isTrigger = false;
            stretchEdge();

        }

        public void SetLineColor(Color color)
        {
            _isTangled = color == Color.red;
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            _lineRenderer.colorGradient = gradient;
            _lineRenderer.material.color = color;
        }

        // Update is called once per frame
        public void stretchEdge()
        {
            if (_gettingStretched) {
                return;
            }
            _gettingStretched = true;
            // Debug.Log("Stretch Edge");
            Vector3 startPoint = transform.InverseTransformPoint(_verticePair.leftVertice._rect.transform.position);
            Vector3 endPoint = transform.InverseTransformPoint(_verticePair.rightVertice._rect.transform.position);


            startPoint.z = 0;
            endPoint.z = 0;
            _lineRenderer.SetPosition(0, startPoint);
            _lineRenderer.SetPosition(1, endPoint);



            UpdateColliderPos();
            _gettingStretched = false;

        }

        public Vertice OtherVertice(Vertice vertice)
        {
            if (_verticePair.leftVertice == vertice)
            {
                return _verticePair.rightVertice;
            }
            else if (_verticePair.rightVertice == vertice)
            {
                return _verticePair.leftVertice;
            }

            Debug.Assert(false);
            return null;
        }

        private void OnCollisionEnter2D(Collision2D overlappingCollider)
        {
            // CheckIfTangled();
            LineRenderer otherLine = overlappingCollider.gameObject.GetComponent<LineRenderer>();
            if (!otherLine)
            {
                return;
            }else if (otherLine.sortingOrder != _mgPanel.gameWindow.CurrentSortingLayer)
            {
                return;
            }

            // Store references to the edges for readability and performance
            var leftEdges = _verticePair.leftVertice.Edges();
            var rightEdges = _verticePair.rightVertice.Edges();
            var overlappingObjectName = overlappingCollider.gameObject.name;

            if (!leftEdges.Exists(edge => edge.gameObject.name == overlappingObjectName)
                && !rightEdges.Exists(edge => edge.gameObject.name == overlappingObjectName))
            {
                SetLineColor(Color.red);
                _tangleCount++;
                var mg = _mgPanel?._miniGame as UntangleMG;
                mg?.TangledEdges.Add(this);
            }
        }

        private void OnCollisionExit2D(Collision2D overlappingCollider)
        {
            LineRenderer otherLine = overlappingCollider.gameObject.GetComponent<LineRenderer>();
            if (!otherLine)
            {
                return;
            }else if (otherLine.sortingOrder != _mgPanel.gameWindow.CurrentSortingLayer)
            {
                return;
            }
            // Store references to the edges for readability and performance
            var leftEdges = _verticePair.leftVertice.Edges();
            var rightEdges = _verticePair.rightVertice.Edges();
            var overlappingObjectName = overlappingCollider.gameObject.name;

            if (!leftEdges.Exists(edge => edge.gameObject.name == overlappingObjectName)
                && !rightEdges.Exists(edge => edge.gameObject.name == overlappingObjectName))
            {
                _tangleCount--;
                if (_tangleCount == 0)
                {
                    Debug.Log(gameObject.name + " has no more tangles");
                    SetLineColor(Color.green);
                    var mg = _mgPanel?._miniGame as UntangleMG;
                    mg?.TangledEdges.Remove(this);
                }
            }
        }
        
        private void UpdateColliderPos()
        {
            Debug.Log("Stretch Edge 2");

            var l = _verticePair.leftVertice.transform.position;
            var r = _verticePair.rightVertice.transform.position;

            var colliderPoints = CalculateColliderPoints();

            _collider.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
            // _collider.SetPath(0, cpoints);
        }

        private List<Vector2> CalculateColliderPoints()
        {
            Debug.Log("Stretch Edge3");

            //Get All positions on the line renderer
            Vector3[] positions = GetPositions();

            //Get the Width of the Line
            float width = _lineRenderer.startWidth;

            //check if vertical
            float div = positions[1].x - positions[0].x;
            if (div == 0)
            {
                div = 0.01f;
            }

            //m = (y2 - y1) / (x2 - x1)
            float m = (positions[1].y - positions[0].y) / div;
            float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
            float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

            //Calculate the Offset from each point to the collision vertex
            Vector3[] offsets = new Vector3[2];
            offsets[0] = new Vector3(-deltaX, deltaY);
            offsets[1] = new Vector3(deltaX, -deltaY);

            //Generate the Colliders Vertices
            List<Vector2> colliderPositions = new List<Vector2>
            {
                positions[0] + offsets[0],
                positions[1] + offsets[0],
                positions[1] + offsets[1],
                positions[0] + offsets[1]
            };

            return colliderPositions;
        }

        public Vector3[] GetPositions()
        {
            Debug.Log("Stretch Edge 4");

            Vector3[] positions = new Vector3[_lineRenderer.positionCount];
            _lineRenderer.GetPositions(positions);
            return positions;
        }

        private void CheckIfTangled()
        {
            // Get the PolygonCollider2D attached to this GameObject


            // Create a ContactFilter2D to filter overlapping colliders
            ContactFilter2D contactFilter = new ContactFilter2D();

            // Check if the polygon collider is currently overlapping with other colliders
            Collider2D[] overlappingColliders = new Collider2D[10]; // Adjust the array size as needed
            int count = _collider.OverlapCollider(contactFilter, overlappingColliders);

            foreach (Collider2D overlappingCollider in overlappingColliders)
            {
                if (overlappingCollider && !_verticePair.leftVertice.Edges()
                                            .Find(edge => edge.gameObject.name == overlappingCollider.gameObject.name)
                                        && !_verticePair.rightVertice.Edges().Find(edge =>
                                            edge.gameObject.name == overlappingCollider.gameObject.name))
                {
                    SetLineColor(Color.red);
                    return; // Return true if tangled
                }
            }

            SetLineColor(Color.green);
        } // Return false if not tangled



    }
}