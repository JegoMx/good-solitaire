using Game.Board;
using Game.Cards;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interactions
{
    public sealed class Player : MonoBehaviour
    {
        [SerializeField][Required]
        private WastePile _wastePile;

        private bool _isHoldingClick;

        private PlayingCard _heldCard;
        private readonly List<Vector3> _heldCardNextCardPositionOffsets = new();
        private Vector2 _heldCardStartPosition;
        private Vector2 _heldCardOffset;
        private int _heldCardStartSortingOrder;

        private void Update()
        {
            // Poll the player input
            if (Input.GetMouseButtonDown(0))
            {
                HandleStartClick();
            } 
            else if (Input.GetMouseButton(0))
            {
                HandleHoldClick();
            } 
            else if (Input.GetMouseButtonUp(0))
            {
                HandleEndClick();
            }
        }

        private void HandleStartClick()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayingCard downedCard))
                {
                    if (downedCard.Pile is StockPile stockPile)
                    {
                        CommandSystem.TryDrawStockPileCard(downedCard, stockPile, _wastePile);
                    }
                    else
                    {
                        _heldCard = downedCard;
                        _heldCardStartSortingOrder = _heldCard.SortingOrder;

                        int heldSortingOrder = 100;
                        _heldCard.SetSortingOrder(heldSortingOrder);
                        _heldCard.SetIsBeingHeld(true);

                        var nextCard = _heldCard.NextCardInPile;
                        while (nextCard != null)
                        {
                            Vector3 positionDifference = nextCard.transform.position - _heldCard.transform.position;
                            _heldCardNextCardPositionOffsets.Add(positionDifference);

                            nextCard.SetSortingOrder(++heldSortingOrder);
                            nextCard.SetIsBeingHeld(true);

                            nextCard = nextCard.NextCardInPile;
                        }



                        _heldCardStartPosition = _heldCard.transform.position;
                        _heldCardOffset = _heldCardStartPosition - (Vector2)mousePos;
                    }
                }
                else if (hit.collider.gameObject.TryGetComponent(out StockPile stockPile))
                {
                    CommandSystem.TryRefillStockPile(stockPile, _wastePile);
                }
            }
        }

        private void HandleHoldClick()
        {
            if (_heldCard != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Vector3 targetPos = mousePos + (Vector3)_heldCardOffset;
                _heldCard.transform.position = targetPos;


                var nextCard = _heldCard.NextCardInPile;
                int nextCardIndex = 0;
                while (nextCard != null)
                {
                    Vector3 newPos = _heldCard.transform.position + _heldCardNextCardPositionOffsets[nextCardIndex];
                    nextCard.transform.position = newPos;

                    nextCard = nextCard.NextCardInPile;
                    nextCardIndex++;
                }
            }
        }

        private readonly Vector3[] _endClickPositionsToTry = new Vector3[5];

        private void HandleEndClick()
        {
            if (_heldCard != null)
            {
                _heldCard.SetIsBeingHeld(false);
                _heldCard.SetColliderEnabled(false);

                var nextCard = _heldCard.NextCardInPile;
                while (nextCard != null)
                {
                    nextCard.SetIsBeingHeld(false);
                    nextCard.SetColliderEnabled(false);

                    nextCard = nextCard.NextCardInPile;
                }

                // Try and find something it hit, starting with the pointer position,
                // and falling back to the corners. Goes clockwise, starts bottom left.
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                var bounds = _heldCard.Bounds;
                _endClickPositionsToTry[0] = mousePos;
                _endClickPositionsToTry[1] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y);
                _endClickPositionsToTry[2] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y);
                _endClickPositionsToTry[3] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y);
                _endClickPositionsToTry[4] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y);

                bool movedCard = false;
                for (int i = 0; i < _endClickPositionsToTry.Length; i++)
                {
                    var pos = _endClickPositionsToTry[i];
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.TryGetComponent(out PlayingCard hitCard))
                        {
                            var targetPile = hitCard.Pile;
                            movedCard = CommandSystem.TryMoveCard(_heldCard, _heldCard.Pile, targetPile);
                        }
                        else if (hit.collider.gameObject.TryGetComponent(out FoundationPile foundationPile))
                        {
                            movedCard = CommandSystem.TryMoveCard(_heldCard, _heldCard.Pile, foundationPile);
                        }
                        else if (hit.collider.gameObject.TryGetComponent(out TableauPile tableauPile))
                        {
                            movedCard = CommandSystem.TryMoveCard(_heldCard, _heldCard.Pile, tableauPile);
                        }
                    }

                    if (movedCard) break;

                }

                _heldCard.SetColliderEnabled(true);
                nextCard = _heldCard.NextCardInPile;
                while (nextCard != null)
                {
                    nextCard.SetColliderEnabled(true);
                    nextCard = nextCard.NextCardInPile;
                }

                if (!movedCard)
                    RestoreHeldCardsToStart();

                _heldCardNextCardPositionOffsets.Clear();
                _heldCard = null;
                return;
            }
        }

        private void RestoreHeldCardsToStart()
        {
            _heldCard.transform.position = _heldCardStartPosition;
            _heldCard.SetSortingOrder(_heldCardStartSortingOrder);

            var nextCard = _heldCard.NextCardInPile;
            int nextCardIndex = 0;
            while (nextCard != null)
            {
                var position = _heldCard.transform.position + _heldCardNextCardPositionOffsets[nextCardIndex];
                nextCard.transform.position = position;
                nextCard.SetSortingOrder(_heldCardStartSortingOrder + (nextCardIndex + 1));

                nextCard = nextCard.NextCardInPile;
                nextCardIndex++;
            }
        }
    }
}