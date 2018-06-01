// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  /// <summary>
  /// Helper library for dealing with Unity Objects
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#objects-helpers">More...</a></remarks>
  public static class Objects {
    /// <summary>
    /// Find an object that has already been loaded into memory given it's type.
    /// If there are more than one of this type, only one is returned.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#findt">More...</a></remarks>
    /// <typeparam name="T">Class that inherits from UnityEngine.Object</typeparam>
    /// <returns>Object if found - or null if not</returns>
    [UsedImplicitly]
    public static T Find<T>() where T : Object { return Find<T>(typeof(T).Name); }

    /// <summary>
    /// Find an GameObject that has already been loaded into memory given it's name in the hierarchy.
    /// If there are more than one of this type, only one is returned.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#findt">More...</a></remarks>
    /// <param name="name">Name of the asset within the project heirarchy</param>
    /// <returns>Object if found - or null if not</returns>
    public static GameObject FindGameObject(string name) { return Find<GameObject>(name); }

    /// <summary>
    /// Find an object that has already been loaded into memory given it's type and it's asset name.
    /// If there are more than one of this type, only one is returned.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#findt">More...</a></remarks>
    /// <param name="name">Name of the asset within the project heirarchy</param>
    /// <see cref="Resources.FindObjectsOfTypeAll"/>
    /// <typeparam name="T">Class that inherits from UnityEngine.Object</typeparam>
    /// <returns>Object if found - or null if not</returns>
    public static T Find<T>(string name) where T : Object {
      T[] all = Resources.FindObjectsOfTypeAll<T>();

      if (name == null) {
        return (all.Length > 0) ? all[0] : null;
      }

      for (int i = 0; i < all.Length; i++) {
        if (all[i].name == name) return all[i];
      }

      return null;
    }

    /// <summary>
    /// Retrieve a reference to an active component by type from GameObject
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#componentt">More...</a></remarks>
    /// <param name="path">Unique path to a GameObject in the scene. Doesn't need to be contiguous</param>
    /// <typeparam name="T">Type of the Component in the GameObject</typeparam>
    /// <returns>Reference to the component or null if not found</returns>
    public static T Component<T>(params string[] path) where T : Component {
      if (path.Length == 0) return default(T);

      GameObject parentObject = FindGameObject(path[0]);
      if (parentObject == null) return default(T);

      T[] components = parentObject.GetComponentsInChildren<T>();
      if (components.Length == 0) return default(T);

      if (path.Length == 1) return components[0];

      for (int i = 0; i < components.Length; i++) {
        bool found = true;

        GameObject childObject = components[i].gameObject;

        for (int j = path.Length - 1; found && (j >= 0); j--) {
          while (found && (childObject.name != path[j])) {
            if ((childObject == parentObject)) {
              found = false;
            } else {
              childObject = childObject.transform.parent.gameObject;
            }
          }
        }

        if (found) return components[i];
      }

      return default(T);
    }

    public static bool IsEnabled(string name) {
      GameObject gameObject = FindGameObject(name);
      return (gameObject != null) && gameObject.activeInHierarchy;
    }
  }
}