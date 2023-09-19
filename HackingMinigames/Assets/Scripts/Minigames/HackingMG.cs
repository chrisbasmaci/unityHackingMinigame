using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Hack;
using UnityEditor;

public class HackingMG : MiniGame
{
    
    private HackSettings InternalSettings => Settings as HackSettings;
    private BUIhack HackBottomUI => BottomUI as BUIhack;
    private UUIhack HackUpperUI => UpperUI as UUIhack;
    
    private HackUsi _uiSettings;

    private GameObject _cardDeckGameObject;
    private List<Card> _cardDeck;
    private List<int> _orderList;


    protected override void  InitializeDerivative()
    {
        _puzzleTimer.InitializeLoadingBar(HackBottomUI.loadingbarTimer);
    }
    protected override UIPanel InitBottomUIChild()
    {
        mgPanel.gameWindow.BUIPanel = Instantiate(Game.Instance.bottomHackPrefab, mgPanel.gameWindow.bottomContainer.transform)
            .GetComponent<BUIhack>();
        var bottomUI = (BUIhack)mgPanel.gameWindow.BUIPanel;
        bottomUI.InitializeRightButton(RetryMinigame);
        return bottomUI;
    }

    protected override UIPanel InitUpperUIChild()
    {
        mgPanel.gameWindow.UUIpanel = Instantiate(Game.Instance.upperHackPrefab, mgPanel.gameWindow.upperContainer.transform)
            .GetComponent<UUIhack>();
        var upperUI = (UUIhack)mgPanel.gameWindow.UUIpanel;
        return upperUI;
    }

    public override MgSettings AddSettings()
    {
        return new HackSettings();
    }

    public override GameObject InstantiateUpperSettings()
    {
        var upperSettings = Resources.Load<GameObject>("Prefabs/Hack/Settings/SubSettingsPanel");
        var _uiSettings = upperSettings.GetComponent<HackUsi>();
        return upperSettings;
    }


    public override void StartMinigameChild()
    {

    
        _cardDeck = new List<Card>(InternalSettings.currentCardTotal);
        StartCoroutine(ToggleCards(true));
    }    
    public override void EndMinigame()
    {
        base.EndMinigame();
        //NOTE If you need replay, you shouldnt destroy this here
        Destroy(_cardDeckGameObject);
        _cardDeck.ForEach(card =>
        {
            card.StopAllCoroutines();
        });
    }

    public override void RetryMinigame()
    {
        base.RetryMinigame();
        StartCoroutine(ToggleCards(true));

    }

    private void continueHacks()
    {
        EndRound();
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
        _orderList = RandomFactory.GetOrderList(InternalSettings.currentCardTotal);
        _cardDeck.AddRange(Enumerable.Range(0, InternalSettings.currentCardTotal)
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
        _cardDeck = new List<Card>(InternalSettings.currentCardTotal);

    }

    public IEnumerator ToggleCards(bool toggle)
    {
        fillCardDeck();
        

        _cardDeck.ForEach(card =>
        { 
            card.gameObject.SetActive(toggle);
           StartCoroutine(card.cardCover.PullCurtainUp());

        });
        yield return new WaitUntil(() => CardCover.CheckAllCurtainsUp(InternalSettings.currentCardTotal));

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

        while (!_cardDeck.All(card => card.isFlippable()))
        {
            yield return null; 
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
            question = HackBottomUI.SetQuestion(InternalSettings.currentCardTotal, _cardDeck);
        }
        StartCoroutine(StartGameTimer(question));

    }

    public IEnumerator StartGameTimer(string question)
    {
        Debug.Log("Game timer started");
        if (Game.Instance.questionFirstToggle)
        {
            HackBottomUI.questionTextFieldObject.SetActive(false);
        }
        
        _puzzleTimer.startPuzzleTimer();
        string correctAnswer = PuzzleFactory.getQuestionSolution(_cardDeck, question);
        while (!_puzzleTimer.IsGameOver())
        {
            if (HackBottomUI.CheckAnswer(correctAnswer))
            {
                Debug.Log("Game success");
                HackUpperUI.updateStreak(++InternalSettings.currentStreak);
                _cardDeck.ForEach(card => card.backSprite =Game.Instance.cardOrderSheet[10]);
                yield return flipCards();
                continueHacks();
                yield break;
            }
            yield return null;
        }
        EndRound();
        HackBottomUI.ShowAnswer(correctAnswer);
        
        yield return flipCards();
        yield return null;
    }


    

}
