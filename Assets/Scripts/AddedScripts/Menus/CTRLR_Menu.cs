using System;
using System.Collections.Generic;
using UnityEngine;

namespace Menus
{
    /// <summary>
    /// Controls the menu and manages button creation and events.
    /// </summary>
    public class CTRL_Menu : MonoBehaviour
    {
        [SerializeField] private CTRL_Button buttonPrefab;
        [SerializeField] private List<string> ids = new();
        [SerializeField] private Transform buttonsParent;

        /// <summary>
        /// Event triggered when the menu is changed.
        /// </summary>
        public event Action<string> OnChangeMenu;

        /// <summary>
        /// Sets up the menu by creating buttons for each ID.
        /// </summary>
        public void Setup()
        {
            foreach (var id in ids)
            {
                var newButton = Instantiate(buttonPrefab, buttonsParent);
                newButton.name = $"{id}_Btn";
                newButton.Setup(id, id, HandleButtonClick);
            }
        }

        /// <summary>
        /// Handles the button click event.
        /// </summary>
        /// <param name="id">The ID of the clicked button.</param>
        private void HandleButtonClick(string id)
        {
            OnChangeMenu?.Invoke(id);
        }
    }
}
