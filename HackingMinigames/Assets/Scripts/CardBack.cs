
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CardBack: CardSide
{
    protected override void InitializeSide()
    {
        Debug.Log(_card.cardOrder);
        _card.backSprite = Game.Instance.cardOrderSheet[_card.cardOrder];
        _card._cardRenderer.sprite = _card.backSprite;
    }

    
    

}
