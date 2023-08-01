using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.Serialization;

public class ButtonManager : MonoBehaviour
{
    // Add an identifier for each button
    [SerializeField] private string normalHackSceneName;
    [SerializeField] private string normalHackGameSceneName;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject navigationPanel;
    [SerializeField] private GameObject hackPanel;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject uuperGUI;

    [FormerlySerializedAs("relativeCanvasProportions")] [SerializeField]
    private GameCanvas gameCanvas;

    [SerializeField] private GameWindow _gameWindow;
    [SerializeField] Slider tileSlider;
    [SerializeField] Slider timeSlider;

    //Toggles

    public void NormalHack()
    {
        Debug.Log("Normal Hack Button Pressed");
        SceneManager.LoadScene(normalHackSceneName, LoadSceneMode.Single);
    }

    public void NormalHackStart()
    {
        if (gameCanvas == null)
            Debug.Log("FAIL");

        Debug.Log("Hack Start Button Pressed");
        // SceneManager.LoadScene(normalHackGameSceneName, LoadSceneMode.Single);
        settingsPanel.SetActive(false);
        navigationPanel.SetActive(false);
        hackPanel.SetActive(true);
        questionPanel.SetActive(true);
        uuperGUI.SetActive(true);
        StartCoroutine(gameCanvas.ChangePaddingWithAnimation(_gameWindow, true));
        //wait one second

    }

    public void TimeAmountSlider()
    {
        Game.Instance.defaultPuzzleTime = (int)timeSlider.value;
    }

    public void TileAmountSlider()
    {
        Game.Instance.defaultTileAmount = (int)tileSlider.value;
    }

    public void mainMenu()
    {
        Debug.Log("Normal Hack Button Pressed");
        SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
    }

    public void backToSettings()
    {
        StopAllCoroutines();
        _gameWindow.questionTextFieldObject.SetActive(true);
        _gameWindow.streakText.text = "Streak: 0";
        _gameWindow.questionTextField.text = " ";
        _gameWindow.questionInputField.text = " ";

        _gameWindow._miniGame._puzzleTimer.reset_timer();
        _gameWindow._miniGame.destr();

        hackPanel.SetActive(false);
        questionPanel.SetActive(false);
        uuperGUI.SetActive(false);

        settingsPanel.SetActive(true);
        navigationPanel.SetActive(true);
        StartCoroutine(gameCanvas.ChangePaddingWithAnimation(_gameWindow));

    }

    public void OnInvertToggleValueChanged(bool isOn)
    {
        Debug.Log("Invert Toggle " + " isOn: " + isOn);
        Game.Instance.invertToggle = isOn;
    }

    public void OnQuestionFirstToggleValueChanged(bool isOn)
    {
        Debug.Log("Invert Toggle " + " isOn: " + isOn);
        Game.Instance.questionFirstToggle = isOn;
    }

    public void RetryPuzzle()
    {
        _gameWindow.stopGameCoroutines();
        _gameWindow.Retry();
        
    }


}
