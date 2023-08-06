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
    private MiniGame _miniGame;
    private MinigameType _minigameType;
    [SerializeField]public TMP_InputField questionInputField;
    [SerializeField]public GameObject questionTextFieldObject;
    [SerializeField]public TMP_Text questionTextField;
    [SerializeField]public TMP_Text streakText;
    [SerializeField]public Image loadingbarTimer;
    

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
        questionTextField = questionTextFieldObject.GetComponent<TMP_Text>();
        //questionInputField.Select();
    }
    public void StartMinigame(MinigameType minigameType)
    {
        switch (minigameType)
        {
            case MinigameType.HACK:
                minigameType = MinigameType.HACK;
                _miniGame = this.AddComponent<HackingMG>();
                break;
            case MinigameType.UNTANGLE:
                minigameType = MinigameType.UNTANGLE;
                _miniGame = this.AddComponent<UntangleMG>();
                break;
            default:
                Debug.Log("NOT IMPLEMENTED");
                break;
        }
        _miniGame.Initialize(_windowSize, this);
        _miniGame.StartMinigame();
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
        _miniGame.RetryMinigame();
    }
    public void stopGameCoroutines()
    {
        _miniGame.EndMinigame();
        StopAllCoroutines();
    }

}
