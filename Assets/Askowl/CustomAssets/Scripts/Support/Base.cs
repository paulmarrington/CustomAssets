/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Dynaic custom asset without any values. Use it to trigger and listen to events.
  /// </summary>
  public partial class Base : ScriptableObject {
#if UNITY_EDITOR
    /// <summary>
    /// Editor only description of what the asset is alla bout.
    /// </summary>
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