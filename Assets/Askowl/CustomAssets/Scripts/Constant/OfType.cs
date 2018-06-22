// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using UnityEngine;

namespace CustomAsset.Constant {
  /// <inheritdoc />
  /// <summary>
  /// Base class for a custom asset. Provides getters and setters for the contained value and
  /// templates for casting to the contained type and to convert it to a string.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
  /// <typeparam name="T">Type of object this custom asset contains</typeparam>
  public class OfType<T> : Base {
    [SerializeField, Value] private T value;

    /// <summary>
    /// For safe(ish) access to the contents field
    /// </summary>
    public virtual T Value { get { return value; } }

    /// <summary>
    /// If this is a project asset, then you will need to reference it somewhere.
    /// Other classes can get a reference using `Instance()` or `Instance(string name)`.
    /// Also useful for creating in-memory versions to share between hosts.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#instance">More...</a></remarks>
    /// <code>Float lifetime = Float.Instance("Lifetime")</code>
    /// <param name="name"></param>
    /// <returns>An instance of OfType&lt;T>, either retrieved or created</returns>
    protected static OfType<T> Instance(string name) { return Instance<OfType<T>>(name); }

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
  }
}