using Game.Board;
using Game.Cards;
using Game.Interactions;
using System;
using UnityEngine;

public class SimpleGame
{
    #region Public

    public float Time { get; private set; }
    public int Score { get; private set; }
    public int Moves { get; private set; }

    public static event Action<int> OnTimeChanged;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnMovesChanged;

    public static event Action OnGameStarted;
    public static event Action OnGameWon;
    
    public SimpleGame(Deck deck, StockPile stockPile, WastePile wastePile, FoundationPile[] foundationPiles, TableauPile[] tableauPiles)
    {
        _deck = deck;
        _stockPile = stockPile;
        _wastePile = wastePile;
        _foundationPiles = foundationPiles;
        _tableauPiles = tableauPiles;

        CommandSystem.OnMoveMade += HandleMoveMade;
        CommandSystem.OnMoveUnmade += HandleMoveUnmade;
        Scoring.OnScoreChanged += HandleScoreChanged;
    }

    ~SimpleGame()
    {
        CommandSystem.OnMoveMade -= HandleMoveMade;
        CommandSystem.OnMoveUnmade -= HandleMoveUnmade;
        Scoring.OnScoreChanged -= HandleScoreChanged;
    }


    public void StartNewGame()
    {
        CommandSystem.ClearCommandHistory();

        ResetKeyStats();

        _stockPile.ClearPile();
        _wastePile.ClearPile();
        foreach (var tableauPile in _tableauPiles)
            tableauPile.ClearPile();
        foreach (var foundationPile in _foundationPiles)
            foundationPile.ClearPile();

        _deck.Shuffle();
        SetupStartingPosition();

        OnGameStarted?.Invoke();
    }

    public void Update()
    {
        var oldTime = Time;
        Time += UnityEngine.Time.deltaTime;
        if (Mathf.FloorToInt(Time) != Mathf.FloorToInt(oldTime))
            OnTimeChanged?.Invoke(Mathf.FloorToInt(Time));
    }

    #endregion

    #region Private

    private Deck _deck;
    private StockPile _stockPile;
    private WastePile _wastePile;
    private FoundationPile[] _foundationPiles;
    private TableauPile[] _tableauPiles;

    private void ResetKeyStats()
    {
        Time = 0;
        Score = 0;
        Moves = 0;

        OnTimeChanged?.Invoke(Mathf.FloorToInt(Time));
        OnScoreChanged?.Invoke(Score);
        OnMovesChanged?.Invoke(Moves);
    }

    private void SetupStartingPosition()
    {
        // Start with the tableau
        for (int i = 0; i < _tableauPiles.Length; i++)
        {
            var pile = _tableauPiles[i];
            
            int cardsToLayDown = i + 1;
            while (cardsToLayDown > 0)
            {
                cardsToLayDown--;

                var card = _deck.GetTopCard();
                pile.AddCard(card);

                // The last card of the tableau pile should be face up
                card.SetIsFaceUp(cardsToLayDown == 0);
            }
        }

        // Add the remaining cards to the stockpile
        while (_deck.RemainingStartingCardCount > 0)
        {
            var card = _deck.GetTopCard();
            _stockPile.AddCard(card);
            card.SetIsFaceUp(false);
        }
    }

    private void HandleMoveMade()
    {
        Moves++;
        OnMovesChanged?.Invoke(Moves);

        int totalFoundationCards = 0;
        foreach (var foundationPile in _foundationPiles)
            totalFoundationCards += foundationPile.CardCount;
        if (totalFoundationCards == 52)
            WinGame();
    }

    private void HandleMoveUnmade()
    {
        Moves--;
        OnMovesChanged?.Invoke(Moves);
    }

    private void HandleScoreChanged(int scoreChange)
    {
        Score += scoreChange;
        OnScoreChanged?.Invoke(Score);
    }

    private void WinGame()
    {
        var timeScore = Scoring.Resolver.GetTimedScore(Time);
        Scoring.HandleScoreChanged(timeScore);

        OnGameWon?.Invoke();
    }

    #endregion
}