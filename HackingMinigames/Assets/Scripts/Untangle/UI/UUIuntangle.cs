
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class UUIuntangle :UIPanel
    {
        [SerializeField]public  Button leftButton;
        [SerializeField]private Button rightButton;
        [SerializeField]public TMP_Text movesText;

        public override void Initialize(GameWindow gameWindow){
            Debug.Log("BUTTON");
            Debug.Log("added listener");
            leftButton.onClick.AddListener(gameWindow.BackButton);
        }

        public override void ResetPanel()
        {
            ResetMoves();
        }

        public void ResetMoves()
        {
            movesText.text = "Moves: " + 0;

        }
        
        public void UpdateMoves(int moveCount)
        {
            movesText.text = "Moves: " + moveCount;
        }
    }
