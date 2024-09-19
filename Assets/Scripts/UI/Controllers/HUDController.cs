using Game.Interactions;
using Game.Management;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Controllers
{
    public sealed class HUDController : MonoBehaviour
    {
        [SerializeField][Required] Button _newGameButton;
        [SerializeField][Required] Button _undoButton;
        [SerializeField][Required] GameObject _gameWonIndicator;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(HandleNewGameButtonClicked);
            _undoButton.onClick.AddListener(HandleUndoButtonClicked);

            SimpleGame.OnGameStarted += HandleGameStarted;
            SimpleGame.OnGameWon += HandleGameWon;
        }

        private void OnDestroy()
        {
            _newGameButton.onClick.RemoveListener(HandleNewGameButtonClicked);
            _undoButton.onClick.RemoveListener(HandleUndoButtonClicked);

            SimpleGame.OnGameStarted -= HandleGameStarted;
            SimpleGame.OnGameWon -= HandleGameWon;
        }

        private void HandleNewGameButtonClicked()
        {
            GameManager.Singleton.SimpleGame.StartNewGame();
        }

        private void HandleUndoButtonClicked()
        {
            CommandSystem.Undo();
        }

        private void HandleGameStarted()
        {
            _gameWonIndicator.SetActive(false);
        }

        private void HandleGameWon()
        {
            _gameWonIndicator.SetActive(true);
        }
    }
}