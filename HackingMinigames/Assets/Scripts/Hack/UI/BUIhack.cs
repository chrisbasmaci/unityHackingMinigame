using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hack;
using Unity.VisualScripting;
using UnityEngine.Events;

public class BUIhack :UIPanel
{

    [SerializeField]private  Button _leftButton;
    [SerializeField]private Button _rightButton;
    [SerializeField]public TMP_InputField questionInputField;
    [SerializeField]public GameObject questionTextFieldObject;
    [SerializeField]public TMP_Text questionTextField;
    [SerializeField]public Image loadingbarTimer;
    private TMP_Text _inputFieldPlaceholder;

    public override void Initialize(GameWindow gameWindow){
        _inputFieldPlaceholder = questionInputField.placeholder.GetComponent<TMP_Text>();
        ResetPanel();
    }

    public override void ResetPanel()
    {
        questionTextField.text = " ";
        questionInputField.text = "";
        _inputFieldPlaceholder.text = "example: blue rectangle";
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
    public void InitializeLeftButton(UnityAction call)
    {
        _leftButton.onClick.AddListener(call);
    }

    public void ShowAnswer(string solution)
    {
        _inputFieldPlaceholder.text = solution;
    }
    
    public void InitializeRightButton(UnityAction call)
    {
        _rightButton.onClick.AddListener(call);
    }


}
