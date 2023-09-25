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
    public static int TopSortingLayer = 0; 
    /// <instruction>
    /// EXAMPLE INSTRUCTION (2)
    /// Add Navigation to the corresponding enum
    /// EXAMPLE INSTRUCTION (2.5)
    /// Bind this to a button to start the game 
    /// </instruction>
    public static void Example()
    {
        NavigationPrep().Initialize(MinigameType.EXAMPLE, TopSortingLayer);
    }
    public static void JumpChess()
    {
        NavigationPrep().Initialize(MinigameType.JumpChess, TopSortingLayer);
    }
    public static void Hack()
    {
        NavigationPrep().Initialize(MinigameType.HACK, TopSortingLayer);
    }
    public static void Untangle()
    {
        NavigationPrep().Initialize(MinigameType.UNTANGLE, TopSortingLayer);
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
        TopSortingLayer += 10;
        GameObject windowObj = Instantiate(Game.Instance.gameWindowPrefab, Game.Instance.gameCanvas.transform);
        GameWindow window = windowObj.GetComponentInChildren<GameWindow>();
        ComponentHandler.AddCanvasWithOverrideSorting(windowObj, "GameWindow", TopSortingLayer);

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
