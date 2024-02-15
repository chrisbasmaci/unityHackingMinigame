using UnityEngine;
using UnityEngine.UI;

namespace UI_Prefab
{
    public class MainMenu : CustomUIComponent
    {
        private Button[] buttons;
        [SerializeField]private GameObject hackButton;
        [SerializeField]private GameObject untangleButton;
        [SerializeField]private GameObject jumpChessButton;
        [SerializeField]private GameObject exampleButton;


        public override void Setup()
        {
            buttons = GetComponentsInChildren<Button>();
            var tran = transform;
            var pos = tran.position;
            hackButton = Instantiate(Game.Instance.dominoButton.gameObject, pos, Quaternion.identity, tran);
            DominoButton dominoButtonScript = hackButton.GetComponent<DominoButton>();
            Sprite hackImg = Resources.Load<Sprite>("Sprites/noun-pixelated-laptop-1713639 1");
            dominoButtonScript.Customize("Hack", "Start", hackImg);
            
            untangleButton = Instantiate(Game.Instance.dominoButton.gameObject, pos, Quaternion.identity, tran);
            dominoButtonScript = untangleButton.GetComponent<DominoButton>();
            Sprite untangleImg = Resources.Load<Sprite>("Sprites/square");
            dominoButtonScript.Customize("Untangle", "Start", untangleImg);
            
            jumpChessButton = Instantiate(Game.Instance.dominoButton.gameObject, pos, Quaternion.identity, tran);
            dominoButtonScript = jumpChessButton.GetComponent<DominoButton>();
            Sprite chessImg = Resources.Load<Sprite>("Sprites/chess");
            dominoButtonScript.Customize("JumpChess", "Start", chessImg);
            
            exampleButton = Instantiate(Game.Instance.dominoButton.gameObject, pos, Quaternion.identity, tran);
            dominoButtonScript = exampleButton.GetComponent<DominoButton>();
            Sprite exampleImg = Resources.Load<Sprite>("Sprites/example");
            dominoButtonScript.Customize("Example", "Start", exampleImg);

        }

        public override void Configure()
        {
            var navigator = GetComponentInChildren<SceneNavigator>();
            navigator = (navigator)? navigator : gameObject.AddComponent<SceneNavigator>();
            
            hackButton.GetComponent<Button>().onClick.AddListener(()      => navigator.CreateAndShowGameWindow(MinigameType.HackingMG));
            exampleButton.GetComponent<Button>().onClick.AddListener(()   => navigator.CreateAndShowGameWindow(MinigameType.ExampleMG));
            jumpChessButton.GetComponent<Button>().onClick.AddListener(()    => navigator.CreateAndShowGameWindow(MinigameType.JumpChessMG));
            untangleButton.GetComponent<Button>().onClick.AddListener(()  => navigator.CreateAndShowGameWindow(MinigameType.UntangleMG));

        }
    }
}