/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

namespace CustomAsset {
  using System.Collections.Generic;
  using UnityEngine;

  [CreateAssetMenu(menuName = "Custom Assets/Events")]
  public class EventActor : ScriptableObject {
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<IEventListener> listeners = new List<IEventListener>();

    public void Raise() {
      for (int i = listeners.Count - 1; i >= 0; i--)
        listeners[i].OnEventRaised(listeners[i]);
    }

    public void Register(IEventListener listener) {
      if (!listeners.Contains(listener))
        listeners.Add(listener);
    }

    public void Deregister(IEventListener listener) {
      if (listeners.Contains(listener))
        listeners.Remove(listener);
    }
  }
}