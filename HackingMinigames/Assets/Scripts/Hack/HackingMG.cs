using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HackingMG : MiniGame
{
    
    public HackSettings _internalSettings;
    private HackSettingsButtonManager _uiSettings;

    private GameObject _cardDeckGameObject;
    private List<Card> _cardDeck;
    private List<int> _orderList;
    private BUIhack _bottomUI;
    private UUIhack _upperUI;
    public int currentStreak;


    protected override void  InitializeDerivative()
    {
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
        StartCoroutine(ToggleCards(true));
    }    
    public override void EndMinigame()
    {
        _bottomUI.questionTextFieldObject.SetActive(true);
        currentStreak = 0;
        _upperUI.resetStreak();
        _bottomUI.questionTextField.text = " ";
        _bottomUI.questionInputField.text = " ";
        _cardDeck.ForEach(card =>
        {
            card.StopAllCoroutines();
        });
        StopAllCoroutines();
        // Destroy(_puzzleTimer);
        // Destroy(this);
    }

    public override void RetryMinigame()
    {
        StopAllCoroutines();

        _bottomUI.questionTextFieldObject.SetActive(true);
        currentStreak = 0;
        _upperUI.resetStreak();
        _bottomUI.questionTextField.text = "retry";
        _bottomUI.questionInputField.text = "";
        // questionTextFieldObject.SetActive(false);

        _puzzleTimer.reset_timer();
        StartCoroutine(ToggleCards(true));

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
        StartCoroutine(ToggleCards(true));
    }
    
    public void fillCardDeck()
    {
        FillCardDeckRoutine();
    }
    private void FillCardDeckRoutine()
    {
        SetupCardDeckGameObject();
        Debug.Log("got all dimensions");
        _orderList = RandomFactory.GetOrderList(_internalSettings.currentCardTotal);
        _cardDeck.AddRange(Enumerable.Range(0, _internalSettings.currentCardTotal)
            .Select(i =>
            {
                var cardGameObject = ComponentHandler.AddChildGameObject(_cardDeckGameObject, "Card" + i);
                var card = cardGameObject.AddComponent<Card>();
                card.Initialize(_orderList[i]);
                return card;
            }));

    }

    private void SetupCardDeckGameObject()
    {
        if (_cardDeckGameObject) {
            Destroy(_cardDeckGameObject);
        }
        _cardDeckGameObject = ComponentHandler.AddChildGameObject(mgPanel.gameObject, "Game");
        ComponentHandler.SetAnchorToStretch(_cardDeckGameObject);
        ComponentHandler.AddMaximisedGridLayout(_cardDeckGameObject, ratio: 600f / 895);
        _cardDeck = new List<Card>(_internalSettings.currentCardTotal);

    }

    public IEnumerator ToggleCards(bool toggle)
    {
        //TODO THIS WAIT IS ANIMATION DUROATION FIX need to only happen at start
        fillCardDeck();
        

        _cardDeck.ForEach(card =>
        { 
            card.gameObject.SetActive(toggle);
           ///TODO FILL CURTAIN UP
           StartCoroutine(card.cardCover.PullCurtainUp());

        });
        yield return new WaitUntil(() => CardCover.CheckAllCurtainsUp(_internalSettings.currentCardTotal));

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

            if (isCardReveal)
            {
                StartCoroutine(card.cardCover.PullCurtainDown());
            }

        }
        Debug.Log("card reveal");
        if (isCardReveal) {
            yield return new WaitUntil(CardCover.CheckAllCurtainsDown);
        }

        foreach (var card in _cardDeck)
        {
            if (isCardReveal)
            {
                card.cardImage.sprite = Game.Instance.cardOrderSheet[10];
                card.cardCover.HideCover();
            }

            //TODO YOU NEED TO WAIT TILL ROTATE FINISHES
            StartCoroutine(card.RotateCard());
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
                _upperUI.updateStreak(++currentStreak);
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
            mgPanel.gameWindow.highscoreBoard.UpdateHighscore("Best Streak", _internalSettings.BestStreak[_internalSettings.currentCardTotal]);
        }
        yield return flipCards();
        yield return null;
    }


    

}
