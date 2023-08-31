
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

        private void aaa(){
            Debug.Log("AAAAA");
            Debug.Log("vvvv");

        }
    }
