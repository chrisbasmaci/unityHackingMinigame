using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

///Source:
///https://www.youtube.com/watch?v=oOQvhIg0ntg&ab_channel=ThisisGameDev
///The code is based from this video, with my tweaks
namespace UI_Prefab 
{
    public class VerticalTriPanel : CustomUIComponent
    {
        public ViewSO viewData;

        public GameObject containerTop;
        public GameObject containerCenter;
        public GameObject containerBottom;
        
        private Image imageTop;
        private Image imageCenter;
        private Image imageBottom;

        private VerticalLayoutGroup verticalLayoutGroup;
        
        public override void Setup()
        {
            verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            imageTop = containerTop.GetComponent<Image>();
            imageCenter = containerCenter.GetComponent<Image>();
            imageBottom = containerBottom.GetComponent<Image>();
            
        }
        public override void Configure()
        {
            verticalLayoutGroup.padding = viewData.padding;
            verticalLayoutGroup.spacing = viewData.spacing;

        }
        
    }
}