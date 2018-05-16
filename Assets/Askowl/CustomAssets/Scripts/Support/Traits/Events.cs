// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System.Collections.Generic;
using JetBrains.Annotations;

namespace CustomAsset {
  public partial class Base {
    private readonly List<Listener> listeners = new List<Listener>();

    /// <summary>
    /// Called automatically when a custom asset is changed to let all the listeners into the secret.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    [UsedImplicitly]
    public bool Changed(string memberName = null) {
      for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnTriggered(memberName);
      return true;
    }

    /// <summary>
    /// Use in Unity inspector to trigger
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    [UsedImplicitly]
    public void Trigger() { Changed(null); }

    /// <summary>
    /// Use in Unity inspector to trigger for a specific member
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    /// <param name="memberName">Name of member must match that for the listener</param>
    [UsedImplicitly]
    public void Trigger(string memberName) { Changed(memberName); }

    /// <summary>
    /// Add a listener to those who want to be informed when a custom asset changed. There are
    /// supplied listeners for text objects, unity events and animation among others. Normally
    /// this would be called in OnEnable() for the interested component.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    /// <param name="listener">Listener to register. Must implement `OnTriggered`</param>
    [UsedImplicitly]
    internal void Register(Listener listener) {
      if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    /// <summary>
    /// Remove a listener who no longer cares about the custom asset. Called in OnDisable if set in OnEnable.
    /// Called in Destroy if it was set in Awake.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-sources">More...</a></remarks>
    /// <param name="listener">Previously registered Listener. Acceptable to deregister twice, or attempt to deregister one never regisgered</param>
    [UsedImplicitly]
    internal void Deregister(Listener listener) {
      if (listeners.Contains(listener)) listeners.Remove(listener);
    }
  }
}