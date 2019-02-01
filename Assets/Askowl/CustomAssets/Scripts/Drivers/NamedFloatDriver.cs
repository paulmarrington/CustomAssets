// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Used for Animator where actions also require a parameter</a> <inheritdoc />
  public class NamedFloatDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    private Float Asset => Listener.AssetToMonitor as Float;

    [Serializable] private class FloatUnityEvent : UnityEvent<string, float> { }

    [SerializeField] private FloatUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter emitter) => componentValueToSet.Invoke(parameterName, Asset.Value);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Named Float Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<NamedFloatDriver>();
    #endif
  }
}