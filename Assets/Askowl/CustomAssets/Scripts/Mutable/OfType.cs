// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using System.Collections.Generic;
using UnityEditor;

namespace CustomAsset.Mutable {
  using UnityEngine;

  /// <summary>
  /// Typeless base class that has an emitter
  /// </summary>
  public class WithEmitter : Base {
    /// <summary>
    /// Emitter reference to tell others of data changes
    /// </summary>
    public readonly Emitter Emitter = new Emitter();

    /// <summary>
    /// THe default is to fire the emitter at each polling interval.
    /// Override Equals in T (Service) for services where you can tell if external data has changed
    /// </summary>
    public void Poll() {
      if (!Equals(null)) Emitter.Fire();
    }
  }

  /// <inheritdoc cref="Constant.OfType&lt;T>" />
  /// <summary>
  /// Base class for a custom asset. Provides getters and setters for the contained value and
  /// templates for casting to the contained type and to convert it to a string.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
  /// <typeparam name="T">Type of object this custom asset contains</typeparam>
  public class OfType<T> : WithEmitter {
    #region Data
    [SerializeField, Value] private T value;

    private bool valueSet;

    /// <summary>
    /// For safe(ish) access to the contents field
    /// </summary>
    protected T Value { get { return valueSet ? value : Initialise(); } set { Set(value); } }

    /// <summary>
    /// Called the first time we want access to T. JIT initialisation helps since RuntimeInitializeOnLoadMethod
    /// does not run until after all the OnEnables.
    /// </summary>
    /// <returns></returns>
    public virtual T Initialise() { return value; }

    /// <summary>
    /// All extraction by casting a custom object to the contained type. Same as getting the Value -
    /// as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <param name="t">Instance of custom asset</param>
    /// <returns>Instance of the contained serializable object</returns>
    public static implicit operator T(OfType<T> t) { return (t == null) ? default(T) : t.value; }

    /// <inheritdoc />
    /// <summary>
    /// Pass string conversion responsibility  from the custom asset to the containing value.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#accessing-custom-assets">More...</a></remarks>
    /// <returns>String representation of the contents of the containing value</returns>
    public override string ToString() { return value.ToString(); }
    #endregion

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
    public new static TC Instance<TC>(string name) where TC : Base {
      TC instance = Base.Instance<TC>(name);
      AssetToUnload(instance); // so unexpected data does not remain between editor plays
      return instance;
    }

    /// <summary>
    /// Create a new instance of the asset with a random name (GUID based)
    /// </summary>
    /// <typeparam name="TC">Type of asset to create</typeparam>
    public static TC New<TC>() where TC : Base {
      var asset = New<TC>(Guid.NewGuid().ToString());
      return asset;
    }

    /// <summary>
    /// Create a new instance of the asset with a supplied name. Note you can get duplicate names.
    /// </summary>
    /// <typeparam name="TC">Type of asset to create</typeparam>
    public static TC New<TC>(string name) where TC : Base {
      TC instance = CreateInstance<TC>();
      instance.name = name;
      NewAsset(instance);
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
    public void Set(T toValue) {
      bool equals = Equals(value, toValue);
      valueSet = true;
      value    = toValue; // do the set anyway since we may be changing object
      if (!equals) Emitter.Fire();
    }
    #endregion

    #region Comparators
    /// <inheritdoc />
    public override int GetHashCode() {
      return Equals(Value, default(T)) ? 0 : Value.GetHashCode();
    }

    /// <summary>
    /// Part of the group of Equals functions. Passes responsibility to the containing data
    /// </summary>
    /// <param name="other">Another reference to a custom asset of the same type</param>
    /// <returns></returns>
    public override bool Equals(object other) {
      try {
        var otherCustomAsset = other as OfType<T>;
        return Value.Equals((otherCustomAsset != null) ? otherCustomAsset.Value : default(T));
      } catch {
        return false;
      }
    }
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
    private static void AssetToUnload(ScriptableObject asset) {
      if (NewAssets.Contains(asset) || AssetsToUnload.Contains(asset)) return;

      AssetsToUnload.Add(asset);
    }

    private static void NewAsset(ScriptableObject asset) { NewAssets.Add(asset); }

    static OfType() { EditorApplication.playModeStateChanged += UnloadResources; }

    // ReSharper disable StaticMemberInGenericType
    private static readonly List<ScriptableObject> AssetsToUnload = new List<ScriptableObject>();

    private static readonly List<ScriptableObject> NewAssets = new List<ScriptableObject>();
    // ReSharper restore StaticMemberInGenericType

    private static void UnloadResources(PlayModeStateChange playModeState) {
      if (playModeState != PlayModeStateChange.ExitingPlayMode) return;

      EditorApplication.playModeStateChanged -= UnloadResources;

      foreach (var asset in AssetsToUnload) Resources.UnloadAsset(asset);
    }
#else
    private static void AssetToUnload(ScriptableObject asset){}
    private static void NewAsset(ScriptableObject asset){}
#endif
    #endregion
  }
}