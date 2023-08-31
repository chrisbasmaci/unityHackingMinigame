using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MgPanel : MonoBehaviour
{
    // [SerializeField]GameCanvas gameCanvas;
    [SerializeField]public GameWindow gameWindow;
    [NonSerialized] private RectTransform _panelRect;
    [NonSerialized]public WindowSize panelBounds;
    [NonSerialized]public MiniGame _miniGame;
    [NonSerialized]private MinigameType _minigameType;





    public void stopGameCoroutines()
    {
        // Destroy(gameWindow.UUIpanel.gameObject);
        // Destroy(gameWindow.BUIPanel.gameObject);
        
        _miniGame.EndMinigame();
        StopAllCoroutines();
    }

    public IEnumerator AddMinigameScript()
    {
        switch (Game.Instance.currentMg)
        {
            case MinigameType.HACK:
                _miniGame = gameObject.AddComponent<HackingMG>();

                break;
            case MinigameType.UNTANGLE:
                _miniGame = gameObject.AddComponent<UntangleMG>();
                break;
            default:
                _miniGame = gameObject.AddComponent<HackingMG>();
                break;
        }
        yield return _miniGame.Initialize(this);
    }

    public void StartMinigame()
    {
        StartCoroutine(_miniGame.StartMinigame());
    }
 
    
}
