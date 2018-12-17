// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#// <inheritdoc />
  public class IntegerDriver : ListenerComponent {
    /// <a href="">Reference to the Asset we are listening to</a> //#TBD#//
    public Integer Asset => Listener.AssetToMonitor as Integer;

    [Serializable] private class IntegerUnityEvent : UnityEvent<int> { }

    [SerializeField] private IntegerUnityEvent componentValueToSet = default;

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() => componentValueToSet.Invoke(Asset.Value);
  }
}