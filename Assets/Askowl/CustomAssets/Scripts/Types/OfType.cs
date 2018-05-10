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
  /// <remarks><a href="http://customasset.marrington.net#oftypet">More...</a></remarks>
  /// <typeparam name="T">Type of object this custom asset contains</typeparam>
  public abstract class OfType<T> : Base {
    /// <summary>
    /// Used by concrete classes for the getter/setter
    /// </summary>
    [SerializeField] private T value;

    [Header("Persistence"), SerializeField, Tooltip("Allow the data to be changed")]
    private bool readWrite = true;

    [SerializeField, Tooltip("Save to storage")]
    private bool persistent;

    [SerializeField, Tooltip("Save on every change")]
    private bool critical;

    /// <summary>
    /// For safe access to the contents field
    /// </summary>
    [UsedImplicitly]
    public T Value { protected get { return value; } set { Set(() => this.value = value); } }

    /// <summary>
    /// Tells the event listeners that something in this value has changed. Designed to be used in setters.
    /// It will also save the data on critical and call all listeners using `Changed`
    /// </summary>
    /// <code>public float aFloat { get { return Value.F; } set { Set(() => Value.F = value); } }</code>
    /// <param name="action">Lambda called if custom asset is read/write</param>
    protected void Set(Action action) {
      if (!readWrite && !persistent && !critical) return;

      action();
      if (critical) Save();
      Changed();
    }

    /// <summary>
    /// All extraction by casting a custom object to the contained type. Same as getting the Value -
    /// as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <param name="t">Instance of custom asset</param>
    /// <returns>Instance of the contained serializable object</returns>
    public static implicit operator T([NotNull] OfType<T> t) { return t.value; }

    /// <inheritdoc />
    /// <summary>
    /// Pass string conversion responsibility  from the custom asset to the containing value.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <returns>String representation of the contents of the containing value</returns>
    public override string ToString() { return value.ToString(); }

    [Serializable]
    private class ForJson<TJ> {
      public TJ Value;
    }

    private string Key { get { return string.Format("{0}:{1}", name, typeof(T)); } }

    /// <summary>
    /// Load the last previously saved value from persistent storage.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    [UsedImplicitly]
    protected TS Loader<TS>() {
      string      json    = PlayerPrefs.GetString(Key, defaultValue: "");
      ForJson<TS> forJson = JsonUtility.FromJson<ForJson<TS>>(json);
      return (forJson != null) ? forJson.Value : default(TS);
    }

    /// <summary>
    /// Load the last previously saved value from persistent storage. Called
    /// implicitly when persistent flag is set and custom asset is enabled.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    public virtual void Load() { Value = Loader<T>(); }

    /// <summary>
    /// Save current value to persistent storage. Called emplicitly when  when persistent flag is set
    /// and custom asset is disabled or on every change if it is marked critical.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    [UsedImplicitly]
    public virtual void Save() { Saver<T>(value); }

    protected void Saver<TS>(TS data) {
      if (persistent || critical) {
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(new ForJson<TS> {Value = data}));
      }
    }

    /// <summary>
    /// Called when an asset is loaded and enabled. Used to ensure the custom asset does not leave memory prematurely and to load it if persistent
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected virtual void OnEnable() {
      if (readWrite && !(persistent || critical)) hideFlags = HideFlags.DontUnloadUnusedAsset;
      Load();
    }

    /// <summary>
    /// OnDisable is called when the program exits or is closed by the platform. It is the last chance to save persistent data.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected void OnDisable() { Save(); }
  }
}