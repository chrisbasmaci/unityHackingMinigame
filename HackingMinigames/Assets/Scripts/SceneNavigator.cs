using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator: MonoBehaviour
{

    public void Untangle()
    {
        Game.Instance.currentMg = MinigameType.UNTANGLE;
        SceneManager.LoadScene("untanglescene");
    }
    public void Hack()
    {
        Game.Instance.currentMg = MinigameType.HACK;
        SceneManager.LoadScene("untanglescene");
    }

}
