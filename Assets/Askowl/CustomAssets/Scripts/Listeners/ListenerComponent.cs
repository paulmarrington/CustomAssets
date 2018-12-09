// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
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

  /// <a href="">Common code for all event listener MonoBehaviours. It registers and deregisters the listener with the channel</a> //#TBD#// <inheritdoc cref="MonoBehaviour" />
  public abstract class ListenerComponent : MonoBehaviour {
    [SerializeField] private Listener listener;

    /// <a href="">Retrieve a reference to the listener attached to this component</a> //#TBD#//
    public Listener Listener => listener;

    private void OnEnable() {
      listener.Register(OnChange);
      OnChange();
    }

    private void OnDisable() => Deregister();

    /// <a href="">Stop listening to changes in the custom asset</a> //#TBD#//
    public void Deregister() => listener.Deregister();

    /// <a href="">After we have ensured the change is for the expected member, tell interested parties</a> //#TBD#//
    protected abstract void OnChange();
  }
}