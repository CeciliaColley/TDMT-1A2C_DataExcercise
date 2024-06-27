using System;
using System.Collections.Generic;
using UnityEngine;
using Scenery;

namespace Events
{
    /// <summary>
    /// Manages events specific to Level 1.
    /// </summary>
    public class Level1Events : MonoBehaviour
    {
        /// <summary>
        /// The old level to switch from.
        /// </summary>
        [SerializeField] private CTRLR_Level oldLevel;

        /// <summary>
        /// The new level to switch to.
        /// </summary>
        [SerializeField] private CTRLR_Level newLevel;
        private void OnEnable()
        {
            EventManager.StartListening("WinSequence", OnWinSequence);
        }

        private void OnDisable()
        {
            EventManager.StopListening("WinSequence", OnWinSequence);
        }

        /// <summary>
        /// Called when the "WinSequence" event is triggered.
        /// Switches the scenery from the old level to the new level.
        /// </summary>
        private void OnWinSequence()
        {
            if (MGR_Scenery.Instance != null && oldLevel != null && newLevel != null)
            {
                MGR_Scenery.Instance.SwitchScenery(oldLevel, newLevel);
            }
            else
            {
                Debug.LogError("Scenery Manager or New Level is not assigned.");
            }
        }
    }
}

