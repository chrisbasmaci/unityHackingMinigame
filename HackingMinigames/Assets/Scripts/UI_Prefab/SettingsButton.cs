using UnityEngine;
using UnityEngine.UI;

namespace UI_Prefab
{
    public class SettingsButton : CustomUIComponent
    {
        [SerializeField] private Image buttonBg;
        private Text _buttonText;
        public string  buttonName;
        
        public override void Setup() {
            _buttonText = GetComponentInChildren<UI_Prefab.Text>();
        }

        public override void Configure()
        {
            _buttonText.style = Style.Tertiary;
            if (buttonName != null)
            {
                _buttonText.text.text = buttonName;
            }
        }
    }
}