
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using TriangleNet;
using TriangleNet.Geometry;
using UnityEngine.UI.Extensions;

public sealed class CardFactory :MonoBehaviour
{
    private FlowLayoutGroup _layoutGroup;
    private RectTransform _rectTransform;

    void Start ()
    {
        _layoutGroup = GetComponent<FlowLayoutGroup> ();
        _rectTransform = GetComponent<RectTransform> ();

    }
    public void Main()
    {
        float largerWidth = _rectTransform.rect.width;
        float largerHeight = _rectTransform.rect.height;

        float ratio = 1.4f;  // Your fixed aspect ratio

        Tuple<float, float> dimensions = CalculateRectangleDimensions(largerWidth, largerHeight, ratio);

        Console.WriteLine($"Maximum dimensions for the rectangles with a ratio of {ratio}: {dimensions.Item1}x{dimensions.Item2}");
    }

    public Tuple<float, float> CalculateRectangleDimensions(float largerWidth, float largerHeight, float ratio)
    {
        float widthScale = largerWidth / (1 + ratio);
        float heightScale = widthScale * ratio;

        return new Tuple<float, float>(widthScale, heightScale);
    }
}
