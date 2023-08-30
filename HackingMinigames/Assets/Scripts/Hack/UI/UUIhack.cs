using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UUIhack :UIPanel
{
    [SerializeField]public  Button leftButton;
    [SerializeField]private Button rightButton;
    [SerializeField]public TMP_Text streakText;
    private GameObject _upperPanel;

    public override void Initialize(GameObject upperPanel)
    {
        Debug.Log("BUTTON");
        _upperPanel = upperPanel;
        leftButton.onClick.AddListener(ButtonManager.Instance.backToSettings);
        rightButton.onClick.AddListener(aaa);
    }

    private void aaa()
    {
        Debug.Log("AAAAA");

    }
}