// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  public abstract class ListenerAsset<T> : OfType<Listener> where T : Base {
    /// <inheritdoc />
    protected override void OnEnable() { Value.Register(OnChange); }

    /// <inheritdoc />
    protected override void OnDisable() { Value.Deregister(); }

    /// <summary>
    /// Reference to the Asset we are listening to
    /// </summary>
    public T Asset { get { return Value.BaseAsset as T; } }

    /// <summary>
    /// After we have ensured the change is for the expected member, tell interested parties.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
    /// <param name="memberName"></param>
    protected abstract void OnChange(string memberName);
  }
}