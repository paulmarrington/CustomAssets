// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
//  /// <inheritdoc cref="MonoBehaviour" />
//  /// <summary>
//  /// Common code for all event listener MonoBehaviours. It registers and deregisters the listener with the channel.
//  /// </summary>
//  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
//  public abstract class ListenerComponent<T> : ListenerComponent where T : Base {
//    /// <summary>
//    /// Reference to the Asset we are listening to
//    /// </summary>
//    // ReSharper disable once MemberCanBeProtected.Global
//    public T Asset { get { return Listener.AssetToMonitor as T; } }
//  }

  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listener MonoBehaviours. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  public abstract class ListenerComponent : MonoBehaviour {
    [SerializeField] private Listener listener;

    /// <summary>
    /// Retrieve a reference to the listener attached to this component
    /// </summary>
    public Listener Listener { get { return listener; } }

    private void OnEnable() {
      listener.Register(OnChange);
      OnChange();
    }

    private void OnDisable() { Deregister(); }

    /// <summary>
    /// Stop listening to changes in the custom asset.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public void Deregister() { listener.Deregister(); }

    /// <summary>
    /// After we have ensured the change is for the expected member, tell interested parties.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
    protected abstract void OnChange();
  }
}