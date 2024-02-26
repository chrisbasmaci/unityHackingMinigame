using CoreScripts;
using UnityEngine;

public class MinigamePanelFactory : AFactory<MgPanel>
{
    private MinigameType _mgType;
    private GameObject _panel;
    private GameWindow _window;
    
    public MinigamePanelFactory(GameObject panel, GameWindow window)
    {
        _panel = panel;
        _window = window;

    }
    public MgPanel createMgPanel(MinigameType mgType)
    {
        _mgType = mgType;
        return base.Create();
    }

    protected override GameObject Instantiate()
    {
        var mgPanelGobj = ComponentHandler.AddChildGameObject(_panel, "MGPanel");
        ComponentHandler.AddCanvasWithOverrideSorting(mgPanelGobj, "GameWindow", _window.CurrentSortingLayer +1);
        return mgPanelGobj;
    }

    protected override MgPanel Initialize(GameObject mgPanelObj)
    {
        var mgPanel = mgPanelObj.AddComponent<MgPanel>();
        ComponentHandler.SetAnchorToStretch(mgPanelObj);
        mgPanel.Initialize(_window, _mgType);
        return mgPanel;
    }
}