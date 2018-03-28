namespace Askowl {
  using System.Collections.Generic;
  using System.Linq;
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

    public static T Asset(string name = "") {
      name = string.IsNullOrEmpty(value: name) ? typeof(T).Name : name;

      // Have I seen it before?
      if (instances == null) {
        instances = new Dictionary<string, T>();
      } else if (instances.ContainsKey(key: name)) {
        return instances[key: name];
      }

      string defaultName = name + "Default";

      // Nope. Is it a ScriptableObject saved as an Asset?
      T[] objects = FindObjectsOfType<T>();

      foreach (T instance in objects.Where(predicate: instance =>
                                             instance.name.Equals(value: name) ||
                                             instance.name.Equals(value: defaultName))) {
        return instances[key: name] = instance;
      }

      // Nope. Is it another sort of resource?
      if (!Equals((instances[key: name] = Resources.Load<T>(path: name)),        default(T)) ||
          !Equals((instances[key: name] = Resources.Load<T>(path: defaultName)), default(T))) {
        return instances[key: name];
      }

      // Nope. Better give up now
      Debug.LogErrorFormat("Project does not contain an asset of type '<b>{0}({1})</b>'", name,
                           typeof(T).Name);

      return default(T);
    }
  }
}