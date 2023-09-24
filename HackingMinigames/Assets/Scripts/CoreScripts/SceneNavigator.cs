using System.Collections;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <instruction>
/// EXAMPLE INSTRUCTION (1)
/// Add corresponding ENUM
/// </instruction>
public enum MinigameType {EXAMPLE, HACK, UNTANGLE,JumpChess}

public class SceneNavigator : MonoBehaviour
{
    private static GameObject settingsPanel;
    /// <instruction>
    /// EXAMPLE INSTRUCTION (2)
    /// Add Navigation to the corresponding enum
    /// EXAMPLE INSTRUCTION (2.5)
    /// Bind this to a button to start the game 
    /// </instruction>
    public static void Example()
    {
        NavigationPrep().Initialize(MinigameType.EXAMPLE);
    }
    public static void JumpChess()
    {
        NavigationPrep().Initialize(MinigameType.JumpChess);
    }
    public static void Hack()
    {
        NavigationPrep().Initialize(MinigameType.HACK);
    }
    public static void Untangle()
    {
        NavigationPrep().Initialize(MinigameType.UNTANGLE);
    }

    public static void ToggleSettings()
    {
        GameObject currentCanvas =Game.Instance.settingCanvas;
        if (settingsPanel != null)
        {
            Destroy(settingsPanel);
        }
        else
        {
            settingsPanel = Resources.Load<GameObject>("UI_Prefabs/SettingsWindow");
            settingsPanel = Instantiate(settingsPanel, currentCanvas.transform);    
        }

    }

    private static GameWindow NavigationPrep()
    {
        GameWindow window =  Instantiate(Game.Instance.gameWindowPrefab, Game.Instance.gameCanvas.transform).GetComponentInChildren<GameWindow>();
        Game.Instance.currentActiveWindows.Add(window);
        Game.Instance.selectionCanvas.SetActive(false);
        Game.Instance.gameCanvas.SetActive(true);
        return window;
    }

    public static void MainMenuButton()
    {
        Debug.Log("Normal Hack Button Pressed");        
        // for (int i = Game.Instance.currentActiveWindows.Count - 1; i >= 0; i--)
        // {
        //     var window = Game.Instance.currentActiveWindows[i];
        //     Destroy(window.GetComponent<UiMethods>().parent);
        //     Game.Instance.currentActiveWindows.RemoveAt(i);
        // }

        Game.Instance.gameCanvas.SetActive(false);
        Game.Instance.selectionCanvas.SetActive(true);
    }



}
