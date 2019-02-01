// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Drive component string values directly from a custom asset</a> <inheritdoc />
  public class StringDriver : ListenerComponent {
    /// <a href="http://bit.ly/2QNmw2q">Reference to the Asset we are listening to</a>
    public string Asset => Listener.AssetToMonitor.ToString();

    [Serializable] private class StringUnityEvent : UnityEvent<string> { }

    [SerializeField] private StringUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter emitter) => componentValueToSet.Invoke(Asset);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/String Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<StringDriver>();
    #endif
  }
}