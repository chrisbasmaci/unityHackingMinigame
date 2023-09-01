
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class UUIuntangle :UIPanel
    {
        [SerializeField]public  Button leftButton;
        [SerializeField]private Button rightButton;
        [SerializeField]public TMP_Text movesText;
        private GameObject _upperPanel;

        public override void Initialize(GameObject gameCanvas){
            Debug.Log("BUTTON");
            _upperPanel = gameCanvas;            
            Debug.Log("added listener");
            leftButton.onClick.AddListener(ButtonManager.Instance.backToSettings);
        }

        public void ResetUI()
        {
            movesText.text = "Moves: " + 0;

        }
        
        public void UpdateMoves(int moveCount)
        {
            movesText.text = "Moves: " + moveCount;
        }
    }
