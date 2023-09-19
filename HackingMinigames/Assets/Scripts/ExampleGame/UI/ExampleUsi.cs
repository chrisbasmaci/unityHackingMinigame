
    public class ExampleUsi : SettingsPanel
    {
        public SettingsSlider exampleSlider;

        public override void InitSliders()
        {
            exampleSlider =  Helpers.PrefabHandler.AddSliderPrefab(gameObject, "ExampleSlider");;
            ExampleSettings settings = (ExampleSettings)GameWindow.MinigamePanel._miniGame.Settings;
            exampleSlider.Initialize("Slider", settings.CurrentPuzzleTimer,5,10);
        }
    }
