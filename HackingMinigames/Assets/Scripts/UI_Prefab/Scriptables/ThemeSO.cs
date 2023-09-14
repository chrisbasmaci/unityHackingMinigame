using UnityEngine;

namespace UI_Prefab
{
    [CreateAssetMenu(menuName = "Assets/Scripts/UI_Prefab/Scriptables/ThemeSO", fileName  = "ThemeSO")]
    public class ThemeSO :ScriptableObject
    {
        [Header("Primary")] 
        public Color PrimaryBg;
        public Color PrimaryText;
        
        [Header("Secondary")] 
        public Color SecondaryBg;
        public Color SecondaryText;
        
        [Header("Tertiary")] 
        public Color TertiaryBg;
        public Color TertiaryText;

        [Header("Other")] public Color disable;

        public Color GetTextOrBgColor(Style style, bool wantBg =false)
        {
            switch (style){
                case Style.Primary:
                    return (wantBg)? PrimaryBg : PrimaryText;
                case Style.Secondary:
                    return (wantBg)? SecondaryBg : SecondaryText;
                case Style.Tertiary:
                    return (wantBg)? TertiaryBg : TertiaryText;
                default:
                    return disable;
            }
        }
        



    }
}