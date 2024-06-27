using System;
using UnityEngine;
using Scenery;
using UnityEditor;

namespace Events
{
    /// <summary>
    /// Manages events specific to Level 2.
    /// </summary>
    public class Level2Events : MonoBehaviour
    {
        /// <summary>
        /// The name of the event triggered on winning level 2.
        /// </summary>
        [SerializeField] private string winEventName = "WinSequence";

        /// <summary>
        /// The name of the event triggered on losing level 2.
        /// </summary>
        [SerializeField] private string loseEventName = "LoseSequence";

        /// <summary>
        /// The level to switch to on winning level 2.
        /// </summary>
        [SerializeField] private CTRLR_Level Win;

        /// <summary>
        /// The level to switch to on losing level 2.
        /// </summary>
        [SerializeField] private CTRLR_Level Lose;

        private void OnEnable()
        {
            EventManager.StartListening(winEventName, OnWinSequence);
            EventManager.StartListening(loseEventName, OnLoseSequence);
        }

        private void OnDisable()
        {
            EventManager.StopListening(winEventName, OnWinSequence);
            EventManager.StopListening(loseEventName, OnLoseSequence);
        }

        /// <summary>
        /// Called when the win event is triggered.
        /// Switches the scenery to the winning level.
        /// </summary>
        private void OnWinSequence()
        {
            if (MGR_Scenery.Instance != null && Win != null)
            {
                MGR_Scenery.Instance.ChangeLevel(Win);
            }
            else
            {
                Debug.LogError("Scenery Manager or Win is not assigned.");
            }
        }

        /// <summary>
        /// Called when the lose event is triggered.
        /// Switches the scenery to the losing level.
        /// </summary>
        private void OnLoseSequence()
        {
            if (MGR_Scenery.Instance != null && Lose != null)
            {
                MGR_Scenery.Instance.ChangeLevel(Lose);
            }
            else
            {
                Debug.LogError("Scenery Manager or Lose is not assigned.");
            }
        }
    }
}


