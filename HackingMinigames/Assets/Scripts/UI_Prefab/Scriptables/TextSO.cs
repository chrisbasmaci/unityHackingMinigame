using TMPro;
using UnityEngine;

namespace UI_Prefab
{
    [CreateAssetMenu(menuName = "Assets/Scripts/UI_Prefab/Scriptables/TextSO", fileName  = "TextSO")]
    public class TextSO : ScriptableObject
    {
        public ThemeSO theme;
        public TMP_FontAsset font;
        public float size;

    }
}