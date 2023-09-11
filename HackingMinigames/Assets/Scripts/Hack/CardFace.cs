
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using ColorUtility = UnityEngine.ColorUtility;

namespace Hack
{
    public class CardFace : CardSide
    {
        private (string shapeText, int shapePos, int backgroundColorNo) _shape;
        private (string shapeText, int shapePos, int backgroundColorNo) _shapePrompt;
        private (string colorText, int colorPos, int backgroundColorNo) _colorPrompt;

        private GameObject _shapeGameObject;
        private GameObject _shapePromptGameObject;
        private GameObject _colorPromptGameObject;



        // private (GameObject _object, SpriteRenderer _renderer) _colorPromptObject;
        private int hierarchy = -1;

        protected override void InitializeSide()
        {
            _card.faceSprite = Game.Instance.cardFace;
            _card.cardImage.sprite = _card.faceSprite;
            ComponentHandler.SetAnchorToStretch(gameObject);

            initPuzzleParts();

        }

        private void initPuzzleParts()
        {
            ComponentHandler.AddVerticalLayoutGroup(gameObject);
            //invert the shape and color text if toggles on


            _shapeGameObject = initPrompt(true);
            _shapePromptGameObject = initShape();
            _colorPromptGameObject = initPrompt(false);

            if (_card.isStartingSideBack)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

        }

        private GameObject initShape()
        {
            //setup gameobject
            //setup shape
            _shape = RandomFactory.GetShapeTuple();

            var shapeColor = ColorHex.colorMap[_shape.backgroundColorNo].colorHex;
            var color = GetColor(shapeColor);
            var shapeGameObject = AddShape("backgroundShape", Game.Instance.shapeSheet[_shape.shapePos], color);
            ComponentHandler.AddLayoutElement(_shapeGameObject, 0, 1000);

            return shapeGameObject;

        }

        private Color GetColor(string colorHex)
        {
            Color color;
            ColorUtility.TryParseHtmlString(colorHex, out color);
            return color;

        }

        private GameObject AddShape(string childName, Sprite shapeImage, Color shapeColor)
        {
            var shapeGameObject = ComponentHandler.AddChildGameObject(gameObject, childName);
            ComponentHandler.AddImageComponent(shapeGameObject, shapeImage, shapeColor);
            ComponentHandler.SetAnchorToStretch(shapeGameObject);

            return shapeGameObject;
        }

        private GameObject initPrompt(bool isShape)
        {
            string colorText;
            (GameObject _object, SpriteRenderer _renderer) promptObject;

            if (isShape)
            {
                _shapePrompt = RandomFactory.GetShapeTuple();

                var shapeColor = ColorHex.colorMap[_shapePrompt.backgroundColorNo].colorHex;
                var color = GetColor(shapeColor);

                var gameObject = AddShape("textShape", Game.Instance.shapeTextSheet[_shapePrompt.shapePos], color);
                ComponentHandler.AddLayoutElement(gameObject, null, 1);

            }
            else
            {

                _colorPrompt = RandomFactory.GetcolorPromptTuple();
                var shapeColor = ColorHex.colorMap[_colorPrompt.backgroundColorNo].colorHex;
                var color = GetColor(shapeColor);

                var gameObject = _colorPromptGameObject =
                    AddShape("textColor", Game.Instance.colorTextSheet[_colorPrompt.colorPos], color);
                ComponentHandler.AddLayoutElement(gameObject, null, 1);
            }

            return gameObject;
        }



        public string getShapeText()
        {
            return _shape.shapeText;
        }

        public string getShapeBC()
        {
            return ColorHex.colorMap[_shape.backgroundColorNo].colorName;
        }

        public string getShapePromptText()
        {
            return _shapePrompt.shapeText;
        }

        public string getShapePromptBC()
        {
            return ColorHex.colorMap[_shapePrompt.backgroundColorNo].colorName;
        }

        public string getColorPromptText()
        {
            return _colorPrompt.colorText;
        }

        public string getColorPromptBC()
        {
            return ColorHex.colorMap[_colorPrompt.backgroundColorNo].colorName;
        }

    }
}