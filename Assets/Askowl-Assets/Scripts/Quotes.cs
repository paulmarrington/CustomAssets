using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

internal sealed class Quotes : Pick<string> {
  private readonly TextAsset quoteAsset;

  private readonly Selector<string> selector = new Selector<string>();

  public Quotes(string quoteResourceName = "quotes") {
    if (quoteAsset == null) {
      quoteAsset = (Resources.Load(path: quoteResourceName) as TextAsset);
    }

    if (quoteAsset != null) Init(listOfQuotes: quoteAsset.text.Split('\n'));
  }

  [UsedImplicitly]
  public Quotes([NotNull] string[] listOfQuotes) { Init(listOfQuotes: listOfQuotes); }

  private void Init([NotNull] string[] listOfQuotes) {
    selector.Choices = listOfQuotes;

    if (listOfQuotes.Length < 100) {
      selector.Exhaustive();
    }
  }

  [NotNull]
  public string Pick() { return RTF(quote: selector.Pick()); }

  [NotNull]
  private string RTF([NotNull] string quote) {
    return Regex.Replace(input: quote, pattern: @"^(.*?)\s*\((.*)\)$", evaluator: m =>
                           string.Format(
                             format: "<b>\"</b><i>{0}</i><b>\"</b>      <color=grey>{1}</color>",
                             arg0: m.Groups[groupnum: 1].Value,
                             arg1: m.Groups.Count > 1 ? m.Groups[groupnum: 2].Value : "")
    );
  }
}