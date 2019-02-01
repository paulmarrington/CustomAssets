// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QQdUIt"></a> <inheritdoc />
  public class FloatDriver : ListenerComponent {
    /// <a href="http://bit.ly/2QQdUIt">Reference to the Asset we are listening to</a>
    public Float Asset => Listener.AssetToMonitor as Float;

    [Serializable] private class FloatUnityEvent : UnityEvent<float> { }

    [SerializeField] private FloatUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QQdUIt">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter emitter) => componentValueToSet.Invoke(Asset.Value);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Float Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<FloatDriver>();
    #endif
  }
}