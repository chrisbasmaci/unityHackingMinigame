using System.Collections;
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
            settingsPanel = Resources.Load<GameObject>("UI_Prefabs/SettingsPrefab");
            settingsPanel = Instantiate(settingsPanel, currentCanvas.transform);    
        }

    }

    private static GameWindow NavigationPrep()
    {
        GameWindow window =  Instantiate(Game.Instance.gameWindowPrefab, Game.Instance.gameCanvas.transform).GetComponent<GameWindow>();
        Game.Instance.selectionCanvas.SetActive(false);
        Game.Instance.gameCanvas.SetActive(true);
        return window;
    }



}
