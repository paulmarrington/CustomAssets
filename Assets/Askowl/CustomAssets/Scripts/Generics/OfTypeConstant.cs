﻿// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using Askowl;

namespace CustomAsset.Constant {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Base class for a custom asset. Provides getters and setters for the contained value and
  /// templates for casting to the contained type and to convert it to a string.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
  /// <typeparam name="T">Type of object this custom asset contains</typeparam>
  public class OfType<T> : ScriptableObject {
    [SerializeField] private T value;

    /// <summary>
    /// For safe(ish) access to the contents field
    /// </summary>
    public T Value { get { return value; } protected set { this.value = value; } }

    /// <summary>
    /// If this is a project asset, then you will need to reference it somewhere.
    /// Other classes can get a reference using `Instance()` or `Instance(string name)`.
    /// Also useful for creating in-memory versions to share between hosts.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#instance">More...</a></remarks>
    /// <code>Float lifetime = Float.Instance("Lifetime")</code>
    /// <param name="name"></param>
    /// <returns>An instance of OfType&lt;T>, either retrieved or created</returns>
    public static TI Instance<TI>(string name) where TI : ScriptableObject {
      TI[] instances = Objects.Find<TI>(name);
      return (instances.Length > 0) ? instances[0] : Resources.Load<TI>(name);
    }

    /// <summary>
    /// All extraction by casting a custom object to the contained type. Same as getting the Value -
    /// as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <param name="t">Instance of custom asset</param>
    /// <returns>Instance of the contained serializable object</returns>
    public static implicit operator T(OfType<T> t) { return t.value; }

    /// <inheritdoc />
    /// <summary>
    /// Pass string conversion responsibility  from the custom asset to the containing value.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <returns>String representation of the contents of the containing value</returns>
    public override string ToString() { return value.ToString(); }

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