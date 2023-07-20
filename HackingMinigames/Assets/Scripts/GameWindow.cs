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
    [SerializeField]TMP_InputField questionInputField;
    [SerializeField]private GameObject questionTextFieldObject;
    [SerializeField]public TMP_Text questionTextField;
    [SerializeField]public TMP_Text streakText;
    [SerializeField] public Image loadingbarTimer;

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
        questionInputField.Select();
    }
    public void StartMinigame()
    {
        _miniGame = this.AddComponent<MiniGame>();
        _miniGame.Initialize(_windowSize, this);
        questionInputField.Select();

    }
    public string SetQuestion(int tileAmount,List<Card> cardDeck)
    {
        questionInputField.Select();
        Debug.Log("sq");
        List<int> wanted_tiles = new List<int>();
        questionTextField.text = RandomFactory.getRandomQuestion(2, tileAmount,ref wanted_tiles);
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

// Update is called once per frame
    void Update()
    {
        
    }
}
