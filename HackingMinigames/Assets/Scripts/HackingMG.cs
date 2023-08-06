using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
public class HackingMG : MiniGame
{
    private List<Card> _cardDeck;
    private CardFactory _cardFactory;
    private int _tileAmount = Game.Instance.defaultTileAmount;
    private List<int> _orderList;

    public override void  Initialize(WindowSize hackWindowDimensions, GameWindow currentWindow)
    {
        _minigameType = MinigameType.HACK;
        _gameWindow = currentWindow;
        _cardDeck = new List<Card>(_tileAmount);
        _cardFactory = new CardFactory();
        _hackWindowDimensions = hackWindowDimensions;
        // fillCardDeck();
        _puzzleTimer = this.AddComponent<PuzzleTimer>();
        _puzzleTimer.Initialize(this);
    }

    public override void StartMinigame()
    {
        ToggleCards(true, true);
    }    
    public override void EndMinigame()
    {
        _gameWindow.questionTextFieldObject.SetActive(true);
        _gameWindow.streakText.text = "Streak: 0";
        _gameWindow.questionTextField.text = " ";
        _gameWindow.questionInputField.text = " ";
        _cardDeck.ForEach(card =>
        {
            card.StopAllCoroutines();
            Destroy(card.gameObject);
        });
        StopAllCoroutines();
    }

    public override void RetryMinigame()
    {
        StopAllCoroutines();

        _gameWindow.questionTextFieldObject.SetActive(true);
        _gameWindow.streakText.text = "Streak: 0";
        _gameWindow.questionTextField.text = "retry";
        _gameWindow.questionInputField.text = "";
        // questionTextFieldObject.SetActive(false);

        _puzzleTimer.reset_timer();
        StartMinigame();
    }
    private void continueHacks()
    {
        //wait 1 second
        ToggleCards(true);
    }
    
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
    
    public IEnumerator flipCards(bool isCardReveal = false)
    {

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

    

}
