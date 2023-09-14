using UnityEngine;

namespace Helpers
{
    public class PrefabHandler : MonoBehaviour
    {
        private GameObject _settingText =Resources.Load<GameObject>("UI_Prefabs/TextPrefab") ;

        public static SettingsSlider AddSliderPrefab(GameObject parentObject,
                                                string name, int startingValue = 0, int minValue= 0, int maxValue= 10)
        {
            GameObject _settingSlider =Resources.Load<GameObject>("UI_Prefabs/SliderPrefab") ;

            var newSlider = Instantiate(_settingSlider, parentObject.transform, false).
                GetComponent<SettingsSlider>();
            
            // newSlider.Initialize(name, startingValue, minValue, maxValue);
            return newSlider;

        }
    }
}