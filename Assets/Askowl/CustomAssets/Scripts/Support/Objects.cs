using UnityEngine;

namespace Askowl {
  /// <summary>
  /// Helper library for dealing with Unity Objects
  /// </summary>
  public static class Objects {
    /// <summary>
    /// Find an object that has already been loaded into memory given it's type.
    /// If there are more than one of this type, only one is returned.
    /// </summary>
    /// <typeparam name="T">Class that inherits from UnityEngine.Object</typeparam>
    /// <returns>Object if found - or null if not</returns>
    public static T Find<T>() where T : Object { return Find<T>(typeof(T).Name); }

    /// <summary>
    /// Find an object that has already been loaded into memory given it's type and it's asset name.
    /// If there are more than one of this type, only one is returned.
    /// </summary>
    /// <param name="name">Name of the asset within the project heirarchy</param>
    /// <typeparam name="T">Class that inherits from UnityEngine.Object</typeparam>
    /// <returns>Object if found - or null if not</returns>
    public static T Find<T>(string name) where T : Object {
      T[] all = Resources.FindObjectsOfTypeAll<T>();

      for (int i = 0; i < all.Length; i++) {
        if (all[i].name == name) return all[i];
      }

      return null;
    }

    /// <summary>
    /// Retrieve a reference to an active component by type from GameObject
    /// </summary>
    /// <param name="name">Name of a GameObject in the scene</param>
    /// <typeparam name="T">Type of the Component in the GameObject</typeparam>
    /// <returns>Reference to the component or null if not found</returns>
    public static T Component<T>(string name) {
      GameObject gameObject = GameObject.Find(name);

      return (gameObject == null) ? default(T) : gameObject.GetComponent<T>();
    }
  }
}