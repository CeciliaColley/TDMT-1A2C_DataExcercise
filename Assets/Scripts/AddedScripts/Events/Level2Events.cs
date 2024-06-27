using System;
using UnityEngine;
using Scenery;
using UnityEditor;

namespace Events
{
    public class Level2Events : MonoBehaviour
    {
        //Dehardcode
        [SerializeField] private CTRLR_Level Win;
        [SerializeField] private CTRLR_Level Lose;

        private void OnEnable()
        {
            EventManager.StartListening("WinSequence", OnWinSequence);
            EventManager.StartListening("LoseSequence", OnLoseSequence);
        }

        private void OnDisable()
        {
            EventManager.StopListening("WinSequence", OnWinSequence);
            EventManager.StopListening("LoseSequence", OnLoseSequence);
        }

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

        private void HandleWin(string id)
        {
            
            Debug.Log($"Loading win canvas with ID: {id}");
            
        }

        private void HandleLose(string id)
        {
            
            Debug.Log($"Loading lose canvas with ID: {id}");
            
        }
    }
}


