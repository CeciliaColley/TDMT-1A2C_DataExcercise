using System;
using System.Collections.Generic;
using UnityEngine;

using Game;
using Menus;
using DataSource;

namespace Navigation
{
    /// <summary>
    /// Manages navigation between different menus.
    /// </summary>
    public class MGR_Navigation : MonoBehaviour
    {
        [Tooltip("First menu in the list is the default one :)")]
        [SerializeField] private List<MenuWithId> menusWithId;

        [SerializeField] private SO_DataSource<MGR_Game> gameManagerDataSource;
        [SerializeField] private List<string> idsToTellGameManager = new();
        private int _currentMenuIndex = 0;

        /// <summary>
        /// Initializes the menus and sets the default menu active.
        /// </summary>
        private void Start()
        {
            InitializeMenus();
            SetDefaultMenuActive();
        }

        /// <summary>
        /// Handles menu change events.
        /// </summary>
        /// <param name="id">The ID of the menu to switch to.</param>
        private void HandleChangeMenu(string id)
        {
            if (idsToTellGameManager.Contains(id))
            {
                gameManagerDataSource?.Reference?.HandlePlayOrExit(id);
            }

            for (var i = 0; i < menusWithId.Count; i++)
            {
                var menuWithId = menusWithId[i];
                if (menuWithId.ID == id)
                {
                    SwitchMenu(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Initializes all menus by setting them up and deactivating them.
        /// </summary>
        private void InitializeMenus()
        {
            foreach (var menu in menusWithId)
            {
                menu.Menu.Setup();
                menu.Menu.OnChangeMenu += HandleChangeMenu;
                menu.Menu.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Sets the default menu active.
        /// </summary>
        private void SetDefaultMenuActive()
        {
            if (menusWithId.Count > 0)
            {
                menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Switches to the menu at the specified index.
        /// </summary>
        /// <param name="newMenuIndex">The index of the menu to switch to.</param>
        private void SwitchMenu(int newMenuIndex)
        {
            menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(false);
            menusWithId[newMenuIndex].Menu.gameObject.SetActive(true);
            _currentMenuIndex = newMenuIndex;
        }

        /// <summary>
        /// Represents a menu with an associated ID.
        /// </summary>
        [Serializable]
        public struct MenuWithId
        {
            [field: SerializeField] public string ID { get; set; }
            [field: SerializeField] public CTRL_Menu Menu { get; set; }
        }
    }
}