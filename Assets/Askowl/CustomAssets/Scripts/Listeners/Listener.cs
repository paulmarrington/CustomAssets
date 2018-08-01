// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// Common code for all event listeners. It registers and deregisters the listener with the channel.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-assets-as-event-listeners">More...</a></remarks>
  [Serializable]
  public class Listener : IObserver {
    [SerializeField] private WithEmitter assetToMonitor;

    /// <summary>
    /// Create a new listener and register it with an emitter
    /// </summary>
    /// <param name="assetToMonitor">Custom asset with a WithEmitter interface</param>
    /// <param name="onTriggered">Function to call on an emission</param>
    /// <returns>A listener instance</returns>
    public static Listener Instance(WithEmitter assetToMonitor, Action onTriggered) {
      Listener listener = new Listener {assetToMonitor = assetToMonitor};
      listener.Register(onTriggered);
      return listener;
    }

    /// <summary>
    /// Must be implemented in containing class as it is called if the listener is triggered.
    /// </summary>
    public Action OnTriggered;

    private IDisposable emitterSubscription;

    /// <summary>
    /// Used to classes that have a listener can get back the Custom Asset that triggered it.
    /// </summary>
    public WithEmitter AssetToMonitor => assetToMonitor;

    /// <summary>
    /// Register an action so that if the custom asset member changes anyone can be told
    /// </summary>
    /// <param name="onTriggered">void OnChange(string) reference</param>
    public void Register(Action onTriggered) {
      OnTriggered = onTriggered;
      Register();
    }

    /// <summary>
    /// Register an action so that if the custom asset member changes anyone can be told
    /// </summary>
    public void Register() { emitterSubscription = assetToMonitor.Emitter.Subscribe(this); }

    /// <summary>
    /// Call to stop receiving change calls
    /// </summary>
    public void Deregister() { emitterSubscription.Dispose(); }

    /// <inheritdoc />
    public void OnNext() { OnTriggered(); }

    /// <inheritdoc />
    public void OnCompleted() { }
  }
}