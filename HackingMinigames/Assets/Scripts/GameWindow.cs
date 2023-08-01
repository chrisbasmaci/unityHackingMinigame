using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{
    [SerializeField]GameCanvas gameCanvas;
    public WindowSize _windowSize;
    public MiniGame _miniGame;
    [SerializeField]public TMP_InputField questionInputField;
    [SerializeField]public GameObject questionTextFieldObject;
    [SerializeField]public TMP_Text questionTextField;
    [SerializeField]public TMP_Text streakText;
    [SerializeField] public Image loadingbarTimer;
    private bool _retryAvailable =true;
    

    // Start is called before the first frame update
    void Start()
    {
        _windowSize = gameCanvas.HackWindowSize;
        questionInputField.onValueChanged.AddListener(UpdateSavedText);
        // questionTextField = questionTextFieldObject.GetComponent<TMP_Text>();
        questionTextField.text = " ";
    }

    public void Initialize(WindowSize windowSize)
    {
        _windowSize = windowSize;
        gameCanvas.gameWindow.StartMinigame();
        questionTextField = questionTextFieldObject.GetComponent<TMP_Text>();
        //questionInputField.Select();
    }
    public void StartMinigame()
    {
        _miniGame = this.AddComponent<MiniGame>();
        _miniGame.Initialize(_windowSize, this);
        //questionInputField.Select();

    }
    public string SetQuestion(int tileAmount,List<Card> cardDeck)
    {
        questionInputField.Select();
        List<int> wanted_tiles = new List<int>();
        questionTextField.text = RandomFactory.getRandomQuestion(2, tileAmount,ref wanted_tiles);
        Debug.Log("NEW QUESTION IS: "+questionTextField.text);

        cardDeck.ForEach(card =>
        {   
            card.isWanted = wanted_tiles.Contains(card.cardOrder);
            if (card.isWanted)
            {
                card.wantedOrder = wanted_tiles.IndexOf(card.cardOrder);
            }
        });
        return questionTextField.text;
    }
public bool CheckAnswer(string ans) {
    if (questionInputField.text == ans)
    {
        questionTextField.text = " ";
        questionInputField.text = "";
        return true;

    } 
    return false;
} 
private void UpdateSavedText(string newText)
{
    questionInputField.text = newText;
    // Debug.Log(questionInputField.text);
}

    public void Retry()
    {

        StopAllCoroutines();

        questionTextFieldObject.SetActive(true);
        streakText.text = "Streak: 0";
        questionTextField.text = "retry";
        questionInputField.text = "";
        // questionTextFieldObject.SetActive(false);

        _miniGame._puzzleTimer.reset_timer();
        // _miniGame.destr();

        // if (_miniGame.flipCardBacks())
        // {
        //     yield return new WaitForSeconds(1);
        // }
        
        // yield return new WaitForSeconds(1);
        _miniGame.ToggleCards(true, true);

    }
    public void stopGameCoroutines()
    {
        _miniGame.stopAllCardRoutines();
        StopAllCoroutines();
    }

}
