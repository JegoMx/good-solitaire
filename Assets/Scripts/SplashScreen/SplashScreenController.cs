using Game.UI.Components;
using Game.Utility;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Game.SplashScreen
{
    public sealed class SplashScreenController : MonoBehaviour
    {
        [SerializeField][Scene]
        private int _gameScene;

        [SerializeField]
        private AssetReference[] _assetsToLoad;

        [SerializeField][Required]
        private SimpleProgressBar _loadingBar;

        // A min loading duration is added so that fast phones don't have a jumpy splash screen.
        // In a real environment, the splash screen would be making API calls and not need this value.
        [SerializeField][MinValue(0)]
        private float _minLoadingDuration;

        private void Start()
        {
            _loadingBar.SetValue(0);
            StartCoroutine(LoadingRoutine());
        }

        private IEnumerator LoadingRoutine()
        {
            yield return null;

            // Assets
            List<AsyncOperationHandle> loadAssetOperations = new();
            foreach (var asset in _assetsToLoad)
            {
                if (asset == null) continue;
                var loadOperation = asset.LoadAssetAsync<object>();
                loadAssetOperations.Add(loadOperation);
            }

            foreach (var loadOperation in loadAssetOperations)
                yield return loadOperation;


            // Scene 
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_gameScene);
            asyncOperation.allowSceneActivation = false;

            float time = 0;

            while (true)
            {
                // Note: Sceneloading treats 0.9f progress as complete instead of 1 for whatever reason.
                float sceneLoadingProgress = asyncOperation.isDone ? 1 : asyncOperation.progress.MapClamped(0, .9f, 0, 1);
                float maxTimedProgress = time.MapClamped(0, _minLoadingDuration, 0, 1);

                float progress = sceneLoadingProgress < maxTimedProgress ? sceneLoadingProgress : maxTimedProgress;

                _loadingBar.SetValue(progress);

                if (Mathf.Approximately(1, progress))
                {
                    asyncOperation.allowSceneActivation = true;
                    break;
                }

                yield return null;
                time += Time.deltaTime;
            }
        }
    }
}