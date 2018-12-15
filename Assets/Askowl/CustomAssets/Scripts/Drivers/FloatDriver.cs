// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="">Drop into the same Game Object as an image component to update the fill amount whenever the Float custom asset changes</a> //#TBD#// <inheritdoc />
  public class FloatDriver : ListenerComponent {
    /// <a href=""></a> //#TBD#//
    /// <a href="">Reference to the Asset we are listening to</a> //#TBD#//
    public Float Asset => Listener.AssetToMonitor as Float;

    [Serializable] private class FloatUnityEvent : UnityEvent<float> { }

    [SerializeField] private FloatUnityEvent componentValueToSet = default;

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() => componentValueToSet.Invoke(Asset.Value);
  }
}