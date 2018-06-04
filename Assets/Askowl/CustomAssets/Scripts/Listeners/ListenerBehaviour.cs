// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  public abstract class ListenerBehaviour : MonoBehaviour {
    [SerializeField] private CustomAsset.Listener listener;

    private void OnEnable()  { listener.Register(OnChange); }
    private void OnDisable() { listener.Deregister(); }

    public Base BaseAsset    { get { return listener.BaseAsset; } }
    public void Deregister() { listener.Deregister(); }

    /// <summary>
    /// After we have ensured the change is for the expected member, tell interested parties.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
    /// <param name="memberName"></param>
    protected abstract void OnChange(string memberName);
  }
}