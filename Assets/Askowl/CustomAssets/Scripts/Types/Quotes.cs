﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Custom Asset for picking one or a list of quotes - either kept in the asset or as a separate text file.
  /// Each quote is on a separate line in the form:
  /// <code>Quote body (attribution)</code>
  /// </summary>
  /// <remarks><a href="http://customasset.marrington.net#quotes">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Quotes")]
  public sealed class Quotes : StringSet {
    [SerializeField, Tooltip("Asset with one quote per line (with attribution in brackets at end)")]
    private TextAsset textFile;

    /// <inheritdoc />
    protected override void OnEnable() {
      base.OnEnable();

#if UNITY_EDITOR
      if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif

      if (Value == null) Value = new List<string>();

      for (int i = 0; i < Value.Count; i++) {
        Value[i] = RTF(Value[i]);
      }

      if (textFile != null) {
        string[] quotes = textFile.text.Split('\n');

        for (int i = 0; i < quotes.Length; i++) {
          if (!string.IsNullOrEmpty(quotes[i])) Value.Add(RTF(quotes[i]));
        }
      }
    }

    /// <summary>
    /// Turn a string into a quote. Any text at the end of the string that is in brackets becomes an attribution in grey.
    /// </summary>
    /// <param name="quote">Text for of quote</param>
    /// <returns>Unity RTF form of quote</returns>
    [UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    public static string RTF(string quote) {
      return Regex.Replace(input: quote, pattern: @"^(.*?)\s*\((.*)\)$", evaluator: m =>
                             string.Format(
                               "<b>\"</b><i>{0}</i><b>\"</b>      <color=grey>{1}</color>",
                               m.Groups[1].Value, m.Groups.Count > 1 ? m.Groups[2].Value : ""));
    }
  }
}