using UnityEngine;

namespace CoreScripts
{
    public class GameWindowFactory : AFactory<GameWindow>
    {
        private static int TopSortingLayer = 0;
        private MinigameType _mgType;
        private GameObject _parent;
        private GameObject _prefab;

        public GameWindowFactory(GameObject prefab, GameObject parent)
        {
            // TopSortingLayer = 0;
            _parent = parent;
            _prefab = prefab;
        }

        public GameWindow CreateGameWindow(MinigameType mgType)
        {
            _mgType = mgType;
            return base.Create();
        }

        protected override GameObject Instantiate()
        {
            TopSortingLayer += 10;
            GameObject windowObj = Object.Instantiate(_prefab, _parent.transform);
            ComponentHandler.AddCanvasWithOverrideSorting(windowObj, "GameWindow", TopSortingLayer);
            return windowObj;
        }

        protected override GameWindow Initialize(GameObject windowObj)
        {
            GameWindow window = windowObj.GetComponentInChildren<GameWindow>();

            Game.Instance.currentActiveWindows.Add(window);

            window.Initialize(_mgType, TopSortingLayer);
            return window;
        }

    }
}