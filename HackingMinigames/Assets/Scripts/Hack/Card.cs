using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Card: MonoBehaviour
{
    public Image cardImage;
    public GameObject backGameObject, faceGameObject;
    private CardBack _back;
    public CardFace _face;
    public Sprite faceSprite;
    public Sprite backSprite;
    private bool coroutineAllowed, facedUp;
    private bool flippingEnabled = true;
    private EdgeCollider2D _collider;
    public bool isStartingSideBack =true;
    
    // private GameObject gameObject;
    public int cardOrder;
    public Animator cardAnimator;
    public GameObject cardCover;
    public bool isWanted;
    public int wantedOrder;
    
    public bool isCardFacedUp()
    {
        return facedUp;
    }
    public void Initialize(int order)
    {
        cardOrder = order;
  
        gameObject.AddComponent<RectTransform>();
        cardImage = ComponentHandler.AddImageComponent(gameObject);
        gameObject.SetActive(false);

        InitTriggers();
        InitSides();
    }
    
    private void OnMouseDown()
    {
        Debug.Log("PRESSED CARD");
        if (flippingEnabled && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }



    private void InitSides()
    {
        //add backgameobject
        Debug.Log("init sides" );

        backGameObject = new GameObject("CardBack");
        backGameObject.transform.SetParent(gameObject.transform, false);
        _back = backGameObject.AddComponent<CardBack>(); 
        _back.Initialize(this);

        //add facegameobject
        faceGameObject = new GameObject("CardFace");
        faceGameObject.transform.SetParent(gameObject.transform, false);
        _face = faceGameObject.AddComponent<CardFace>();
        _face.Initialize(this);

        //starting sprite
        Debug.Log("init pos sprite");
        cardCover = Instantiate(Game.Instance.cardBackPrefab, gameObject.transform, false);
        cardCover.transform.localPosition = new Vector3(0, 0 , 0);
        cardAnimator = cardCover.AddComponent<Animator>();
        cardAnimator.runtimeAnimatorController = Game.Instance.curtainController;
    }

    private void InitTriggers()
    {
        var eventTrigger = gameObject.AddComponent<EventTrigger>();
        EventHandler.AddTrigger(eventTrigger, EventTriggerType.PointerClick, OnImageClick);
    }


    

    public void OnImageClick()
    {
        // Handle the click event here
        Debug.Log("PRESSED CARD");
        if (flippingEnabled && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }    
    }
    public void enableFlipping()
    {
        flippingEnabled = true;
    }    
    public void disableFlipping()
    {
        flippingEnabled = false;
    }    
    public bool isFlippable()
    {
        return flippingEnabled;
    }    
    

    public IEnumerator RotateCard(bool isBackflip = false)
    {
        
        coroutineAllowed = false;
        if (!facedUp)
        {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    cardImage.sprite = faceSprite;
                    _face.gameObject.SetActive(true);

                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        else if (facedUp)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    cardImage.sprite = backSprite;
                    _face.gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        
        coroutineAllowed = true;
  
        facedUp = !facedUp;
        enableFlipping();

    }
}
