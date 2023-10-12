using UnityEngine;
using UnityEngine.UI;

namespace UI_Prefab
{
    public class MainMenu : CustomUIComponent
    {
        private Button[] buttons;
        [SerializeField]private Button hackButton;
        [SerializeField]private Button untangleButton;
        [SerializeField]private Button jumpChessButton;
        [SerializeField]private Button exampleButton;


        public override void Setup()
        {
            buttons = GetComponentsInChildren<Button>();
        }

        public override void Configure()
        {
            hackButton = buttons[0];
            untangleButton = buttons[1];
            jumpChessButton = buttons[2];
            exampleButton = buttons[3];
            
            var navigator = GetComponentInChildren<SceneNavigator>();
            navigator = (navigator)? navigator : gameObject.AddComponent<SceneNavigator>();
            
            hackButton.onClick.AddListener(()      => navigator.CreateAndShowGameWindow(MinigameType.HackingMG));
            exampleButton.onClick.AddListener(()   => navigator.CreateAndShowGameWindow(MinigameType.ExampleMG));
            jumpChessButton.onClick.AddListener(()    => navigator.CreateAndShowGameWindow(MinigameType.JumpChessMG));
            untangleButton.onClick.AddListener(()  => navigator.CreateAndShowGameWindow(MinigameType.UntangleMG));

        }
    }
}