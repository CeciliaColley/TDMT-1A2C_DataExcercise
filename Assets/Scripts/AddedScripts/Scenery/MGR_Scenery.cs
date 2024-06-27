using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataSource;

namespace Scenery
{
    public class MGR_Scenery : MonoBehaviour
    {
        [SerializeField] private SO_DataSource<MGR_Scenery> sceneryManagerDataSource;
        [SerializeField] private CTRLR_Level defaultLevel;
        private CTRLR_Level _currentLevel;
        public event Action onLoading = delegate { };
        /// <summary>
        /// The float given is always between 0 and 1
        /// </summary>
        public event Action<float> onLoadPercentage = delegate { };
        public event Action onLoaded = delegate { };

        public static MGR_Scenery Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            if (sceneryManagerDataSource != null)
                sceneryManagerDataSource.Reference = this;
        }

        private void Start()
        {
            //Load default level
            StartCoroutine(LoadFirstLevel(defaultLevel));
        }

        private void OnDisable()
        {
            if (sceneryManagerDataSource != null && sceneryManagerDataSource.Reference == this)
            {
                sceneryManagerDataSource.Reference = null;
            }
        }

        public void ChangeLevel(CTRLR_Level level)
        {
            StartCoroutine(ChangeLevel(_currentLevel, level));
        }

        public void LoadLevel(CTRLR_Level level)
        {
            StartCoroutine(LoadLevel(_currentLevel, level));
        }

        private IEnumerator ChangeLevel(CTRLR_Level currentLevel, CTRLR_Level newLevel)
        {
            onLoading();
            onLoadPercentage(0);
            var unloadCount = currentLevel.SceneNames.Count;
            var loadCount = newLevel.SceneNames.Count;
            var total = unloadCount + loadCount;
            yield return new WaitForSeconds(2);
            yield return Unload(currentLevel,
                currentIndex => onLoadPercentage((float)currentIndex / total));
            yield return new WaitForSeconds(2);
            yield return Load(newLevel,
                currentIndex => onLoadPercentage((float)(currentIndex + unloadCount) / total));
            yield return new WaitForSeconds(2);

            _currentLevel = newLevel;
            onLoaded();
        }

        private IEnumerator LoadLevel(CTRLR_Level currentLevel, CTRLR_Level newLevel)
        {
            yield return new WaitForSeconds(2);
            yield return Load(newLevel,
                currentIndex => onLoadPercentage((float)(currentIndex)));
            yield return new WaitForSeconds(2);

            _currentLevel = newLevel;
            onLoaded();
        }

        private IEnumerator LoadFirstLevel(CTRLR_Level level)
        {
            //This is a cheating value, do not use in production!
            var addedWeight = 5;

            onLoading();
            onLoadPercentage(0);
            var total = level.SceneNames.Count + addedWeight;
            var current = 0;
            yield return Load(level,
                currentIndex => onLoadPercentage((float)currentIndex / total));

            //This is cheating so the screen is shown over a lot of time :)
            for (; current <= total; current++)
            {
                yield return new WaitForSeconds(1);
                onLoadPercentage((float)current / total);
            }
            _currentLevel = level;
            onLoaded();
        }

        private IEnumerator Load(CTRLR_Level level, Action<int> onLoadedSceneQtyChanged)
        {
            var current = 0;
            foreach (var sceneName in level.SceneNames)
            {
                var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onLoadedSceneQtyChanged(current);
            }
        }

        private IEnumerator Unload(CTRLR_Level level, Action<int> onUnloadedSceneQtyChanged)
        {
            var current = 0;
            foreach (var sceneName in level.SceneNames)
            {
                var loadOp = SceneManager.UnloadSceneAsync(sceneName);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onUnloadedSceneQtyChanged(current);
            }
        }

        public void SwitchScenery(CTRLR_Level oldLevel, CTRLR_Level newLevel)
        {
            StartCoroutine(SwitchScenes(oldLevel, newLevel));
        }

        private IEnumerator SwitchScenes(CTRLR_Level oldLevel, CTRLR_Level newLevel)
        {
            onLoading();
            onLoadPercentage(0);

            var unloadCount = oldLevel.SceneNames.Count;
            var loadCount = newLevel.SceneNames.Count;
            var total = unloadCount + loadCount;
            var currentIndex = 0;

            foreach (var sceneName in oldLevel.SceneNames)
            {
                var unloadOp = SceneManager.UnloadSceneAsync(sceneName);
                yield return new WaitUntil(() => unloadOp.isDone);
                currentIndex++;
                onLoadPercentage((float)currentIndex / total);
            }

            foreach (var sceneName in newLevel.SceneNames)
            {
                var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                currentIndex++;
                onLoadPercentage((float)currentIndex / total);
            }

            _currentLevel = newLevel;
            onLoaded();
        }
    }
}
