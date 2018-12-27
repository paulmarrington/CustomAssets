// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

#endif

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2CwSVl4">Provides access from manager custom asset to any game object</a>
  public class GameObjectConnector : MonoBehaviour {
    [SerializeField] private GameObject gameObjectAsset = default;

    private void Awake() => gameObjectAsset.Value = gameObject;

    private void OnDestroy() => gameObjectAsset.Value = default;

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/GameObject Connector")]
    private static void AddConnector() => Selection.activeTransform.gameObject.AddComponent<GameObjectConnector>();
    #endif
  }
}