// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#// <inheritdoc />
  public class BooleanDriver : ListenerComponent {
    /// <a href="">Reference to the Asset we are listening to</a> //#TBD#//
    public Boolean Asset => Listener.AssetToMonitor as Boolean;

    [Serializable] private class BooleanUnityEvent : UnityEvent<bool> { }

    [SerializeField] private BooleanUnityEvent componentValueToSet = default;

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() => componentValueToSet.Invoke(Asset.Value);
  }
}