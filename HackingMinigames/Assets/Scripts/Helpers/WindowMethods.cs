using CoreScripts;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Helpers
{
    public enum WindowType
    {
        None,
        GameWindow,
        MenuWindow
    }
    public class WindowMethods : UiMethods
    {
        public WindowType windowType;
        public GameObject mainWindow;
        public GameWindow GameWindow => mainWindow.GetComponent<GameWindow>();
        private RectTransform MainWindowBounds => mainWindow.GetComponent<RectTransform>();
        [CanBeNull] public GameObject backgroundGj;
        [SerializeField]public Image curtain;
        private PixelBar bar;
        private GameObject resizeButton;
        public bool isMinimized;
        

        public void Awake()
        {
            if (windowType ==WindowType.None )
            {
                Debug.LogError("WindowType is null");
            }

            bar = GetComponentInChildren<PixelBar>();
            if (bar == null)
            { 
                Debug.LogError("Bar is null");
            }
            resizeButton = gameObject.GetComponentInChildren<ResizePanel>().gameObject;
            resizeButton.GetComponent<Button>().onClick.AddListener(ResizeNotify);
        }
        public void ToggleCurtain()
        {
            curtain.gameObject.SetActive(!curtain.gameObject.activeSelf);
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
            if (backgroundGj != null)
            {
                backgroundGj.SetActive(true);
            }

            var mainHeight = mainWindow.GetComponent<LayoutElement>().minHeight;
            var barHeight = bar.gameObject.GetComponent<LayoutElement>().minHeight;
            ParentFitVerticalUnconstrained(mainHeight + barHeight);
            ParentMoveToMiddle();
            bar.SetLowered(false);

            if (windowType == WindowType.GameWindow)
            {
                var gameWindow = mainWindow.GetComponent<GameWindow>();
                Game.Instance.WindowMinimized(gameWindow, false);
            }


        }
        private void lowerWindow()
        {
            mainWindow.SetActive(false);
            if (backgroundGj != null)
            {
                backgroundGj.SetActive(false);
            }

            var lay = parent.GetComponent<LayoutElement>();
            ParentFitVerticalTight(new Vector2(lay.minWidth, lay.minHeight), bar.gameObject.GetComponent<RectTransform>().rect.height);
            ParentRefresh();
            ParentMoveToBottom();
            bar.SetLowered(true);
            
            
            if (windowType == WindowType.GameWindow)
            {
                var gameWindow = mainWindow.GetComponent<GameWindow>();
                Game.Instance.WindowMinimized(gameWindow, true);
            }

        }


        public void FixLayer()
        {
            //fixes for gamewindow
            if(windowType == WindowType.GameWindow)
            {
                var gameWindow = mainWindow.GetComponent<GameWindow>();
                gameWindow.CurrentSortingLayer = GameWindowFactory.useTopSpot();
                gameWindow.MinigamePanel.FixLayoutOrder(gameWindow.CurrentSortingLayer + 1);
                curtain.gameObject.GetComponent<Canvas>().sortingOrder = gameWindow.CurrentSortingLayer + 1;
            }
            // gameWindow?.CurrentSortingLayer = GameWindowFactory.useTopSpot();
            // gameWindow?.MinigamePanel.FixLayoutOrder(gameWindow.CurrentSortingLayer + 1);
        }


    }
}