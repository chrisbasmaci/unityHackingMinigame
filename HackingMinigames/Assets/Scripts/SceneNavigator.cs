using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator: MonoBehaviour
{

    public void Untangle()
    {
        Game.Instance.currentMg = MinigameType.UNTANGLE;
        Game.Instance.currentSettingsPrefab = null;

        SceneManager.LoadScene("untanglescene");


        // Game.Instance.CurrentGameWindow.
    }
    public void Hack()
    {
        Game.Instance.currentMg = MinigameType.HACK;
        Game.Instance.currentSettingsPrefab = Resources.Load<GameObject>("Prefabs/Hack/Settings/SubSettingsPanel");

        SceneManager.LoadScene("untanglescene");

    }


}
