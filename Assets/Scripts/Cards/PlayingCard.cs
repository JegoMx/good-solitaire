using Game.Board;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace Game.Cards
{
    public sealed class PlayingCard : MonoBehaviour
    {
        #region Public

        public Pile Pile { get; set; }

#if UNITY_EDITOR
        [ShowNativeProperty]
#endif
        public PlayingCard NextCardInPile { get; set; }

        public Suit Suit => _suit;
        public Denomination Denomination => _denomination;

        public Bounds Bounds => _spriteRenderer.bounds;

        public void SetCardData(Suit suit, Denomination denomination)
        {
            _suit = suit;
            _denomination = denomination;
        }

        public bool IsFaceUp => _isFaceUp;

        public void SetIsFaceUp(bool isFaceUp)
        {
            _isFaceUp = isFaceUp;
            _spriteRenderer.sprite = _isFaceUp ? _frontFace : _backFace;
        }

        public int SortingOrder => _spriteRenderer.sortingOrder;

        public void SetSortingOrder(int sortingOrder)
        {
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        public bool IsColliderEnabled => _collider.enabled;

        public void SetColliderEnabled(bool enabled)
        {
            _collider.enabled = enabled;
        }

        public void SetIsBeingHeld(bool isBeingHeld)
        {
            transform.localScale = isBeingHeld ? _holdScale : _defaultScale;
        }


#endregion

        #region Private

        [SerializeField][Required]
        private SpriteRenderer _spriteRenderer;

        [SerializeField][Required][ShowAssetPreview]
        private Sprite _backFace;
        
        [SerializeField][Required][ShowAssetPreview]
        private Sprite _frontFace;

        [SerializeField][Required]
        private Collider2D _collider;

        [SerializeField]
        private Vector3 _defaultScale;

        [SerializeField]
        private Vector3 _holdScale;

        [Space(10)]

        [SerializeField][ReadOnly]
        private Suit _suit;

        [SerializeField][ReadOnly]
        private Denomination _denomination;

        private bool _isFaceUp;

        #endregion
    }
}