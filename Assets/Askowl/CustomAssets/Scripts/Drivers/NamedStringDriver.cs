// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#// <inheritdoc />
  public class NamedStringDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    private string Asset => Listener.AssetToMonitor.ToString();

    [Serializable] private class StringUnityEvent : UnityEvent<string, string> { }

    [SerializeField] private StringUnityEvent componentValueToSet = default;

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() => componentValueToSet.Invoke(parameterName, Asset);
  }
}