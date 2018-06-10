// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System.Collections.Generic;

namespace CustomAsset {
  public partial class Base {
    private readonly List<Listener> listeners = new List<Listener>();

    /// <summary>
    /// Tell all watchers we have changed
    /// </summary>
    /// <param name="memberName">Member that changed or null for default</param>
    protected void Changed(string memberName = null) {
      for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnTriggered(memberName);
    }

    /// <summary>
    /// Add a listener to those who want to be informed when a custom asset changed. There are
    /// supplied listeners for text objects, unity events and animation among others. Normally
    /// this would be called in OnEnable() for the interested component.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    /// <param name="listener">Listener to register. Must implement `OnTriggered`</param>
    internal void Register(Listener listener) {
      if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    /// <summary>
    /// Remove a listener who no longer cares about the custom asset. Called in OnDisable if set in OnEnable.
    /// Called in Destroy if it was set in Awake.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    /// <param name="listener">Previously registered Listener. Acceptable to deregister twice, or attempt to deregister one never regisgered</param>
    internal void Deregister(Listener listener) {
      if (listeners.Contains(listener)) listeners.Remove(listener);
    }
  }
}