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
    [SerializeField]public RectTransform canvasRect;
    public HorizontalLayoutGroup canvasHorizontalLayout;
    // [SerializeField] public MgPanel mgPanel;
    [SerializeField] public GameWindow gameWindow;
    [NonSerialized] private float _animationDuration =0.3f;
    [NonSerialized] public WindowSize settingWindowSize;

    

    private void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        settingWindowSize = CalculateWsWithPadding(canvasRect.rect, 0);
        Debug.Log("current height: "+settingWindowSize.Height);
    }

    public WindowSize GetGameWindowSize()
    {
        return CalculateWsWithPadding(canvasRect.rect, 0);
    }
    
    
    public static WindowSize CalculateWsWithPadding(Rect panelRect, float paddingPercentage)
    {
        float width, height, leftBorder, rightBorder, topBorder, bottomBorder;
        width = panelRect.width -(paddingPercentage * panelRect.width*2);
        height = panelRect.height - (paddingPercentage * panelRect.width*2);
        leftBorder = panelRect.xMin +(paddingPercentage * panelRect.width);
        rightBorder = panelRect.xMax - (paddingPercentage * panelRect.width);
        topBorder = panelRect.yMax - (paddingPercentage * panelRect.width);
        bottomBorder = panelRect.yMin + (paddingPercentage *panelRect.width);
        Debug.Log("width: "+width + "height: "+height + "leftBorder: "+leftBorder + "rightBorder: "+rightBorder + "topBorder: "+topBorder + "bottomBorder: "+bottomBorder);
        WindowSize tmpWindow = new WindowSize(width, height, leftBorder, rightBorder, topBorder, bottomBorder);
        return tmpWindow;
    }

    public void InitPanels()
    {
    }
    
}
