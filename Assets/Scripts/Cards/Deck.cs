using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
using Game.Utility;


#if UNITY_EDITOR
using System;
#endif

namespace Game.Cards
{
    public sealed class Deck : MonoBehaviour
    {
        [SerializeField][ReadOnly]
        private PlayingCard[] Clubs;

        [SerializeField][ReadOnly]
        private PlayingCard[] Diamonds;
        
        [SerializeField][ReadOnly]
        private PlayingCard[] Hearts;
        
        [SerializeField][ReadOnly]
        private PlayingCard[] Spades;

        [SerializeField][MinValue(1)]
        private int _shuffleIterations;

        private readonly Stack<PlayingCard> _startingStack = new();

        public void Shuffle()
        {
            _startingStack.Clear();
            
            var tempList = new List<PlayingCard>();

            tempList.AddRange(Clubs);
            tempList.AddRange(Diamonds);
            tempList.AddRange(Hearts);
            tempList.AddRange(Spades);

            for (int i = 0; i < _shuffleIterations; i++)
                tempList.Shuffle();

            for (int i = 0; i < tempList.Count; i++)
            {
                var card = tempList[i];
                card.NextCardInPile = null;
                _startingStack.Push(card);
            }
        }

        public int RemainingStartingCardCount => _startingStack.Count;

        public PlayingCard GetTopCard() => _startingStack.Pop();

        #region Editor

#if UNITY_EDITOR

        private bool ShowEditorButtons => !Application.isPlaying;

        [SerializeField][BoxGroup("Editor Only")]
        private PlayingCard _cardPrefab;

        [Button][ShowIf(nameof(ShowEditorButtons))]
        private void PopulateDeck()
        {
            if (_cardPrefab == null)
            {
                Debug.LogError("Cannot populate the deck without a card prefab reference. Please assign it and try again.");
                return;
            }

            for (int i = 0; i < transform.childCount; i++)
                DestroyImmediate(transform.GetChild(i).gameObject);

            int denominatorCount = Enum.GetNames(typeof(Denomination)).Length;

            Clubs = new PlayingCard[denominatorCount];
            PopulateSuitArray(Clubs, Suit.Clubs);

            Diamonds = new PlayingCard[denominatorCount];
            PopulateSuitArray(Diamonds, Suit.Diamonds);

            Hearts = new PlayingCard[denominatorCount];
            PopulateSuitArray(Hearts, Suit.Hearts);

            Spades = new PlayingCard[denominatorCount];
            PopulateSuitArray(Spades, Suit.Spades);
        }

        private void PopulateSuitArray(PlayingCard[] array, Suit suit)
        {
            Transform root = transform;
            for (int i = 0; i < array.Length; i++)
            {
                var newCard = Instantiate(_cardPrefab, root);
                array[i] = newCard;

                var denomination = (Denomination)(i + 1);
                newCard.SetCardData(suit, denomination);
                newCard.gameObject.name = $"{suit} - {denomination}";
            }
        }

#endif

        #endregion
    }
}