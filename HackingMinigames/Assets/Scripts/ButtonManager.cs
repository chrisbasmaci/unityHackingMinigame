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
    [SerializeField] private string normalHackSceneName;
    [SerializeField] private string normalHackGameSceneName;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject navigationPanel;
    [SerializeField] private GameObject hackPanelGobj;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject uuperGUI;

    [FormerlySerializedAs("relativeCanvasProportions")] [SerializeField]
    private GameCanvas gameCanvas;

    [FormerlySerializedAs("_gameWindow")] [SerializeField] private MgPanel mgPanel;
    [SerializeField] Slider tileSlider;
    [SerializeField] Slider timeSlider;
    private MinigameType _selectedMinigame;

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


    public void SwitchToUntangleScene(){
        _selectedMinigame = MinigameType.UNTANGLE;
        SceneManager.LoadScene("untangleScene", LoadSceneMode.Single);
        
    }
    public void CardHackStart(){
        _selectedMinigame = MinigameType.UNTANGLE;

        StartCoroutine(HackCoroutine());
    }
    public void UntangleStart(){
        StartCoroutine(UntangleCoroutine());
    }

    public IEnumerator HackCoroutine(){
        Debug.Log("Hack Start Button Pressed");
        settingsPanel.SetActive(false);
        navigationPanel.SetActive(false);
        hackPanelGobj.SetActive(true);
        // questionPanel.SetActive(true);
        uuperGUI.SetActive(true);
        questionPanel.SetActive(true);
        yield return gameCanvas.ChangePaddingWithAnimation(mgPanel);
        yield return gameCanvas.gameWindow.StartMinigame(_selectedMinigame);
        //wait one second

    }
    public IEnumerator UntangleCoroutine(){
        Debug.Log("Untangle Button Pressed");
        settingsPanel.SetActive(false);
        navigationPanel.SetActive(false);
        hackPanelGobj.SetActive(true);
        // questionPanel.SetActive(true);
        uuperGUI.SetActive(true);
        questionPanel.SetActive(true);
        yield return gameCanvas.ChangePaddingWithAnimation(mgPanel);
        yield return gameCanvas.gameWindow.StartMinigame(Game.Instance.currentMg);

        //wait one second
    }
    public void TimeAmountSlider(){
        Game.Instance.defaultPuzzleTime = (int)timeSlider.value;
    }

    public void TileAmountSlider(){
        Game.Instance.defaultTileAmount = (int)tileSlider.value;
    }

    public void mainMenu(){
        Debug.Log("Normal Hack Button Pressed");
        SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
    }

    private int i =1;
    public void backToSettings(){
        Debug.Log("go back Pressed" + i++);

        StopAllCoroutines();
        
        gameCanvas.gameWindow.MinigamePanel.stopGameCoroutines();
        hackPanelGobj.SetActive(false);
        settingsPanel.SetActive(true);
        navigationPanel.SetActive(true);
        StartCoroutine(gameCanvas.gameWindow.InitPanels(250f,0f,100f));
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

    public void RetryPuzzle(){
        mgPanel.stopGameCoroutines();
        mgPanel.Retry();
    }




}
