
    public class JumpChessUsi : SettingsPanel
    {
        public SettingsSlider JumpChessSlider;

        public override void InitSliders()
        {
            JumpChessSettings settings = (JumpChessSettings)GameWindow.MinigamePanel._miniGame.Settings;

            JumpChessSlider =  Helpers.PrefabHandler.AddSliderPrefab(gameObject, "JumpChessSlider");;
            JumpChessSlider.Initialize("Slider", settings.CurrentPuzzleTimer,5,10);
        }
    }
