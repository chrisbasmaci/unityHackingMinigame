
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CardBack: CardSide
{
    protected override void InitializeSide()
    {
        Debug.Log(_card.cardOrder);
        _card.backSprite = Game.Instance.cardOrderSheet[_card.cardOrder];
        ComponentHandler.SetAnchorToStretch(gameObject);
    }

    
    

}
