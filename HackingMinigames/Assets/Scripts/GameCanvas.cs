using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public struct WindowSize
{
    public float Width { get; private set; }
    public float Height { get; private set; }
    public float LeftBorder { get; private set; }
    public float RightBorder { get; private set; }
    public float TopBorder { get; private set; }
    public float BottomBorder { get; private set; }

    public WindowSize(float width, float height, float leftBorder, float rightBorder, float topBorder, float bottomBorder)
    {
        Width = width;
        Height = height;
        LeftBorder = leftBorder;
        RightBorder = rightBorder;
        TopBorder = topBorder;
        BottomBorder = bottomBorder;
    }
}
public class GameCanvas : MonoBehaviour
{
    [FormerlySerializedAs("layoutGroupRect")] public RectTransform canvasRect;
    [FormerlySerializedAs("layoutGroupRect")] public RectTransform hackRect;
    [FormerlySerializedAs("horizontalLayout")] public HorizontalLayoutGroup canvasHorizontalLayout;
    [SerializeField] public GameWindow gameWindow;
    private float _paddingPercentage = 0.1f;
    private float _targetPaddingPercentage = 0.025f;
    private float _animationDuration = 0.3f;
    public WindowSize _canvasWindowSize;
    private WindowSize _hackWindowSize;
    public WindowSize HackWindowSize { get => _hackWindowSize; private set => _hackWindowSize = value; }
    private WindowSize _settingWindowSize;
    private void Start()
    {
        //startingPadding
        SetPadding(_paddingPercentage);
        //---------------------------------------------
        _canvasWindowSize = SetupWindow(0f);
        _settingWindowSize = SetupWindow(_paddingPercentage);
   
        // gameWindow.Initialize(_canvasWindowSize);
    }

    public IEnumerator ChangePaddingWithAnimation(GameWindow gameWindow, bool gameStart = false)
    {
        float elapsedTime = 0f;
        float startPadding = _paddingPercentage;
        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            _paddingPercentage = Mathf.Lerp(startPadding, _targetPaddingPercentage, elapsedTime / _animationDuration);
            SetPadding(_paddingPercentage);
            yield return null;
        }

        // Debug.Log("paddingPercentage"+_targetPaddingPercentage);

        SetPadding(_paddingPercentage);
        var tmp = _targetPaddingPercentage;
        _targetPaddingPercentage = startPadding;
        _paddingPercentage = tmp;
        if (gameStart)
        {
            _hackWindowSize = SetupWindow2(0f);
            gameWindow.Initialize(_hackWindowSize);
        }


        yield return null;
    }

    public void SetPadding(float paddingPercentage)
    {
        float padding = canvasRect.rect.width * paddingPercentage;
        // float padding = 0;
        // Debug.Log("current  padding"+padding);
        canvasHorizontalLayout.padding.left = (int)padding;
        canvasHorizontalLayout.padding.top = (int)padding;
        canvasHorizontalLayout.padding.bottom = (int)padding;
        canvasHorizontalLayout.padding.right = (int)padding;
        LayoutRebuilder.MarkLayoutForRebuild(canvasRect as RectTransform);

    }

    private WindowSize SetupWindow(float paddingPercentage)
    {
        Rect canvasRectValue = canvasRect.rect;

        float width, height, leftBorder, rightBorder, topBorder, bottomBorder;
        width = canvasRectValue.width - (paddingPercentage * canvasRectValue.width *2);
        height = canvasRectValue.height - (paddingPercentage * canvasRectValue.height*2);
        leftBorder = canvasRectValue.xMin + (paddingPercentage * canvasRectValue.width);
        rightBorder = canvasRectValue.xMax - (paddingPercentage * canvasRectValue.width);
        topBorder = canvasRectValue.yMax - (paddingPercentage * canvasRectValue.height);
        bottomBorder = canvasRectValue.yMin + (paddingPercentage * canvasRectValue.height);
        
        Debug.Log("width: "+width + "height: "+height + "leftBorder: "+leftBorder + "rightBorder: "+rightBorder + "topBorder: "+topBorder + "bottomBorder: "+bottomBorder);
        WindowSize tmpWindow = new WindowSize(width, height, leftBorder, rightBorder, topBorder, bottomBorder);
        return tmpWindow;
    }
    
    private WindowSize SetupWindow2(float paddingPercentage)
    {
        Rect canvasRectValue = hackRect.rect;
        float width, height, leftBorder, rightBorder, topBorder, bottomBorder;
        Debug.Log("Scale:" + hackRect.transform.localScale);
        width = canvasRectValue.width;
        height = canvasRectValue.height - (paddingPercentage * canvasRectValue.height*2);
        leftBorder = canvasRectValue.xMin ;
        rightBorder = canvasRectValue.xMax - (paddingPercentage * canvasRectValue.width);
        topBorder = canvasRectValue.yMax - (paddingPercentage * canvasRectValue.height);
        bottomBorder = canvasRectValue.yMin + (paddingPercentage * canvasRectValue.height);
        Debug.Log("width: "+width + "height: "+height + "leftBorder: "+leftBorder + "rightBorder: "+rightBorder + "topBorder: "+topBorder + "bottomBorder: "+bottomBorder);
        WindowSize tmpWindow = new WindowSize(width, height, leftBorder, rightBorder, topBorder, bottomBorder);
        return tmpWindow;
    }
    
}
