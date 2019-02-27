// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Used for Animator where actions also require a parameter</a> <inheritdoc />
  public class NamedIntegerDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    private Integer Asset => Listener.AssetToMonitor as Integer;

    [Serializable] private class IntegerUnityEvent : UnityEvent<string, int> { }

    [SerializeField] private IntegerUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter emitter) => componentValueToSet.Invoke(parameterName, Asset.Value);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Named Integer Driver")] private static void AddConnector() =>
      Selection.activeTransform.gameObject.AddComponent<NamedIntegerDriver>();
    #endif
  }
}