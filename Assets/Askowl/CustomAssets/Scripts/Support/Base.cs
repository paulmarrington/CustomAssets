/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System.Collections.Generic;
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Dynamic custom asset without any values. Use it to trigger and listen to events.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
  public partial class Base : ScriptableObject {
    /// <summary>
    /// Used by string listener who only can't cast a type it is not aware of. Overridden in Members.cs
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    /// <param name="memberName">Name of member to retrieve and ToStringify</param>
    /// <returns>string reprentation of value associated with this member</returns>
    public virtual string ToStringForMember(string memberName) { return memberName; }

    /// <summary>
    /// If this is a project asset, then you will need to reference it somewhere.
    /// Other classes can get a reference using `Instance()` or `Instance(string name)`.
    /// Also useful for creating in-memory versions to share between hosts.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#instance">More...</a></remarks>
    /// <code>Float lifetime = Float.Instance("Lifetime")</code>
    /// <param name="name"></param>
    /// <returns>An instance of OfType&lt;T>, either retrieved or created</returns>
    public static TI Instance<TI>(string name = null) where TI : Base {
      TI[] instances = Objects.Find<TI>(name);
      if (instances.Length > 0) return instances[0];

      TI instance = CreateInstance<TI>();
      instance.name = name ?? typeof(TI).Name;
#if UNITY_EDITOR
      instance.unloadable = false;
#endif
      return instance;
    }

    /// <inheritdoc />
    public override bool Equals(object other) { return false; }

    /// <summary>
    /// Just to fulfill pedantic requirements
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    // ReSharper disable once UnusedParameter.Global
    protected bool Equals(Base other) { return false; }

    /// <inheritdoc />
    public override int GetHashCode() { return base.GetHashCode(); }

#if UNITY_EDITOR
    /// <summary>
    /// Editor only description of what the asset is all about.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
    [Multiline] public string Description = " ";

    private bool unloadable = true;

    static Base() { EditorApplication.playModeStateChanged += UnloadResources; }

    /// <inheritdoc />
    public Base() { AssetsToUnload.Add(this); }

    private static readonly List<Base> AssetsToUnload = new List<Base>();

    private static void UnloadResources(PlayModeStateChange playModeState) {
      if (playModeState != PlayModeStateChange.ExitingPlayMode) return;

      EditorApplication.playModeStateChanged -= UnloadResources;

      foreach (var asset in AssetsToUnload) {
        if (asset.unloadable) Resources.UnloadAsset(asset);
      }
    }
#endif
  }
}