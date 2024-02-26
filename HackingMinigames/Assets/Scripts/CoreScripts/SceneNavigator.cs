using System;
using System.Collections;
using CoreScripts;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <instruction>
/// EXAMPLE INSTRUCTION (1)
/// Add corresponding ENUM
/// IMPORTANT: The names of the enums must match the names of the minigame scripts!!
/// </instruction>
public enum MinigameType {ExampleMG, HackingMG, UntangleMG, JumpChessMG}

public class SceneNavigator : MonoBehaviour
{
    private static GameObject settingsPanel;
    GameWindowFactory GameWindowFactory;

    /// <instruction>ss
    /// EXAMPLE INSTRUCTION (2)
    /// Add Navigation to the corresponding enum
    /// EXAMPLE INSTRUCTION (2.5)
    /// Bind this to a button to start the game 
    /// </instruction>
    public void Awake()
    {
        GameWindowFactory = new GameWindowFactory(Game.Instance.gameWindowPrefab, Game.Instance.gameCanvas);
    }

    public void CreateAndShowGameWindow(MinigameType type)
    {
        ShowGameCanvas(true);
        GameWindowFactory.CreateGameWindow(type);
    }

    
    public static void ShowGameCanvas(bool show)
    {
        if (Game.Instance.gameCanvas.activeSelf != show)
        {
            Debug.Log("ShowGameCanvas: " + show);
            Game.Instance.gameCanvas.SetActive(show);
        }
    
        if (Game.Instance.selectionCanvas.activeSelf == show)
        {
            Game.Instance.selectionCanvas.SetActive(!show);
        }
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
            settingsPanel = Resources.Load<GameObject>("UI_Prefabs/CoolSettinsWindow");
            settingsPanel = Instantiate(settingsPanel, currentCanvas.transform);    
        }

    }

    public static void MainMenuButton()
    {
        Debug.Log("MainMenuButton Button Pressed");
        ShowGameCanvas(false);
    }



}
