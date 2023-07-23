
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using ColorUtility = UnityEngine.ColorUtility;


public class CardFace : CardSide
{
    private (string shapeText, int shapePos, int backgroundColorNo) _shape;
    private (string shapeText,int shapePos, int backgroundColorNo) _shapePrompt;
    private (string colorText,int colorPos, int backgroundColorNo) _colorPrompt;


    
    private (GameObject _object, SpriteRenderer _renderer) _shapeObject;
    private (GameObject _object, SpriteRenderer _renderer) _shapePromptObject;
    private (GameObject _object, SpriteRenderer _renderer) _colorPromptObject;
    private int hierarchy = -1;

    protected override void InitializeSide()
    {
        _card.faceSprite = Game.Instance.cardFace;
        _card._cardRenderer.sprite = _card.faceSprite;
        initPuzzleParts();
    }

    private void initPuzzleParts()
    {
        initShape();
        bool isShape =true;
        //invert the shape and color text if toggles on
        if(Game.Instance.invertToggle && RandomFactory.isSwitched()){
            isShape = !isShape;
        }
        initUpperPrompt(isShape);
        initLowerPrompt(!isShape);
        if (_card.isStartingSideBack)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

    }

    private void initShape()
    {
        //setup gameobject
        _shapeObject = initSpriteObject();
        //setup shape
        _shape = RandomFactory.GetShapeTuple();
        
        Color color;
        ColorUtility.TryParseHtmlString(ColorHex.colorMap[_shape.backgroundColorNo].colorHex, out color);
        _shapeObject._renderer.color = color;
        _shapeObject._renderer.sprite = Game.Instance.shapeSheet[_shape.shapePos];
    }

    private void initUpperPrompt(bool isShape)
    {
        initPrompt(isShape ,0.5f);
    }        
    private void initLowerPrompt(bool isShape)
    {
        initPrompt(isShape ,-0.5f);
    }      
    private void initPrompt(bool isShape, float promptY)
    {
        string colorText;
        (GameObject _object, SpriteRenderer _renderer) promptObject;
        
        if (isShape)
        {
            _shapePromptObject = initSpriteObject(promptY);
            promptObject = _shapePromptObject;
            _shapePrompt = RandomFactory.GetShapeTuple();
            colorText = ColorHex.colorMap[_shapePrompt.backgroundColorNo].colorHex;
            promptObject._renderer.sprite = Game.Instance.shapeTextSheet[_shapePrompt.shapePos];

        }else {
            _colorPromptObject = initSpriteObject(promptY);
            promptObject = _colorPromptObject;
            _colorPrompt = RandomFactory.GetcolorPromptTuple();
            colorText = ColorHex.colorMap[_colorPrompt.backgroundColorNo].colorHex;
            Debug.Log("xCheck: "+_card.cardOrder);
            promptObject._renderer.sprite = Game.Instance.colorTextSheet[_colorPrompt.colorPos];

        }
        Color color;
        ColorUtility.TryParseHtmlString(colorText, out color);
        promptObject._renderer.color = color;
    }

    private (GameObject _object, SpriteRenderer _renderer) initSpriteObject(float objectY =0)
    {
        (GameObject _object, SpriteRenderer _renderer) spriteObject;
        spriteObject._object = new GameObject("Prompt1");
        spriteObject._renderer = spriteObject._object.AddComponent<SpriteRenderer>();
        spriteObject._object.transform.parent = gameObject.transform;
        spriteObject._object.transform.localPosition = new Vector3(0, objectY, hierarchy);
        spriteObject._object.transform.localScale = gameObject.transform.localScale;
        return spriteObject;
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
