using System.Collections;
using System.Collections.Generic;
using Askowl;
using UnityEngine;

namespace CustomAsset {
  public class Base : ScriptableObject {
    /// <summary>
    /// If this is a project asset, then you will need to reference it somewhere.
    /// Other classes can get a reference using `Instance()` or `Instance(string name)`.
    /// Also useful for creating in-memory versions to share between hosts.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#instance">More...</a></remarks>
    /// <code>Float lifetime = Float.Instance("Lifetime")</code>
    /// <param name="name"></param>
    /// <returns>An instance of OfType&lt;T>, either retrieved or created</returns>
    public static T Instance<T>(string name) where T : Base {
      T[] instances = Objects.Find<T>(name);
      return (instances.Length > 0) ? instances[0] : Resources.Load<T>(name);
    }

    #region EditorOnly
#if UNITY_EDITOR
    /// <summary>
    /// Editor only description of what the asset is all about.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
    [SerializeField, Multiline] private string description = " ";
#endif
    #endregion
  }
}