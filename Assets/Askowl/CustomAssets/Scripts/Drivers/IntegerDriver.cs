// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q"></a> <inheritdoc />
  public class IntegerDriver : ListenerComponent {
    /// <a href="http://bit.ly/2QNmw2q">Reference to the Asset we are listening to</a>
    public Integer Asset => Listener.AssetToMonitor as Integer;

    [Serializable] private class IntegerUnityEvent : UnityEvent<int> { }

    [SerializeField] private IntegerUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter emitter) => componentValueToSet.Invoke(Asset.Value);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Integer Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<IntegerDriver>();
    #endif
  }
}