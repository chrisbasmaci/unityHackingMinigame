
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using TriangleNet;
using TriangleNet.Geometry;

public class CardFactory
{
    //TODO FIX SCALE CORRECTLY
    private float ratio = 1.4f;
    public const float CardWidth = 6000f;
    public const float CardHeight = 600f;
    // public const float CardEdgeLen = 1000f;
    public const float CardMiddleSpacing = 10f;
    public const float CardSidePadding = 20f;
    
    private float currentSidePadding = CardSidePadding;
    private float currentWidth = CardWidth;
    private float currentHeight = CardHeight;

    
    public List<WindowSize> getAllCardDimensions(int cardTotal, WindowSize displaySize)
    {
        ///ASSERT cardTotal >0
        Debug.Log("cardTotal: " + cardTotal);
        Assert.IsTrue(cardTotal > 0);
        Debug.Log("starting getAllCardDimensions");

        currentSidePadding = CardSidePadding;
        currentWidth = CardWidth;
        currentHeight = CardHeight;
    
        float totalSidePadding = (2 * CardSidePadding);
        float totalCardMiddleSpacing = ((cardTotal -1)* CardMiddleSpacing);
        float totalCardWidhtLen = (cardTotal * CardWidth);
        
        float neededWidth =  totalSidePadding + totalCardWidhtLen + totalCardMiddleSpacing;
        Debug.Log("neededWidth: " + neededWidth);

        //check width
        if(neededWidth > displaySize.Width) {
            Debug.Log("Width too big");
            // if width is not enough make smaller cards (perfect fit)
            if (totalSidePadding + totalCardMiddleSpacing > displaySize.Width)
            {
                ///TODO TOO MANY CARDS
                Debug.Log("display width"+ displaySize.Width);
                Debug.Log("This should never happen");
                Assert.IsFalse(true);
            }
            float availableWidth = displaySize.Width - totalSidePadding - totalCardMiddleSpacing;
            currentWidth = availableWidth / cardTotal;
            currentHeight = currentWidth * ratio;
        }else {
            //adjust side padding to center cards
            float free_space = displaySize.Width - neededWidth;
            currentSidePadding += free_space / 2;
            // currentSidePadding = free_space / 2;
        }
        //check height
        Debug.Log("Height check:"+  displaySize.Height +"current height"+ currentHeight);
        if (displaySize.Height < currentHeight)
        {
            Debug.Log("Height too big");
            // if height is not enough make smaller cards (perfect fit)
            if (displaySize.Height < 2f)
            {
                ///Too litte vertical space
                Debug.Log("display height"+ displaySize.Height);
                Debug.Log("This should never happen");
                Assert.IsFalse(true);
            }
            currentHeight = displaySize.Height - CardSidePadding;
            currentWidth = currentHeight / ratio;
            neededWidth =(currentWidth*cardTotal) + ((cardTotal -1)* CardMiddleSpacing);
            float free_space = displaySize.Width - neededWidth;
            currentSidePadding = free_space / 2;
        }
        //calculate card positions
        Debug.Log("about to calculate card positions");
        List<WindowSize> cardDimensionList = new List<WindowSize>(cardTotal);

        Debug.Log("LeftBorder" + displaySize.LeftBorder);
        Debug.Log("RightBorder" + displaySize.RightBorder);
        Debug.Log("currentHeight:" + currentHeight);
        Debug.Log("full width:" + displaySize.Width);
        Debug.Log("neededWidth:" + neededWidth);
        Debug.Log("currentSidePadding" + currentSidePadding);
        float leftBorder = displaySize.LeftBorder + currentSidePadding;
        cardDimensionList.AddRange(Enumerable.Range(0, cardTotal)
            .Select(pos =>
            {
                var size = new WindowSize(
                    currentWidth,
                    currentHeight,
                    leftBorder,
                    leftBorder + currentWidth,
                    displaySize.TopBorder - (displaySize.Height / 2) + (currentHeight / 2),
                    displaySize.BottomBorder + (displaySize.Height / 2) - (currentHeight / 2)
                );
                leftBorder += CardMiddleSpacing + currentWidth;
                return size;
            }));
        return cardDimensionList;
    }
}
