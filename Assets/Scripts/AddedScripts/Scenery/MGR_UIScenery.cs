using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Scenery
{
    /// <summary>
    /// Manages the UI elements related to scene loading, such as the loading screen and loading bar.
    /// </summary>
    [RequireComponent(typeof(MGR_Scenery))]
    public class MGR_UIScenery : MonoBehaviour
    {
        [SerializeField] private Canvas loadingScreen;
        [SerializeField] private Image loadingBarFill;
        [SerializeField] private float fillDuration = 0.25f;

        private void Awake()
        {
            var sceneryManager = GetComponent<MGR_Scenery>();
            sceneryManager.onLoading += EnableLoadingScreen;
            sceneryManager.onLoaded += DisableLoadingScreen;
            sceneryManager.onLoadPercentage += UpdateLoadBarFill;
        }

        /// <summary>
        /// Enables the loading screen UI.
        /// </summary>
        private void EnableLoadingScreen()
        {
            loadingScreen.enabled = true;
        }

        /// <summary>
        /// Disables the loading screen UI after a delay.
        /// </summary>
        private void DisableLoadingScreen()
        {
            Invoke(nameof(TurnOffLoadingScreen), fillDuration);
        }

        /// <summary>
        /// Turns off the loading screen UI.
        /// </summary>
        private void TurnOffLoadingScreen()
        {
            loadingScreen.enabled = false;
        }

        /// <summary>
        /// Updates the fill amount of the loading bar.
        /// </summary>
        /// <param name="percentage">The percentage to set the loading bar fill amount to.</param>
        private void UpdateLoadBarFill(float percentage)
        {
            StartCoroutine(LerpFill(loadingBarFill.fillAmount, percentage));
        }

        /// <summary>
        /// Smoothly interpolates the loading bar fill amount from one value to another.
        /// </summary>
        /// <param name="from">The initial fill amount.</param>
        /// <param name="to">The target fill amount.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator LerpFill(float from, float to)
        {
            var start = Time.time;
            var now = start;

            while (start + fillDuration > now)
            {
                loadingBarFill.fillAmount = Mathf.Lerp(from, to, (now - start) / fillDuration);
                yield return null;
                now = Time.time;
            }

            loadingBarFill.fillAmount = to;
        }
    }
}
