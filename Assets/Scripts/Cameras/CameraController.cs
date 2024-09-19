using NaughtyAttributes;
using UnityEngine;

namespace Game.Cameras
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraController : MonoBehaviour
    {
        [Tooltip("The distance that should be visible horizontally, eg -5 to 5.")]
        [SerializeField][MinValue(1)]
        private float _horizontalHalfExtent;

        [SerializeField][Required]
        private RectTransform _keyStatsHeader;

        [SerializeField][Required]
        private RectTransform _canvasRectangle;

        [SerializeField]
        private float _screenSpaceOffserUnderStatsHeader;

        [SerializeField][Required]
        private Transform _boardRoot;

        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Start()
        {
            AdjustCamera();
        }

#if UNITY_EDITOR
        private void LateUpdate()
        {
            AdjustCamera();
        }
#endif

        private void AdjustCamera()
        {
            // Size
            float aspectRatio = (float)Screen.width / (float)Screen.height;
            _cam.orthographicSize = _horizontalHalfExtent / aspectRatio;

            // Board pos
            Vector3[] uiCorners = new Vector3[4];
            _keyStatsHeader.GetWorldCorners(uiCorners);

            Vector3 bottomLeftCornerScreenPos = uiCorners[0];
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasRectangle, bottomLeftCornerScreenPos, _cam, out Vector3 bottomLeftCornerWorldPos);

            float screenHeight = Screen.height;
            float worldPadding = _screenSpaceOffserUnderStatsHeader / screenHeight * Camera.main.orthographicSize * 2;

            float targetY = bottomLeftCornerWorldPos.y - worldPadding;

            Vector3 bordPos = _boardRoot.position;
            bordPos.y = targetY;
            _boardRoot.position = bordPos;
        }
    }
}