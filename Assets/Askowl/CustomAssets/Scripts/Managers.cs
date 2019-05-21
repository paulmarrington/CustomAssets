// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href="http://bit.ly/2RjdFF2">MonoBehaviour container for all manager custom assets for a game</a><inheritdoc />
  public class Managers : MonoBehaviour {
    /// <a href="http://bit.ly/2RjdFF2">List of managers to be loaded</a>
    [SerializeField] private Manager[] managers = default;

    private static readonly Map map = Map.Instance;

    private void OnEnable() {
      foreach (Manager manager in managers) {
        map.Add(manager.GetType(), manager);
        map.Add(manager.name,      manager);
      }
    }

    public static T       Find<T>() where T : Manager            => map[typeof(T)].Value as T;
    public static T       Find<T>(string name) where T : Manager => map[name].Value as T;
    public static Manager Find(string    name)                   => map[name].Value as Manager;

    #if UNITY_EDITOR
    public static void Add<T>(string name) where T : Manager => Add(name, AssetDb.Load<T>(name));
    public static void Add(string    name, Manager value)    => map.Add(name, value);
    /// <a href="http://bit.ly/2RjdFF2">Create a managers MonoBehaviour in the hierarchy</a>
    [MenuItem("GameObject/Create Managers")] public static void CreateManagersGameObject() {
      var prefab     = Resources.Load("Managers");
      var gameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
      gameObject.name = prefab.name;
    }
    #else
    public static void Add<T>(string name) where T : Manager { }
    #endif
  }
}