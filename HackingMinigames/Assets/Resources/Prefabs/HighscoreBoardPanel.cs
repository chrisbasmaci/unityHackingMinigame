
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class HighscoreBoardPanel : MonoBehaviour
    {
        [SerializeField]private TMP_Text highScore;
        private GameObject _upperPanel;
        

        public void ResetUI()
        {
            highScore.text = "No Highscore Yet!";

        }
        
        public void UpdateHighscore(string mode, int score)
        {
            highScore.text =mode+ " : " + score;
        }
    }
