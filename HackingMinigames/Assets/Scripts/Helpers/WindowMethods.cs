using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Helpers
{
    public class WindowMethods : UiMethods
    {
        public GameObject mainWindow;
        public RectTransform MainWindowBounds => mainWindow.GetComponent<RectTransform>();
        private GameWindow gameWindow => mainWindow.GetComponent<GameWindow>();
        public GameObject bar;
        [SerializeField]public GameObject resizeButton;
        public bool isMinimized;
        

        public void Start()
        {
            var but = resizeButton.GetComponent<Button>();
            but.onClick.AddListener(ResizeNotify);
        }

        public void ResizeNotify()
        {
            Debug.Log("Resize Clicked");
            GameEvent a = Resources.Load<GameEvent>("EventSystem_v1.0/ResizedWindowEvent");
            a.Raise(this, MainWindowBounds.rect);
            // gameWindow.MinigamePanel.WindowResizeEvent(this, MainWindowBounds.rect);
        }
        
        public void MinimizeWindow()
        {
            if (isMinimized)
            {
                showWindow();
            }
            else
            {
                lowerWindow();
            }
            isMinimized = !isMinimized;

        }
        public void CloseWindow()
        {
            Destroy(parent);
        }

        private void showWindow()
        {            
            mainWindow.SetActive(true);
            ParentFitVerticalUnconstrained();
            ParentMoveToMiddle();
            bar.GetComponentInChildren<DragPanel>().ToggleVerticalDrag();
            Game.Instance.WindowMinimized(gameWindow, false);

        }
        private void lowerWindow()
        {
            mainWindow.SetActive(false);
            ParentFitVerticalTight();
            ParentRefresh();
            ParentMoveToBottom();
            bar.GetComponentInChildren<DragPanel>().ToggleVerticalDrag();
            Game.Instance.WindowMinimized(gameWindow, true);

        }


    }
}