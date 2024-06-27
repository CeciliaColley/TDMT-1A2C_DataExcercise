using System;
using Scenery;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataSource;

namespace Game
{
    /// <summary>
    /// Manages game-specific events and states.
    /// </summary>
    public class MGR_Game : MonoBehaviour
    {
        [SerializeField] private string playId = "Play";
        [SerializeField] private string exitId = "Exit";
        [SerializeField] private SO_DataSource<MGR_Game> gameManagerDataSource;
        [SerializeField] private SO_DataSource<MGR_Scenery> sceneryManagerDataSource;
        [SerializeField] private CTRLR_Level level1;

        private void OnEnable()
        {
            if (gameManagerDataSource != null)
                gameManagerDataSource.Reference = this;
        }

        private void OnDisable()
        {
            if (gameManagerDataSource != null && gameManagerDataSource.Reference == this)
            {
                gameManagerDataSource.Reference = null;
            }
        }

        /// <summary>
        /// Handles Play or Exit events.
        /// </summary>
        /// <param name="id">The event identifier. Should be "Play" or "Exit".</param>
        public void HandlePlayOrExit(string id)
        {
            if (id == playId)
            {
                if (sceneryManagerDataSource != null && sceneryManagerDataSource.Reference != null)
                {
                    sceneryManagerDataSource.Reference.ChangeLevel(level1);
                }
            }
            else if (id == exitId)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }

    }
}