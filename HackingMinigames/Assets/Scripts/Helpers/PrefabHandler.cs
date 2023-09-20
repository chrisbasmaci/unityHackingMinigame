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
        public static T[,] CreateGrid<T>(GameObject parentObject, int dimension) where T : Component
        {
            Sprite squarePrefab = Resources.Load<Sprite>("Sprites/greenSquare");
            ComponentHandler.AddMaximisedGridLayout(parentObject);

            T[,] componentMatrix = new T[dimension, dimension];

            for (int x = 0; x < dimension; x++)
            {
                for (int y = 0; y < dimension; y++)
                {
                    GameObject child = new GameObject("GridChild_" + x + "_" + y);
                    child.AddComponent<Image>().sprite = squarePrefab;
                    child.transform.SetParent(parentObject.transform, false);
                    componentMatrix[x, y] = child.AddComponent<T>();
                }
            }

            return componentMatrix;
        }


    }
}