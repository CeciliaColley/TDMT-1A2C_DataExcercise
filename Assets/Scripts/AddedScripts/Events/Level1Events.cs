using System;
using System.Collections.Generic;
using UnityEngine;
using Scenery;

namespace Events
{
    public class Level1Events : MonoBehaviour
    {
        [SerializeField] private CTRLR_Level oldLevel;
        [SerializeField] private CTRLR_Level newLevel;

        private void OnEnable()
        {
            EventManager.StartListening("WinSequence", OnWinSequence);
        }

        private void OnDisable()
        {
            EventManager.StopListening("WinSequence", OnWinSequence);
        }

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

