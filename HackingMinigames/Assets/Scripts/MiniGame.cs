using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class MiniGame : MonoBehaviour
{
    private int _tileAmount = Game.Instance.defaultTileAmount;
    private List<Card> _cardDeck;
    private CardFactory _cardFactory;
    private WindowSize _hackWindowDimensions;
    public GameWindow _gameWindow;
    private List<int> _orderList;
    public PuzzleTimer _puzzleTimer;
    private int currentStreak = 0;

    //initialization
    public void Initialize(WindowSize hackWindowDimensions, GameWindow window)
    {
        _gameWindow = window;
        _cardDeck = new List<Card>(_tileAmount);
        _cardFactory = new CardFactory();
        _hackWindowDimensions = hackWindowDimensions;
        // fillCardDeck();
        _puzzleTimer = this.AddComponent<PuzzleTimer>();
        _puzzleTimer.Initialize(this);
    }

    public IEnumerator StartIntroTimer()
    {
        Debug.Log("Checking Time");
        _puzzleTimer.startTimer();
        while (!_puzzleTimer.introFinished())
        {
            yield return null;
            
        }
        Debug.Log("Timer check 1");
        flipCards(true);
        yield return new WaitForSeconds(1);
        var question = _gameWindow.SetQuestion(_tileAmount, _cardDeck);
        StartCoroutine(StartGameTimer(question));

    }

    public IEnumerator StartGameTimer(string question)
    {
        Debug.Log("Game timer started");
        _puzzleTimer.startPuzzleTimer();
        while (!_puzzleTimer.IsGameOver())
        {
            if (_gameWindow.CheckAnswer(PuzzleFactory.getQuestionSolution(_cardDeck, question)))
            {
                Debug.Log("Game success");
                _gameWindow.streakText.text = "Streak: "+ (++currentStreak);
                _cardDeck.ForEach(card => card.backSprite =Game.Instance.cardOrderSheet[10]);
                flipCards();
                _puzzleTimer.reset_timer();
                yield return new WaitForSeconds(1);
                continueHacks();
                yield break;
            }
            Debug.Log("Timer check 2");
            yield return null;

        }
        Debug.Log("Timer check hello");
        Debug.Log("Timer check 3");
        flipCards();
        yield return null;
    }

    private void continueHacks()
    {
        //wait 1 second
        ToggleCards(true);
    }

    //getter setter
    public int TileAmount
    {
        get => _tileAmount;
        set
        {
            _tileAmount = value;
            // fillCardDeck();
        }
    }
    //methods
    public void fillCardDeck()
    {
        StopCoroutine("FillCardDeckRoutine");
        StartCoroutine("FillCardDeckRoutine",FillCardDeckRoutine());
    }
    private IEnumerator FillCardDeckRoutine()
    {
        Debug.Log("FillCardDeckRoutine");
        _cardDeck.ForEach(card => Destroy(card.gameObject));
        _cardDeck = new List<Card>(_tileAmount);
        var cardDimensions = _cardFactory.getAllCardDimensions(_tileAmount, _hackWindowDimensions);
        Debug.Log("got all dimensions");
        _orderList = RandomFactory.GetOrderList(_tileAmount);
        _cardDeck.AddRange(Enumerable.Range(0, _tileAmount)
            .Select(i => {
                var tmpObject = new GameObject("Card"+i);
                var card = tmpObject.AddComponent<Card>();
                card.Initialize(cardDimensions[i], _gameWindow, _orderList[i]);
                return card;
            }));

        yield return null; 
    }

    public void destr()
    {
        _cardDeck.ForEach(card => Destroy(card.gameObject));

    }
    public void ToggleCards(bool toggle, bool isStart  =false)
    {
        //TODO THIS WAIT IS ANIMATION DUROATION FIX need to only happen at start
        fillCardDeck();
        // if (isStart)
        // {
        //     yield return new WaitForSeconds(0.3f);
        //
        // }
        _cardDeck.ForEach(card =>
        { 
            card.gameObject.SetActive(toggle);
            card.cardAnimator.SetTrigger("pullCurtainUp");
        });
        Debug.Log("about to start timer");
        StartCoroutine( StartIntroTimer());
    }
    public void flipCards(bool isCardReveal = false)
    {
        
        foreach (var card in _cardDeck)
        {
            StartCoroutine(pullCurtainDown(card, isCardReveal));
        }
        
    }
    private IEnumerator disableCurtain(Card card)
    {
        // Wait until the animation finishes playing
        while (!card.cardAnimator.GetCurrentAnimatorStateInfo(0).IsName("pullFinished"))
        {
            yield return null;
        }

        card.cardCover.SetActive(false);
        // Animation has finished, continue with other actions if needed
    }
    private IEnumerator pullCurtainDown(Card card, bool isCardReveal = false)
    {
        if (isCardReveal)
        {
            Debug.Log("curtain down start");
            card.cardAnimator.SetTrigger("pullCurtainDown");
            // Wait until the animation finishes playing
            yield return new WaitForSeconds(1);
            StartCoroutine(card.RotateCard());
            //tmp back to cover
            card._cardRenderer.sprite = Game.Instance.cardOrderSheet[10];
            StartCoroutine(disableCurtain(card));
            Debug.Log("curtain down");
        }
        else
        {
            StartCoroutine(card.RotateCard());

        }

    }


}

