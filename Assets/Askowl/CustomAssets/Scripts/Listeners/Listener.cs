// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  [Serializable]
  public class Listener {
    #pragma warning disable 649
    [SerializeField] private HasEmitter assetToMonitor;

    // ReSharper disable once MemberCanBePrivate.Global
    /// <summary>
    /// Must be implemented in containing class as it is called if the listener is triggered.
    /// </summary>
    public Action OnTriggered;

    /// <summary>
    /// Used to classes that have a listener can get back the Custom Asset that triggered it.
    /// </summary>
    public HasEmitter AssetToMonitor { get { return assetToMonitor; } }

    /// <summary>
    /// Register an action so that if the custom asset member changes anyone can be told
    /// </summary>
    /// <param name="onTriggered">void OnChange(string) reference</param>
    public void Register(Action onTriggered) {
      OnTriggered = onTriggered;
      assetToMonitor.Emitter.Register(this);
    }

    /// <summary>
    /// Call to stop receiving change calls
    /// </summary>
    public void Deregister() { assetToMonitor.Emitter.Deregister(this); }
  }
}