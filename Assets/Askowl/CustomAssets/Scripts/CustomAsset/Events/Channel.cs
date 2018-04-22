/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

namespace Events {
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;

  [CreateAssetMenu(menuName = "Custom Assets/Events")]
  public sealed class Channel : ScriptableObject {
    private readonly List<Listener> listeners = new List<Listener>();

    [UsedImplicitly]
    public void Trigger() {
      for (int i = listeners.Count - 1; i >= 0; i--) {
        listeners[i].OnTriggered(listeners[i]);
      }
    }

    public void Register(Listener listener) {
      if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    public void Deregister(Listener listener) {
      if (listeners.Contains(listener)) listeners.Remove(listener);
    }
  }
}