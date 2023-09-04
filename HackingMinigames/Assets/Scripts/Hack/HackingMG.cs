using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Tilemaps;

public class HackingMG : MiniGame
{
    
    public HackSettings _internalSettings;
    private HackSettingsButtonManager _uiSettings;

    private List<Card> _cardDeck;
    private CardFactory _cardFactory;
    private List<int> _orderList;
    private BUIhack _bottomUI;
    private UUIhack _upperUI;
    

    protected override void  InitializeDerivative()
    {
        _cardFactory = new CardFactory();
    }
    public override GameObject getUpperSettingPrefab()
    {
        var upperSettings = Resources.Load<GameObject>("Prefabs/Hack/Settings/SubSettingsPanel");
        var _uiSettings = upperSettings.GetComponent<HackSettingsButtonManager>();
        return upperSettings;
    }


    public override void ChildStartMinigame()
    {
        _internalSettings = (HackSettings)Settings;
        _puzzleTimer.Initialize(ref _bottomUI.loadingbarTimer, Settings);
    
        _cardDeck = new List<Card>(_internalSettings.currentCardTotal);
        ToggleCards(true, true);
    }    
    public override void EndMinigame()
    {
        _bottomUI.questionTextFieldObject.SetActive(true);
        _upperUI.streakText.text = "Streak: 0";
        _bottomUI.questionTextField.text = " ";
        _bottomUI.questionInputField.text = " ";
        _cardDeck.ForEach(card =>
        {
            card.StopAllCoroutines();
        });
        CleanDeck();
        StopAllCoroutines();
        // Destroy(_puzzleTimer);
        // Destroy(this);
    }

    public override void RetryMinigame()
    {
        StopAllCoroutines();

        _bottomUI.questionTextFieldObject.SetActive(true);
        _upperUI.streakText.text = "Streak: 0";
        _bottomUI.questionTextField.text = "retry";
        _bottomUI.questionInputField.text = "";
        // questionTextFieldObject.SetActive(false);

        _puzzleTimer.reset_timer();
        ToggleCards(true, false);

    }
    protected override void InitBottomUI()
    {
        mgPanel.gameWindow.BUIPanel = Instantiate(Game.Instance.bottomHackPrefab, mgPanel.gameWindow.bottomContainer.transform)
            .GetComponent<BUIhack>();
        _bottomUI = (BUIhack)mgPanel.gameWindow.BUIPanel;
        _bottomUI.InitializeRightButton(RetryMinigame);
    }

    protected override void InitUpperUI()
    {
        mgPanel.gameWindow.UUIpanel = Instantiate(Game.Instance.upperHackPrefab, mgPanel.gameWindow.upperContainer.transform)
            .GetComponent<UUIhack>();
        _upperUI = (UUIhack)mgPanel.gameWindow.UUIpanel;
    }

    private void continueHacks()
    {
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
        CleanDeck();
        var cardDimensions = _cardFactory.getAllCardDimensions(_internalSettings.currentCardTotal, mgPanel.panelBounds);
        Debug.Log("got all dimensions");
        _orderList = RandomFactory.GetOrderList(_internalSettings.currentCardTotal);
        _cardDeck.AddRange(Enumerable.Range(0, _internalSettings.currentCardTotal)
            .Select(i => {
                var tmpObject = new GameObject("Card"+i);
                var card = tmpObject.AddComponent<Card>();
                card.Initialize(cardDimensions[i], mgPanel, _orderList[i]);
                return card;
            }));

    }
    private void CleanDeck()
    {
        _cardDeck.ForEach(card => Destroy(card.gameObject));
        _cardDeck = new List<Card>(_internalSettings.currentCardTotal);
    }
    
    public void ToggleCards(bool toggle, bool isStart  =false)
    {
        //TODO THIS WAIT IS ANIMATION DUROATION FIX need to only happen at start
        fillCardDeck();


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

        _puzzleTimer.startIntroTimer();

        while (!_puzzleTimer.introFinished())
        {
            yield return null;
            
        }
        yield return flipCards(true);
        Debug.Log("flip cards end");

        if (!questionFirst)
        { 
            question = _bottomUI.SetQuestion(_internalSettings.currentCardTotal, _cardDeck);
        }
        StartCoroutine(StartGameTimer(question));

    }

    public IEnumerator StartGameTimer(string question)
    {
        Debug.Log("Game timer started");
        if (Game.Instance.questionFirstToggle)
        {
            _bottomUI.questionTextFieldObject.SetActive(false);
        }
        
        _puzzleTimer.startPuzzleTimer();
        string correctAnswer = PuzzleFactory.getQuestionSolution(_cardDeck, question);
        while (!_puzzleTimer.IsGameOver())
        {
            if (_bottomUI.CheckAnswer(correctAnswer))
            {
                Debug.Log("Game success");
                _upperUI.streakText.text = "Streak: "+ (++currentStreak);
                _cardDeck.ForEach(card => card.backSprite =Game.Instance.cardOrderSheet[10]);
                yield return flipCards();
                _puzzleTimer.reset_timer();
                continueHacks();
                yield break;
            }
            yield return null;

        }
        _internalSettings.UpdateRecords(currentStreak);
        if (mgPanel.gameWindow.highscoreBoardPanel)
        {
            mgPanel.gameWindow.highscoreBoard.UpdateHighscore("Minimum Moves", _internalSettings.BestStreak[_internalSettings.currentCardTotal]);
        }
        yield return flipCards();
        yield return null;
    }


    

}
