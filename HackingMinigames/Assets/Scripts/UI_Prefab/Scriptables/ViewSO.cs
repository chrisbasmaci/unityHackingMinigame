using UnityEngine;

namespace UI_Prefab
{
    [CreateAssetMenu(menuName = "Assets/Scripts/UI_Prefab/Scriptables/ViewSO", fileName  = "ViewSO")]
    public class ViewSO :ScriptableObject
    {
        public RectOffset padding;
        public float spacing;
    }
}