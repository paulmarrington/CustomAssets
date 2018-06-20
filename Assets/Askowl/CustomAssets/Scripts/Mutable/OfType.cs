// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using System.Collections.Generic;
using Askowl;
using UnityEditor;

namespace CustomAsset.Mutable {
  using UnityEngine;

  /// <inheritdoc cref="Constant.OfType&lt;T>" />
  /// <summary>
  /// Base class for a custom asset. Provides getters and setters for the contained value and
  /// templates for casting to the contained type and to convert it to a string.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
  /// <typeparam name="T">Type of object this custom asset contains</typeparam>
  public class OfType<T> : Constant.OfType<T>, HasEmitter {
    #region Construction
    /// <summary>
    /// If this is a project asset, then you will need to reference it somewhere.
    /// Other classes can get a reference using `Instance()` or `Instance(string name)`.
    /// Also useful for creating in-memory versions to share between hosts.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#instance">More...</a></remarks>
    /// <code>Float lifetime = Float.Instance("Lifetime")</code>
    /// <param name="name"></param>
    /// <returns>An instance of OfType&lt;T>, either retrieved or created</returns>
    public new static OfType<T> Instance(string name) {
      OfType<T>[] instances = Objects.Find<OfType<T>>(name);

      OfType<T> instance =
        (instances.Length > 0) ? instances[0] : Resources.Load<OfType<T>>(name);

      if (instance == null) return New(name);

      AssetToUnload(instance);
      return instance;
    }

    /// <summary>
    /// Create a new instance dynamically.
    /// </summary>
    /// <param name="name"></param>
    public static OfType<T> New(string name = null) {
      OfType<T> instance = CreateInstance<OfType<T>>();
      instance.name = name ?? Guid.NewGuid().ToString();
      return instance;
    }

    /// <summary>
    /// Called when an asset is loaded and enabled. Used to ensure the custom asset does not leave memory prematurely and to load it if persistent
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected virtual void OnEnable() {
      hideFlags = HideFlags.DontUnloadUnusedAsset;
      Load();
    }

    private void OnDisable() { Save(); }
    #endregion

    #region Listeners
    /// <summary>
    /// For safe(ish) access to the contents field
    /// </summary>
    public void Set(T value) {
      if (Equals(Value, value)) return;

      Value = value;
      emitter.Fire();
    }

    private Emitter emitter = new Emitter();

    /// <inheritdoc cref="Emitter" />
    public Emitter Emitter { get { return emitter; } }
    #endregion

    #region Comparators
    /// <summary>
    /// Implement in concrete class to compare data (Equals).
    /// </summary>
    /// <param name="other">The other data object to compare to</param>
    /// <returns></returns>
    protected virtual bool Equals(T other) { return Equals(Value, other); }

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
    #endregion

    #region Persistence
    [SerializeField] private bool persistent;

    private string Key { get { return string.Format("{0}:{1}", name, typeof(T)); } }

    /// <summary>
    /// Basic load for a persistent custom asset.
    /// </summary>
    public void Load() {
      if (persistent) Value = Loader<T>();
    }

    /// <summary>
    /// Basic save for a persistent custom asset.
    /// </summary>
    public void Save() { Saver(data: Value); }

    /// <summary>
    /// Load the last previously saved value from persistent storage.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected TD Loader<TD>() {
      return JsonUtility.FromJson<TD>(PlayerPrefs.GetString(Key, defaultValue: ""));
    }

    /// <summary>
    /// Save data to persistent storage for the next run.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    /// <param name="data">data to save</param>
    /// <typeparam name="TD">Class for save data</typeparam>
    // ReSharper disable once MemberCanBePrivate.Global
    protected void Saver<TD>(TD data) {
      if (persistent) PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
    }
    #endregion

    #region EditorOnly
#if UNITY_EDITOR
    private static void AssetToUnload(ScriptableObject asset) { AssetsToUnload.Add(asset); }

    static OfType() { EditorApplication.playModeStateChanged += UnloadResources; }

    // ReSharper disable once StaticMemberInGenericType
    private static readonly List<ScriptableObject> AssetsToUnload = new List<ScriptableObject>();

    private static void UnloadResources(PlayModeStateChange playModeState) {
      if (playModeState != PlayModeStateChange.ExitingPlayMode) return;

      EditorApplication.playModeStateChanged -= UnloadResources;

      foreach (var asset in AssetsToUnload) Resources.UnloadAsset(asset);
    }
#else
    private static void AssetToUnload(ScriptableObject asset){}
    #endif
    #endregion
  }
}