// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Used for Animator where actions also require a parameter</a> <inheritdoc />
  public class NamedStringDriver : ListenerComponent {
    [SerializeField] private string parameterName = default;

    private string Asset => Listener.AssetToMonitor.ToString();

    [Serializable] private class StringUnityEvent : UnityEvent<string, string> { }

    [SerializeField] private StringUnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter emitter) => componentValueToSet.Invoke(parameterName, Asset);

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Named String Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<NamedStringDriver>();
    #endif
  }
}