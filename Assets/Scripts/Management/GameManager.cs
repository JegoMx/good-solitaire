using Game.Board;
using Game.Cards;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Management
{
    public sealed class GameManager : MonoBehaviour
    {
        #region Public

        public static GameManager Singleton {  get; private set; }

        public SimpleGame SimpleGame { get; private set; }

        #endregion

        #region Private

        [SerializeField][Required]
        private Deck _deck;

        [SerializeField][Required]
        private StockPile _stockPile;

        [SerializeField][Required]
        private WastePile _wastePile;

        [SerializeField]
        private FoundationPile[] _foundationPiles;

        [SerializeField]
        private TableauPile[] _tableauPiles;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            SimpleGame = new SimpleGame(_deck, _stockPile, _wastePile, _foundationPiles, _tableauPiles);
        }


        private void Start()
        {
            SimpleGame.StartNewGame();
        }

        private void Update()
        {
            SimpleGame.Update();
        }

        #endregion
    }
}