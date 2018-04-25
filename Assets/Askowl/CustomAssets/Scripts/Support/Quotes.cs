using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

namespace Askowl {
  public sealed class Quotes : Pick<string> {
    private readonly TextAsset quoteAsset;

    private Selector<string> selector;

    [UsedImplicitly]
    public int Length { get; private set; }

    public Quotes(string quoteResourceName = "quotes") {
      if (quoteAsset == null) {
        quoteAsset = (Resources.Load(path: quoteResourceName) as TextAsset);
      }

      if (quoteAsset != null) Init(listOfQuotes: quoteAsset.text.Split('\n'));
    }

    [UsedImplicitly]
    public Quotes([NotNull] string[] listOfQuotes) { Init(listOfQuotes: listOfQuotes); }

    private void Init([NotNull] string[] listOfQuotes) {
      Length   = listOfQuotes.Length;
      selector = new Selector<string>(choices: listOfQuotes, isRandom: (Length < 100));
    }

    [NotNull]
    public string Pick() { return RTF(quote: selector.Pick()); }

    [NotNull, UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    public string RTF([NotNull] string quote) {
      return Regex.Replace(input: quote, pattern: @"^(.*?)\s*\((.*)\)$", evaluator: m =>
                             string.Format(
                               format: "<b>\"</b><i>{0}</i><b>\"</b>      <color=grey>{1}</color>",
                               arg0: m.Groups[1].Value,
                               arg1: m.Groups.Count > 1 ? m.Groups[2].Value : "")
      );
    }
  }
}