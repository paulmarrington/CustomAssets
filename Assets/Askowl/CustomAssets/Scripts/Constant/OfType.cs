// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using Askowl;
using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="">Base class for a custom asset. Provides getters and setters for the contained value and templates for casting to the contained type and to convert it to a string.</a> //#TBD#//
  public class OfType<T> : Base {
    [SerializeField, Label] private T value = default;

    /// <a href="">For safe(ish) access to the contents field</a> //#TBD#//
    public T Value => value;

    /// <a href="">If this is a project asset, then you will need to reference it somewhere. Other classes can get a reference using `Instance()` or `Instance(string name)`. Also useful for creating in-memory versions to share between hosts.</a> //#TBD#//
    protected static OfType<T> Instance(string name) => Instance<OfType<T>>(name);

    /// <a href="">All extraction by casting a custom object to the contained type. Same as getting the Value - as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset</a> //#TBD#//
    public static implicit operator T(OfType<T> t) => t.value;

    /// <a href="">Pass string conversion responsibility  from the custom asset to the containing value</a> //#TBD#// <inheritdoc />
    public override string ToString() => value.ToString();
  }
}