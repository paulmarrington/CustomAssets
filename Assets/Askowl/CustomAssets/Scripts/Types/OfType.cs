// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;

namespace CustomAsset {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Base class for a custom asset. Provides getters and setters for the contained value and
  /// templates for casting to the contained type and to convert it to a string.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
  /// <typeparam name="T">Type of object this custom asset contains</typeparam>
  public abstract partial class OfType<T> : Base {
    /// <summary>
    /// For safe(ish) access to the contents field
    /// </summary>
    public T Value {
      protected get { return seed; }
      set {
        if (Equals(value) || !AmChanging) return;

        seed = value;
        Changed();
      }
    }

    /// <summary>
    /// Set a field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change.
    /// </summary>
    /// <param name="field">ref myCustomAsset.aField</param>
    /// <param name="from">Value to set the field to if all checks pass</param>
    /// <typeparam name="TF">Anything that is a direct field in the CustomAsset</typeparam>
    // ReSharper disable once MemberCanBePrivate.Global
    protected void Set<TF>(ref TF field, TF from) {
      if (Equals(field, from) || !AmChanging) return;

      field = from;
      Changed();
    }

    /// <summary>
    /// Set a field inside a CustomAsset compound object where comparison is not straightforward. It checks for read/write and that the field is different before triggering a change.
    /// </summary>
    /// <param name="field">ref myCustomAsset.aField</param>
    /// <param name="from">Value to set the field to if all checks pass</param>
    /// <param name="equals">Comparison operator. Returns true if the items are equal or close enough</param>
    /// <typeparam name="TF">Anything that is a direct field in the CustomAsset</typeparam>
    // ReSharper disable once MemberCanBePrivate.Global
    protected void Set<TF>(ref TF field, TF from, Func<TF, TF, bool> equals) {
      if (equals(field, from) || !AmChanging) return;

      field = from;
      Changed();
    }

    /// <summary>
    /// Check two floating point numbers to be within rounding tolerance.
    /// </summary>
    protected static bool AlmostEqual(float a, float b) { return Math.Abs(a - b) < 1e-5; }

    /// <summary>
    /// Check two double floating point numbers to be within rounding tolerance.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    protected static bool AlmostEqual(double a, double b) { return Math.Abs(a - b) < 1e-5; }

    /// <summary>
    /// Set a float field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref float myCustomAsset.aField to update</param>
    /// <param name="from">float to set the field to if all checks pass</param>
    protected void Set(ref float field, float from) { Set(ref field, from, AlmostEqual); }

    /// <summary>
    /// Set a double field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref double myCustomAsset.aField to update</param>
    /// <param name="from">double to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref double field, double from) { Set(ref field, from, AlmostEqual); }

    /// <summary>
    /// Set a int field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref int myCustomAsset.aField to to update</param>
    /// <param name="from">int to set the field to if all checks pass</param>
    protected void Set(ref int field, int from) { Set<int>(ref field, from); }

    /// <summary>
    /// Set a long field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref long myCustomAsset.aField to update</param>
    /// <param name="from">long to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref long field, long from) { Set<long>(ref field, from); }

    /// <summary>
    /// Set a bool field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref bool myCustomAsset.aField to update</param>
    /// <param name="from">bool to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref bool field, bool from) { Set<bool>(ref field, from); }

    /// <summary>
    /// Set a float string inside a CustomAsset compound object. It checks for read/write and that the field is different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref string field, string from) { Set<string>(ref field, from); }

    /// <summary>
    /// Set a Vector2 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref Vector2 field, Vector2 from) { Set(ref field, from, (a, b) => a == b); }

    /// <summary>
    /// Set a Vector3 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref Vector3 field, Vector3 from) { Set(ref field, from, (a, b) => a == b); }

    /// <summary>
    /// Set a Vector4 inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref Vector4 field, Vector4 from) { Set(ref field, from, (a, b) => a == b); }

    /// <summary>
    /// Set a Quaternion inside a CustomAsset compound object. It checks for read/write and that the field is approximately different before triggering a change.
    /// </summary>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    // ReSharper disable once UnusedMember.Global
    protected void Set(ref Quaternion field, Quaternion from) {
      Set(ref field, from, (a, b) => a == b);
    }

    /// <summary>
    /// All extraction by casting a custom object to the contained type. Same as getting the Value -
    /// as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <param name="t">Instance of custom asset</param>
    /// <returns>Instance of the contained serializable object</returns>
    public static implicit operator T(OfType<T> t) { return t.seed; }

    /// <inheritdoc />
    /// <summary>
    /// Pass string conversion responsibility  from the custom asset to the containing value.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <returns>String representation of the contents of the containing value</returns>
    public override string ToString() { return seed.ToString(); }

    /// <summary>
    /// Called when an asset is loaded and enabled. Used to ensure the custom asset does not leave memory prematurely and to load it if persistent
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected virtual void OnEnable() {
      if (readWrite && !persistent) hideFlags = HideFlags.DontUnloadUnusedAsset;
      Load();
    }

    /// <summary>
    /// OnDisable is called when the program exits or is closed by the platform. It is the last chance to save persistent data.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected virtual void OnDisable() { Save(); }

    /// <summary>
    /// Implement in concrete class to compare data (Equals).
    /// </summary>
    /// <param name="other">The other data object to compare to</param>
    /// <returns></returns>
    protected abstract bool Equals(T other);

    /// <inheritdoc />
    public override bool Equals(object other) { return (other is T) && Equals((T) other); }

    /// <inheritdoc />
    public override int GetHashCode() {
      return Equals(Value, default(T)) ? 0 : Value.GetHashCode();
    }

    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Part of the group of Equals functions. Passes responsibility to the containing data
    /// </summary>
    /// <param name="other">Another reference to a custom asset of the same type</param>
    /// <returns></returns>
    protected bool Equals(OfType<T> other) { return Equals(other.Value); }
  }
}