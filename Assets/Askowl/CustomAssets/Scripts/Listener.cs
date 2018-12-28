// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Common code for all event listeners. It registers and deregisters the listener with the channel</a> <inheritdoc />
  [Serializable] public class Listener : IObserver {
    [SerializeField] private WithEmitter assetToMonitor;

    /// <a href="http://bit.ly/2QNmw2q">Create a new listener and register it with an emitter</a>
    public static Listener Instance(WithEmitter assetToMonitor, Action onTriggered) {
      Listener listener = new Listener {assetToMonitor = assetToMonitor};
      listener.Register(onTriggered);
      return listener;
    }

    private Listener() { }

    /// <a href="http://bit.ly/2QNmw2q">Must be implemented in containing class as it is called if the listener is triggered</a>
    public Action OnTriggered;

    /// <a href="http://bit.ly/2QNmw2q">Used to classes that have a listener can get back the Custom Asset that triggered it</a>
    public WithEmitter AssetToMonitor => assetToMonitor;

    /// <a href="http://bit.ly/2QNmw2q">Register an action so that if the custom asset member changes anyone can be told</a>
    public void Register(Action onTriggered) {
      OnTriggered = onTriggered;
      Register();
    }

    /// <a href="http://bit.ly/2QNmw2q">Register an action so that if the custom asset member changes anyone can be told</a>
    public void Register() => assetToMonitor.Emitter.Subscribe(this);

    /// <a href="http://bit.ly/2QNmw2q">Call to stop receiving change calls</a>
    public void Deregister() => assetToMonitor.Emitter.RemoveAllListeners();

    /// <a href="http://bit.ly/2QNmw2q"></a> <inheritdoc />
    public void OnNext() => OnTriggered();

    /// <a href="http://bit.ly/2QNmw2q"></a> <inheritdoc />
    public void OnCompleted() { }
  }
}