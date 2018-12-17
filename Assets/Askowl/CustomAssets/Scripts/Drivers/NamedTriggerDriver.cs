// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#// <inheritdoc />
  public class NamedTriggerDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    [Serializable] private class TriggerUnityEvent : UnityEvent<string> { }

    [SerializeField] private TriggerUnityEvent componentValueToSet = default;

    /// <a href="">On a change the listener needs a copy of the changed data to react to</a> //#TBD#//
    protected override void OnChange() => componentValueToSet.Invoke(parameterName);
  }
}