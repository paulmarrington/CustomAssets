using UnityEngine.UI;

namespace CustomAsset {
  /// <summary>
  /// Drop into the same Game Object as a text component to update text content
  /// whenever the string custom asset changes. Alternatively, set the reference
  /// to a text component to use.
  /// </summary>
  // ReSharper disable once InconsistentNaming
  public sealed class UITextListener : ComponentListener<Text> {
    protected override void Change(string value) { Component.text = value; }
  }
}