
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class UUIuntangle :UIPanel
    {
        [SerializeField]public  Button leftButton;
        [SerializeField]private Button rightButton;
        [SerializeField]public TMP_Text movesText;

        public override void Initialize()
        {
            Debug.Log("BUTTON");
            // leftButton.onClick = new Button.ButtonClickedEvent();
            leftButton.onClick.AddListener(ButtonManager.Instance.backToSettings);
            rightButton.onClick.AddListener(aaa);
        }

        private void aaa()
        {
            Debug.Log("AAAAA");

        }
    }
