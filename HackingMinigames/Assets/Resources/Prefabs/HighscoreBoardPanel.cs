
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class HighscoreBoardPanel : UIPanel
    {
        [SerializeField]private TMP_Text highScore;
        private GameObject _upperPanel;
        private GameWindow _gameWindow;
        

        public override void Initialize(GameWindow gameWindow) {
            _gameWindow = gameWindow;
        }
        
        public void UpdateHighscore(string highscore)
        {
            highScore.text = highscore;
        }
    }
