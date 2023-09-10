
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
        public void ResetUI((string mode, int? record) lastRecord)
        {
            if (lastRecord.record == null) {
                highScore.text = lastRecord.mode + " No Highscore Yet!";
            }else {
                UpdateHighscore(lastRecord.mode, (int)lastRecord.record);
            }
        }
        
        public void UpdateHighscore(string mode, int score)
        {
            highScore.text =mode+ " : " + score;
        }
    }
