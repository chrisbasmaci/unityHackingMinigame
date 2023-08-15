using System;
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
    public RectTransform canvasRect;
    public HorizontalLayoutGroup canvasHorizontalLayout;
    [SerializeField] public MgPanel mgPanel;
    [NonSerialized] private float _paddingPercentage = 0.1f;
    [NonSerialized] private float _targetPaddingPercentage = 0.025f;
    [NonSerialized] private float _animationDuration = 0.3f;
    [NonSerialized] private WindowSize _hackWindowSize;
    [SerializeField] public GameObject upperGUI;
    [SerializeField] public GameObject bottomGUI;

    private void Start()
    {
        SetPadding(_paddingPercentage);
    }

    public IEnumerator ChangePaddingWithAnimation(MgPanel mgPanel, bool gameStart = false)
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
            // _hackWindowSize = SetupWindow2(0f);
            mgPanel.Initialize(this);
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
    
    
    public WindowSize CalculateWsWithPadding(Rect panelRect, float paddingPercentage)
    {
        float width, height, leftBorder, rightBorder, topBorder, bottomBorder;
        width = panelRect.width -(paddingPercentage * panelRect.width);
        height = panelRect.height - (paddingPercentage * panelRect.height*2);
        leftBorder = panelRect.xMin +(paddingPercentage * panelRect.width)/2;
        rightBorder = panelRect.xMax - (paddingPercentage * panelRect.width)/2;
        topBorder = panelRect.yMax - (paddingPercentage * panelRect.height);
        bottomBorder = panelRect.yMin + (paddingPercentage * panelRect.height);
        Debug.Log("width: "+width + "height: "+height + "leftBorder: "+leftBorder + "rightBorder: "+rightBorder + "topBorder: "+topBorder + "bottomBorder: "+bottomBorder);
        WindowSize tmpWindow = new WindowSize(width, height, leftBorder, rightBorder, topBorder, bottomBorder);
        return tmpWindow;
    }

    public void InitPanels()
    {
    }
    
}
