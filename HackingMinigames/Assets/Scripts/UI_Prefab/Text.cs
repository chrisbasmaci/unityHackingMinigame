using System;
using UnityEngine;
using TMPro;

namespace UI_Prefab 
{
    public enum Style {Primary,Secondary,Tertiary }

    public class Text : CustomUIComponent
    {
        public TextSO textData;
        public Style style;
        public TMP_Text text;

        private TextMeshProUGUI textMeshProUGUI;

        
        public override void Setup()
        {
            if (!textMeshProUGUI)
            {
                textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            }
            text = GetComponentInChildren<TMP_Text>();
        }        
        
        public override void Configure()
        {
            if (textData != null && textMeshProUGUI != null)
            {
                textMeshProUGUI.color = textData.theme.GetTextOrBgColor(style);
                textMeshProUGUI.font = textData.font;
                textMeshProUGUI.fontSize = textData.size;
                textMeshProUGUI.alignment = TextAlignmentOptions.Center;
            }
            else
            {
                Debug.LogError("Text data or TextMeshProUGUI is null. Ensure they are properly initialized.");
            }
        }


    }

}
