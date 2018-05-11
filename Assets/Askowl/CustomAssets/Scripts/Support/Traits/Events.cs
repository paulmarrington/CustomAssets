using System.Collections.Generic;
using JetBrains.Annotations;

namespace CustomAsset {
  public partial class Base {
    private readonly List<Listener> listeners = new List<Listener>();

    /// <summary>
    /// Called automatically when a custom asset is changed to let all the listeners into the secret.
    /// </summary>
    [UsedImplicitly]
    public bool Changed(string memberName = null) {
      for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnTriggered(memberName);
      return true;
    }

    /// <summary>
    /// Add a listener to those who want to be informed when a custom asset changed. There are
    /// supplied listeners for text objects, unity events and animation among others. Normally
    /// this would be called in OnEnable() for the interested component.
    /// </summary>
    /// <param name="listener">Listener to register. Must implement `OnTriggered`</param>
    [UsedImplicitly]
    internal void Register(Listener listener) {
      if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    /// <summary>
    /// Remove a listener who no longer cares about the custom asset. Called in OnDisable if set in OnEnable.
    /// Called in Destroy if it was set in Awake.
    /// </summary>
    /// <param name="listener">Previously registered Listener. Acceptable to deregister twice, or attempt to deregister one never regisgered</param>
    [UsedImplicitly]
    internal void Deregister(Listener listener) {
      if (listeners.Contains(listener)) listeners.Remove(listener);
    }
  }
}