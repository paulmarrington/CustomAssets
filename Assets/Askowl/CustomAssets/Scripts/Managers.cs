// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href=""></a> //#TBD#//
  public class Managers : MonoBehaviour {
    #pragma warning disable CS0414
    [SerializeField] public Manager[] managers = default;

    [MenuItem("GameObject/Create Managers")]
    public static void CreateManagersGameObject() {
      var prefab     = Resources.Load("Managers");
      var gameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
      gameObject.name = prefab.name;
    }
  }
}