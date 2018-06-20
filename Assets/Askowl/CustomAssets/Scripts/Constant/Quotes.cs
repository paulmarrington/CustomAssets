// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset.Constant {
  /// <inheritdoc />
  /// <summary>
  /// Class for picking one or a list of quotes - either set in the inspector or in a separate text file.
  /// Each quote is on a separate line in the form:
  /// <code>Quote body (attribution)</code>
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#quotes">More...</a></remarks>
  [Serializable]
  public sealed class QuoteSet : Set<string> {
    [SerializeField, Tooltip("Asset with one quote per line (with attribution in brackets at end)")]
    private TextAsset[] quoteFiles;

    /// <inheritdoc />
    protected override void BuildSelector() {
      base.BuildSelector(); // renews Choices
      List<string[]> lists = new List<string[]> {Rtf(Selector.Choices)};

      if (quoteFiles != null) {
        foreach (var textFile in quoteFiles) lists.Add(Read(textFile));
      }

      List<string> amalgum = new List<string>();
      foreach (string[] list in lists) amalgum.AddRange(list);

      Selector.Choices = amalgum.ToArray();
    }

    /// <summary>
    /// Read in some text from a file
    /// </summary>
    /// <param name="textFile">The TextAsset referencing a file in a Resources directory</param>
    /// <returns>A list of strings - one for each line</returns>
    public static string[] Read(TextAsset textFile) { return Rtf(textFile.text.Split('\n')); }

    /// <summary>
    /// Turn a list of strings into a list of formatted quotes
    /// </summary>
    /// <param name="quotes">Strings to format</param>
    /// <returns>List of quotes</returns>
    public static string[] Rtf(string[] quotes) {
      List<string> results = new List<string>();

      for (int i = 0; i < quotes.Length; i++) {
        if (!string.IsNullOrEmpty(quotes[i])) results.Add(Rtf(quotes[i]));
      }

      return results.ToArray();
    }

    /// <summary>
    /// Turn a string into a quote. Any text at the end of the string that is in brackets becomes an attribution in grey.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#rtf">More...</a></remarks>
    /// <param name="quote">Text for of quote</param>
    /// <returns>Unity RTF form of quote</returns>
    public static string Rtf(string quote) {
      return Regex.Replace(input: quote, pattern: @"^(.*?)\s*\((.*)\)$", evaluator: m =>
                             string.Format(
                               "<b>\"</b><i>{0}</i><b>\"</b>      <color=grey>{1}</color>",
                               m.Groups[1].Value, m.Groups.Count > 1 ? m.Groups[2].Value : ""));
    }
  }

  /// <inheritdoc cref="QuoteSet" />
  /// <summary>
  /// Custom Asset for picking one or a list of quotes - either kept in the asset or as a separate text file.
  /// Each quote is on a separate line in the form:
  /// <code>Quote body (attribution)</code>
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#quotes">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Quotes")]
  public sealed class Quotes : OfType<QuoteSet>, Pick<string> {
    /// <inheritdoc cref="Quotes()" />
    private void OnEnable() {
#if UNITY_EDITOR
      if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
      Value.Reset();
    }

    /// <inheritdoc />
    public string Pick() { return Value.Pick(); }
  }
}