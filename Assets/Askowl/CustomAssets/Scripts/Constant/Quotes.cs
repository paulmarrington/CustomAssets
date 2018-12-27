// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Text.RegularExpressions;
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="http://bit.ly/2QR9rF8">Class for picking one or a list of quotes - either set in the inspector or in a separate text file. Each quote is on a separate line in the form:</a> <inheritdoc />
  [Serializable] public sealed class QuoteSet : Set<string> {
    [SerializeField, Tooltip("Asset with one quote per line (with attribution in brackets at end)")]
    private TextAsset[] quoteFiles = default;

    /// <a href="http://bit.ly/2QR9rF8"></a> <inheritdoc />
    protected override void BuildSelector() {
      Fifo<string> choices = new Fifo<string>();
      if (InitialSize > 0) {
        base.BuildSelector(); // renews Choices
        Rtf(choices, Selector.Choices);
      }

      if (quoteFiles != null) {
        for (int i = 0; i < quoteFiles.Length; i++) Rtf(choices, quoteFiles[i].text.Split('\n'));
      }

      Selector.Choices = choices.ToArray();
    }

    /// <a href="http://bit.ly/2QR9rF8"></a>
    public int Count => Selector.Choices.Length;

    /// <a href="http://bit.ly/2RjdIRe">Turn a list of strings into a list of formatted quotes</a>
    public static void Rtf(Fifo<string> to, string[] from) {
      for (int i = 0; i < from.Length; i++) {
        if (!string.IsNullOrEmpty(from[i])) to.Push(Rtf(from[i]));
      }
    }

    /// <a href="http://bit.ly/2RjdIRe">Turn a string into a quote. Any text at the end of the string that is in brackets becomes an attribution in grey</a>
    public static string Rtf(string quote) =>
      Regex.Replace(
        input: quote, pattern: @"^(.*?)\s*\((.*)\)$", evaluator: m =>
          $"<b>\"</b><i>{m.Groups[1].Value}</i><b>\"</b>      <color=grey>{(m.Groups.Count > 1 ? m.Groups[2].Value : "")}</color>");
  }

  /// <a href="http://bit.ly/2QR9rF8">Custom Asset for picking one or a list of quotes - either kept in the asset or as a separate text file. Each quote is on a separate line in the form: <code>Quote body (attribution)</code></a> <inheritdoc cref="QuoteSet" />
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Quotes")]
  public class Quotes : OfType<QuoteSet>, Pick<string> {
    /// <a href="http://bit.ly/2QR9rF8"></a> <inheritdoc cref="Quotes()" />
    protected override void OnEnable() {
      base.OnEnable();
      #if UNITY_EDITOR
      if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
      #endif
      Value.Reset();
    }

    /// <a href="http://bit.ly/2QMafv0"></a> <inheritdoc />
    public string Pick() => Value.Pick();

    /// <a href="http://bit.ly/2RjdIRe"><see cref="QuoteSet.Rtf(string)"/></a>
    public static string Rtf(string quote) => QuoteSet.Rtf(quote);

    /// <a href="http://bit.ly/2QR9rF8"></a>
    public int Count => Value.Count;
  }
}