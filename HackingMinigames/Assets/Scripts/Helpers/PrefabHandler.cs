using UnityEngine;
using UnityEngine.UI;

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
        public static void CreateGrid(GameObject parentObject, int dimension)
        {
            Sprite squarePrefab =Resources.Load<Sprite>("Sprites/greenSquare") ;
            
            GridLayoutGroup gridLayout = parentObject.AddComponent<GridLayoutGroup>();
        
            // Set the cell size, spacing, etc. as needed
            gridLayout.cellSize = new Vector2(100, 100); // Example size

            for (int i = 0; i < dimension * dimension; i++)
            {
                GameObject child = new GameObject("GridChild_" + i);
                child.AddComponent<Image>().sprite = squarePrefab; // Add Image component
                child.transform.SetParent(parentObject.transform, false);
            }
        }
    }
}