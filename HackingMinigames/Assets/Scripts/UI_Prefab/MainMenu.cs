using UnityEngine;
using UnityEngine.UI;

namespace UI_Prefab
{
    public class MainMenu : CustomUIComponent
    {
        private Button[] buttons;
        [SerializeField]private Button hackButton;
        [SerializeField]private Button untangleButton;
        [SerializeField]private Button jumpchessButton;
        [SerializeField]private Button exampleButton;


        public override void Setup()
        {
            buttons = GetComponentsInChildren<Button>();
        }

        public override void Configure()
        {
            hackButton = buttons[0];
            untangleButton = buttons[1];
            jumpchessButton = buttons[2];
            exampleButton = buttons[3];
            
            hackButton.onClick.AddListener(SceneNavigator.Hack);
            untangleButton.onClick.AddListener(SceneNavigator.Untangle);
            jumpchessButton.onClick.AddListener(SceneNavigator.JumpChess);
            exampleButton.onClick.AddListener(SceneNavigator.Example);
        }
    }
}