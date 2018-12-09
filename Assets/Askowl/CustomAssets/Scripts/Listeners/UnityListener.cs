// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="">Fire a Unity event when a custom asset value changes</a> //#TBD#//
  public sealed class UnityListener : ListenerComponent {
    [SerializeField] private UnityEvent unityEvent;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override void OnChange() => unityEvent.Invoke();
  }
}