using NaughtyAttributes;
using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class KeyStatsDisplay : MonoBehaviour
    {
        [SerializeField][ColorUsage(false)]
        private Color _highLightColor = Color.yellow;

        private string _highlightColorAsHtmlString;

        [SerializeField][Required]
        private TextMeshProUGUI _scoreField;

        [SerializeField]
        private string _scoreText = "<color=#[c]>Score:</color> [x]";
        
        [SerializeField][Required]
        private TextMeshProUGUI _timeField;

        [SerializeField]
        private string _timeText = "<color=#[c]>Time:</color> [x]";

        [SerializeField][Required]
        private TextMeshProUGUI _movesField;

        [SerializeField]
        private string _movesText = "<color=#[c]>Moves:</color> [x]";


        private void Awake()
        {
            _highlightColorAsHtmlString = ColorUtility.ToHtmlStringRGB(_highLightColor);

            SimpleGame.OnScoreChanged += HandleScoreChanged;
            SimpleGame.OnTimeChanged  += HandleTimeChanged;
            SimpleGame.OnMovesChanged += HandleMovesChanged;
        }

        private void OnDestroy()
        {
            SimpleGame.OnScoreChanged += HandleScoreChanged;
            SimpleGame.OnTimeChanged  -= HandleTimeChanged;
            SimpleGame.OnMovesChanged += HandleMovesChanged;
        }

        private void HandleScoreChanged(int score)
        {
            StringBuilder sb = new StringBuilder(_scoreText);
            sb.Replace("[c]", _highlightColorAsHtmlString);
            sb.Replace("[x]", score.ToString());
            _scoreField.text = sb.ToString();
        }

        private void HandleTimeChanged(int time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            string timeValue = string.Empty;

            if (timeSpan.Hours == 0)
                timeValue = $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
            else
                timeValue = $"{timeSpan.Hours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

            StringBuilder sb = new StringBuilder(_timeText);
            sb.Replace("[c]", _highlightColorAsHtmlString);
            sb.Replace("[x]", timeValue);
            _timeField.text = sb.ToString();
        }

        private void HandleMovesChanged(int moves)
        {
            StringBuilder sb = new StringBuilder(_movesText);
            sb.Replace("[c]", _highlightColorAsHtmlString);
            sb.Replace("[x]", moves.ToString());
            _movesField.text = sb.ToString();
        }
    }
}