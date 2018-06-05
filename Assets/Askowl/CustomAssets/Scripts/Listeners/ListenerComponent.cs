// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listener MonoBehaviours. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  public abstract class ListenerComponent<T> : MonoBehaviour where T : Base {
    [SerializeField] private Listener listener;

    private void OnEnable()  { listener.Register(OnChange); }
    private void OnDisable() { Deregister(); }

    /// <summary>
    /// Reference to the Asset we are listening to
    /// </summary>
    public T Asset { get { return listener.BaseAsset as T; } }

    /// <summary>
    /// Stop listening to changes in the custom asset.
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public void Deregister() { listener.Deregister(); }

    /// <summary>
    /// After we have ensured the change is for the expected member, tell interested parties.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
    /// <param name="memberName"></param>
    protected abstract void OnChange(string memberName);
  }
}