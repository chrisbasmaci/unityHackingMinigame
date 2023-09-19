using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UI_Prefab
{
    public abstract class CustomUIComponent: MonoBehaviour
    {
        public abstract void Setup();
        public abstract void Configure();

        public void Init()
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