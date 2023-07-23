using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Card: MonoBehaviour
{
    
    public GameObject backGameObject, faceGameObject;
    private CardBack _back;
    public CardFace _face;
    public Sprite faceSprite, backSprite;
    private bool coroutineAllowed, facedUp, flippingEnabled;
    private BoxCollider2D _collider;
    public bool isStartingSideBack =true;
    
    public WindowSize _cardDimensions;
    // private GameObject gameObject;
    private GameWindow _gameWindow;
    public SpriteRenderer _cardRenderer;
    public int cardOrder;
    public Animator cardAnimator;
    public GameObject cardCover;
    public bool isWanted;
    public int wantedOrder;
    public bool cardBeingFlipped = false;
    public Sprite CurrentSprite
    {
        get { return _cardRenderer.sprite; }
        set { _cardRenderer.sprite = value;
            if (value == faceSprite) {
                _face.InitPosScale();
                _collider.size = new Vector2(faceSprite.bounds.size.x, faceSprite.bounds.size.y);
            }else {
                _back.InitPosScale();
                _collider.size = new Vector2(backSprite.bounds.size.x, backSprite.bounds.size.y);
    
            }
        }
    }

    public bool isCardFacedUp()
    {
        return facedUp;
    }
    public void Initialize(WindowSize cardDimensions, GameWindow window, int order)
    {
        cardOrder = order;
        Debug.Log("Card added left pos: " + cardDimensions.LeftBorder + "len:"+cardDimensions.Height);
        _gameWindow = window;
        _cardDimensions = cardDimensions;
        _cardRenderer = gameObject.AddComponent<SpriteRenderer>();
        gameObject.transform.SetParent(_gameWindow.transform, false);
        
        InitCollider();
        InitPosition();
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

    private void InitPosition()
    {
        var pos = new Vector3(_cardDimensions.LeftBorder + (_cardDimensions.Width / 2),  _cardDimensions.BottomBorder + (_cardDimensions.Height / 2), -1);
        gameObject.transform.localPosition = pos;
        gameObject.SetActive(false);
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
        CurrentSprite = backSprite;
        cardCover = Instantiate(Game.Instance.cardBackPrefab, gameObject.transform, false);
        cardCover.transform.localPosition = new Vector3(0, 0 , 0);
        cardAnimator = cardCover.AddComponent<Animator>();
        cardAnimator.runtimeAnimatorController = Game.Instance.curtainController;
    }

    private void InitCollider()
    {
        //collider
        _collider = gameObject.AddComponent<BoxCollider2D>();
        // _collider.size = new Vector2(_cardDimensions.Width, _cardDimensions.Height);
    }

    public void enableFlipping()
    {
        flippingEnabled =true;
    }    
    public void disableFlipping()
    {
        flippingEnabled = false;
    }


    public IEnumerator RotateCard(bool isBackflip = false)
    {
        
        coroutineAllowed = false;
        if (isBackflip && !facedUp)
        {
            coroutineAllowed = true;

            yield break;
        }

        if (!facedUp)
        {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    CurrentSprite = faceSprite;
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
                    CurrentSprite = backSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        
        coroutineAllowed = true;

        facedUp = !facedUp;


    }
}
