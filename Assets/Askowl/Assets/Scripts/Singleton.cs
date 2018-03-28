/*
 * From http://wiki.unity3d.com/index.php?title=Singleton
 */

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
  private static T instance;

  // ReSharper disable once StaticMemberInGenericType
  private static readonly object Lock = new object();

  public static T Instance {
    get {
      if (instance != null) return instance;
      if (applicationIsQuitting) return null;

      lock (Lock) {
        instance = (T) FindObjectOfType(typeof(T));
        if (instance != null) return instance;

        GameObject singleton = new GameObject();

        instance       = singleton.AddComponent<T>();
        singleton.name = "(singleton) " + typeof(T);

        DontDestroyOnLoad(target: singleton);
        return instance;
      }
    }
  }

  // ReSharper disable once StaticMemberInGenericType
  private static bool applicationIsQuitting = false;

  /// <summary>
  /// When Unity quits, it destroys objects in a random order.
  /// In principle, a Singleton is only destroyed when application quits.
  /// If any script calls Instance after it have been destroyed,
  ///   it will create a buggy ghost object that will stay on the Editor scene
  ///   even after stopping playing the Application. Really bad!
  /// So, this was made to be sure we're not creating that buggy ghost object.
  /// </summary>
  public void OnDestroy() { applicationIsQuitting = true; }
}