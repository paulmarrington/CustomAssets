// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Text.RegularExpressions;
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="">Class for picking one or a list of quotes - either set in the inspector or in a separate text file. Each quote is on a separate line in the form:</a> //#TBD#// <inheritdoc />
  [Serializable] public sealed class QuoteSet : Set<string> {
    [SerializeField, Tooltip("Asset with one quote per line (with attribution in brackets at end)")]
    private TextAsset[] quoteFiles;

    /// <a href=""></a> //#TBD#// <inheritdoc />
    protected override Selector<string> BuildSelector() {
      base.BuildSelector(); // renews Choices

      Fifo<string> choices = new Fifo<string>();

      if (Selector.Choices.Length > 0) Rtf(choices, Selector.Choices);

      if (quoteFiles != null) {
        for (int i = 0; i < quoteFiles.Length; i++) Rtf(choices, quoteFiles[i].text.Split('\n'));
      }

      Selector.Choices = choices.ToArray();
      return Selector;
    }

    /// <a href=""></a> //#TBD#//
    public int Count => Selector.Choices.Length;

    /// <a href="">Turn a list of strings into a list of formatted quotes</a> //#TBD#//
    public static void Rtf(Fifo<string> to, string[] from) {
      for (int i = 0; i < from.Length; i++) {
        if (!string.IsNullOrEmpty(from[i])) to.Push(Rtf(from[i]));
      }
    }

    /// <a href="">Turn a string into a quote. Any text at the end of the string that is in brackets becomes an attribution in grey</a> //#TBD#//
    public static string Rtf(string quote) {
      return Regex.Replace(input: quote, pattern: @"^(.*?)\s*\((.*)\)$", evaluator: m =>
                             $"<b>\"</b><i>{m.Groups[1].Value}</i><b>\"</b>      <color=grey>{(m.Groups.Count > 1 ? m.Groups[2].Value : "")}</color>");
    }
  }

  /// <a href="">Custom Asset for picking one or a list of quotes - either kept in the asset or as a separate text file. Each quote is on a separate line in the form: <code>Quote body (attribution)</code></a> //#TBD#// <inheritdoc cref="QuoteSet" />
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Quotes")]
  public sealed class Quotes : OfType<QuoteSet>, Pick<string> {
    /// <a href=""></a> //#TBD#// <inheritdoc cref="Quotes()" />
    private void OnEnable() {
      #if UNITY_EDITOR
      if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
      #endif
      Value.Reset();
    }

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public string Pick() => Value.Pick();

    /// <a href=""><see cref="QuoteSet.Rtf(string)"/></a> //#TBD#//
    public static string Rtf(string quote) => QuoteSet.Rtf(quote);

    /// <a href=""></a> //#TBD#//
    public int Count => Value.Count;
  }
}