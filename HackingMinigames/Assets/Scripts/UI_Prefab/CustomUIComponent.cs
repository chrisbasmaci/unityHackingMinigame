using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UI_Prefab
{
    public abstract class CustomUIComponent: MonoBehaviour
    {
        // private void Awake()
        // {
        //     Init();
        // }

        public abstract void Setup();
        public abstract void Configure();

        protected void Init()
        {
            Setup();
            Configure();
        }

        private void OnValidate()
        {
            Init();
        }
    }
}