// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QNmw2q">Trigger a component function directly from a custom asset</a> <inheritdoc />
  public class TriggerDriver : ListenerComponent {
    [SerializeField] private UnityEvent componentValueToSet = default;

    /// <a href="http://bit.ly/2QNmw2q">On a change the listener needs a copy of the changed data to react to</a>
    protected override void OnChange(Emitter emitter) => componentValueToSet.Invoke();

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Trigger Driver")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<TriggerDriver>();
    #endif
  }
}