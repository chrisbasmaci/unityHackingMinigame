using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <instruction>
/// EXAMPLE INSTRUCTION (1)
/// Add corresponding ENUM
/// </instruction>
public enum MinigameType {EXAMPLE, HACK, UNTANGLE,JumpChess}

public class SceneNavigator: MonoBehaviour
{
    /// <instruction>
    /// EXAMPLE INSTRUCTION (2)
    /// Add Navigation to the corresponding enum
    /// EXAMPLE INSTRUCTION (2.5)
    /// Bind this to a button to start the game 
    /// </instruction>
    public void Example()
    {
        NavigationPrep().Initialize(MinigameType.EXAMPLE);
    }
    public void JumpChess()
    {
        NavigationPrep().Initialize(MinigameType.JumpChess);
    }
    public void Hack()
    {
        NavigationPrep().Initialize(MinigameType.HACK);
    }
    public void Untangle()
    {
        NavigationPrep().Initialize(MinigameType.UNTANGLE);
    }

    private static GameWindow NavigationPrep()
    {
        GameWindow window =  Instantiate(Game.Instance.gameWindowPrefab, Game.Instance.gameCanvas.transform).GetComponent<GameWindow>();
        Game.Instance.selectionCanvas.SetActive(false);
        Game.Instance.gameCanvas.SetActive(true);
        return window;
    }



}
