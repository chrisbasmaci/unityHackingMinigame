using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.Serialization;
// public enum CardSideType
// {
//     FACE,
//     BACK
// }
public abstract class CardSide : MonoBehaviour
{
    protected Card _card;
    
    public void Initialize(Card card)
    {
        _card = card;

        
        InitializeSide();
        //set sprite
        
        // gameObject.SetActive(true);
    }

    protected abstract void InitializeSide();
    public void InitPosScale()
    {
        //setposition
        
        float currentWidth = _card._cardRenderer.sprite.bounds.size.x;
        float currentHeight = _card._cardRenderer.sprite.bounds.size.y;
        Debug.Log("currentWidth: " + currentWidth + " currentHeight: " + currentHeight);
        
    }
    
}
