// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using System.Collections.Generic;
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QR9q42">Typeless base class that has an emitter</a>
  public class WithEmitter : Base {
    /// <a href="http://bit.ly/2QR9q42">Emitter reference to tell others of data changes</a>
    public readonly Emitter Emitter = Emitter.Instance;

    #region Polling
    /// <a href="http://bit.ly/2QR9q42">Poll to fire an event where no other mechanism exists</a>
    [Serializable] public class Polling {
      [SerializeField] private bool  enabled                 = false;
      [SerializeField] private float secondsDelayAtStart     = 5;
      [SerializeField] private float updateIntervalInSeconds = 1;

      /// <a href="http://bit.ly/2QR9q42"><see cref="OfType{T}.Initialise"/></a>
      public void Initialise(WithEmitter componentToPoll) {
        if (!enabled) return;

        void poll(Fiber fiber) {
          if (!componentToPoll.Equals(null)) componentToPoll.Emitter.Fire();
        }

        Fiber.Start("Polling").WaitFor(secondsDelayAtStart).Begin.Do(poll).WaitFor(updateIntervalInSeconds).Again.Finish();
      }
    }

    [SerializeField] private Polling polling = default;

    /// <a href="http://bit.ly/2QR9q42">Called when an asset is loaded and enabled. Used to ensure the custom asset does not leave memory prematurely and to load it if persistent</a>
    protected override void OnEnable() {
      base.OnEnable();
      polling?.Initialise(this);
    }
    #endregion
  }

  /// <a href="http://bit.ly/2QQjKcL">Base class for a custom asset. Provides getters and setters for the contained value and templates for casting to the contained type and to convert it to a string</a> <inheritdoc cref="Constant.OfType&lt;T>" />
  public class OfType<T> : WithEmitter {
    #region Data
    [SerializeField, Label] private T value = default;

    /// <a href="http://bit.ly/2QQjKcL">For safe(ish) access to the contents field</a>
    public virtual T Value { get => value; set => Set(value); }

    /// <a href="http://bit.ly/2QQjKcL">All extraction by casting a custom object to the contained type. Same as getting the Value - as in myCustomAsset.Value === (MyCustomAsset) myCustomAsset</a>
    public static implicit operator T(OfType<T> t) => (t == null) ? default : t.value;

    /// <a href="http://bit.ly/2QQjKcL">Pass string conversion responsibility from the custom asset to the containing value</a> <inheritdoc />
    public override string ToString() => value.ToString();
    #endregion

    #region Construction
    /// <a href="http://bit.ly/2QQjKcL">If this is a project asset, then you will need to reference it somewhere. Other classes can get a reference using `Instance()` or `Instance(string name)`. Also useful for creating in-memory versions to share between hosts</a>
    public new static TC Instance<TC>(string path = null) where TC : Base {
      var instance = Base.Instance<TC>(path);
      AssetToUnload(instance); // so unexpected data does not remain between editor plays
      return instance;
    }

    /// <a href="http://bit.ly/2QQjKcL">Create a new instance of the asset with a random name (GUID based)</a>
    public static TC New<TC>() where TC : Base => New<TC>(Guid.NewGuid().ToString());

    /// <a href="http://bit.ly/2QQjKcL">Create a new instance of the asset with a supplied name. Note you can get duplicate names</a>
    public static TC New<TC>(string name) where TC : Base {
      TC instance = CreateInstance<TC>();
      instance.name = name;
      NewAsset(instance);
      return instance;
    }

    /// <a href="http://bit.ly/2QQjKcL">Create a new instance dynamically</a>
    public static OfType<T> New(string name = null) {
      OfType<T> instance = CreateInstance<OfType<T>>();
      instance.name = name ?? Guid.NewGuid().ToString();
      return instance;
    }

    /// <a href="http://bit.ly/2QQjKcL">Called when an asset is loaded and enabled. Used to ensure the custom asset does not leave memory prematurely and to load it if persistent</a>
    protected override void OnEnable() {
      base.OnEnable();
      hideFlags = HideFlags.DontUnloadUnusedAsset;
      Load();
    }

    /// <a href="http://bit.ly/2QQjKcL"></a>
    protected override void OnDisable() => Save();
    #endregion

    #region Listeners
    /// <a href="http://bit.ly/2QQjKcL">For safe(ish) access to the contents field</a>
    public virtual void Set(T toValue) {
      value = toValue;
      Emitter.Fire();
    }
    #endregion

    #region Comparators
    /// <a href="http://bit.ly/2QQjKcL"></a> <inheritdoc />
    public override int GetHashCode() => Equals(Value, default(T)) ? 0 : Value.GetHashCode();

    /// <a href="http://bit.ly/2QQjKcL">Part of the group of Equals functions. Passes responsibility to the containing data</a>
    public override bool Equals(object other) {
      try {
        var otherCustomAsset = other as OfType<T>;
        return Value.Equals((otherCustomAsset != null) ? otherCustomAsset.Value : default);
      } catch {
        return false;
      }
    }
    #endregion

    #region Persistence
    [SerializeField] private bool persistent = default;

    private string Key => $"{name}:{typeof(T)}";

    [Serializable] private struct Wrap {
      public T data;
    }

    /// <a href="http://bit.ly/2QMNebi">Basic load for a persistent custom asset</a>
    public void Load() {
      if (!persistent) return;
      var json                   = PlayerPrefs.GetString(Key, defaultValue: "");
      if (json.Length > 0) Value = JsonUtility.FromJson<Wrap>(json).data;
    }

    /// <a href="http://bit.ly/2QMNebi">Basic save for a persistent custom asset</a>
    public void Save() {
      if (persistent) PlayerPrefs.SetString(Key, JsonUtility.ToJson(new Wrap {data = Value}));
    }
    #endregion

    #region EditorOnly
    #if UNITY_EDITOR
    private static void AssetToUnload(ScriptableObject asset) {
      if (newAssets.Contains(asset) || assetsToUnload.Contains(asset)) return;
      assetsToUnload.Add(asset);
    }

    private static void NewAsset(ScriptableObject asset) => newAssets.Add(asset);

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