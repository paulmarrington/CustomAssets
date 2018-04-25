/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System;
using System.Collections.Generic;

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
        this.value = value;
        if (persistent && critical) Save();
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

    [Serializable]
    private class ForJson<TJ> {
      public TJ Value;
    }

    /// <summary>
    /// Load the last previously saved value from persistent storage. Called
    /// implicitly when persistent flag is set and custom asset is enabled.
    /// </summary>
    [UsedImplicitly]
    public void Load() {
      if (!persistent) return;

      string json = PlayerPrefs.GetString(name, null);
      Value = JsonUtility.FromJson<ForJson<T>>(json).Value;
    }

    /// <summary>
    /// Save current value to persistent storage. Called emplicitly when  when persistent flag is set
    /// and custom asset is disabled or on every change if it is marked critical.
    /// </summary>
    [UsedImplicitly]
    public void Save() {
      if (!persistent) return;

      PlayerPrefs.SetString(name, JsonUtility.ToJson(new ForJson<T> {Value = value}));
    }

    /// <inheritdoc />
    protected override void OnEnable() {
#if UNITY_EDITOR
      // so editor behaves like a target platform - and has the asset contents, not those from last run
      if (jsonForReset == null) {
        jsonForReset = JsonUtility.ToJson(new ForJson<T> {Value = value});
      } else {
        // lower case 'value' so that the update trigger doesn't happen
        value = JsonUtility.FromJson<ForJson<T>>(jsonForReset).Value;
      }
#endif
      base.OnEnable();
      hideFlags = HideFlags.DontUnloadUnusedAsset;
      Load();
    }

    private string jsonForReset;

    private void OnDisable() { Save(); }
  }
}