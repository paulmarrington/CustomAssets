// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q"></a> <inheritdoc />
  public class NamedBooleanDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    private Boolean Asset => Listener.AssetToMonitor as Boolean;

    [Serializable] private class BooleanUnityEvent : UnityEvent<string, bool> { }

    [SerializeField] private BooleanUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange() => componentValueToSet.Invoke(parameterName, Asset.Value);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Named Boolean Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<NamedBooleanDriver>();
    #endif
  }
}