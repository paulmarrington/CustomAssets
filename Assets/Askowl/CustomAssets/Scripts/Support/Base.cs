/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System.Collections.Generic;
using UnityEditor;

namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Dynaic custom asset without any values. Use it to trigger and listen to events.
  /// </summary>
  public class Base : ScriptableObject {
#if UNITY_EDITOR
    /// <summary>
    /// Editor only description of what the asset is alla bout.
    /// </summary>
    [Multiline] public string Description = "";
#endif

    private readonly List<Listener> listeners = new List<Listener>();

    /// <summary>
    /// Called automatically when a custom asset is changed to let all the listeners into the secret.
    /// If the contained value is an instance of a serialised class then it won't know in a member
    /// of that class is changed. In these cases, call Changed explicitly.
    /// </summary>
    [UsedImplicitly]
    public bool Changed() {
      for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnTriggered();
      return true;
    }

    /// <summary>
    /// Add a listener to those who want to be informed when a custom asset changed. There are
    /// supplied listeners for text objects, unity events and animation among others. Normally
    /// this would be called in OnEnable() for the interested component.
    /// </summary>
    /// <param name="listener">Listener to register. Must implement `OnTriggered`</param>
    [UsedImplicitly]
    public void Register(Listener listener) {
      if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    /// <summary>
    /// Remove a listener who no longer cares about the custom asset. Called in OnDisable if set in OnEnable.
    /// Called in Destroy if it was set in Awake.
    /// </summary>
    /// <param name="listener">Previously registered Listener. Acceptable to deregister twice, or attempt to deregister one never regisgered</param>
    [UsedImplicitly]
    public void Deregister(Listener listener) {
      if (listeners.Contains(listener)) listeners.Remove(listener);
    }

#if UNITY_EDITOR
    static Base() { EditorApplication.playModeStateChanged += UnloadResources; }

    /// <inheritdoc />
    public Base() { AssetsToUnload.Add(this); }

    private static readonly List<Base> AssetsToUnload = new List<Base>();

    private static void UnloadResources(PlayModeStateChange playModeState) {
      if (playModeState != PlayModeStateChange.ExitingPlayMode) return;

      EditorApplication.playModeStateChanged -= UnloadResources;

      foreach (var asset in AssetsToUnload) Resources.UnloadAsset(asset);
    }
#endif
  }
}