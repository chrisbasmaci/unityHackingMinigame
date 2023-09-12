using System.Collections;

using UnityEngine;
using Image = UnityEngine.UI.Image;

public class CardCover : MonoBehaviour
{
    [SerializeField]private GameObject curtain;
    public float fillTime = 1f;
    public Image.FillMethod fillMethod = Image.FillMethod.Vertical;

    private bool _isCurtainUp = false;
    private static int curtainsUp = 0;

    
    
    public IEnumerator PullCurtainUp()
    {
        if (!_isCurtainUp)
        {
            yield return ComponentHandler.FillImage(curtain, fillTime, 1, fillMethod, 1f, 0f );
            _isCurtainUp = true;
            curtainsUp++;
        }
    }    
    public IEnumerator PullCurtainDown()
    {
        if (_isCurtainUp)
        {
            yield return ComponentHandler.FillImage(curtain, fillTime, 1, fillMethod, 0f, 1f);
            _isCurtainUp = false;
            curtainsUp--;
        }
    }

    public static bool CheckAllCurtainsUp(int curtainTotal)
    {
        if (curtainsUp == curtainTotal) {
            return true;
        }
        return false;
    }
    
    public static bool CheckAllCurtainsDown()
    {
        if (curtainsUp == 0) {
            return true;
        }

        return false;
    }
    

    public void ShowCover() {
        gameObject.SetActive(true);
    }

    public void HideCover() {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (curtainsUp == 0) {
            return;
        }
        
        if (_isCurtainUp) {
            curtainsUp--;
        }
    }
}
