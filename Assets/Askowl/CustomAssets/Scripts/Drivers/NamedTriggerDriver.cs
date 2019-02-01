// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Used for Animator where actions also require a parameter</a> <inheritdoc />
  public class NamedTriggerDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    [Serializable] private class TriggerUnityEvent : UnityEvent<string> { }

    [SerializeField] private TriggerUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter _) => componentValueToSet.Invoke(parameterName);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Named Trigger Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<NamedTriggerDriver>();
    #endif
  }
}