// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;

namespace CustomAsset {
  using JetBrains.Annotations;
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
    /// For safe access to the contents field
    /// </summary>
    [UsedImplicitly]
    public T Value { protected get { return seed; } set { Set(() => seed = value); } }

    /// <summary>
    /// Does the CustomAsset have permission to change it's values?
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public bool ChangeAllowed { get { return (readWrite || persistent); } }

    /// <summary>
    /// Tells the event listeners that something in this value has changed. Designed to be used in setters.
    /// It will also save the data on critical and call all listeners using `Changed`
    /// </summary>
    /// <code>public float aFloat { get { return Value.F; } set { Set(() => Value.F = value); } }</code>
    /// <param name="action">Lambda called if custom asset is read/write</param>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected void Set(Action action) {
      if (!ChangeAllowed) return;

      action();
      Changed();
    }

    /// <summary>
    /// Set a field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <param name="field">ref myCustomAsset.aField</param>
    /// <param name="from">Value to set the field to if all checks pass</param>
    /// <typeparam name="TF">Anything that is a direct field in the CustomAsset</typeparam>
    [UsedImplicitly]
    protected void Set<TF>(ref TF field, TF from) {
      if (!ChangeAllowed || field.Equals(from)) return;

      field = from;
      Changed();
    }

    /// <summary>
    /// Set a float field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <see cref="Set&lt;TF>"/>
    /// <param name="field">ref float myCustomAsset.aField to update</param>
    /// <param name="from">float to set the field to if all checks pass</param>
    [UsedImplicitly]
    protected void Set(ref float field, float from) { Set<float>(ref field, from); }

    /// <summary>
    /// Set a double field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <see cref="Set&lt;TF>"/>
    /// <param name="field">ref double myCustomAsset.aField to update</param>
    /// <param name="from">double to set the field to if all checks pass</param>
    [UsedImplicitly]
    protected void Set(ref double field, double from) { Set<double>(ref field, from); }

    /// <summary>
    /// Set a int field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <see cref="Set&lt;TF>"/>
    /// <param name="field">ref int myCustomAsset.aField to to update</param>
    /// <param name="from">int to set the field to if all checks pass</param>
    [UsedImplicitly]
    protected void Set(ref int field, int from) { Set<int>(ref field, from); }

    /// <summary>
    /// Set a long field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <see cref="Set&lt;TF>"/>
    /// <param name="field">ref long myCustomAsset.aField to update</param>
    /// <param name="from">long to set the field to if all checks pass</param>
    [UsedImplicitly]
    protected void Set(ref long field, long from) { Set<long>(ref field, from); }

    /// <summary>
    /// Set a bool field inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <see cref="Set&lt;TF>"/>
    /// <param name="field">ref bool myCustomAsset.aField to update</param>
    /// <param name="from">bool to set the field to if all checks pass</param>
    [UsedImplicitly]
    protected void Set(ref bool field, bool from) { Set<bool>(ref field, from); }

    /// <summary>
    /// Set a float string inside a CustomAsset compound object. It checks for read/write and that the field is different before triggerina a change.
    /// </summary>
    /// <see cref="Set&lt;TF>"/>
    /// <param name="field">ref string myCustomAsset.aField to update</param>
    /// <param name="from">string to set the field to if all checks pass</param>
    [UsedImplicitly]
    protected void Set(ref string field, string from) { Set<string>(ref field, from); }

    /// <summary>
    /// All extraction by casting a custom object to the contained type. Same as getting the Value -
    /// as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <param name="t">Instance of custom asset</param>
    /// <returns>Instance of the contained serializable object</returns>
    public static implicit operator T([NotNull] OfType<T> t) { return t.seed; }

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
    protected void OnDisable() { Save(); }
  }
}