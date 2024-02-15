using System;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UIElements.Image;

namespace UI_Prefab
{
    public class DominoButton : CustomUIComponent
    {

        public string upperText;
        public string lowerText;
        public Sprite image;
        private TMP_Text _upperText;
        private TMP_Text _lowerText;
        private UnityEngine.UI.Image _image;
        public override void Setup()
        {
            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text text in texts)
            {
                if (text.gameObject.name == "UpperText")
                {
                    _upperText = text;
                }
                if (text.gameObject.name == "LowerText")
                {
                    _lowerText = text;
                }
                if (_lowerText && _upperText)
                {
                    break;
                }
            }
            UnityEngine.UI.Image[] images = GetComponentsInChildren<UnityEngine.UI.Image>();
            foreach (UnityEngine.UI.Image img in images)
            {
                if (img.gameObject.name == "MinigameIcon")
                {
                    _image = img;
                    break; // Exit the loop once the image is found
                }
            }
            if (_upperText == null)
            {
                Debug.Log("upperText not found.");
            }            
            if (_lowerText == null)
            {
                Debug.Log("lowertext not found.");
            }

            if (_image == null)
            {
                Debug.Log("image not found.");

            }
            _image.sprite = image;
        }

        public override void Configure()
        {
            _upperText.text = upperText;
            _lowerText.text = lowerText;
        }

        public void Customize(string upperT, string lowerT, Sprite img)
        {
            upperText = upperT;
            lowerText = lowerT;
            image = img;
            Init();
        }
    }
}