using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class UntangleMG : MiniGame
{
    public override void Initialize(WindowSize hackWindowDimensions, GameWindow currentWindow)
    {
        ///TODO merge main minigame inits
        _gameWindow = currentWindow;
        _hackWindowDimensions = hackWindowDimensions;

    }

    public override void StartMinigame()
    {
        InstantiateVertices();
    }

    public override void EndMinigame()
    {
        StopAllCoroutines();
    }

    public override void RetryMinigame()
    {
        
    }
    //===========
    private void InstantiateVertices()
    {
        var tmpObject = new GameObject("LVertice");
        var lVertice = tmpObject.AddComponent<Vertice>();
        lVertice.transform.localPosition =
            new Vector3(-5f, lVertice.transform.position.y, -2);
        
        var tmpObject2 = new GameObject("RVertice");
        var rVertice = tmpObject2.AddComponent<Vertice>();
        rVertice.transform.localPosition =
            new Vector3(0f, lVertice.transform.position.y, -2);
        
        tmpObject = new GameObject("Edge");
        var lEdge = tmpObject.AddComponent<Edge>();
        lEdge.transform.localPosition =
            new Vector3(0f, lVertice.transform.position.y, -1);
        
        // var rEdge = tmpObject.AddComponent<Edge>();
        rVertice.Initialize(_gameWindow);
        lVertice.Initialize(_gameWindow);
        
        rVertice.addEdge(lEdge);
        lVertice.addEdge(lEdge);
        lEdge.Initialize(_gameWindow, lVertice,rVertice);
        

    }
}
