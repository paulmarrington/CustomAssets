// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using System.Collections.Generic;
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Typeless base class that has an emitter</a> //#TBD#//
  public class WithEmitter : Base {
    /// <a href="">Emitter reference to tell others of data changes</a> //#TBD#//
    public readonly Emitter Emitter = new Emitter();

    /// <a href="">The default is to fire the emitter at each polling interval.  Override Equals in T (Service) for services where you can tell if external data has changed</a> //#TBD#//
    public void Poll() {
      if (!Equals(null)) Emitter.Fire();
    }
  }

  /// <a href="">Base class for a custom asset. Provides getters and setters for the contained value and templates for casting to the contained type and to convert it to a string</a> //#TBD#// <inheritdoc cref="Constant.OfType&lt;T>" />
  public class OfType<T> : WithEmitter {
    #region Data

    [SerializeField, Label] private T value;

    private bool initialised;

    /// <a href="">For safe(ish) access to the contents field</a> //#TBD#//
    public T Value {
      get => initialised ? value : FirstAccess();
      set => Set(value);
    }

    private T FirstAccess() {
      initialised = true;
      Initialise();
      return value;
    }

    /// <a href="">Called the first time we want access to T. JIT initialisation helps since RuntimeInitializeOnLoadMethod does not run until after all the OnEnables.</a> //#TBD#//
    public virtual void Initialise() { }

    /// <a href="">All extraction by casting a custom object to the contained type. Same as getting the Value - as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset</a> //#TBD#//
    public static implicit operator T(OfType<T> t) => (t == null) ? default : t.value;

    /// <a href="">Pass string conversion responsibility from the custom asset to the containing value</a> //#TBD#// <inheritdoc />
    public override string ToString() => value.ToString();

    #endregion

    #region Construction

    /// <a href="">If this is a project asset, then you will need to reference it somewhere. Other classes can get a reference using `Instance()` or `Instance(string name)`. Also useful for creating in-memory versions to share between hosts</a> //#TBD#//
    public new static TC Instance<TC>(string name = null) where TC : Base {
      var instance = Base.Instance<TC>(name);
      AssetToUnload(instance); // so unexpected data does not remain between editor plays
      return instance;
    }

    /// <a href="">Create a new instance of the asset with a random name (GUID based)</a> //#TBD#//
    public static TC New<TC>() where TC : Base => New<TC>(Guid.NewGuid().ToString());

    /// <a href="">Create a new instance of the asset with a supplied name. Note you can get duplicate names</a> //#TBD#//
    public static TC New<TC>(string name) where TC : Base {
      TC instance = CreateInstance<TC>();
      instance.name = name;
      NewAsset(instance);
      return instance;
    }

    /// <a href="">Create a new instance dynamically</a> //#TBD#//
    public static OfType<T> New(string name = null) {
      OfType<T> instance = CreateInstance<OfType<T>>();
      instance.name = name ?? Guid.NewGuid().ToString();
      return instance;
    }

    /// <a href="">Called when an asset is loaded and enabled. Used to ensure the custom asset does not leave memory prematurely and to load it if persistent</a> //#TBD#//
    protected virtual void OnEnable() {
      hideFlags = HideFlags.DontUnloadUnusedAsset;
      Load();
    }

    private void OnDisable() {
      initialised = false;
      Save();
    }

    #endregion

    #region Listeners

    /// <a href="">For safe(ish) access to the contents field</a> //#TBD#//
    public virtual void Set(T toValue) {
      if (!initialised) FirstAccess();
      value = toValue;
      Emitter.Fire();
    }

    #endregion

    #region Comparators

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public override int GetHashCode() => Equals(Value, default(T)) ? 0 : Value.GetHashCode();

    /// <a href="">Part of the group of Equals functions. Passes responsibility to the containing data</a> //#TBD#//
    public override bool Equals(object other) {
      try {
        var otherCustomAsset = other as OfType<T>;
        return Value.Equals((otherCustomAsset != null) ? otherCustomAsset.Value : default);
      }
      catch {
        return false;
      }
    }

    #endregion

    #region Persistence

    [SerializeField] private bool persistent = default;

    private string Key => $"{name}:{typeof(T)}";

    [Serializable] private struct Wrap {
      public T Data;
    }

    /// <a href="">Basic load for a persistent custom asset</a> //#TBD#//
    public void Load() {
      if (persistent) Value = JsonUtility.FromJson<Wrap>(PlayerPrefs.GetString(Key, defaultValue: "")).Data;
    }

    /// <a href="">Basic save for a persistent custom asset</a> //#TBD#//
    public void Save() {
      if (persistent) PlayerPrefs.SetString(Key, JsonUtility.ToJson(new Wrap {Data = Value}));
    }

    #endregion

    #region EditorOnly

    #if UNITY_EDITOR
    private static void AssetToUnload(ScriptableObject asset) {
      if (newAssets.Contains(asset) || assetsToUnload.Contains(asset)) return;
      assetsToUnload.Add(asset);
    }

    private static void NewAsset(ScriptableObject asset) {
      newAssets.Add(asset);
    }

    static OfType() => EditorApplication.playModeStateChanged += UnloadResources;

    private static readonly List<ScriptableObject> assetsToUnload = new List<ScriptableObject>();

    private static readonly List<ScriptableObject> newAssets = new List<ScriptableObject>();

    private static void UnloadResources(PlayModeStateChange playModeState) {
      if (playModeState != PlayModeStateChange.ExitingPlayMode) return;
      EditorApplication.playModeStateChanged -= UnloadResources;
      for (int i = assetsToUnload.Count - 1; i >= 0; i--) Resources.UnloadAsset(assetsToUnload[i]);
    }
    #else
    private static void AssetToUnload(ScriptableObject asset){}
    private static void NewAsset(ScriptableObject asset){}
#endif

    #endregion
  }
}