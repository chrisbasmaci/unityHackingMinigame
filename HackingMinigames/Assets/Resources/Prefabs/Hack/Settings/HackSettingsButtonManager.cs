using UnityEngine;
using UnityEngine.UI;

public class HackSettingsButtonManager : MonoBehaviour
{
    [SerializeField] Slider tileSlider;
    [SerializeField] Slider timeSlider;
    
    // Start is called before the first frame update
    public void TimeAmountSlider(){
        Game.Instance.defaultPuzzleTime = (int)timeSlider.value;
    }

    public void TileAmountSlider(){
        Game.Instance.defaultTileAmount = (int)tileSlider.value;
    }

}
