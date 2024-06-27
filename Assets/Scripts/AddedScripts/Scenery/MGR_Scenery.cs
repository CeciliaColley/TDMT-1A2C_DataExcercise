using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataSource;

namespace Scenery
{
    /// <summary>
    /// Manages the scenery of the game by loading and unloading scenes.
    /// </summary>
    public class MGR_Scenery : MonoBehaviour
    {
        [SerializeField] private SO_DataSource<MGR_Scenery> sceneryManagerDataSource;
        [SerializeField] private CTRLR_Level defaultLevel;
        private CTRLR_Level _currentLevel;

        public static MGR_Scenery Instance;

        /// <summary>
        /// Event triggered when loading starts.
        /// </summary>
        public event Action onLoading = delegate { };

        /// <summary>
        /// Event triggered to indicate the load percentage. The float is always between 0 and 1.
        /// </summary>
        public event Action<float> onLoadPercentage = delegate { };

        /// <summary>
        /// Event triggered when loading is completed.
        /// </summary>
        public event Action onLoaded = delegate { };

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
            {
                sceneryManagerDataSource.Reference = this;
            }
        }

        private void Start()
        {
            StartCoroutine(LoadFirstLevel(defaultLevel));
        }

        private void OnDisable()
        {
            if (sceneryManagerDataSource != null && sceneryManagerDataSource.Reference == this)
            {
                sceneryManagerDataSource.Reference = null;
            }
        }

        /// <summary>
        /// Changes the current level to a new level.
        /// </summary>
        /// <param name="level">The new level to load.</param>
        public void ChangeLevel(CTRLR_Level level)
        {
            StartCoroutine(ChangeLevelCoroutine(_currentLevel, level));
        }

        /// <summary>
        /// Loads a new level.
        /// </summary>
        /// <param name="level">The new level to load.</param>
        public void LoadLevel(CTRLR_Level level)
        {
            StartCoroutine(LoadLevelCoroutine(_currentLevel, level));
        }

        /// <summary>
        /// Switches the scenery from the old level to the new level.
        /// </summary>
        /// <param name="oldLevel">The level to unload.</param>
        /// <param name="newLevel">The level to load.</param>
        public void SwitchScenery(CTRLR_Level oldLevel, CTRLR_Level newLevel)
        {
            StartCoroutine(SwitchScenesCoroutine(oldLevel, newLevel));
        }

        private IEnumerator ChangeLevelCoroutine(CTRLR_Level currentLevel, CTRLR_Level newLevel)
        {
            yield return ExecuteLevelChange(currentLevel, newLevel, true);
        }

        private IEnumerator LoadLevelCoroutine(CTRLR_Level currentLevel, CTRLR_Level newLevel)
        {
            yield return ExecuteLevelChange(currentLevel, newLevel, false);
        }

        private IEnumerator LoadFirstLevel(CTRLR_Level level)
        {
            int addedWeight = 5;

            onLoading();
            onLoadPercentage(0);
            int total = level.SceneNames.Count + addedWeight;

            yield return LoadScenes(level, currentIndex => onLoadPercentage((float)currentIndex / total));

            for (int current = 0; current <= total; current++)
            {
                yield return new WaitForSeconds(1);
                onLoadPercentage((float)current / total);
            }

            _currentLevel = level;
            onLoaded();
        }

        private IEnumerator ExecuteLevelChange(CTRLR_Level currentLevel, CTRLR_Level newLevel, bool unloadCurrentLevel)
        {
            onLoading();
            onLoadPercentage(0);

            int unloadCount = unloadCurrentLevel ? currentLevel.SceneNames.Count : 0;
            int loadCount = newLevel.SceneNames.Count;
            int total = unloadCount + loadCount;

            if (unloadCurrentLevel)
            {
                yield return UnloadScenes(currentLevel, currentIndex => onLoadPercentage((float)currentIndex / total));
            }

            yield return LoadScenes(newLevel, currentIndex => onLoadPercentage((float)(currentIndex + unloadCount) / total));

            _currentLevel = newLevel;
            onLoaded();
        }

        private IEnumerator LoadScenes(CTRLR_Level level, Action<int> onLoadedSceneQtyChanged)
        {
            int current = 0;

            foreach (var sceneName in level.SceneNames)
            {
                var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onLoadedSceneQtyChanged(current);
            }
        }

        private IEnumerator UnloadScenes(CTRLR_Level level, Action<int> onUnloadedSceneQtyChanged)
        {
            int current = 0;

            foreach (var sceneName in level.SceneNames)
            {
                if (TryUnloadScene(sceneName, out var loadOp))
                {
                    yield return new WaitUntil(() => loadOp.isDone);
                }

                current++;
                onUnloadedSceneQtyChanged(current);
            }
        }

        private bool TryUnloadScene(string sceneName, out AsyncOperation loadOp)
        {
            if (SceneManager.GetSceneByName(sceneName).IsValid())
            {
                loadOp = SceneManager.UnloadSceneAsync(sceneName);
                return true;
            }

            loadOp = null;
            return false;
        }

        private IEnumerator SwitchScenesCoroutine(CTRLR_Level oldLevel, CTRLR_Level newLevel)
        {
            onLoading();
            onLoadPercentage(0);

            int unloadCount = oldLevel.SceneNames.Count;
            int loadCount = newLevel.SceneNames.Count;
            int total = unloadCount + loadCount;
            int currentIndex = 0;

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
