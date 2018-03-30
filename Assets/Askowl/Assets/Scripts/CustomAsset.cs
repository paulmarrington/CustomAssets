namespace Askowl {
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

  public class CustomAsset<T> : ScriptableObject where T : Object {
    private static Dictionary<string, T> instances;

    public static T Asset(string name = null) {
      name = string.IsNullOrEmpty(value: name) ? typeof(T).Name : name;

      // Have I seen it before?
      if (instances == null) {
        instances = new Dictionary<string, T>();
      } else if (instances.ContainsKey(key: name)) {
        return instances[key: name];
      }

      // Nope. Is it a ScriptableObject saved as an Asset?
      instances[key: name] = Components.Find<T>(name);
      if (!Equals(instances[key: name], default(T))) return instances[key: name];

      // Or a reference to a default for the Asset?
      instances[key: name] = Components.Find<T>(name + "Default");
      if (!Equals(instances[key: name], default(T))) return instances[key: name];

      // Better give up now
      Debug.LogErrorFormat("Project does not contain an asset of type '<b>{0}({1})</b>'", name,
                           typeof(T).Name);

      return default(T);
    }
  }
}