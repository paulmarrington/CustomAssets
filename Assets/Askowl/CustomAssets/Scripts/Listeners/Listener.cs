// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Common code for all event listeners. It registers and deregisters the listener with the channel</a> //#TBD#// <inheritdoc />
  [Serializable] public class Listener : IObserver {
    [SerializeField] private WithEmitter assetToMonitor;

    /// <a href="">Create a new listener and register it with an emitter</a> //#TBD#//
    public static Listener Instance(WithEmitter assetToMonitor, Action onTriggered) {
      Listener listener = new Listener {assetToMonitor = assetToMonitor};
      listener.Register(onTriggered);
      return listener;
    }

    private Listener() { }

    /// <a href="">Must be implemented in containing class as it is called if the listener is triggered</a> //#TBD#//
    public Action OnTriggered;

    /// <a href="">Used to classes that have a listener can get back the Custom Asset that triggered it</a> //#TBD#//
    public WithEmitter AssetToMonitor => assetToMonitor;

    /// <a href="">Register an action so that if the custom asset member changes anyone can be told</a> //#TBD#//
    public void Register(Action onTriggered) {
      OnTriggered = onTriggered;
      Register();
    }

    /// <a href="">Register an action so that if the custom asset member changes anyone can be told</a> //#TBD#//
    public void Register() => assetToMonitor.Emitter.Subscribe(this);

    /// <a href="">Call to stop receiving change calls</a> //#TBD#//
    public void Deregister() => assetToMonitor.Emitter.RemoveAllListeners();

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public void OnNext() => OnTriggered();

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public void OnCompleted() { }
  }
}