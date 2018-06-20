// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Mutable {
  using UnityEngine;
  using UnityEngine.Events;

  /// <inheritdoc />
  /// <summary>
  /// Fire a Unity event when a custom asset value changes
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#unity-event-listeners">More...</a></remarks>
  public sealed class UnityListener : ListenerComponent {
    [SerializeField] private UnityEvent unityEvent;

    /// <inheritdoc />
    /// <remarks><a href="http://customassets.marrington.net#unity-event-listeners">More...</a></remarks>
    protected override void OnChange() { unityEvent.Invoke(); }
  }
}