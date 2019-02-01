// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Common code for all event listeners. It registers and deregisters the listener with the channel</a>
  [Serializable] public class Listener {
    [SerializeField] private WithEmitter assetToMonitor;

    /// <a href="http://bit.ly/2QNmw2q">Create a new listener and register it with an emitter</a>
    public static Listener Instance(WithEmitter assetToMonitor, Emitter.Action onTriggered) {
      Listener listener = new Listener {assetToMonitor = assetToMonitor};
      listener.Register(onTriggered);
      return listener;
    }

    private Listener() { }

    /// <a href="http://bit.ly/2QNmw2q">Must be implemented in containing class as it is called if the listener is triggered</a>
    [HideInInspector] public Emitter.Action onTriggered;

    /// <a href="http://bit.ly/2QNmw2q">Used to classes that have a listener can get back the Custom Asset that triggered it</a>
    public WithEmitter AssetToMonitor => assetToMonitor;

    /// <a href="http://bit.ly/2QNmw2q">Register an action so that if the custom asset member changes anyone can be told</a>
    public void Register(Emitter.Action actionOnTriggered) {
      assetToMonitor.Emitter.Remove(action: onTriggered);
      onTriggered = actionOnTriggered;
      Register();
    }

    /// <a href="http://bit.ly/2QNmw2q">Register an action so that if the custom asset member changes anyone can be told</a>
    public void Register() => assetToMonitor.Emitter.Listen(onTriggered);

    /// <a href="http://bit.ly/2QNmw2q">Call to stop receiving change calls</a>
    public void Deregister() => assetToMonitor.Emitter.Remove(onTriggered);
  }
}