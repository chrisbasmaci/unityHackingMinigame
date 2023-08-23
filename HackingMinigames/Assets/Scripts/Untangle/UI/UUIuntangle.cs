
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

        public override void Initialize(GameObject gameCanvas, float height){
            Debug.Log("BUTTON");
            _upperPanel = gameCanvas;
            var rectTransform = _upperPanel.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, height);
            leftButton.onClick.AddListener(ButtonManager.Instance.backToSettings);
            Debug.Log("added listener");
            rightButton.onClick.AddListener(aaa);
        }

        private void aaa(){
            Debug.Log("AAAAA");
            Debug.Log("vvvv");

        }
    }
