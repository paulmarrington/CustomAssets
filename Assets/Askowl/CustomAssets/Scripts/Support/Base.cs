/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System.Collections.Generic;
using JetBrains.Annotations;
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
    [UsedImplicitly]
    public virtual string ToStringForMember(string memberName) { return memberName; }

#if UNITY_EDITOR
    /// <summary>
    /// Editor only description of what the asset is all about.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#oftypet">More...</a></remarks>
    [Multiline] public string Description = "";

    static Base() { EditorApplication.playModeStateChanged += UnloadResources; }

    /// <inheritdoc />
    public Base() { AssetsToUnload.Add(this); }

    private static readonly List<Base> AssetsToUnload = new List<Base>();

    private static void UnloadResources(PlayModeStateChange playModeState) {
      if (playModeState != PlayModeStateChange.ExitingPlayMode) return;

      EditorApplication.playModeStateChanged -= UnloadResources;

      foreach (var asset in AssetsToUnload) Resources.UnloadAsset(asset);
    }
#endif
  }
}