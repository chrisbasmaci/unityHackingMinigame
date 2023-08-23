using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class BUIhack :UIPanel
{

    [SerializeField]public TMP_InputField questionInputField;
    [SerializeField]public GameObject questionTextFieldObject;
    [SerializeField]public TMP_Text questionTextField;
    [SerializeField]public Image loadingbarTimer;
    private GameObject _bottomPanel;

    public override void Initialize(GameObject bottomPanel, float height){
        _bottomPanel = bottomPanel;
        var rectTransform = _bottomPanel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, height);
    }

    private void aaa()
    {
        Debug.Log("AAAAA");

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

}
