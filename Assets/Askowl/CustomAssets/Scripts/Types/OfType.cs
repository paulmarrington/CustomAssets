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
    [SerializeField] private T value;

    [Header("Persistence"), SerializeField, Tooltip("Allow the data to be changed")]
    private bool readWrite = true;

    [SerializeField, Tooltip("Save to storage")]
    private bool persistent;

    [SerializeField, Tooltip("Save on every change")]
    private bool critical;

    /// <summary>
    /// Value contained within the custom asset. The getter is plain, but the setter
    /// calls registered events. It will also persist the data for critical
    /// applications
    /// </summary>
    public T Value {
      get { return value; }
      // ReSharper disable once MemberCanBePrivate.Global
      set {
        if (!readWrite) return;

        this.value = value;
        if (persistent && critical) Save();
        Changed();
      }
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
    public override string ToString() { return Value.ToString(); }

    [Serializable]
    private class ForJson<TJ> {
      public TJ Value;
    }

    private string Key { get { return string.Format("{0}:{1}", name, typeof(T)); } }

    /// <summary>
    /// Load the last previously saved value from persistent storage. Called
    /// implicitly when persistent flag is set and custom asset is enabled.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    [UsedImplicitly]
    public void Load() {
      if (!persistent) return;

      string json = PlayerPrefs.GetString(Key, null);
      Value = JsonUtility.FromJson<ForJson<T>>(json).Value;
    }

    /// <summary>
    /// Save current value to persistent storage. Called emplicitly when  when persistent flag is set
    /// and custom asset is disabled or on every change if it is marked critical.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    [UsedImplicitly]
    public void Save() {
      if (!persistent) return;

      PlayerPrefs.SetString(Key, JsonUtility.ToJson(new ForJson<T> {Value = value}));
    }

    /// <summary>
    /// Called when an asset is loaded and enabled. Used to ensure the custom asset does not leave memory prematurely and to load it if persistent
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected virtual void OnEnable() {
#if UNITY_EDITOR
      ResetValueInEditorOnly();
#endif
      hideFlags = HideFlags.DontUnloadUnusedAsset;
      Load();
    }

    /// <summary>
    /// OnDisable is called when the program exits or is closed by the platform. It is the last chance to save persistent data.
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected void OnDisable() { Save(); }

#if UNITY_EDITOR
    private void ResetValueInEditorOnly() {
      // so editor behaves like a target platform - and has the asset contents, not those from last run
      if (!string.IsNullOrEmpty(jsonForReset)) {
        var wrapper = JsonUtility.FromJson<ForJson<T>>(jsonForReset);

        if (wrapper != null) {
          value = wrapper.Value;
          return;
        }
      }

      jsonForReset = JsonUtility.ToJson(new ForJson<T> {Value = value});
    }

    private string jsonForReset;
#endif
  }
}