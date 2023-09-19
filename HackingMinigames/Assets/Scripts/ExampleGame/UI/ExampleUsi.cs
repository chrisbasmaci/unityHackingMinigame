
    public class ExampleUsi :UIPanel
    {
        public SettingsSlider exampleSlider;

        public void InitSlider(SettingsSlider slider)
        {
            exampleSlider = slider;
            ExampleSettings settings = (ExampleSettings)GameWindow.MinigamePanel._miniGame.Settings;
            slider.Initialize("Slider", settings.CurrentPuzzleTimer,5,10);
        }
    }
