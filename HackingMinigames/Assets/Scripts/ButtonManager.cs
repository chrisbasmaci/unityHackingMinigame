using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.Serialization;

/// <summary>
/// TODO MAKE THIS A SINGLETON
/// </summary>
public class ButtonManager : MonoBehaviour
{
    // Add an identifier for each button
    [FormerlySerializedAs("relativeCanvasProportions")] [SerializeField]
    private GameCanvas gameCanvas;



    //Toggles
    private static ButtonManager _instance;

    private ButtonManager() { } // Private constructor to prevent instantiation from outside

    public static ButtonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ButtonManager>(); // Find the existing instance in the scene
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<ButtonManager>();
                    singletonObject.name = "ButtonManager (Singleton)";
                }
            }

            return _instance;
        }
    }
    
    public void MinigameStart(){
        StartCoroutine(MinigameCoroutine());
    }
    public IEnumerator MinigameCoroutine(){
        Debug.Log("Untangle Button Pressed");
        gameCanvas.gameWindow.ShowGame();
        yield return gameCanvas.ChangePaddingWithAnimation();
        yield return gameCanvas.gameWindow.StartMinigame();
    }

    public void mainMenu(){
        Debug.Log("Normal Hack Button Pressed");
        SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
    }

    public void backToSettings(){
        StopAllCoroutines();
        
        gameCanvas.gameWindow.MinigamePanel.stopGameCoroutines();
        gameCanvas.gameWindow.ShowSettings();
        var upperLE = gameCanvas.gameWindow.upperContainer.gameObject.GetComponent<LayoutElement>();
        var lowerLE = gameCanvas.gameWindow.bottomContainer.gameObject.GetComponent<LayoutElement>();
        upperLE.flexibleHeight = 100;
        lowerLE.flexibleHeight = 1;

        StartCoroutine(gameCanvas.ChangePaddingWithAnimation());

    }

    public void OnInvertToggleValueChanged(bool isOn){
        Debug.Log("Invert Toggle " + " isOn: " + isOn);
        Game.Instance.invertToggle = isOn;
    }

    public void OnQuestionFirstToggleValueChanged(bool isOn){
        Debug.Log("Invert Toggle " + " isOn: " + isOn);
        Game.Instance.questionFirstToggle = isOn;
    }
    




}
