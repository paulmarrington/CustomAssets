using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [CreateAssetMenu(menuName="Custom Asset Type/Custom Asset Name")]
 * public class MyCustomAsset: CustomAsset<MyCustomAsset> {
 *   public int anInteger;
 * }
 * 
 * public class MyMB : MonoBehaviour {
 *   MyCustomAsset customSingletonAsset;
 *   MyCustomAsset namedSingletonAsset;
 * 
 *   void Start() {
 *     customSingletonAsset = MyCustomAsset.Asset();
 *     namedSingletonAsset = MyCustomAsset.Asset("what's in a name");
 *   }
 * }
 */
using System.IO;
using System;
using UnityEngineInternal;

public class CustomAsset<T> : ScriptableObject where T : UnityEngine.Object {
  protected static Dictionary<string,T> instances;

  public static T Asset(string name = "") {
    name = (name == null || name.Length == 0) ? typeof(T).Name : name;
      

    // Have I seen it before?
    if (instances == null) {
      instances = new Dictionary<string, T> ();
    } else if (instances.ContainsKey(name)) {
      return instances [name];
    }

    string defaultName = name + "Default";

    // Nope. Is it a ScriptableObject saved as an Asset?
    T[] objects = UnityEngine.Object.FindObjectsOfType<T>();
    foreach (T instance in objects) {
      if (instance.name.Equals(name) || instance.name.Equals(defaultName)) {
        return instances [name] = instance;
      }
    }

    // Nope. Is it another sort of resource?
    if (!object.Equals((instances [name] = Resources.Load<T>(name)), default(T)) ||
        !object.Equals((instances [name] = Resources.Load<T>(defaultName)), default(T))) {
      return instances [name];
    }

    // Nope. Better give up now
    Debug.LogErrorFormat("Project does not contain an asset of type '<b>{0}({1})</b>'", name, typeof(T).Name);
    return default(T);
  }
}