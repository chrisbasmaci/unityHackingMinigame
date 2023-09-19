
    public class ExampleUsi : SettingsPanel
    {
        public SettingsSlider exampleSlider;

        public override void InitSliders()
        {
            ExampleSettings settings = (ExampleSettings)GameWindow.MinigamePanel._miniGame.Settings;

            exampleSlider =  Helpers.PrefabHandler.AddSliderPrefab(gameObject, "ExampleSlider");;
            exampleSlider.Initialize("Slider", settings.CurrentPuzzleTimer,5,10);
        }
    }
