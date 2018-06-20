// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  public abstract class ListenerAsset : OfType<Listener> {
    /// <inheritdoc />
    protected override void OnEnable() { Value.Register(OnChange); }

    /// <inheritdoc cref="ListenerAsset()" />
    protected void OnDisable() { Value.Deregister(); }

    /// <summary>
    /// Reference to the Asset we are listening to
    /// </summary>
    public HasEmitter Asset { get { return Value.AssetToMonitor; } }

    /// <summary>
    /// After we have ensured the change is for the expected member, tell interested parties.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
    protected abstract void OnChange();
  }
}