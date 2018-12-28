// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href="http://bit.ly/2RjdFF2">MonoBehaviour container for all manager custom assets for a game</a> //#TBD#//
  public class Managers : MonoBehaviour {
    #pragma warning disable CS0414
    /// <a href="http://bit.ly/2RjdFF2">List of managers to be loaded</a> //#TBD#//
    [SerializeField] public Manager[] managers = default;

    /// <a href="http://bit.ly/2RjdFF2">Create a managers MonoBehaviour in the hierarchy</a> //#TBD#//
    [MenuItem("GameObject/Create Managers")]
    public static void CreateManagersGameObject() {
      var prefab     = Resources.Load("Managers");
      var gameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
      gameObject.name = prefab.name;
    }
  }
}