using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine.Assertions;

public class Edge : MonoBehaviour
{
    public (Vertice leftVertice, Vertice rightVertice) _verticePair;
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _collider;
    private bool _isMerged;
    private int _tangleCount;

    public bool IsMerged()
    {
        return _isMerged;
    }
    // Start is called before the first frame update
    void Start()
    {
        // var lVert = GameObject.Find("leftV").GetComponent<Vertice>();
        // var rVert = GameObject.Find("rightV").GetComponent<Vertice>();
        // _verticePair = (lVert, rVert);
    }

    public void Initialize(MgPanel mgPanel, Vertice leftVertice, Vertice rightVertice)
    {

        _collider = gameObject.AddComponent<PolygonCollider2D>();
        _verticePair = (leftVertice, rightVertice);
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        transform.SetParent(mgPanel.transform);
        transform.position = new Vector3(0f,0f,0f);
        
        _lineRenderer.startWidth = leftVertice.GetEdgeThickness();
        _lineRenderer.endWidth = rightVertice.GetEdgeThickness();
        _lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        SetLineColor(Color.green);

        var rigidbdy = gameObject.AddComponent<Rigidbody2D>();
        rigidbdy.bodyType = RigidbodyType2D.Dynamic;
        rigidbdy.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        rigidbdy.constraints = RigidbodyConstraints2D.FreezeAll;
        _collider.isTrigger = false;
        // rigidbdy.sleepMode = RigidbodySleepMode2D.NeverSleep;
        
        // rigidbdy.isKinematic = false;
        // rigidbdy.simulated = false;
        stretchEdge();
        
    }

    public void SetLineColor(Color color)
    {
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
        Vector3 startPoint = _verticePair.leftVertice.transform.position;
        Vector3 endPoint = _verticePair.rightVertice.transform.position;

        startPoint.z = 0;
        endPoint.z = 0;
        _lineRenderer.SetPosition(0, startPoint);
        _lineRenderer.SetPosition(1, endPoint);

        

        UpdateColliderPos();
        // CheckIfTangled();
        // _verticePair.rightVertice.Edges().ForEach(edge=> edge.CheckIfTangled());
        // _verticePair.leftVertice.Edges().ForEach(edge=> edge.CheckIfTangled());


    }

    private Vertice ConnectedTo(Vertice vertice)
    {
        if (_verticePair.leftVertice == vertice)
        {
            return _verticePair.rightVertice;
        }else if (_verticePair.rightVertice == vertice)
        {
            return _verticePair.leftVertice;
        }

        Debug.Assert(false);
        return null;
    }
    public Vertice OtherVertice(Vertice vertice){
        if(_verticePair.leftVertice == vertice)
        {
            return _verticePair.rightVertice;
        }else if(_verticePair.rightVertice == vertice)
        {
            return _verticePair.leftVertice;
        }
        Debug.Assert(false);
        return null;
    }
    public (Vertice leftVertice, Vertice rightVertice) SubEdge()
    {
        var leftVertice = _verticePair.leftVertice;
        var rightVertice = _verticePair.rightVertice;
        
        Debug.Log("leftVertice"+leftVertice.verticeNo+"weight: " + leftVertice.weight + 
                         "rightVertice"+rightVertice.verticeNo+"weight"+ rightVertice.weight);
        if (leftVertice.weight == 2)
        {
            leftVertice = ConnectedToChainMerge(leftVertice);
        }

        if (rightVertice.weight == 2)
        {
            rightVertice = ConnectedToChainMerge(rightVertice);
        }

        return (leftVertice, rightVertice);
    }
    private Vertice ConnectedToChainMerge(Vertice vertice)
    {
        var currentEdge = this;
        _isMerged = true;
        var startingConnection = vertice;

        var connection = startingConnection;
        do{
            Debug.Log("this vertice is not needed: " +vertice.verticeNo);
            connection = connection.ConnectedTo(ref currentEdge);
            currentEdge._isMerged = true;
            if (connection == startingConnection)
            {
                Debug.Log("2-Chain loop shouldn happen");
                Debug.Assert(false);
                return connection;
            }
        } while (connection.weight == 2);
        

        return connection;

    }

    // private void OnCollisionStay2D(Collision2D other)
    // {
    //     
    //
    // }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Trigger entered with: " + other.gameObject.name);
    //     _tangleCount++;
    //     SetLineColor(Color.red);
    //
    //
    // }

    private void OnCollisionEnter2D(Collision2D overlappingCollider)
    {
        // CheckIfTangled();
        
        if (!_verticePair.leftVertice.Edges().Find(edge => edge.gameObject.name == overlappingCollider.gameObject.name)
                                &&!_verticePair.rightVertice.Edges().Find(edge => edge.gameObject.name == overlappingCollider.gameObject.name))
        {
            SetLineColor(Color.red);
            _tangleCount++;
        }
    }

    private void OnCollisionExit2D(Collision2D overlappingCollider)
    {
        if (!_verticePair.leftVertice.Edges().Find(edge => edge.gameObject.name == overlappingCollider.gameObject.name)
            &&!_verticePair.rightVertice.Edges().Find(edge => edge.gameObject.name == overlappingCollider.gameObject.name))
        {
            _tangleCount--;
            if (_tangleCount == 0)
            {
                SetLineColor(Color.green);

            }
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //
    //     if (collision.collider != _collider 
    //         &&!_verticePair.leftVertice.Edges().Find(edge => edge.gameObject.name == collision.gameObject.name)
    //         &&!_verticePair.rightVertice.Edges().Find(edge => edge.gameObject.name == collision.gameObject.name))        {
    //         Debug.Log("Collision with: " + collision.gameObject.name);
    //         _tangleCount++;
    //         SetLineColor(Color.red);
    //     }
    //     Debug.Log(_tangleCount);
    //
    // }
    //
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //
    //     if (other.collider != _collider 
    //         &&!_verticePair.leftVertice.Edges().Find(edge => edge.gameObject.name == other.gameObject.name)
    //         &&!_verticePair.rightVertice.Edges().Find(edge => edge.gameObject.name == other.gameObject.name))        {
    //         Debug.Log("Collision with: " + other.gameObject.name);
    //         if (--_tangleCount <= 0)
    //         {
    //             SetLineColor(Color.green);
    //         }           
    //     }
    // }
    //
    private void UpdateColliderPos()
    {
        var l = _verticePair.leftVertice.transform.position;
        var r = _verticePair.rightVertice.transform.position;
        // Debug.Log(_verticePair+"l: "+l + "r: " +r);
        var newPoints = new List<Vector2> {
            l,
            r,
            r,
            l
        };
        var colliderPoints = CalculateColliderPoints();
        
        _collider.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        // _collider.SetPath(0, cpoints);
    }
    private List<Vector2> CalculateColliderPoints() {
        //Get All positions on the line renderer
        Vector3[] positions = GetPositions();

        //Get the Width of the Line
        float width = _lineRenderer.startWidth;

        //check if vertical
        float div = positions[1].x - positions[0].x;
        if (div ==0)
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
        List<Vector2> colliderPositions = new List<Vector2> {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return colliderPositions;
    }
    private List<Vector2> CalculateColliderPoints2() {
        // Get All positions on the line renderer
        Vector3[] positions = GetPositions();

        // Get the Width of the Line
        float width = _lineRenderer.startWidth;

        // Calculate the Offset from each point to the collision vertex
        Vector2 offset;

        // Check if the line is vertical
        if (Mathf.Approximately(positions[0].x, positions[1].x)) {
            offset = new Vector2(width / 2f, 0);
        }
        // Check if the line is horizontal
        else if (Mathf.Approximately(positions[0].y, positions[1].y)) {
            offset = new Vector2(0, width / 2f);
        }
        // Handle non-vertical and non-horizontal lines
        else {
            float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
            float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
            float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));
            offset = new Vector2(deltaX, deltaY);
        }

        // Convert the Vector2 offset to Vector3
        Vector3 offset3D = new Vector3(offset.x, offset.y, 0);

        // Generate the Colliders Vertices
        List<Vector2> colliderPositions = new List<Vector2> {
            (Vector2)(positions[0] + offset3D),
            (Vector2)(positions[1] + offset3D),
            (Vector2)(positions[1] - offset3D),
            (Vector2)(positions[0] - offset3D)
        };

        return colliderPositions;
    }

    public Vector3[] GetPositions() {
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
            if (overlappingCollider && !_verticePair.leftVertice.Edges().Find(edge => edge.gameObject.name == overlappingCollider.gameObject.name)
                &&!_verticePair.rightVertice.Edges().Find(edge => edge.gameObject.name == overlappingCollider.gameObject.name))
            {
                SetLineColor(Color.red);
                return; // Return true if tangled
            }
        }
        SetLineColor(Color.green);
    } // Return false if not tangled
    


}
