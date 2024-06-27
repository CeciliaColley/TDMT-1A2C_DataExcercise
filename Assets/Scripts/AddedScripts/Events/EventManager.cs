using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Manages event listeners and event triggers.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        /// <summary>
        /// Dictionary that maps event names to their respective listeners.
        /// </summary>
        private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

        /// <summary>
        /// Registers a listener for a specific event.
        /// </summary>
        /// <param name="eventName">The name of the event to listen to.</param>
        /// <param name="listener">The action to be executed when the event is triggered.</param>
        public static void StartListening(string eventName, Action listener)
        {
            if (eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent += listener;
                eventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Unregisters a listener from a specific event.
        /// </summary>
        /// <param name="eventName">The name of the event to stop listening to.</param>
        /// <param name="listener">The action to be removed from the event listeners.</param>
        public static void StopListening(string eventName, Action listener)
        {
            if (eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent -= listener;
                eventDictionary[eventName] = thisEvent;
            }
        }

        /// <summary>
        /// Triggers an event, invoking all its listeners.
        /// </summary>
        /// <param name="eventName">The name of the event to trigger.</param>
        public static void TriggerEvent(string eventName)
        {
            if (eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}

