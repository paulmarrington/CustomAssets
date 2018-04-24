/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  /// <summary>
  /// Base class for a custom asset. Provides getters and setters for the contained value and
  /// templates for casting to the contained type and to convert it to a string.
  /// </summary>
  /// <typeparam name="T">Type of object this custom asset contains</typeparam>
  public abstract class OfType<T> : Base {
    [SerializeField] private T value;

    /// <summary>
    /// Value contained within the custom asset. The getter is plain, but the setter
    /// calls registered events.
    /// </summary>
    public T Value {
      get { return value; }
      set {
        this.value = value;
        Changed();
      }
    }

    /// <summary>
    /// All extraction by casting a custom object to the contained type. Same as getting the Value -
    /// as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset
    /// </summary>
    /// <param name="t">Instance of custom asset</param>
    /// <returns>Instance of the contained serializable object</returns>
    public static implicit operator T([NotNull] OfType<T> t) { return t.value; }

    /// <summary>
    /// Pass string conversion responsibility  from the custom asset to the containing value.
    /// </summary>
    /// <returns>String representation of the contents of the containing value</returns>
    public override string ToString() { return Value.ToString(); }
  }
}