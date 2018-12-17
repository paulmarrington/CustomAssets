// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#// <inheritdoc />
  public class NamedFloatDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    private Float Asset => Listener.AssetToMonitor as Float;

    [Serializable] private class FloatUnityEvent : UnityEvent<string, float> { }

    [SerializeField] private FloatUnityEvent componentValueToSet = default;

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() => componentValueToSet.Invoke(parameterName, Asset.Value);
  }
}