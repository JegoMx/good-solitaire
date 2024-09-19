using Game.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Game.UI.Components
{
    public sealed class SimpleProgressBar : MonoBehaviour
    {
        [SerializeField][Required]
        private RectTransform _fill;

        [SerializeField][Required]
        private RectTransform _background;
    
        public void SetValue(float normalizedProgress)
        {
            normalizedProgress = Mathf.Clamp01(normalizedProgress);

            float maxWidth = _background.rect.width;
            float newWidth = normalizedProgress.MapClamped(0, 1, 0, maxWidth);
            _fill.sizeDelta = new Vector2(newWidth, _fill.sizeDelta.y);
        }
    }
}