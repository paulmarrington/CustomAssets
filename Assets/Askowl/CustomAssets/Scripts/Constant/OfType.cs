// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using Askowl;
using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="http://bit.ly/2RfdWZz">Base class for a custom asset. Provides getters and setters for the contained value and templates for casting to the contained type and to convert it to a string.</a>
  public class OfType<T> : Base {
    [SerializeField, Label] private T value = default;

    /// <a href="http://bit.ly/2CwSVS6">For safe(ish) access to the contents field</a>
    public T Value => value;

    /// <a href="">If this is a project asset, then you will need to reference it somewhere. Other classes can get a reference using `Instance()` or `Instance(string name)`. Also useful for creating in-memory versions to share between hosts.</a>
    protected static OfType<T> Instance(string name) => Instance<OfType<T>>(name);

    /// <a href="http://bit.ly/2CzuMKF">All extraction by casting a custom object to the contained type. Same as getting the Value - as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset</a>
    public static implicit operator T(OfType<T> t) => t.value;

    /// <a href="http://bit.ly/2CwSS8S">Pass string conversion responsibility  from the custom asset to the containing value</a> <inheritdoc />
    public override string ToString() => value.ToString();
  }
}