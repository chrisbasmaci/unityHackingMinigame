using System.Collections;
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
        string question ="";
        var questionFirst = Game.Instance.questionFirstToggle;
        // if (questionFirst)
        // {
        //     Debug.Log("Set question");
        //
        //     question = _gameWindow.SetQuestion(_tileAmount, _cardDeck);
        // }

        _puzzleTimer.startTimer();

        while (!_puzzleTimer.introFinished())
        {
            yield return null;
            
        }
        yield return flipCards(true);
        Debug.Log("flip cards end");

        if (!questionFirst)
        { 
            question = _gameWindow.SetQuestion(_tileAmount, _cardDeck);
        }
        StartCoroutine(StartGameTimer(question));

    }

    public IEnumerator StartGameTimer(string question)
    {
        Debug.Log("Game timer started");
        if (Game.Instance.questionFirstToggle)
        {
            _gameWindow.questionTextFieldObject.SetActive(false);
        }
        
        _puzzleTimer.startPuzzleTimer();
        string correctAnswer = PuzzleFactory.getQuestionSolution(_cardDeck, question);
        while (!_puzzleTimer.IsGameOver())
        {
            if (_gameWindow.CheckAnswer(correctAnswer))
            {

                _gameWindow.questionTextFieldObject.SetActive(true);
                Debug.Log("Game success");
                _gameWindow.streakText.text = "Streak: "+ (++currentStreak);
                _cardDeck.ForEach(card => card.backSprite =Game.Instance.cardOrderSheet[10]);
                yield return flipCards();
                _puzzleTimer.reset_timer();
                continueHacks();
                yield break;
            }
            yield return null;

        }
        yield return flipCards();
        _gameWindow.questionTextFieldObject.SetActive(true);
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
        // StopCoroutine("FillCardDeckRoutine");
        FillCardDeckRoutine();
    }
    private void FillCardDeckRoutine()
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

    }

    public void destr()
    {
        _cardDeck.ForEach(card => Destroy(card.gameObject));

    }
    public void ToggleCards(bool toggle, bool isStart  =false)
    {
        StopAllCoroutines();
        //TODO THIS WAIT IS ANIMATION DUROATION FIX need to only happen at start
        fillCardDeck();
        // if (isStart)
        // {
        //     yield return new WaitForSeconds(0.3f);
        //
        // }
        _gameWindow.enableRetry();

        _cardDeck.ForEach(card =>
        { 
            card.gameObject.SetActive(toggle);
            card.cardAnimator.SetTrigger("pullCurtainUp");
        });
        Debug.Log("about to start timer");
        StartCoroutine( StartIntroTimer());
    }
    public IEnumerator flipCards(bool isCardReveal = false)
    {
        if (!_gameWindow.isRetryable())
        {
            yield break;
        }
        _gameWindow.disableRetry();
        int flippedCount = 0;
        foreach (var card in _cardDeck)
        {
            if (!card.isFlippable())
            {
                yield return null;
            }
            else
            {
                card.disableFlipping();
            }
            StartCoroutine(pullCurtainDown(card, isCardReveal));
        }

        foreach (var card in _cardDeck)
        {
            yield return new WaitUntil(() => card.isFlippable());
        }
        _gameWindow.enableRetry();

    }
    private void disableCurtain(Card card)
    {
        // Wait until the animation finishes playing
        // while (card  && !card.cardAnimator.GetCurrentAnimatorStateInfo(0).IsName("pullFinished"))
        // {
        //     yield return null;
        // }

        // Check if the card reference is still valid
        if (card)
        {
            card.cardCover.SetActive(false);
            // Animation has finished, continue with other actions if needed
        }
    }
    private IEnumerator pullCurtainDown(Card card, bool isCardReveal = false)
    {
        if (isCardReveal)
        {
            Debug.Log("curtain down start");
            card.cardAnimator.SetTrigger("pullCurtainDown");
            // Wait until the animation finishes playing
            yield return new WaitForSeconds(1);

            card._cardRenderer.sprite = Game.Instance.cardOrderSheet[10];
            disableCurtain(card);
            yield return card.RotateCard();
        
            //tmp back to cover

            Debug.Log("curtain down");
        }
        else
        {
            yield return card.RotateCard();
        }


    }

    public bool flipCardBacks()
    {
        bool needed = false;
        foreach (var card in _cardDeck)
        {
            card.disableFlipping();
            if (card.isCardFacedUp())
            {
                needed = true;
            }

            if (card)
            {
                StartCoroutine(card.RotateCard(true));
            }
        }

        return needed;
    }




}

