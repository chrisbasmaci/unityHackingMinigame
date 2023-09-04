using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator: MonoBehaviour
{

    public void Untangle()
    {
        // Game.Instance.currentSettingsPrefab = null;
        Game.Instance.selectionCanvas.SetActive(false);
        Game.Instance.gameCanvas.SetActive(true);
        var window =  Instantiate(Game.Instance.gameWindowPrefab, Game.Instance.gameCanvas.transform).GetComponent<GameWindow>();
        window.Initialize(MinigameType.UNTANGLE);

        // Game.Instance.CurrentGameWindow.
    }
    public void Hack()
    {
        Game.Instance.selectionCanvas.SetActive(false);
        Game.Instance.gameCanvas.SetActive(true);
        var window =  Instantiate(Game.Instance.gameWindowPrefab, Game.Instance.gameCanvas.transform).GetComponent<GameWindow>();
        window.Initialize(MinigameType.HACK);
        // SceneManager.LoadScene("untanglescene");

    }


}
