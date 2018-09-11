using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A Singleton that controls the listening and invoking of events in the game.
/// </summary>
public class EventManager : Singleton<EventManager> {
    #region Properties
    public Dictionary<string, UnityEvent> EventDictionary { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Instantiates the event dictionary.
    /// </summary>
    private void Start() {
        if (EventDictionary == null) {
            EventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    /// <summary>
    /// Starts listening to an action if the event exists.
    /// If the event doesn't exist, it creates a new event to listen to the action.
    /// </summary>
    /// <param name="name">The name of the event</param>
    /// <param name="listener">The action to listen for</param>
    public void StartListening(string name, UnityAction listener) {

        // Add a listener to the event if it exists
        UnityEvent e = null;
        if (EventDictionary.TryGetValue(name, out e)) {
            e.AddListener(listener);
        }
        // Otherwise, create a new event
        else {
            e = new UnityEvent();
            e.AddListener(listener);
            EventDictionary.Add(name, e);
        }
    }

    /// <summary>
    /// Stops listening to an action if the event exists.
    /// </summary>
    /// <param name="name">The name of the event</param>
    /// <param name="listener">The action to listen for</param>
    public void StopListening(string name, UnityAction listener) {
        
        // Remove a listener from the event if it exists
        UnityEvent e = null;
        if (EventDictionary.TryGetValue(name, out e)) {
            e.RemoveListener(listener);
        }
    }
    
    /// <summary>
    /// Triggers an event if it exists.
    /// </summary>
    /// <param name="name">The name of the event</param>
    public void TriggerEvent(string name) {

        // Invoke the event if it exists
        UnityEvent e = null;
        if (EventDictionary.TryGetValue(name, out e)) {
            e.Invoke();
        }
    }
    #endregion
}
